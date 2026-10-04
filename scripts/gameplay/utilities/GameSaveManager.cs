using System;
using Game.Gameplay;
using Godot;

namespace Game.Utilities;

public static class GameSavemanager
{
    public static string InventorySavePath = "res://resources/UI/inventory/InventoryData.tres";


    public static event Action<string> SaveInventory;
    


}