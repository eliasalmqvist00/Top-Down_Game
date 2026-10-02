using Godot;

namespace Game.Gameplay;

[GlobalClass]
public partial class InventortSlotData : Resource
{
    [Export] public ItemData Item { get; set; }
    [Export] public int ItemQuantity { get; set; } = 0;

    public bool IsEmpty => Item == null || ItemQuantity <= 0;

    public void Clear()
    {
        Item = null;
        ItemQuantity = 0;
    }


}