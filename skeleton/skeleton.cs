using Godot;
using System;

public partial class skeleton : CharacterBody2D
{

	public AnimationPlayer PlayerAnimation;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		PlayerAnimation = GetNode<AnimationPlayer>("AnimationPlayer");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		PlayerAnimation.Play("idle");
	}
}
