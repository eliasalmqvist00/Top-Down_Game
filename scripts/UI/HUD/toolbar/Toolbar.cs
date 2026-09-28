using System;
using System.Collections.Generic;
using Game.Gameplay;
using Godot;


namespace Game.HUD;

public partial class Toolbar :  HBoxContainer
{	
	private readonly List<ToolbarSlot> _slots = new();
	private ItemData _activeItem;
	public int MaxIdx = 5;
	public int MinIdx = 1;
	public int SelectedIdx;
	public override void _Ready()
	{
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
