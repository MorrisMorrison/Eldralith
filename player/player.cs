using Godot;
using System;

public partial class Player : CharacterBody2D
{
	private const float _speed = 200.0f;
	private const float _jumpVelocity = -400.0f;
	private Vector2 _syncPos = new(0, 0);
	private float _cameraFollowSpeed = 10f;

	private AnimationPlayer _playerAnimation;
	private AnimatedSprite2D _playerSprite;
	private Camera2D _playerCamera;
	private MultiplayerSynchronizer _multiplayerSynchronizer;


	public override void _Ready()
	{
		_playerAnimation = GetNode<AnimationPlayer>("AnimationPlayer");
		_playerSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		_multiplayerSynchronizer = GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer");
		_multiplayerSynchronizer.SetMultiplayerAuthority(int.Parse(Name));
		SetupPlayerCamera();
	}

	public override void _PhysicsProcess(double delta)
	{
		if (IsCurrentPlayer())
		{
			HandleCurrentPlayerMovement();
			_syncPos = GlobalPosition;
		}
		else
		{
			GlobalPosition = GlobalPosition.Lerp(_syncPos, .1f);
		}
	}

	private bool IsCurrentPlayer()
	{
		return _multiplayerSynchronizer.GetMultiplayerAuthority() == Multiplayer.GetUniqueId();
	}

	private void SetupPlayerCamera()
	{
		if (!IsCurrentPlayer()) return;

		Camera2D playerCamera = new Camera2D
		{
			Enabled = true,
			Name = "PlayerCamera",
			AnchorMode = Camera2D.AnchorModeEnum.DragCenter,
			Zoom = new Vector2(1.6f, 1.6f),
			Position = Position,
			LimitLeft = 0,
			LimitTop = 0,
			LimitRight = (int)GetViewportRect().Size.X,
			LimitBottom = (int)GetViewportRect().Size.Y
		};

		_playerCamera = playerCamera;

		AddChild(_playerCamera);
	}

	private void HandlePlayerCamera(double delta)
	{
		Vector2 cameraPosition = _playerCamera.Position;
		Vector2 targetPosition = Position - GetViewportRect().Size / 2;
		// Calculate the new camera position based on the player's position
		cameraPosition = cameraPosition.Lerp(targetPosition, (float)delta * _cameraFollowSpeed);

		// Set the new camera position
		_playerCamera.Position = cameraPosition;
	}

	private void HandleCurrentPlayerMovement()
	{
		Vector2 velocity = Velocity;
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		if (direction != Vector2.Zero)
		{
			if (direction == Vector2.Left)
			{
				_playerSprite.FlipH = true;
			}

			if (direction == Vector2.Right)
			{
				_playerSprite.FlipH = false;
			}

			if (direction == Vector2.Up)
			{
				_playerAnimation.Play("run_up");
				velocity.X = 0;
				velocity.Y = direction.Y * _speed;
			}

			if (direction == Vector2.Down)
			{
				_playerAnimation.Play("run_down");
				velocity.X = 0;
				velocity.Y = direction.Y * _speed;
			}

			if (direction == Vector2.Left || direction == Vector2.Right)
			{
				_playerAnimation.Play("run_side");
				velocity.Y = 0;
				velocity.X = direction.X * _speed;
			}
		}
		else
		{
			if (velocity.Y == 0 && velocity.X == 0)
			{
				_playerAnimation.Play("idle_down");
			}

			velocity.X = Mathf.MoveToward(Velocity.X, 0, _speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, _speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

}
