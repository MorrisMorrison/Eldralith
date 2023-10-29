using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 200.0f;
	public const float JumpVelocity = -400.0f;
	private Vector2 syncPos = new Vector2(0, 0);

	public AnimationPlayer PlayerAnimation;
	public AnimatedSprite2D PlayerSprite;

	public override void _Ready()
	{
		PlayerAnimation = GetNode<AnimationPlayer>("AnimationPlayer");
		PlayerSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").SetMultiplayerAuthority(int.Parse(Name));
	}

	public override void _PhysicsProcess(double delta)
	{
		if (GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").GetMultiplayerAuthority() == Multiplayer.GetUniqueId())
		{
			HandleCurrentPlayerMovement();
			syncPos = GlobalPosition;
		}
		else
		{
			GlobalPosition = GlobalPosition.Lerp(syncPos, .1f);
		}
	}

	private void HandleCurrentPlayerMovement()
	{
		Vector2 velocity = Velocity;
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		if (direction != Vector2.Zero)
		{
			if (direction == Vector2.Left)
			{
				PlayerSprite.FlipH = true;
			}

			if (direction == Vector2.Right)
			{
				PlayerSprite.FlipH = false;
			}

			if (direction == Vector2.Up)
			{
				PlayerAnimation.Play("run_up");
				velocity.X = 0;
				velocity.Y = direction.Y * Speed;
			}

			if (direction == Vector2.Down)
			{
				PlayerAnimation.Play("run_down");
				velocity.X = 0;
				velocity.Y = direction.Y * Speed;
			}

			if (direction == Vector2.Left || direction == Vector2.Right)
			{
				PlayerAnimation.Play("run_side");
				velocity.Y = 0;
				velocity.X = direction.X * Speed;
			}
		}
		else
		{
			if (velocity.Y == 0 && velocity.X == 0)
			{
				PlayerAnimation.Play("idle_down");
			}

			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

}
