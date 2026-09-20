using Game.Utilities;
using Godot;
using System;

namespace Game.Gameplay;

public partial class Player : Entity
{
	[ExportCategory("Player Nodes")]
	[Export] public StateMachine StateMachine;
	[Export] public Marker2D HitboxPivot;

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

    public override void _Process(double delta)
    {
        HitboxPivot.Rotation = Direction.Angle() + Vector2.Up.Angle();
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
