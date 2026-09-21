using System;
using System.Threading.Tasks;
using Game.Core;
using Game.Utilities;
using Godot;

namespace Game.Gameplay;

public partial class EnemyChaseState : State
{

	[ExportCategory("Nodes")]
	[Export] public Enemy Enemy;
	[Export] public EnemyMovement EnemyMovement;
	[Export] public Area2D AttackRange;
	

	public override void _Ready()
	{
		Enemy.MovementSpeed = 10;
	}

	public override void EnterState()
	{
		base.EnterState();
		AttackRange.AreaEntered += PlayerInRange;
	}

	public void PlayerInRange(Area2D area)
	{
		if(area is PlayerHurtbox)
		{
			StateMachine.ChangeState(StateMachine.GetNode<State>("Attack"));
		}
	}

	public override void _Process(double delta)
	{
		BeginChase();
	}

	private async void BeginChase()
	{
		await Chase();
	}

	public async Task Chase()
	{
		Enemy.EnemyAnimation.PlayWalk();
		
		Enemy.ActiveTween = CreateTween();
		Enemy.ActiveTween.SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.InOut);
		Enemy.ActiveTween.TweenProperty(Enemy, "position", GetTargetPlayerPosition(), 0.5f);

		await ToSignal(Enemy.ActiveTween, Tween.SignalName.Finished);

		EnemyMovement.SnapPositionToGrid();
		Enemy.MovementSpeed = 10;
		//EnemyMovement.TryMove(GetTargetPlayerPosition());
	}

	private Vector2 GetTargetPlayerPosition()
	{
		Vector2 targetDir = (Enemy.Player.Position - Enemy.Position).Normalized();
		Enemy.Direction = targetDir;

		Vector2 targetPos = Enemy.Position + targetDir * Globals.Instance.GRID_SIZE;

		return targetPos;
	}

	public override void ExitState()
	{
		base.ExitState();
		Enemy.ActiveTween.Kill();
		AttackRange.AreaEntered -= PlayerInRange;
	}

}
