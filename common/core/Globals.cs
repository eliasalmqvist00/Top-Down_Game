using Game.Gameplay;
using Godot;
using System;

namespace Game.Core
{
	public partial class Globals : Node
	{
		public static Globals Instance {get; private set;}
		
		[ExportCategory("Gameplay")]
		[Export] public int GRID_SIZE = 4;

		[ExportCategory("Player")]
		[Export] public Player Player {get; private set;}
		public override void _EnterTree()
		{
			Instance = this;
			Logger.Info("Loading Globals...");
		}

		public void RegisterPlayer(Player player)
        {
            Player = player;
        }
		public void UnegisterPlayer(Player player)
        {
            if (Player == player)
			{
				Player = null;
			}
        }
	} 
}
