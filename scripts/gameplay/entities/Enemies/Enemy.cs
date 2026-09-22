using Godot;
using Game.Utilities;
using System;

namespace Game.Gameplay;

public partial class Enemy : Entity
{   
	[Signal] public delegate void AnimationEventHandler(string animationType);
	
	[ExportCategory("Nodes")]
	[Export] public Player Player;
	[Export] public EnemyMovement EnemyMovement;
	[Export] public EnemyHealthbar EnemyHealthbar;
	[Export] public EnemyHurtbox EnemyHurtbox;
	[Export] public EntityAnimation EnemyAnimation;
	[Export] public Area2D DetectionArea;
	[Export] public StateMachine StateMachine;


	[ExportCategory("Spawn-Point")]
	[Export] public Marker2D SpawnPoint;
	
	
	[ExportCategory("Enemy Vars")]
	[Export] public int AttackDamage = 4;



	//Tweens
	private Tween _knockbackTween;
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
		if(!IsAlive) return;

		Vector2 knockback = dmgInfo.KnockbackForce;

		CurrentHealth -= dmgInfo.Amount;
		Core.Logger.Info($"Enemy health = {CurrentHealth}");
		
		DamageFlash();

		if(CurrentHealth <= 0)
		{
			Death();
			return;
		}

		ApplyKnockback(dmgInfo.KnockbackForce);

		await ToSignal(GetTree().CreateTimer(0.8f), SceneTreeTimer.SignalName.Timeout);
	}

	public void Death()
	{
		if(!IsAlive) return;
		IsAlive = false;

		_knockbackTween.Kill();
		_dmgFlashTween.Kill();

		DisableAreaMonitoring();

		EnemyAnimation.Modulate = Colors.White;

		StateMachine.ChangeState(StateMachine.GetNode<State>("Dead"));
	}

	private void ApplyKnockback(Vector2 knockback)
	{
		_knockbackTween?.Kill();
		_knockbackTween = CreateTween();
		
		EnemyMovement.TargetPosition = Position + knockback;

		_knockbackTween.TweenProperty(this, "position", EnemyMovement.TargetPosition, 0.15f)
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

	private void DisableAreaMonitoring()
    {
        DetectionArea.SetDeferred(Area2D.PropertyName.Monitoring, false);
		DetectionArea.SetDeferred(Area2D.PropertyName.Monitorable, false);
		EnemyHurtbox.SetDeferred(Area2D.PropertyName.Monitoring, false);
		EnemyHurtbox.SetDeferred(Area2D.PropertyName.Monitorable, false);
		GetNode<Marker2D>("HitboxPivot").GetNode<EnemyHitbox>("Hitbox").SetDeferred(Area2D.PropertyName.Monitoring, false);
		GetNode<Marker2D>("HitboxPivot").GetNode<EnemyHitbox>("Hitbox").SetDeferred(Area2D.PropertyName.Monitorable, false);
		GetNode<Marker2D>("AttackRangePivot").GetNode<Area2D>("AttackRange").SetDeferred(Area2D.PropertyName.Monitoring, false);
		GetNode<Marker2D>("AttackRangePivot").GetNode<Area2D>("AttackRange").SetDeferred(Area2D.PropertyName.Monitorable, false);
    }

}
