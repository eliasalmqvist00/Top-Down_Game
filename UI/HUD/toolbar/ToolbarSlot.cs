using Game.Gameplay;
using Godot;

namespace Game.HUD;

public partial class ToolbarSlot : InventorySlot
{
	[ExportGroup("Slot Styles")]
	[Export] private StyleBox _normalStyle;
	[Export] private StyleBox _highlightStyle;

	public void SetSelected(bool isSelected)
	{
		StyleBox activeStyle = isSelected ? _highlightStyle : _normalStyle;
		if (activeStyle != null)
		{
			AddThemeStyleboxOverride("panel", activeStyle);
		}
		
	}

}
