using Godot;
using System;

public partial class MyerPoint : Node3D
{
	[Export] public MeshInstance3D spawnPoint;
	[Export] public MeshInstance3D hidePoint;
	[Export] public Area3D playerDetectionArea;
	[Export] public Michael myer;
	[Export] public float hideSpeed = 200;
	[Export] public float stareTime = 1;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		spawnPoint.Visible = false;
		hidePoint.Visible = false;
	}

	private void _on_area_3d_body_entered(Node3D body)
	{
		myer.GlobalPosition = spawnPoint.GlobalPosition;
		myer.hidePoint = hidePoint.GlobalPosition;
		myer.hiding = false;
		myer.Visible = true;
		myer.hideSpeed = hideSpeed;
		myer.visibleToPlayerTimer.WaitTime = stareTime;


		GD.Print("Myer has moved");
		// Replace with function body.
	}
}



