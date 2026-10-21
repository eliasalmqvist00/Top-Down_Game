using Game.HUD;
using Godot;
using System;

public partial class HUD : Control
{
	[Export] public PackedScene InventoryScene;
	[Export] public PackedScene ToolbarScene;

	private InventoryUI _inventoryUI;
	private ToolbarUI _toolbarUI;

	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
