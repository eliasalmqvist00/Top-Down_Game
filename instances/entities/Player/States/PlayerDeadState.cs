using Godot;
using Game.Utilities;

namespace Game.Gameplay;
public partial class PlayerDeadState : State
{
	[ExportCategory("State Vars")]
	[Export] public Player Player;
	[Export] public PlayerAnimation PlayerAnimation;

	public override void EnterState()
	{
		base.EnterState();

		Player.PlayerHurtbox.Dispose();
		PlayerAnimation.PlayDeath();

		Core.Logger.Debug("Death animation played...");
	}

	public override void ExitState()
	{
		base.ExitState();
	}
}
