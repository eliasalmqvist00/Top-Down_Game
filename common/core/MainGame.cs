using Game.Gameplay;
using Game.UI;
using Godot;
using System;

public partial class MainGame : Node
{
	[Export] public PackedScene InitialLevelScene { get; set; }
	[Export] public PackedScene PlayerScene { get; set; }

	private Node2D _levelRoot;
	private Node2D _entityRoot;
	private Node2D _fxRoot;
	private HUD _hudRoot;

	private Node2D _currentLevel;

	public override void _Ready()
	{
		_levelRoot = GetNode<Node2D>("%LevelRoot");
		_entityRoot = GetNode<Node2D>("%EntityRoot");
		_fxRoot = GetNode<Node2D>("%EffectRoot");
		_hudRoot = GetNode<HUD>("%HUDRoot");

		if (InitialLevelScene != null)
		{
			LoadLevel(InitialLevelScene);
		}

		if (PlayerScene != null)
		{
			SpawnPlayer();
		}

		LoadHUD();
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

	public void LoadHUD()
    {
        if(IsInstanceValid(_hudRoot))
        {
            _hudRoot.BindInventory();
        }
    }

}
