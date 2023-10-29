using Godot;
using System;
using System.Linq;

public partial class MultiplayerController : Control
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	private int port = 8910;
	[Export]
	private string ip = "127.0.0.1";

	private ENetMultiplayerPeer peer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Multiplayer.PeerConnected += PlayerConnected;
		Multiplayer.PeerDisconnected += PlayerDisconnected;
		Multiplayer.ConnectedToServer += ConnectionSuccessful;
		Multiplayer.ConnectionFailed += ConnectionFailed;
	}

	private void ConnectionFailed()
	{
		GD.Print("Connection failed.");
	}

	private void ConnectionSuccessful()
	{
		GD.Print("Connection Successful");
		GameManager.PlayerName = GetNode<LineEdit>("PlayerName").Text;
		RpcId(1, "SendPlayerInformation", GameManager.PlayerName, Multiplayer.GetUniqueId());
	}

	private void PlayerConnected(long id)
	{
		GD.Print(id);
	}

	private void PlayerDisconnected(long id)
	{
		GD.Print("Player Disconnected: " + id.ToString());
		GameManager.Players.Remove(GameManager.Players.Where(i => i.Id == id).First<PlayerInfo>());
		var players = GetTree().GetNodesInGroup("Player");
		
		foreach (var item in players)
		{
			if(item.Name == id.ToString()){
				item.QueueFree();
			}
		}
		GD.Print(id);
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public void _on_join_pressed()
	{
		GD.Print("join clicked.");
		peer = new ENetMultiplayerPeer();
		var status = peer.CreateClient(ip, port);
		if (status != Error.Ok)
		{
			GD.Print("Creating client failed");
			return;
		}

		peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
		Multiplayer.MultiplayerPeer = peer;

	}

	public void _on_start_game_pressed()
	{
		Rpc("StartGame");
	}


	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	private void StartGame()
	{
		var scene = ResourceLoader.Load<PackedScene>("res://world/world.tscn").Instantiate<SceneManager>();
		GetTree().Root.AddChild(scene);
		this.Hide();
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	public void SendPlayerInformation(string name, int id)
	{
		GD.Print("SendPlayerInformation");
		PlayerInfo playerInfo = new PlayerInfo()
		{
			Name = name,
			Id = id
		};

		if (!GameManager.Players.Any( p => p.Id == playerInfo.Id))
		{
			GameManager.Players.Add(playerInfo);
		}

		foreach (PlayerInfo player in GameManager.Players){
			GD.Print(player);
		}
	}
}
