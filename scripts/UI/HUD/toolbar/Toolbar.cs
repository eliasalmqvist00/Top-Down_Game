using System;
using System.Collections.Generic;
using System.Linq;
using Game.Gameplay;
using Godot;

namespace Game.HUD;

public partial class Toolbar :  CanvasLayer
{	
	[Export] public HBoxContainer ToolbarSlotContainer;
	private InventoryData _inventoryData;
	private readonly List<ToolbarSlot> _toolbarSlots = new();

	private ItemData _activeItem;
	public int MaxIdx = 10;
	public int MinIdx = 1;
	public int SelectedIdx;
	public override void _Ready()
	{
        _toolbarSlots.Clear();
		foreach (Node child in ToolbarSlotContainer.GetChildren())
		{
			if (child is ToolbarSlot slot)
			{
				_toolbarSlots.Add(slot);
			}
		}
		SelectedIdx = 1;
	}

    public void BindInventory(InventoryData inventoryData)
    {
        if(inventoryData == null) return;
        
        _inventoryData = inventoryData;
    }

    public void InitializeToolbar()
    {
        _inventoryData.SlotUpdated += OnSlotUpdated;
        for(int i = 0; i < _toolbarSlots.Count; i++)
        {
            _toolbarSlots[i].SetItem(_inventoryData.Slots[i]);
        }
        UpdateSelected();
    }

	public void OnSlotUpdated(InventorySlotData slotData, int idx)
	{
		_toolbarSlots[idx].SetItem(slotData);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if(@event.IsActionPressed("ui_scroll_up"))
		{
			SelectPreviousItem();
		}
		else if(@event.IsActionPressed("ui_scroll_down"))
		{
			SelectNextItem();
		}
	}

	public ItemData GetCurrentItem()
	{
		return _activeItem;
	}	

	private void SelectNextItem()
	{
		SelectedIdx++;
		if(SelectedIdx > MaxIdx) SelectedIdx = MinIdx;
		UpdateSelected();
	}

	private void SelectPreviousItem()
	{
		SelectedIdx--;
		if(SelectedIdx < MinIdx) SelectedIdx = MaxIdx;
		UpdateSelected();
	}
	
	private void UpdateSelected()
	{
		if (_toolbarSlots.Count == 0) return;

        for (int i = 0; i < _toolbarSlots.Count; i++)
        {
            _toolbarSlots[i].SetSelected(SelectedIdx == (i + 1));
        }

        int targetIndex = SelectedIdx - 1;
        if (targetIndex >= 0 && targetIndex < _toolbarSlots.Count)
        {
            _activeItem = _toolbarSlots[targetIndex].SlotData?.Item;
        }
        else
        {
            _activeItem = null;
        }
		GameEvents.EmitActiveItemChanged(_activeItem);
	}

}
