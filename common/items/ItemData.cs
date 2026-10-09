using Godot;

namespace Game.Gameplay;

[GlobalClass]
public partial class ItemData : Resource
{
    [Export] public string ItemID { get; set; } = "";
    [Export] public string ItemName { get; set; } = "New Item";
    [Export] public Texture2D ItemIcon { get; set; }
    [Export] public bool IsConsumable { get; set; } = false;
    [Export] public int StackQuantity { get; set; } = 1;
    [Export] public int Damage {get; set;} = 0;

    public bool IsStackable => StackQuantity != 1;


}