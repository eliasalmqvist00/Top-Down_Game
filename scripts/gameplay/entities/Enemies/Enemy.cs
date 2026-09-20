using Godot;
using Game.Utilities;

namespace Game.Gameplay;

public partial class Enemy : Entity
{   
	[Signal] public delegate void AnimationEventHandler(string animationType);
	
	[ExportCategory("Nodes")]
	[Export] public Player Player;
	[Export] public EnemyHealthbarcs EnemyHealthbar;
	[Export] public EnemyHurtbox EnemyHurtbox;
	[Export] public EntityAnimation EnemyAnimation;
	[Export] public CollisionShape2D CollisionBox;
	[Export] public EnemyMovement EnemyMovement;
	[Export] public StateMachine StateMachine;
	
	
	[ExportCategory("Enemy Vars")]
	[Export] public int MaxHealth = 20;
	[Export] public int AttackDamage = 4;

	public bool IsAttacking = false;


	public Tween ActiveTween;

	public override void _Ready()
	{
		CurrentHealth = MaxHealth;
		EnemyHurtbox.ReceivedDamage += OnDamageReceived;

		StateMachine.ChangeState(StateMachine.GetNode<State>("Roam"));
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

	public async void Death()
	{
		StateMachine.ChangeState(StateMachine.GetNode<State>("Dead"));
	}

}
