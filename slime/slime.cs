using Godot;
using System;

public partial class Slime : CharacterBody2D
{
	public AnimationPlayer _playerAnimation;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_playerAnimation = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_playerAnimation.Play("idle");
	}
}
