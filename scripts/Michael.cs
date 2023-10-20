using Godot;
using System;

public partial class Michael : Node3D
{
	
	[Export] CharacterController player;
	[Export] public Timer visibleToPlayerTimer;
	[Export] public Vector3 hidePoint;
	[Export] public RayCast3D playerLOSRay;
	public bool hiding = false;
	public float hideSpeed;

	bool playerInLOS = false;

	public override void _Ready()
	{
		hidePoint = Position;
	}
	public override void _PhysicsProcess(double delta)
	{
		if (!hiding)
		{
			LookAt(player.Position);
			playerLOSRay.LookAt(new Vector3(player.Position.X, player.Position.Y + 0.5f, player.Position.Z));
			var targetCol = playerLOSRay.GetCollider();
			if (targetCol != null && targetCol.GetType() == typeof(CharacterController))
			{
				playerInLOS = true;
			}
			else 
			{
				playerInLOS = false;
			}
		}
		else
		{
			GlobalPosition = GlobalPosition.MoveToward(hidePoint, hideSpeed * (float)delta);
			if (GlobalPosition.IsEqualApprox(hidePoint))
			{
				//Visible = false;
			}
		}

		



	}



	private void _on_visible_on_screen_notifier_3d_screen_entered()
	{
		if (playerInLOS)
		{
			GD.Print("myer spotted");
			visibleToPlayerTimer.Start();
			visibleToPlayerTimer.Paused = false;
		}
	}


	private void _on_visible_on_screen_notifier_3d_screen_exited()
	{
		GD.Print("lost sight of myer");
		visibleToPlayerTimer.Paused = true; 
		// Replace with function body.
	}
	private void _on_timer_timeout()
	{
		GD.Print("Myer is fleeing");
		GD.Print(hidePoint);
		hiding = true;
		LookAt(hidePoint);
	}
}


