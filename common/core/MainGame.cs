using Game.Gameplay;
using Godot;
using System;

public partial class MainGame : Node
{
	[Export] public PackedScene InitialLevelScene { get; set; }
	[Export] public PackedScene PlayerScene { get; set; }

	private Node2D _levelRoot;
	private Node2D _entityRoot;
	private Control _hudRoot;

	private Node2D _currentLevel;

	public override void _Ready()
	{
		return;
		
		_levelRoot = GetNode<Node2D>("%LevelRoot");
		_entityRoot = GetNode<Node2D>("%EntityRoot");
		_hudRoot = GetNode<Control>("%HUDRoot");

		
		if (InitialLevelScene != null)
		{
			LoadLevel(InitialLevelScene);
		}

		if (PlayerScene != null)
		{
			SpawnPlayer();
		}
	}

	public void LoadLevel(PackedScene levelScene)
	{
		if(IsInstanceValid(_currentLevel))
		{
			_currentLevel.QueueFree();
			_currentLevel = null;
		}
		_currentLevel = levelScene.Instantiate<Node2D>();
		_levelRoot.AddChild(_currentLevel);
	}

	public void SpawnPlayer()
	{
		Player player = PlayerScene.Instantiate<Player>();
		_entityRoot.AddChild(player);

		
	}

}
