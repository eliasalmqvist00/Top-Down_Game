using Game.Core;
using Godot;

namespace Game.Gameplay;


public partial class SlingshotProjectileHitbox : Area2D, IHitbox
{   
	[ExportCategory("Nodes")]
	[Export] public SlingshotProjectile Projectile;
	
	public DamageInfo GetDamageInfo(Vector2 targetPosition)
	{   
		Vector2 knockback = Projectile.Direction * Projectile.KnockbackForce;

		return new DamageInfo
		{
			Amount = Projectile.Damage,
			KnockbackForce = knockback,
			Attacker = Owner,
			HitDirection = Projectile.Direction
		};
	}

}
