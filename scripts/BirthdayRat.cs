using Godot;
using System;

public partial class BirthdayRat : Node3D
{
	Random random;
	Vector3 spawnPoint;
	Vector3 hidePoint;
	[Export] Timer wanderTimer;
	[Export] BoxShape3D scuttleArea;
	[Export] float moveSpeed = 4;
	Vector3 scuttleSize;
	float xScuttle;
	float zScuttle;

	bool isHiding;
	bool canMove;

	Vector3 targetPos;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		scuttleSize = scuttleArea.Size;
		random = new Random();

		xScuttle = scuttleSize.X * 0.5f;
		zScuttle = scuttleSize.Z * 0.5f;


	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if (!isHiding)
		{
			GlobalPosition.MoveToward(targetPos, moveSpeed * (float)delta);
		}
	}

	Vector3 GetNextPos()
	{
		float x = (float)GD.RandRange((double)(spawnPoint.X - xScuttle), (double)(spawnPoint.X + xScuttle));
		float z = (float)GD.RandRange((double)(spawnPoint.Z - zScuttle), (double)(spawnPoint.Z + zScuttle));

		return new(x, spawnPoint.Y, z);
	}
	
	private void _on_scuttle_timer_timeout()
	{
		targetPos = GetNextPos();
	}
}



