using Game.Gameplay;
using Godot;

namespace Game.HUD;


public partial class ToolbarSlot : PanelContainer
{
    private Sprite2D _itemSprite;
    private ItemData _slotItem;

    public ToolbarSlot()
    {
        
    }

    public Sprite2D GetItemSprite()
    {
        return _itemSprite;
    }

}
