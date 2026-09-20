using Game.Gameplay;
using Godot;
using System;

namespace Game.Gameplay;
public partial class EnemyHealthbarcs : ProgressBar
{
	[ExportCategory("Nodes")]
	[Export] public Enemy Enemy;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		MaxValue = Enemy.MaxHealth;
		Value = Enemy.MaxHealth;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Value = Enemy.CurrentHealth;
		
		if(Value <= 0)
		{
			Dispose();
		}
	}
}
