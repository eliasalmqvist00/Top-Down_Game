using Godot;

namespace Game.Gameplay;

[GlobalClass]
public partial class ItemData : Resource
{
    [Export] public string ItemID { get; set; } = "";
    [Export] public string ItemName { get; set; } = "New Item";
    [Export] public Texture2D ItemIcon { get; set; }
    [Export] public bool IsConsumable { get; set; } = false;


}