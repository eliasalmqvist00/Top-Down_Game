using Game.Gameplay;
using Game.HUD;
using Godot;
using Game.Utilities;

namespace Game.UI;

public partial class HUD : Control
{
	[Export] public PackedScene InventoryScene;
	[Export] public PackedScene ToolbarScene;

	private InventoryUI _inventoryUI;
	private ToolbarUI _toolbarUI;

	public override void _Ready()
	{
		if (InventoryScene != null)
		{
			_inventoryUI = InventoryScene.Instantiate<InventoryUI>();
			AddChild(_inventoryUI);
			_inventoryUI.Hide();
		}

		if (ToolbarScene != null)
		{
			_toolbarUI = ToolbarScene.Instantiate<ToolbarUI>();
			AddChild(_toolbarUI);
		}
	}

	public void BindInventory()
    {
		InventoryData inventoryData = InventoryData.LoadOrCreate(GameSavemanager.InventorySavePath);

        _inventoryUI.BindInventory(inventoryData);
		_toolbarUI.BindInventory(inventoryData);

		_inventoryUI.InitializeInventoryUI();
		_toolbarUI.InitializeToolbar();
    }
}
