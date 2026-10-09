using Game.Gameplay;
using Godot;
using System;

public partial class PlayerHealthbar : ProgressBar
{
	[ExportCategory("Nodes")]
	[Export] public Player Player;
	
	public override void _Ready()
	{
		MaxValue = Player.MaxHealth;
		Value = Player.MaxHealth;
	}

	public override void _Process(double delta)
	{
		Value = Player.CurrentHealth;
	}
}
