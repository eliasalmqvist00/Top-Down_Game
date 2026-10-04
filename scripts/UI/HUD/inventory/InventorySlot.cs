using Game.Gameplay;
using Godot;

namespace Game.HUD;

public partial class InventorySlot : PanelContainer
{
	[ExportCategory("Nodes")]
	[Export] private TextureRect _icon;
	[Export] private Label _countLabel;

	[ExportCategory("SlotData")]
	[Export] public InventorySlotData SlotData { get; private set; }

	public override void _Ready()
	{
		Clear();
	}

	public void SetItem(InventorySlotData slotData)
	{
		SlotData = slotData;
		if(SlotData != null && SlotData.Item is ToolData)
		{
			_countLabel.Visible = false;
			_icon.Visible = true;
            _icon.Texture = SlotData.Item.ItemIcon;
		}
		else if (SlotData != null && SlotData.Item != null)
		{
			_icon.Texture = SlotData.Item.ItemIcon;
			_icon.Visible = true;
            _icon.Texture = SlotData.Item.ItemIcon;

			int itemCount = SlotData.ItemQuantity;
			_countLabel.Text = itemCount.ToString();
            _countLabel.Visible = true;
		}
		else
		{
			_icon.Texture = null;
			_icon.Visible = false;
			
			_countLabel.Text = "";
            _countLabel.Visible = false;
		}
	}

	public void Clear()
	{
		if (_icon != null)
		{
			_icon.Texture = null;
			_icon.Visible = false;
		}

		if (_countLabel != null)
		{
			_countLabel.Text = string.Empty;
			_countLabel.Visible = false;
		}
	}

	public override string ToString()
	{
		string item = "None";
		if(SlotData.Item != null) item = SlotData.Item.ItemName;

		return $"Item = {item}, Quantity = {SlotData.ItemQuantity}";
	}

}
