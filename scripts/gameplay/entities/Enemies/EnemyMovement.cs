using System.Threading.Tasks;
using Game.Core;
using Game.Utilities;
using Godot;

namespace Game.Gameplay;

public partial class EnemyMovement : EntityMovement
{
	[ExportCategory("Nodes")]
	[Export] public Enemy Enemy;
	[Export] public Marker2D HitboxPivot;


	[ExportCategory("Enemy Patrol Area")]
	[Export] public Vector2 PatrolOrigin;
	[Export] public Vector2 PatrolRadius = new Vector2(64,64);


	public override void _Ready()
	{
		base._Ready();
	}
	
	public override void _Process(double delta)
	{
		HitboxPivot.Rotation = Enemy.Direction.Angle();
	}

	

}
