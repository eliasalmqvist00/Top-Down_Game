using Godot;
using System;

namespace Game.Gameplay;

public interface IHitbox
{
	public DamageInfo GetDamageInfo(Vector2 targetPosition);
	
}
