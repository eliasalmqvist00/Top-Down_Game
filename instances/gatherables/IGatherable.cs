using Godot;

namespace Game.Gameplay;

public interface IGatherable
{
    ToolData RequiredTool {get;}

    void DamageTaken();
    void SpawnLoot(Vector2 position);

}