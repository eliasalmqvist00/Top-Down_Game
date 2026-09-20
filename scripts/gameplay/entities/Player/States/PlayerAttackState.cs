using Godot;
using Game.Utilities;
using System.Collections.Generic;
using Game.Core;
using System;

namespace Game.Gameplay;
public partial class PlayerAttackState : State
{
	[ExportCategory("State Vars")]
	[Export] public Player Player;
	[Export] public PlayerAnimation PlayerAnimation;

	

	public override void EnterState()
    {
        base.EnterState();
    }

	public override void ExitState()
	{
		base.ExitState();
	}
}
