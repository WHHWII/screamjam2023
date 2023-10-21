using Godot;
using System;

public partial class WaterArea : Area3D
{
	private void _on_body_entered(Node3D body)
	{
		if (body.HasMethod("EnteredWater"))
		{
			body.Call("EnteredWater");
		}
	}
	private void _on_body_exited(Node3D body)
	{
		if (body.HasMethod("ExitedWater"))
		{
			body.Call("ExitedWater");
		}
	}
}



