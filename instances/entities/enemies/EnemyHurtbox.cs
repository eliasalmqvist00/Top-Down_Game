using System;
using Godot;

namespace Game.Gameplay;

public partial class EnemyHurtbox : Area2D
{
    [ExportCategory("Nodes")]
    [Export] public Enemy Enemy;

	public event Action<DamageInfo> ReceivedDamage;

	public override void _Ready()
	{
		AreaEntered += OnAreaEntered;
	}

	private void OnAreaEntered(Area2D area)
	{
		if(area is PlayerHitbox hitbox && Enemy.IsAlive)
		{
			DamageInfo dmgInfo = hitbox.GetDamageInfo(Enemy.Position);
			ReceivedDamage?.Invoke(dmgInfo);
		}
	}

}
