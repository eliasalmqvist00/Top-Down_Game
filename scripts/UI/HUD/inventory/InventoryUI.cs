using System;
using System.Collections.Generic;
using System.Linq;
using Game.Gameplay;
using Game.Utilities;
using Godot;


namespace Game.HUD;

public partial class InventoryUI : CanvasLayer
{
	private InventoryData _inventoryData;
	private readonly List<InventorySlot> _inventorySlots = new();

	[Export] public GridContainer InventoryGrid;

	[ExportCategory("Toolbar")]
	[Export] private CanvasLayer _toolbar;

	public override void _Ready()
	{
		Visible = false;

		foreach (Node child in InventoryGrid.GetChildren())
		{
			if (child is InventorySlot slot)
			{
				_inventorySlots.Add(slot);
			}
		}
	}

	public void InitializeInventoryUI()
	{
		_inventoryData.SlotUpdated += OnSlotUpdated;
		
		for(int i = 0; i < _inventorySlots.Count; i++)
		{
			_inventorySlots[i].SetItem(_inventoryData.Slots[i]);
			string item = "empty";
			if(_inventorySlots[i].SlotData.Item != null)
			{
				item = _inventorySlots[i].SlotData.Item.ItemName;
			}
			Core.Logger.Info($"Loaded itemUI at idx={i}, item = {item}");
		}
	}

	public void BindInventory(InventoryData inventoryData)
	{
		if(inventoryData == null) return;

		_inventoryData = inventoryData;
	}

	public void OnSlotUpdated(InventorySlotData slotData, int idx)
	{
		_inventorySlots[idx].SetItem(slotData);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if(@event.IsActionPressed("ui_cancel"))
		{
			Visible = !Visible;
			_toolbar.Visible = !Visible;

			if(!Visible) _inventoryData.Save(GameSavemanager.InventorySavePath);
		}	
	}

	public InventorySlot FindSlotWithItem(ItemData item)
	{
		if(item == null) return null;
		return _inventorySlots.FirstOrDefault(slot => slot.SlotData.Item == item);
	}

	private bool HasItem(ItemData item)
	{
		return FindSlotWithItem(item) != null;
	}


}
