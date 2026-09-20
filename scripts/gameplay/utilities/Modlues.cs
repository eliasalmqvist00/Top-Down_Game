using Godot;
using System;
using System.Collections.Generic;

namespace Game.Gameplay;
public static class Modules
{
    public static bool IsActionJustPressed()
    {
        return Input.IsActionJustPressed("ui_up") || Input.IsActionJustPressed("ui_down") ||Input.IsActionJustPressed("ui_left") ||Input.IsActionJustPressed("ui_right");
    }

    public static bool IsActionPressed()
    {
        return Input.IsActionPressed("ui_up") || Input.IsActionPressed("ui_down") ||Input.IsActionPressed("ui_left") ||Input.IsActionPressed("ui_right");
    }

    public static bool IsActionJustReleased()
    {
        return Input.IsActionJustReleased("ui_up") || Input.IsActionJustReleased("ui_down") ||Input.IsActionJustReleased("ui_left") ||Input.IsActionJustReleased("ui_right");
    }

    public static bool IsAttackJustPressed()
    {
        return Input.IsActionJustPressed("ui_attack");
    }

    public static Vector2 GetDirectionVector(string directionName)
    {
        if (DirectionVector.TryGetValue(directionName, out Vector2I direction))
        {
            return direction;
        }

        GD.PrintErr($"Invalid direction string: '{directionName}'");
        return Vector2I.Zero;
    }

    public static readonly Dictionary<string, Vector2I> DirectionVector = new(StringComparer.OrdinalIgnoreCase)
    {
        { "North",      Vector2I.Up },
        { "South",      Vector2I.Down },
        { "West",       Vector2I.Left },
        { "East",       Vector2I.Right },
        { "North-East", new Vector2I(1, -1) },
        { "North-West", new Vector2I(-1, -1) },
        { "South-East", new Vector2I(1, 1) },
        { "South-West", new Vector2I(-1, 1) }
    };
}
