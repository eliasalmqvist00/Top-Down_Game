using System;
using Game.Gameplay;
using Godot;


namespace Game.HUD;

public partial class Toolbar :  PanelContainer
{
	public event Action<ItemData, int> ItemSelected;
	
	public ToolbarSlot SelectedItem;

	
	public void SelectNext()
	{
		
	}

	public void SelectPrevious()
	{
		
	}


}
