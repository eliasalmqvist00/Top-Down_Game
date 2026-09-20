using Game.Utilities;
using Godot;
using System;

namespace Game.Gameplay;

public partial class Player : Entity
{
	[Export] public StateMachine StateMachine;

	[ExportCategory("Player Vars")]
	[Export] public double AttackCoolDown = 0.0;
	[Export] public int AttackDamage = 5;
	[Export] public int PlayerMovementSpeed = 5;

	public bool IsAttacking = false;
	
	public override void _Ready()
	{
		base._Ready();
		MovementSpeed = PlayerMovementSpeed;
		StateMachine.ChangeState(StateMachine.GetNode<State>("Roam"));
	}

	private void OnDamageReceived(DamageInfo dmgInfo)
	{
		CurrentHealth -= dmgInfo.Amount;
		Core.Logger.Info($"Enemy health = {CurrentHealth}");

		if(CurrentHealth <= 0)
		{
			OnDeath();
		}
        Position += dmgInfo.KnockbackForce;
	}

    public override void OnDeath()
    {
        throw new NotImplementedException();
    }
}
