using System.Collections.Generic;
using System.Linq;
using Game.Gameplay;
using Godot;


namespace Game.HUD;

public partial class Inventory : CanvasLayer
{
	[ExportCategory("Toolbar")]
	[Export] private CanvasLayer _toolbar;

	private readonly List<ToolbarSlot> _inventorySlots = new();
	private bool _inventoryFull;

	public override void _Ready()
	{
		Visible = false;
		
		foreach (Node child in GetChildren())
		{
			if (child is ToolbarSlot slot)
			{
				_inventorySlots.Add(slot);
			}
		}

		_inventoryFull = false;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if(@event.IsActionPressed("ui_cancel"))
		{
			Visible = !Visible;
			_toolbar.Visible = !Visible;
		}	
	}

	public void AddItem(ItemData item)
	{
		if(_inventoryFull) return;

		// check if item exists in invetory -> place there
		// if(_inventorySlots.Contains())
		// {
		int idx = 0;
		foreach(ToolbarSlot slot in _inventorySlots)
		{
			if(slot.SlotItem == item)
			{
				slot.UpdateCount();
				return;
			}
			idx++;
		}

		// }
		// otherwise
		// add item at first free spot in inventory
		int freeIdx = 0;

		_inventorySlots[freeIdx].SetItem(item);

		UpdateInventory();
	}

	private void UpdateInventory()
	{
		if(!HasItem(null)) _inventoryFull = true;



	}

	public ToolbarSlot FindSlotWithItem(ItemData item)
	{
		if(item == null) return null;
		return _inventorySlots.FirstOrDefault(slot => slot.SlotItem == item);
	}

	private bool HasItem(ItemData item)
	{
		return FindSlotWithItem(item) != null;
	}


}
