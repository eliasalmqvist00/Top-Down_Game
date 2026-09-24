using Godot;

namespace Game.Gameplay;

public partial class ToolData : ItemData
{
    [Export] public int Damage {get; set;} = 0;

}