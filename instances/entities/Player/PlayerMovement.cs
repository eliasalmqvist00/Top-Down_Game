using Godot;

namespace Game.Gameplay;
public partial class PlayerMovement : EntityMovement
{
	[ExportCategory("Player Inputs")]
	[Export] public Player Player;
	[Export] public PlayerAnimation PlayerAnimation;

	public override void _Ready()
	{
		base._Ready();
	}
	
}
