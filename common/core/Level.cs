using Godot;
using System;

public partial class Level : Node2D
{
	[Export] public Marker2D PlayerSpawnPoint;
	
	public override void _Ready()
	{
	}

	public Vector2 GetPlayerSpawnPoint()
	{
		return PlayerSpawnPoint.GlobalPosition;
	}
	
}
