using Godot;
using System;
using System.Linq;

public partial class MultiplayerController : Control
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	private int _port = 8910;
	[Export]
	private string _ip = "127.0.0.1";

	private ENetMultiplayerPeer _peer;
	private int _playerId;
	private SceneManager _scene;

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
		GDPrint.Print("Connection FAILED.");
		GDPrint.Print("Could not connect to server.");
	}

	private void ConnectionSuccessful()
	{
		GDPrint.Print("Connection SUCCESSFULL");

		_playerId = Multiplayer.GetUniqueId();
		GameManager.PlayerName = GetNode<LineEdit>("PlayerName").Text;
		
		GDPrint.Print(_playerId, "Sending player information to server.");
		GDPrint.Print(_playerId, $"Id: {_playerId} - Name: {GameManager.PlayerName}");
		
		RpcId(1, "SendPlayerInformation", GameManager.PlayerName, _playerId);
	}

	private void PlayerConnected(long id)
	{
		GDPrint.Print(_playerId, $"Player <{id}> connected.");
	}

	private void PlayerDisconnected(long id)
	{
		GDPrint.Print(_playerId, $"Player <${id}> disconnected.");
		GameManager.Players.Remove(GameManager.Players.FirstOrDefault(i => i.Id == id));
		var players = GetTree().GetNodesInGroup("Player");
		
		foreach (var item in players)
		{
			if(item.Name == id.ToString()){
				item.QueueFree();
			}
		}
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public void _on_join_pressed()
	{
		_peer = new ENetMultiplayerPeer();
		var status = _peer.CreateClient(_ip, _port);
		if (status != Error.Ok)
		{
			GDPrint.PrintErr("Creating client FAILED.");
			return;
		}

		_peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
		Multiplayer.MultiplayerPeer = _peer;

		StartGame();
	}

	private void StartGame()
	{
		GDPrint.Print(_playerId, "Starting game.");
		_scene = ResourceLoader.Load<PackedScene>("res://world/world.tscn").Instantiate<SceneManager>();
		GetTree().Root.AddChild(_scene);
		this.Hide();
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	public void SendPlayerInformation(string name, int id)
	{
		if (id == 1) return;

		PlayerInfo playerInfo = new PlayerInfo()
		{
			Name = name,
			Id = id
		};

		if (GameManager.Players.Any(player => player.Id == id)) return;

		GameManager.Players.Add(playerInfo);
		_scene?.SpawnPlayers();
	}
}
