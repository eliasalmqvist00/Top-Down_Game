using Game.Core;
using Godot;
using System;

namespace Game.Gameplay;

public partial class CharacterMovement : Node
{
	[Signal] public delegate void AnimationEventHandler(string animationType);

	[ExportCategory("Nodes")]
	[Export] public CharacterBody2D Character;

	[Export] public EntityAnimation CharacterAnimation;

	[ExportCategory("Movement")]
	[Export] public Vector2 TargetPosition = Vector2.Down;
	[Export] public bool IsWalking = false;
	[Export] public bool CollisionDetected = false;
	[Export] public float MoveSpeed = 4;

	private RectangleShape2D _queryShape;

	public override void _Ready()
	{
		_queryShape = new RectangleShape2D { Size = new Vector2(8,8) };
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Walk(delta);
	}

	public bool IsMoving()
	{
		return IsWalking;
	}

	public bool IsColliding()
	{
		return CollisionDetected;
	}

	protected virtual bool IsTargetOccupied(Vector2 targetWorldPosition)
	{
		var spaceState = Character.GetWorld2D().DirectSpaceState;

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

		if (Character is CollisionObject2D colObj)
		{
			query.Exclude = new Godot.Collections.Array<Rid> { colObj.GetRid() };
		}

		var result = spaceState.IntersectShape(query, 1);
		return result.Count > 0;
	}

	public virtual bool TryMove(Vector2 direction)
	{
		if (IsMoving() || direction == Vector2.Zero) return false;

		float grid = Globals.Instance.GRID_SIZE;

		// Ensure character is cleanly snapped before computing next cell
		Vector2 currentSnapped = new Vector2(
			Mathf.Round(Character.Position.X / grid) * grid,
			Mathf.Round(Character.Position.Y / grid) * grid
		);
		Character.Position = currentSnapped;

		Vector2 nextTarget = currentSnapped + (direction * grid);

		if (!IsTargetOccupied(nextTarget))
		{
			TargetPosition = nextTarget;
			EmitSignal(SignalName.Animation, "walk");
			IsWalking = true;
			return true;
		}
		else
		{
			Turn();
			return false;
		}
	}

	protected virtual void Walk(double delta)
	{
		if (IsWalking)
		{
			Character.Position = Character.Position.MoveToward(TargetPosition, (float)delta * Globals.Instance.GRID_SIZE * MoveSpeed);

			if(Character.Position.DistanceTo(TargetPosition) < 1f)
			{
				StopWalking();
			}
		}
		else
		{
			EmitSignal(SignalName.Animation, "idle");
		}
	}

	protected virtual void StopWalking()
	{
		Character.Position = TargetPosition;
		IsWalking = false;
		SnapPositionToGrid();
	}

	protected virtual void Turn()
	{
		EmitSignal(SignalName.Animation, "turn");
	}

	private void SnapPositionToGrid()
	{
		Character.Position = new Vector2(
			Mathf.Round(Character.Position.X / Globals.Instance.GRID_SIZE) * Globals.Instance.GRID_SIZE,
			Mathf.Round(Character.Position.Y / Globals.Instance.GRID_SIZE) * Globals.Instance.GRID_SIZE
		);
	}
}