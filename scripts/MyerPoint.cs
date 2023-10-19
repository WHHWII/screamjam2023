using Godot;
using System;

public partial class MyerPoint : Node3D
{
	[Export] public MeshInstance3D spawnPoint;
	[Export] public MeshInstance3D hidePoint;
	[Export] public Area3D playerDetectionArea;
	[Export] public CharacterBody3D myer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		spawnPoint.Visible = false;
		hidePoint.Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}





	private void _on_area_3d_body_entered(Node3D body)
	{
		myer.Position = spawnPoint.Position;
		// Replace with function body.
	}
	private void _on_visible_on_screen_notifier_3d_screen_entered()
	{
		// Replace with function body.
	}
}



