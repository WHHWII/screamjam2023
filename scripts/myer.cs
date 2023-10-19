using Godot;
using System;

public partial class myer : CharacterBody3D
{
	[Export] CharacterController player;
	[Export] Timer visibleToPlayerTimer;

	public override void _PhysicsProcess(double delta)
	{
		LookAt(player.Position);

	}



	private void _on_visible_on_screen_notifier_3d_screen_entered()
	{
		visibleToPlayerTimer.Start();
		visibleToPlayerTimer.Paused = false;
	}


	private void _on_visible_on_screen_notifier_3d_screen_exited()
	{
		visibleToPlayerTimer.Paused = true; 
		// Replace with function body.
	}
	private void _on_timer_timeout()
	{
		//hide
	}
}



