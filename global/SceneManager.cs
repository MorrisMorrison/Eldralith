using Godot;
using System;

public partial class SceneManager : Node
{

	[Export]
	private PackedScene _playerScene;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GameManager.Players.Each((player, index) => {
			Player currentPlayer = _playerScene.Instantiate<Player>();
			currentPlayer.Name = player.Id.ToString();
			currentPlayer.SetupPlayer(player.Name);
			AddChild(currentPlayer);

			GetTree().GetNodesInGroup("PlayerSpawnPoints").Each(playerSpawnPoint => {
				if (int.Parse(playerSpawnPoint.Name) != index) return;
					
				currentPlayer.GlobalPosition = ((Node2D) playerSpawnPoint).GlobalPosition;
			});
		});
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
