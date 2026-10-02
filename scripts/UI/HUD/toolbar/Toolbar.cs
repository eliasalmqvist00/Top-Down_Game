using System;
using System.Collections.Generic;
using System.Linq;
using Game.Gameplay;
using Godot;


namespace Game.HUD;

public partial class Toolbar :  HBoxContainer
{	
	private readonly List<ToolbarSlot> _slots = new();
	private ItemData _activeItem;
	public int MaxIdx = 10;
	public int MinIdx = 1;
	public int SelectedIdx;
	public override void _Ready()
	{
		GameEvents.OnItemPickedUp += AddItem;

		foreach (Node child in GetChildren())
		{
			if (child is ToolbarSlot slot)
			{
				_slots.Add(slot);
			}
		}
		SelectedIdx = 1;
		
		UpdateSelected();
	}

	public void AddItem(ItemData item)
	{
		ToolbarSlot slot = FindSlotWithItem(item);
		if(slot != null)
		{
			slot.UpdateCount();
			return;
		}
		slot = FindFirstEmptySlot();
		if(slot == null) return;
		slot.SetItem(item);
	}

	public ToolbarSlot FindFirstEmptySlot()
	{
		return _slots.FirstOrDefault(slot => slot.SlotItem == null);
	}

	public ToolbarSlot FindSlotWithItem(ItemData item)
	{
		if(item == null) return null;
		return _slots.FirstOrDefault(slot => slot.SlotItem == item);
	}

	private bool HasItem(ItemData item)
	{
		return FindSlotWithItem(item) != null;
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

	private void SelectNextItem()
	{
		SelectedIdx++;
		if(SelectedIdx > MaxIdx) SelectedIdx = 1;
		UpdateSelected();
	}

	private void SelectPreviousItem()
	{
		SelectedIdx--;
		if(SelectedIdx < MinIdx) SelectedIdx = 5;
		UpdateSelected();
	}
	
	private void UpdateSelected()
	{
		for(int i = MinIdx; i <= _slots.Count; i++)
		{
			_slots[i-1].SetSelected(SelectedIdx == i);
		}
		_activeItem = _slots[SelectedIdx-1].SlotItem;
		GameEvents.EmitActiveItemChanged(_activeItem);
	}

}
