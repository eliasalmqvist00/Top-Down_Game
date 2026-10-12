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

    public static bool IsSprintPressed()
    {
        return Input.IsActionPressed("ui_shift");
    }

    public static bool ScrollUp()
    {
        return Input.IsActionJustPressed("ui_scroll_up");
    }

    public static bool ScrollDown()
    {
        return Input.IsActionPressed("ui_scroll_down");
    }

    public static Vector2 GetMouseDirection(Node2D caller, Vector2 playerPosition)
	{
		Vector2 toMouse = (GetGlobalMousePosition(caller) - playerPosition).Normalized();

		return toMouse;
	}

    public static Vector2 GetQuantizedMouseDirection(Node2D caller, Vector2 playerPosition)
	{
		Vector2 toMouse = GetGlobalMousePosition(caller) - playerPosition;
		float angle = Mathf.PosMod(Mathf.RadToDeg(toMouse.Angle()), 360f);

		if(angle > 45 && angle <= 135)
		{
			return Vector2.Down;
		}
		else if(angle > 135 && angle <= 225)
		{
			return Vector2.Left;
		}
		else if(angle > 225 && angle <= 315)
		{
			return Vector2.Up;
		}
		else
		{
			return Vector2.Right; 
		}
	}

    public static Vector2 GetGlobalMousePosition(Node2D caller)
    {
        Vector2 mousePos = caller.GetGlobalMousePosition();
        return mousePos;
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
