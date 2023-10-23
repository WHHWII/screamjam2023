using Godot;
using System;

public partial class DropHead : Node3D
{
	[Export] private RigidBody3D head;

	public override void _Ready()
	{
		head.Freeze = true;
	}

	private void _on_area_3d_body_entered(Node3D body)
	{
		head.Freeze = false;
	}
}

