using Godot;

namespace Game.Gameplay;


public partial class EnemyHitbox : Area2D
{   
	[ExportCategory("Nodes")]
	[Export] public Enemy Enemy;
	[Export] public CollisionShape2D HitboxShape;
	

	public DamageInfo GetDamageInfo(Vector2 targetPosition)
	{   
		Vector2 knockbackDir = (targetPosition - GlobalPosition).Normalized();
		
		return new DamageInfo
		{
			Amount = Enemy.AttackDamage,
			KnockbackForce = new Vector2(0,0),
			Attacker = Owner,
			HitDirection = Enemy.Direction
		};
	}

}
