using Godot;

namespace Game.Gameplay;

public partial class Item : Node
{
    
    private string _itemName;
    private int _itemId;
    private Texture2D _itemIcon;

    public Item(string itemName, int itemId)
    {
        _itemName = itemName;
        _itemId = itemId;
    }



}