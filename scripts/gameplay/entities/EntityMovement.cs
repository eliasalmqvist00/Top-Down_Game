using Game.Core;
using Godot;
using System;

namespace Game.Gameplay;

public partial class EntityMovement : Node
{
	[Signal] public delegate void StepStartedEventHandler();
    [Signal] public delegate void StepFinishedEventHandler();
    [Signal] public delegate void StepBlockedEventHandler();

	[ExportCategory("Entity Nodes")]
	[Export] public Entity Entity;


	// Class variables
    public bool IsMoving { get; private set; } = false;
    public Vector2 TargetPosition;
	private float StepDuration = 0.3f;

	private Tween MoveTween;
	private RectangleShape2D _queryShape;

	public override void _Ready()
	{
		TargetPosition = Entity.Position;
		_queryShape = new RectangleShape2D { Size = new Vector2(8,8) };
	}

	public virtual bool TryMove(Vector2 direction, float gridSize)
	{
		// Don't interrupt an existing step
		if (IsMoving || direction == Vector2.Zero) return false;

		TargetPosition = Entity.Position + direction * gridSize;

		IsMoving = true;
        EmitSignal(SignalName.StepStarted);

		//if(IsTargetOccupied(TargetPosition)) return false;

		MoveTween?.Kill();
		MoveTween = CreateTween();

		float stepSpeed;

		if(direction.Length() != 1)
        {
            stepSpeed = (float)(StepDuration * Math.Sqrt(2) / Entity.MovementSpeed);
        }
        else
        {
            stepSpeed = (float)(StepDuration / Entity.MovementSpeed);
        }

		MoveTween.TweenProperty(Entity, "position", TargetPosition, stepSpeed)
                  .SetTrans(Tween.TransitionType.Linear);

		MoveTween.Finished += OnTweenFinished;
		return true;
	}

	private void OnTweenFinished()
    {
        Entity.Position = TargetPosition;
        IsMoving = false;
        EmitSignal(SignalName.StepFinished);
    }

    protected virtual bool IsTargetOccupied(Vector2 targetWorldPosition)
	{
		var spaceState = Entity.GetWorld2D().DirectSpaceState;

		// targetWorldPosition IS ALREADY the center of the entity
		Vector2 checkCenter = targetWorldPosition;

		var query = new PhysicsShapeQueryParameters2D
		{
			ShapeRid = _queryShape.GetRid(),
			Transform = new Transform2D(0, checkCenter),
			CollisionMask = 1,
			CollideWithAreas = true,
			CollideWithBodies = true
		};

		if (Entity is CollisionObject2D colObj)
		{
			query.Exclude = new Godot.Collections.Array<Rid> { colObj.GetRid() };
		}

		var result = spaceState.IntersectShape(query, 1);
		return result.Count > 0;
	}

	public virtual void StopWalking()
	{	
		Core.Logger.Debug("Walking stopped");
		Entity.Position = TargetPosition;
        IsMoving = false;
		SnapPositionToGrid();
		EmitSignal(SignalName.StepFinished);
	}

	private void SnapPositionToGrid()
	{
		Entity.Position = new Vector2(
			Mathf.Round(Entity.Position.X / Globals.Instance.GRID_SIZE) * Globals.Instance.GRID_SIZE,
			Mathf.Round(Entity.Position.Y / Globals.Instance.GRID_SIZE) * Globals.Instance.GRID_SIZE
		);
	}

    public virtual bool IsCurrentlyMoving() => IsMoving;

    public virtual Vector2 GetTargetPosition() => TargetPosition;

    public virtual void SetTargetPosition(Vector2 newTargetPosition)
    {
        TargetPosition = newTargetPosition;
    }
}