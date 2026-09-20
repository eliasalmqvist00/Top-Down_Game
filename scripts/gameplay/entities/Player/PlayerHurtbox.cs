using System;
using Godot;

namespace Game.Gameplay;

public partial class PlayerHurtbox : Area2D
{
	[ExportCategory("Nodes")]
	[Export] public Player Player;

	public event Action<DamageInfo> ReceivedDamage;

	public override void _Ready()
	{
		AreaEntered += OnAreaEntered;
	}

	private void OnAreaEntered(Area2D area)
	{
		if(area is EnemyHitbox hitbox)
		{
			DamageInfo dmgInfo = hitbox.GetDamageInfo(Player.Position);
			ReceivedDamage?.Invoke(dmgInfo);
		}
	}

}
