using System;
using Game.Core;
using Godot;

namespace Game.Gameplay;

public partial class EntityAnimation : AnimatedSprite2D
{		
	[ExportCategory("Nodes")]
	[Export] public Entity Entity;

	[ExportCategory("Animation Vars")]
	[Export] public ECharacterAnimation CurrentAnimation = ECharacterAnimation.idle_down;
	


	public void PlayWalk() => PlayAnimation("move");
	public void PlayIdle() => PlayAnimation("idle");
	public void PlayTurn() => PlayAnimation("turn");
	public void PlayAttack() => PlayAnimation("attack");
	public void PlayDeath() => Play("death");

	private void PlayAnimation(string animationPrefix)
	{
		string animation = GetDirectionalAnimation(animationPrefix).ToString();

		if(SpriteFrames.HasAnimation(animation))
		{
			if(Animation != animation || !IsPlaying())
			{
				Play(animation);
			}
		}
		else
		{
			Core.Logger.Error($"No animation named '{animation}' found...");
		}
	}

	private ECharacterAnimation GetDirectionalAnimation(string animationPrefix)
	{
		Vector2 CurrentDirection = Entity.Direction;

		bool isVertical = Mathf.Abs(CurrentDirection.Y) > Mathf.Abs(CurrentDirection.X);

		if(CurrentDirection.X < 0)
        {
            FlipH = true;
        }
        else
        {
            FlipH = false;
        }

		if (isVertical)
		{
			return CurrentDirection.Y > 0 
				? ParseAnimationEnum($"{animationPrefix}_down") 
				: ParseAnimationEnum($"{animationPrefix}_up");
			
		}
		else
		{
			return CurrentDirection.X > 0 
				? ParseAnimationEnum($"{animationPrefix}_right") 
				: ParseAnimationEnum($"{animationPrefix}_left");
		}
	}

	private ECharacterAnimation ParseAnimationEnum(string animName)
	{
		if (System.Enum.TryParse(animName, out ECharacterAnimation result))
		{
			return result;
		}
		return CurrentAnimation;
	}
}
