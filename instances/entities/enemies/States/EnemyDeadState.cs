using System.Threading.Tasks;
using Game.Utilities;
using Godot;

namespace Game.Gameplay;

public partial class EnemyDeadState : State
{

	[ExportCategory("Nodes")]
	[Export] public Enemy Enemy;

	public override void EnterState()
	{
		base.EnterState();
		Death();
	}

	public async void Death()
	{   
		Enemy.EnemyHealthbar.Visible = false;

		Enemy.EnemyAnimation.PlayDeath();
		await ToSignal(Enemy.EnemyAnimation, AnimatedSprite2D.SignalName.AnimationFinished);

		Enemy.CollisionLayer = 2;
		Enemy.EnemyHurtbox.Dispose();

		await ToSignal(GetTree().CreateTimer(5.0f), SceneTreeTimer.SignalName.Timeout);
		Enemy.QueueFree();
	}

}
