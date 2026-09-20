using Godot;

namespace Game.Gameplay;


public partial class EnemyHitbox : Area2D
{   
	[ExportCategory("Nodes")]
	[Export] public Enemy Enemy;

	public bool HitboxEntered = false;
	
	public override void _Ready()
	{
		AreaEntered += OnAreaEntered;
		AreaExited += OnAreaExited;
	}

	public DamageInfo GetDamageInfo(Vector2 targetPosition)
	{   
		Vector2 knockbackDir = (targetPosition - GlobalPosition).Normalized();
		
		return new DamageInfo
		{
			Amount = Enemy.AttackDamage,
			KnockbackForce = new Vector2(0,0),
			Attacker = Owner
		};
	}

	private void OnAreaEntered(Area2D area)
	{
		if(area is PlayerHurtbox)
		{
			HitboxEntered = true;
		}
	}

	private void OnAreaExited(Area2D area)
	{
		if(area is PlayerHitbox hitbox)
		{
			HitboxEntered = false;
		}
	}

}
