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
	public bool chasePlayer = false;
	public bool playerInLOS = false;
	public bool onPlayerScreen = false;


	[ExportGroup("Audio")]
	[Export] public AudioStreamPlayer3D footstepAudio;
	[Export] Timer footStepTimer;
	[Export] AudioStreamPlayer3D scream;
	public override void _Ready()
	{
		hidePoint = Position;
		origVolume = footstepAudio.VolumeDb;
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
		else if (chasePlayer)
		{
			if (!scream.Playing) scream.Play();
			GlobalPosition = GlobalPosition.MoveToward(player.GlobalPosition, hideSpeed * (float)delta);
		}
		else
		{
			
			GlobalPosition = GlobalPosition.MoveToward(hidePoint, hideSpeed * (float)delta);
			//if (GlobalPosition.IsEqualApprox(hidePoint))
			//{
			//	Visible = false;

			//}
			if (!footStepsPlaying)
				PlayFootStep(footstepAudio);


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
		RunFromPlayer();
	}
	public void RunFromPlayer()
	{
		GD.Print("Myer is fleeing");
		hiding = true;
		footstepAudio.Play();
		//LookAt(hidePoint);
	}
	bool footStepsPlaying;
	int clipIndex = 0;
	int clipCount = 7;
	float origVolume;
	void PlayFootStep(AudioStreamPlayer3D foot)
	{
		foot.PitchScale = (float)GD.RandRange(0.7f, 1f);
		foot.Play(clipIndex * 0.6f);
		if (clipIndex >= clipCount)
		{
			clipIndex = 0;
			foot.VolumeDb = origVolume;
		}
		else
		{
			++clipIndex;
			foot.VolumeDb -= 4;

		}
		footStepTimer.Start();
		footStepsPlaying = true;
	}
	private void _on_foot_step_timer_timeout()
	{
		footstepAudio.Stop();
		footStepsPlaying = false;
	}
	private void _on_player_detection_ar_ea_body_entered(Node3D body)
	{
		GetTree().Quit();
	}
}














