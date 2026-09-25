using Godot;

namespace Game.Gameplay;

public partial class PlayerAnimation : EntityAnimation
{
	[ExportCategory("Nodes")]
	[Export] public PlayerAttack PlayerAttack;

	public bool IsAttacking = false;
	public override void _Ready()
	{
		base._Ready();

	}
	
	public void PlayChop() => PlayAnimation("chop");

}
