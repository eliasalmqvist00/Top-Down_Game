using Godot;
using Game.Utilities;
using System;

namespace Game.Gameplay;

public partial class Enemy : Entity
{   
	[Signal] public delegate void AnimationEventHandler(string animationType);
	
	[ExportCategory("Nodes")]
	[Export] public Player Player;
	[Export] public EnemyHealthbarcs EnemyHealthbar;
	[Export] public EnemyHurtbox EnemyHurtbox;
	[Export] public EntityAnimation EnemyAnimation;
	[Export] public EnemyMovement EnemyMovement;
	[Export] public Area2D DetectionArea;
	[Export] public StateMachine StateMachine;
	
	
	[ExportCategory("Enemy Vars")]
	[Export] public int MaxHealth = 20;
	[Export] public int AttackDamage = 4;


	public Tween ActiveTween;

	public override void _Ready()
	{
		CurrentHealth = MaxHealth;
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

	private void OnDamageReceived(DamageInfo dmgInfo)
	{
		CurrentHealth -= dmgInfo.Amount;
		Core.Logger.Info($"Enemy health = {CurrentHealth}");
		
		SelfModulate = new Color(1.839f, 0.121f, 0.227f);

		//TODO : Implement knockback

		if(CurrentHealth <= 0)
		{
			Death();
		}
	}

	public void Death()
	{
		StateMachine.ChangeState(StateMachine.GetNode<State>("Dead"));
	}

}
