using Godot;
using Game.Utilities;
using System;

namespace Game.Gameplay;

public partial class Enemy : Entity
{   
	[Signal] public delegate void AnimationEventHandler(string animationType);
	
	[ExportCategory("Nodes")]
	[Export] public Player Player;
	[Export] public EnemyHealthbar EnemyHealthbar;
	[Export] public EnemyHurtbox EnemyHurtbox;
	[Export] public EntityAnimation EnemyAnimation;
	[Export] public EnemyMovement EnemyMovement;
	[Export] public Area2D DetectionArea;
	[Export] public StateMachine StateMachine;
	
	
	[ExportCategory("Enemy Vars")]
	[Export] public int AttackDamage = 4;


	public Tween ActiveTween;

	private Tween _dmgFlashTween;

	public override void _Ready()
	{
		MaxHealth = 20;
		
		base._Ready();

		MovementSpeed = 4;

		EnemyHurtbox.ReceivedDamage += OnDamageReceived;
		DetectionArea.AreaEntered += OnDetectionAreaEntered;
		DetectionArea.AreaExited += OnDetectionAreaExited;

		StateMachine.ChangeState(StateMachine.GetNode<State>("Roam"));
	}

	public void OnDetectionAreaEntered(Area2D area)
	{
		//Add if enemy is looking towards player
		if(area is PlayerHurtbox && StateMachine.GetCurrentState() == "Roam")
		{
			StateMachine.ChangeState(StateMachine.GetNode<State>("Chase"));
		}
	}

	public void OnDetectionAreaExited(Area2D area)
	{
		if(area is PlayerHurtbox && StateMachine.GetCurrentState() == "Chase")
		{
			StateMachine.ChangeState(StateMachine.GetNode<State>("Roam"));
		}
	}

	private async void OnDamageReceived(DamageInfo dmgInfo)
	{	
		Vector2 knockback = dmgInfo.KnockbackForce;

		CurrentHealth -= dmgInfo.Amount;
		Core.Logger.Info($"Enemy health = {CurrentHealth}");
		
		DamageFlash();

		ApplyKnockback(dmgInfo.KnockbackForce);

		if(CurrentHealth <= 0)
		{
			Death();
		}
		await ToSignal(GetTree().CreateTimer(0.5f), SceneTreeTimer.SignalName.Timeout);
	}

	public void Death()
	{
		StateMachine.ChangeState(StateMachine.GetNode<State>("Dead"));
	}

	private void ApplyKnockback(Vector2 knockbackForce)
	{
		ActiveTween?.Kill();
		ActiveTween = CreateTween();
		
		EnemyMovement.TargetPosition = Position + knockbackForce;

		ActiveTween.TweenProperty(this, "position", EnemyMovement.TargetPosition, 0.15f)
				.SetTrans(Tween.TransitionType.Spring)
				.SetEase(Tween.EaseType.Out);
	}

	public void DamageFlash()
    {
        // Cancel any existing flash tween so it doesn't fight the new one
        _dmgFlashTween?.Kill();
        _dmgFlashTween = CreateTween();

        // 10x overbright dmgFlash (or use Colors.Red for a red tint)
        EnemyAnimation.Modulate = new Color(10, 10, 10, 1);

        // Tween back to normal over 0.15 seconds
        _dmgFlashTween.TweenProperty(EnemyAnimation, "modulate", Colors.White, 0.15f);
    }

}
