using Godot;

namespace Game.Gameplay;

public interface IGatherable
{
    RequiredTool RequiredTool {get;}

    void DamageTaken(Area2D area);
    void SpawnLoot(Vector2 position);
    void ClearObject();

}

public enum RequiredTool
{
    Axe,
    Pickaxe
}