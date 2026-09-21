using Godot;
using Game.Utilities;
using System.Collections.Generic;
using Game.Core;
using System;

namespace Game.Gameplay;
public partial class PlayerRoamState : State
{
	[ExportCategory("State Vars")]
	[Export] public Player Player;
	[Export] public PlayerMovement PlayerMovement;
	[Export] public PlayerAnimation PlayerAnimation;

	[ExportCategory("Input Settings")]
	[Export] public double HoldThreshold = 0.15;
	private double HoldTime = 0.0;


	private static readonly StringName ActionUp = "ui_up";
	private static readonly StringName ActionDown = "ui_down";
	private static readonly StringName ActionLeft = "ui_left";
	private static readonly StringName ActionRight = "ui_right";

	// Tracks held directional inputs in the order they were pressed
	private readonly List<StringName> ActiveInputs = new();

	public override void _Process(double delta)
	{
		// 1. Attack priority check
		if (Modules.IsAttackJustPressed())
		{
			StateMachine.ChangeState(StateMachine.GetNode<State>("Attack"));
			return;
		}
		// 2. Movement handling
		GetInput(delta);
	}

	private void GetInput(double delta)
	{

		if(PlayerMovement.IsCurrentlyMoving())
        {
            return;
        }

		if (ActiveInputs.Count == 0)
		{
			HoldTime = 0;
			PlayerAnimation.PlayIdle();
			return;
		}
		Player.Direction =  GetInputDirection();

		if (Modules.IsActionPressed())
		{
			HoldTime += delta;

			if (HoldTime > HoldThreshold)
			{
				bool canMove = PlayerMovement.TryMove(Player.Direction);

			}
			else
            {
                PlayerAnimation.PlayTurn();
            }
		}
		else
		{
			PlayerAnimation.PlayIdle();
		}
	}

	private void OnStepStarted()
    {
        PlayerAnimation.PlayWalk();
    }

    private void OnStepFinished()
    {
        // Check if player is still holding a direction to chain steps smoothly
        if (Player.Direction != Vector2.Zero)
        {
            PlayerAnimation.PlayWalk();
        }
        else
        {
            PlayerAnimation.PlayIdle();
        }
    }

    private void OnStepBlocked()
    {
        PlayerAnimation.PlayIdle();
    }

	private Vector2 GetInputDirection()
	{
		if (ActiveInputs.Count == 0)
		{
			return Vector2.Zero;
		}

		bool isUp = ActiveInputs.Contains(ActionUp);
		bool isDown = ActiveInputs.Contains(ActionDown);
		bool isLeft = ActiveInputs.Contains(ActionLeft);
		bool isRight = ActiveInputs.Contains(ActionRight);

		// Cancel opposing inputs on the same axis (holding W + S cancels out)
		int x = 0;
		if (isRight && !isLeft) x = 1;
		else if (isLeft && !isRight) x = -1;

		int y = 0;
		if (isDown && !isUp) y = 1;
		else if (isUp && !isDown) y = -1;

		// Determine the direction string based on combined axes
		string directionName = (x, y) switch
		{
			( 0, -1) => "North",
			( 0,  1) => "South",
			(-1,  0) => "West",
			( 1,  0) => "East",
			( 1, -1) => "North-East",
			(-1, -1) => "North-West",
			( 1,  1) => "South-East",
			(-1,  1) => "South-West",
			_        => null
		};

		if (directionName == null)
		{
			// Inputs canceled each other out (e.g. holding both Left and Right)
			HoldTime = 0.0;
			return Vector2.Zero;
		}

		return Modules.GetDirectionVector(directionName);
	}


	public override void _UnhandledInput(InputEvent @event)
	{
		UpdateInputStack(ActionUp);
		UpdateInputStack(ActionDown);
		UpdateInputStack(ActionLeft);
		UpdateInputStack(ActionRight);
	}

	private void UpdateInputStack(StringName action)
	{
		if (Input.IsActionJustPressed(action))
		{
			if (!ActiveInputs.Contains(action))
			{
				ActiveInputs.Add(action);
			}
		}
		else if (Input.IsActionJustReleased(action))
		{
			ActiveInputs.Remove(action);
		}
	}

	public override void EnterState()
    {
        base.EnterState();

        // Connect signals only while in this state
        PlayerMovement.StepStarted += OnStepStarted;
        PlayerMovement.StepFinished += OnStepFinished;
        PlayerMovement.StepBlocked += OnStepBlocked;

        HoldTime = 0.0;
        PlayerAnimation.PlayIdle();
    }

	public override void ExitState()
	{
		base.ExitState();
		ActiveInputs.Clear();

        // Disconnectonnect signals only while not in this state
		PlayerMovement.StepStarted -= OnStepStarted;
        PlayerMovement.StepFinished -= OnStepFinished;
        PlayerMovement.StepBlocked -= OnStepBlocked;

		HoldTime = 0.0f;
	}
}
