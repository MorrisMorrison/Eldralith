using Godot;
using System;
using System.Linq;

public partial class SceneManager : Node
{

	[Export]
	private PackedScene _playerScene;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GameManager.Players.Where(player => player.Id != 1)
							.Each((player, index) => SpawnPlayer(GetPlayerSpawnPoint(index), player));
	}

	public void SpawnPlayers()
	{
		GameManager.Players.Where(player => !player.IsSpawned)
							.Each(player => SpawnPlayer(GetPlayerSpawnPoint(), player));
		GameManager.Players.Each(player => player.IsSpawned = true);
	}

	private void SpawnPlayer(Node2D playerSpawnPoint, PlayerInfo player)
	{
		Player currentPlayer = _playerScene.Instantiate<Player>();
		currentPlayer.Name = player.Id.ToString();
		currentPlayer.SetupPlayer(player.Name);

		AddChild(currentPlayer);

		currentPlayer.GlobalPosition = ((Node2D) playerSpawnPoint).GlobalPosition;
	}

	private Node2D GetPlayerSpawnPoint(int index = 0)
	{
		return (Node2D)GetTree().GetNodesInGroup("PlayerSpawnPoints")[index];
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
