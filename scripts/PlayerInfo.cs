using Godot;
using System;

public partial class PlayerInfo : Node
{
	public int keys = 0;

	[Export] public CharacterController controller;




	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
