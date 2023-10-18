using Godot;
using System;

public partial class player : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	// public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	public AnimationPlayer PlayerAnimation;
	public AnimatedSprite2D PlayerSprite;

	public override void _Ready()
	{
		PlayerAnimation = GetNode<AnimationPlayer>("AnimationPlayer");
		PlayerSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		// if (!IsOnFloor())
		//	velocity.Y += gravity * (float)delta;

		// Handle Jump.
		// if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		//	velocity.Y = JumpVelocity;
		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		
		if (direction != Vector2.Zero)
		{
			if (direction == Vector2.Left){
				PlayerSprite.FlipH = true;
			}

			if (direction == Vector2.Right){
				PlayerSprite.FlipH = false;
			}

			if(direction == Vector2.Up){
				PlayerAnimation.Play("run_up");
				velocity.Y = direction.Y * Speed;
			}

			if(direction == Vector2.Down){
				PlayerAnimation.Play("run_down");
				velocity.Y = direction.Y * Speed;
			}

			if(direction == Vector2.Left || direction == Vector2.Right){
				PlayerAnimation.Play("run_side");
				velocity.X = direction.X * Speed;
			}
		}
		else
		{
			if (velocity.Y == 0 && velocity.X == 0) {
				PlayerAnimation.Play("idle_down");
			}

			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
		}

		//if (velocity.Y > 0) {
		//	PlayerAnimation.Play("fall");
		//}

		Velocity = velocity;
		MoveAndSlide();
	}

}
