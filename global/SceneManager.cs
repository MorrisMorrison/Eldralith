using Godot;
using System;

public partial class SceneManager : Node
{

	[Export]
	private PackedScene playerScene;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		int index = 0;
		foreach (PlayerInfo player in GameManager.Players)
		{
			Player currentPlayer = playerScene.Instantiate<Player>();
			currentPlayer.Name = player.Id.ToString();
			AddChild(currentPlayer);

			foreach (Node2D playerSpawnPoint in GetTree().GetNodesInGroup("PlayerSpawnPoints"))
			{
				GD.Print("PlayerSpawnPoint");
				if (int.Parse(playerSpawnPoint.Name) == index)
				{
					GD.Print("PlayerSpawnPoint -> set global pos");
					currentPlayer.GlobalPosition = playerSpawnPoint.GlobalPosition;
				}
			}
			index++;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
