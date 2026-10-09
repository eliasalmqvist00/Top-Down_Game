using System;
using Godot;

namespace Game.Gameplay;

public static class GameEvents
{
    public static event Action<ItemData> OnActiveItemChanged;
    public static event Action<ItemData> OnItemPickedUp;

    public static void EmitActiveItemChanged(ItemData item)
    {
        OnActiveItemChanged?.Invoke(item);
    }

    public static void EmitItemPickedUp(ItemData item)
    {
        OnItemPickedUp?.Invoke(item);
    }


}