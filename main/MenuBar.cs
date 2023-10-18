using Godot;
using Microsoft.VisualBasic;
using System;

public partial class MenuBar : Godot.MenuBar
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _on_play_pressed()
	{
		GetTree().ChangeSceneToFile("world/world.tscn");
	}

	private void _on_options_pressed()
	{

		GetTree().ChangeSceneToFile("options/options.tscn");
	}


	private void _on_quit_pressed()
	{
		GetTree().Quit();
	}
}



