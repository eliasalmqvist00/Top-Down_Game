using Godot;

namespace Game.Gameplay;

[GlobalClass]
public partial class ToolData : ItemData
{
    [Export] public int Damage {get; set;} = 0;

}