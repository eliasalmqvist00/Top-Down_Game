using System.Threading.Tasks;
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
	[Export] public Area2D DetectionArea;


	[Export(PropertyHint.Layers2DPhysics)] public uint ObstacleCollisionMask = 1;

	private float SecondsPerTile = 0.7f;

	public override void _Ready()
	{
		BeginRoaming();
	}

    public override void EnterState()
    {
        base.EnterState();
		DetectionArea.AreaEntered += OnPlayerDetected;
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
		int steps = GD.RandRange(4,8);

		Vector2 randDir = GetRandomDirectionVector();
		Enemy.Direction = randDir;

		EnemyAnimation.PlayWalk();

		for(int i = 0; i <= steps; i++)
		{	
			Enemy.EnemyMovement.TargetPosition = Enemy.Position + Enemy.Direction * Core.Globals.Instance.GRID_SIZE;

			if(IsTargetOccupied(Enemy.EnemyMovement.TargetPosition)) return;
			
			Enemy.ActiveTween = CreateTween();
			Enemy.ActiveTween.SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.InOut);
			Enemy.ActiveTween.TweenProperty(Enemy, "position", Enemy.EnemyMovement.TargetPosition, SecondsPerTile);

			await ToSignal(Enemy.ActiveTween, Tween.SignalName.Finished);

			if (!Enemy.IsAlive|| !IsInstanceValid(Enemy)) return;
		}
	}

	protected bool IsTargetOccupied(Vector2 targetPosition)
	{
		if (CollisionShape?.Shape == null) return false;

		// 1. Access the 2D physics world state
		PhysicsDirectSpaceState2D spaceState = Enemy.GetWorld2D().DirectSpaceState;

		// 2. Configure the shape query parameters
		PhysicsShapeQueryParameters2D queryParams = new PhysicsShapeQueryParameters2D
		{
			Shape = CollisionShape.Shape,
			Transform = new Transform2D(0, targetPosition), // Place shape at target tile
			CollisionMask = ObstacleCollisionMask,
			Exclude = [Enemy.GetRid()] // Ignore the enemy itself
		};

		// 3. Test if any collision overlaps at that position
		// IntersectShape returns all colliders; empty means tile is free
		var results = spaceState.IntersectShape(queryParams, 1);
		return results.Count > 0;
	}

	public void OnPlayerDetected(Area2D area)
    {
        //Add if enemy is looking towards player
        if(area is PlayerHurtbox && Enemy.StateMachine.GetCurrentState() == "Roam")
        {
		    Enemy.StateMachine.ChangeState(Enemy.StateMachine.GetNode<State>("Attack"));
        }
    }

	private static readonly Vector2[] CardinalDirections = 
	[
		Vector2.Up,    // (0, -1)
		Vector2.Down,  // (0, 1)
		Vector2.Left,  // (-1, 0)
		Vector2.Right  // (1, 0)
	];

	public static Vector2 GetRandomDirectionVector()
	{
		int index = GD.RandRange(0, CardinalDirections.Length - 1);
		return CardinalDirections[index];
	}

	public override void ExitState()
	{
		base.ExitState();
		DetectionArea.AreaEntered -= OnPlayerDetected;
		Enemy.ActiveTween.Kill();
	}

}
