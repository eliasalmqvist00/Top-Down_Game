using Game.Gameplay;
using Godot;

namespace Game.HUD;

public partial class ToolbarSlot : PanelContainer
{
	[ExportCategory("Nodes")]
	[Export] private TextureRect _icon;
	[Export] private Label _countLabel;

	[ExportGroup("Slot Styles")]
	[Export] private StyleBox _normalStyle;
	[Export] private StyleBox _highlightStyle;

	[ExportCategory("Item")]
	[Export] public ItemData SlotItem { get; private set; }

	public int ItemCount { get; set; } = 0;

	public override void _Ready()
	{
		SetItem(SlotItem);
	}

	public void SetItem(ItemData item)
	{
		SlotItem = item;
		if(item != null && item is ToolData)
        {
            _countLabel.Visible = false;
        }
		
		if (item != null && item.ItemIcon != null)
		{
			_icon.Texture = item.ItemIcon;
			_icon.Visible = true;

			ItemCount = 1;
			_countLabel.Text = ItemCount.ToString();
		}
		else
		{
			_icon.Texture = null;
			_icon.Visible = false;
			
			ItemCount = 0;
			_countLabel.Text = "";
		}
	}

	public void SetSelected(bool isSelected)
	{
		StyleBox activeStyle = isSelected ? _highlightStyle : _normalStyle;
		if (activeStyle != null)
		{
			AddThemeStyleboxOverride("panel", activeStyle);
		}
		
	}

	public void UpdateCount()
    {
        ItemCount++;
		_countLabel.Text = ItemCount.ToString();
    }

}
