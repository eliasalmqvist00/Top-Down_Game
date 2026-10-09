using Game.Core;
using Godot;

namespace Game.Gameplay;


public partial class PlayerHitbox : Area2D
{   
	[ExportCategory("Nodes")]
	[Export] public Player Player;
	
	public DamageInfo GetDamageInfo(Vector2 targetPosition)
	{   
        Vector2 knockback = Player.Direction * Globals.Instance.GRID_SIZE * Player.KnockbackForce;

		return new DamageInfo
		{
			Amount = Player.HeldItem.Damage,
			KnockbackForce = knockback,
			Attacker = Owner,
			HitDirection = Player.Direction
		};
	}

}
