using System.Linq;
using System.Threading.Tasks;
using Game.Core;
using Game.Utilities;
using Godot;

namespace Game.Gameplay;

public partial class EnemyRoamState : State
{
	[Signal] public delegate void AnimationEventHandler(string animationType);

	[ExportCategory("Nodes")]
	[Export] public Enemy Enemy;
	[Export] public EnemyMovement EnemyMovement;
	[Export] public EntityAnimation EnemyAnimation;
	[Export] public CollisionShape2D CollisionShape;

	[Export(PropertyHint.Layers2DPhysics)] public uint ObstacleCollisionMask = 1;

	private Tween _moveTween;
	private float SecondsPerTile = 0.7f;

    public override void EnterState()
    {
        base.EnterState();
		BeginRoaming();
		Enemy.MovementSpeed = 3;

		ReturnToOirigin();
    }

	public async void ReturnToOirigin()
    {
		while(Enemy.Position != Enemy.SpawnPoint.Position)
        {
            Enemy.Direction = (Enemy.SpawnPoint.Position - Enemy.Position).Normalized();
			EnemyMovement.TargetPosition = Enemy.Position + Enemy.Direction * Globals.Instance.GRID_SIZE;

			EnemyAnimation.PlayWalk();

			_moveTween?.Kill();
			_moveTween = CreateTween();
			_moveTween.SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.InOut);
			_moveTween.TweenProperty(Enemy, "position", EnemyMovement.TargetPosition, 1f / Enemy.MovementSpeed);

			await ToSignal(_moveTween, Tween.SignalName.Finished);

			EnemyMovement.SnapPositionToGrid();
			Core.Logger.Debug($"Enemy Position = {Enemy.Position}");
        }

    }

	public async void BeginRoaming()
	{	
		EnemyAnimation.PlayIdle();

		await ToSignal(GetTree().CreateTimer(1f), SceneTreeTimer.SignalName.Timeout);

		while (IsInstanceValid(this) && IsInstanceValid(Enemy))
		{
			await Roam();

			EnemyAnimation.PlayIdle();
			await ToSignal(GetTree().CreateTimer(1.5f), SceneTreeTimer.SignalName.Timeout);
		}
	}

	private async Task Roam()
	{	
		int steps = GD.RandRange(4,24);

		Vector2 randDir = GetRandomDirectionVector();
		Enemy.Direction = randDir;

		for(int i = 0; i <= steps; i++)
		{	
			EnemyMovement.TargetPosition = Enemy.Position + Enemy.Direction * Core.Globals.Instance.GRID_SIZE;

			EnemyAnimation.PlayWalk();

			if(IsTargetOccupied(Enemy.EnemyMovement.TargetPosition)) break;

			_moveTween?.Kill();
			_moveTween = CreateTween();
			_moveTween.SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.InOut);
			_moveTween.TweenProperty(Enemy, "position", EnemyMovement.TargetPosition, 1f / Enemy.MovementSpeed);

			await ToSignal(_moveTween, Tween.SignalName.Finished);

			EnemyMovement.SnapPositionToGrid();
		}
	}

	protected bool IsTargetOccupied(Vector2 targetPosition)
	{
		if (CollisionShape?.Shape == null) return false;

		PhysicsDirectSpaceState2D spaceState = Enemy.GetWorld2D().DirectSpaceState;

		PhysicsShapeQueryParameters2D queryParams = new PhysicsShapeQueryParameters2D
		{
			Shape = CollisionShape.Shape,
			Transform = new Transform2D(0, targetPosition), // Place shape at target tile
			CollisionMask = ObstacleCollisionMask,
			Exclude = [Enemy.GetRid()] // Ignore the enemy itself
		};

		var results = spaceState.IntersectShape(queryParams, 1);
		return results.Count > 0;
	}

	private static readonly Vector2[] CardinalDirections = 
	[
		Vector2.Up,    // (0, -1)
		Vector2.Down,  // (0, 1)
		Vector2.Left,  // (-1, 0)
		Vector2.Right,  // (1, 0)
		new Vector2I(1, -1),
        new Vector2I(-1, -1),
        new Vector2I(1, 1),
        new Vector2I(-1, 1)
	];

	public static Vector2 GetRandomDirectionVector()
	{	
		int index = GD.RandRange(0, CardinalDirections.Length - 1);
		return CardinalDirections[index];
	}

	public override void ExitState()
	{
		base.ExitState();
		_moveTween?.Kill();
	}

}
