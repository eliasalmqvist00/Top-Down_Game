using System.Threading.Tasks;
using Game.Utilities;
using Godot;

namespace Game.Gameplay;

public partial class EnemyDeadState : State
{
	[Signal] public delegate void AnimationEventHandler(string animationType);

	[ExportCategory("Nodes")]
	[Export] public Enemy Enemy;


	private float SecondsPerTile = 0.7f;

	public override void _Ready()
	{
	}

	public override void EnterState()
	{
		base.EnterState();
		Death();
	}

	public async void Death()
	{   
		Enemy.EnemyHealthbar.Visible = false;

		Core.Logger.Debug("Enemy has died");
	
		Enemy.CollisionLayer = 2;
		Enemy.EnemyAnimation.PlayDeath();

		await ToSignal(GetTree().CreateTimer(10.0f), SceneTreeTimer.SignalName.Timeout);
		QueueFree();
	}

}
