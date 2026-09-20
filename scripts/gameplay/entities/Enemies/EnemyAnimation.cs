using Game.Utilities;
using Godot;

namespace Game.Gameplay;

public partial class EnemyAnimation : EntityAnimation
{
	[ExportCategory("Nodes")]
	[Export] public Enemy Enemy;
	[Export] public EnemyMovement EnemyMovement;
	[Export] public EnemyRoamState EnemyRoamState;
	[Export] public EnemyAttackState EnemyAttackState;

	public override void _Ready()
	{
		
	}
}
