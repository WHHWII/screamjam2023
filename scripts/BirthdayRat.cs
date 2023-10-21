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
	}
}
