using System;
using Godot;

namespace Game.Gameplay;

public static class GameEvents
{
    public static event Action<ItemData> OnActiveItemChanged;

    public static void EmitActiveItemChanged(ItemData item)
    {
        OnActiveItemChanged?.Invoke(item);
    }


}