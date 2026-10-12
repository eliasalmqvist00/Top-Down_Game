using System;
using Godot;

namespace Game.Gameplay;

public static class GameEvents
{
	public static event Action<ItemData> OnActiveItemChanged;
	public static event Action<ItemData> OnItemPickedUp;
	public static event Action<string, int> OnItemUsed;
	public static event Func<string, int> FindItemQuantity;

	public static void EmitActiveItemChanged(ItemData item)
	{
		OnActiveItemChanged?.Invoke(item);
	}

	public static void EmitItemPickedUp(ItemData item)
	{
		OnItemPickedUp?.Invoke(item);
	}

	public static void EmitItemUSed(string itemName, int quantity)
	{
		OnItemUsed?.Invoke(itemName, quantity);
	}

	public static int EmitFindItemAndQuantity(string itemName)
	{
		return FindItemQuantity.Invoke(itemName);
	}


}
