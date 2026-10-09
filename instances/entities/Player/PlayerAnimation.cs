using Godot;

namespace Game.Gameplay;

public partial class PlayerAnimation : EntityAnimation
{

	public bool IsAttacking = false;
	public override void _Ready()
	{
		base._Ready();

	}
	
	public void PlayChop() => PlayAnimation("axe");

}
