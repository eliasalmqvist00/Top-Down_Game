using Godot;

namespace Game.Gameplay;


public partial class PlayerHitbox : Area2D
{   
	[ExportCategory("Nodes")]
	[Export] public Player Player;
	
	public DamageInfo GetDamageInfo(Vector2 targetPosition)
	{   
        Vector2 knockbackDir = (targetPosition - GlobalPosition).Normalized();
		return new DamageInfo
		{
			Amount = Player.AttackDamage,
			KnockbackForce = new Vector2(0,0),
			Attacker = Owner
		};
	}

}
