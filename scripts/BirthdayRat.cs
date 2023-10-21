using Godot;
using System;

public partial class BirthdayRat : Node3D
{
	Random random;
	Vector3 spawnPoint;
	[Export] Node3D hidePoint;
	[Export] bool idleWander = true;
	[Export] Timer wanderTimer;
	[Export] CollisionShape3D wanderArea;
	[Export] float wanderSpeed = 4;
	[Export] float hideSpeed = 50;


	[ExportGroup("Audio")]
	[Export] AudioStreamPlayer3D audioPlayer;
	[Export] AudioStream wanderSound;
	[Export] AudioStream hideSound;
	[Export] Timer ratNoiseTimer;

	Vector3 wanderAreaSize;
	float xWander;
	float zWander;

	bool isHiding;
	bool canMove;

	Vector3 targetPos;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		hidePoint.Visible = false;
		spawnPoint = GlobalPosition;
		BoxShape3D wanderShape = wanderArea.Shape as BoxShape3D;
		wanderAreaSize = wanderShape.Size;
		random = new Random();

		xWander = wanderAreaSize.X * 0.5f;
		zWander = wanderAreaSize.Z * 0.5f;
		targetPos = GetNextPos();


	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if (!Visible) return;
		if(!isPlayingRatNoise) PlayWanderSound();
		if (idleWander && !isHiding && canMove)
		{
			GlobalPosition = GlobalPosition.MoveToward(targetPos, wanderSpeed * (float)delta);
			if (GlobalPosition.IsEqualApprox(targetPos))
			{
				canMove = false;
				wanderTimer.Start();
			}

		}
		else if (isHiding)
		{
			GlobalPosition = GlobalPosition.MoveToward(hidePoint.GlobalPosition, wanderSpeed * (float)delta);
			LookAt(hidePoint.GlobalPosition);
			if (Visible && GlobalPosition.IsEqualApprox(hidePoint.GlobalPosition))
			{
				Visible = false;
			}

		}
	}

	Vector3 GetNextPos()
	{
		float x = (float)GD.RandRange((double)(spawnPoint.X - xWander), (double)(spawnPoint.X + xWander));
		float z = (float)GD.RandRange((double)(spawnPoint.Z - zWander), (double)(spawnPoint.Z + zWander));

		return new(x, spawnPoint.Y, z);
	}
	
	private void _on_scuttle_timer_timeout()
	{
		targetPos = GetNextPos();
		wanderTimer.WaitTime = GD.Randf();
		float diceRoll = GD.Randf();
		if (diceRoll < 0.3f) wanderTimer.WaitTime += GD.RandRange(0.5f, 1); 
		LookAt(targetPos);
		canMove = true;
	}
	private void _on_wander_zone_body_entered(Node3D body)
	{
		if (isHiding) return;
		if(body.Name == "RatBlaster")
		{
			GD.Print($"ratblaster detected");
			if (body.Visible == false)
			{
				GD.Print($"ratblaster off");
				return;
			}
		}
		GD.Print($"rat fleeing from {body.Name}");
		
		isHiding = true;

		audioPlayer.Stop();
		audioPlayer.Stream = hideSound;
		audioPlayer.PitchScale = (float)GD.RandRange(2f, 3f);
		audioPlayer.VolumeDb += GD.RandRange(15, 20);
		audioPlayer.Play(GD.Randf() * 0.1f);
	}

	int clipIndex = 1;
	bool isPlayingRatNoise = false;
	void PlayWanderSound()
	{
		isPlayingRatNoise = true;
		audioPlayer.PitchScale = (float)GD.RandRange(1f, 2f);
		audioPlayer.Play(clipIndex * 0.3f);
		clipIndex += GD.RandRange(0, 1);
		if (clipIndex >= 5) clipIndex = 1;
		ratNoiseTimer.WaitTime = GD.RandRange(0.3f, 3);
		ratNoiseTimer.Start();
	}

	private void _on_rat_noise_timer_timeout()
	{
		audioPlayer.Stop();
		isPlayingRatNoise = false;
	}


}


