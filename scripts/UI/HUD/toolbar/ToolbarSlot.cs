using Game.Gameplay;
using Godot;

namespace Game.HUD;


public partial class ToolbarSlot : PanelContainer
{
	[Export] private TextureRect _icon;

	[ExportGroup("Slot Styles")]
	[Export] private StyleBox _normalStyle;
	[Export] private StyleBox _highlightStyle;

	[ExportGroup("Item")]
	[Export] public ItemData SlotItem { get; private set; }

	public override void _Ready()
	{
		SetItem(SlotItem);
	}

	public void SetItem(ItemData item)
	{
		SlotItem = item;
		
		if (item != null && item.ItemIcon != null)
		{
			_icon.Texture = item.ItemIcon;
			_icon.Visible = true;
		}
		else
		{
			_icon.Texture = null;
			_icon.Visible = false;
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

}
