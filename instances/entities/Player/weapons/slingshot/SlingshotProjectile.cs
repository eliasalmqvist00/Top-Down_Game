using Game.Gameplay;
using Godot;
using System;

public partial class SlingshotProjectile : StaticBody2D
{
	[ExportCategory("Slingshot Projectile Nodes")]
	[Export] public Area2D Hitbox;
	[Export] public Timer DespawnTimer;
	//[Export] public CollisionShape2D CollisionShape;

	[ExportGroup("Slingshot Projectile Vars")]
	[Export] public int Damage = 0;
	[Export] public int KnockbackForce = 10;
	[Export] private int _speed = 1000;

	public Vector2 Direction;

	public override void _Ready()
	{
		Hitbox.AreaEntered += EnemyHit;
		DespawnTimer.Timeout += OnTimeout;

		DespawnTimer.Start();
	}

	public void Instialize(Vector2 spawnPosition, Vector2 fireDirection)
	{
		GlobalPosition = spawnPosition;

		Direction = fireDirection;
		Rotation = Direction.Angle();
	}

    public override void _PhysicsProcess(double delta)
    {
        GlobalPosition += Direction * _speed * (float)delta;
    }

	public void OnTimeout()
    {
        ClearProjectile();
    }

	public void EnemyHit(Area2D area)
	{
		if(area is EnemyHurtbox hurtbox)
		{
			DespawnTimer.Stop();
			ClearProjectile();
		}
	}

	private void ClearProjectile()
	{
		Hitbox.AreaEntered -= EnemyHit;
		DespawnTimer.Timeout -= OnTimeout;
		QueueFree();
	}
}
