using Godot;

namespace Game.Gameplay;
public record struct DamageInfo
{
    public int Amount { get; init; }
    public Vector2 KnockbackForce { get; init; }
    public Node Attacker { get; init; }
}