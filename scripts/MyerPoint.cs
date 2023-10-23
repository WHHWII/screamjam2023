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
	[Export] public bool approachFromHidePoint;
	[Export] public bool chasePlayer = false;
	bool hasBeenTriggered;
	bool isActive;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		spawnPoint.Visible = false;
		hidePoint.Visible = false;
	}

	private void _on_area_3d_body_entered(Node3D body)
	{
		//playerDetectionArea.QueueFree();
		CharacterController player = body as CharacterController;
		player.playerInfo.hasBeenMyered = false;
		myer.footstepAudio.Stop();
		if (hasBeenTriggered) return;
		hasBeenTriggered = true;
		isActive = true;
		myer.hidePoint = hidePoint.GlobalPosition;
		myer.showPoint = spawnPoint.GlobalPosition;
		myer.hiding = false;
		myer.Visible = true;
		myer.chasePlayer = chasePlayer;
		
		myer.hideSpeed = hideSpeed;
		myer.visibleToPlayerTimer.WaitTime = stareTime;
		myer.visibleToPlayerTimer.Stop();
		myer.visibleToPlayerTimer.Paused = true;
		if (approachFromHidePoint)
		{
			myer.GlobalPosition = hidePoint.GlobalPosition;
			myer.aproaching = true;
		}
		else
		{
			myer.GlobalPosition = spawnPoint.GlobalPosition;
			myer.aproaching = false;
		}

		//myer.MyerWrapper();
		GD.Print("Myer has moved");
		myer.footstepAudio.Play();
		// Replace with function body.

	}

	private void _on_myer_flee_area_body_entered(Node3D body)
	{
		if (!hasBeenTriggered) return;
		myer.RunFromPlayer();
	}
}






