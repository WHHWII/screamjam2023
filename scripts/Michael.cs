using Godot;
using System;

public partial class Michael : Node3D
{
	
	[Export] CharacterController player;
	[Export] public Timer visibleToPlayerTimer;
	[Export] public Vector3 hidePoint;
	[Export] public Vector3 showPoint;
	[Export] public RayCast3D playerLOSRay;
	public bool hiding = false;
	public bool aproaching = false;
	public float hideSpeed;

	bool playerInLOS = false;
	bool onPlayerScreen = false;
	public override void _Ready()
	{
		hidePoint = Position;
	}
	public override void _PhysicsProcess(double delta)
	{
		LookAt(player.Position);
		if (aproaching)
		{
			GlobalPosition = GlobalPosition.MoveToward(showPoint, hideSpeed * (float)delta);
			if (GlobalPosition.IsEqualApprox(showPoint))
			{
				aproaching = false;
			}
		}
		else if (!hiding)
		{
			
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

		if (playerInLOS && onPlayerScreen)
		{
			if (visibleToPlayerTimer.Paused)
			{
				GD.Print("myer spotted");
				visibleToPlayerTimer.Start();
				visibleToPlayerTimer.Paused = false;
			}
		}
		else if (!visibleToPlayerTimer.Paused)
		{
			GD.Print("lost sight of myer");
			visibleToPlayerTimer.Paused = true;
		}



	}

	public void MyerWrapper()
	{
		_on_visible_on_screen_notifier_3d_screen_exited();
		_on_visible_on_screen_notifier_3d_screen_entered();

	}

	private void _on_visible_on_screen_notifier_3d_screen_entered()
	{
		onPlayerScreen = true;
	}


	private void _on_visible_on_screen_notifier_3d_screen_exited()
	{

		onPlayerScreen = false;
		// Replace with function body.
	}
	private void _on_timer_timeout()
	{
		GD.Print("Myer is fleeing");
		hiding = true;
		//LookAt(hidePoint);
	}
}








