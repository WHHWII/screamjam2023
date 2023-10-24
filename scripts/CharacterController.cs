using Godot;
using Microsoft.VisualBasic;
using System;

public partial class CharacterController : CharacterBody3D
{
	//TODO clamp camera
	[Export] public Camera3D camera;
	float lookSensitivity = (float)ProjectSettings.GetSetting("player/look_sensitivity");
	[Export] public PlayerInfo playerInfo; // TODO change to singleton or smth
	[Export] private RayCast3D interactRay;
	[Export] private Light3D interactLight;

	[Export] public SpotLight3D flashLight;
	[Export] public StaticBody3D ratBlaster;
	[Export] AnimationPlayer animator;

	[ExportGroup("Audio")]
	[Export] AudioStreamPlayer3D lFootAudio;
	[Export] AudioStreamPlayer3D rFootAudio;
	[Export] Timer footStepTimer;
	[Export] AudioStream groundStep;
	[Export] AudioStream waterStep;
	[Export] AudioStreamPlayer3D flashLightAudio;
	public float speed = 5.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}
	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * speed;
			velocity.Z = direction.Z * speed;
			animator.Play("head_bob");
			if(!footStepsPlaying)
			FootStepAudio();
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, speed);
			animator.Stop();
			_on_foot_step_timer_timeout();
		}

		Velocity = velocity;
		MoveAndSlide();


		var targetCol = interactRay.GetCollider();
		bool targetIsInteractable = false;

		if (targetCol != null)
		{
			//GD.Print(targetCol);
			targetIsInteractable = targetCol.HasMethod("Interact");
			//GD.Print(targetIsInteractable);
		}

		if (targetIsInteractable)
		{
			Node3D interNode = ((targetCol as Node).GetParent() as Node3D);
			if (interNode != null)
			{
				interactLight.Visible = true;
				Vector3 lightPos = interNode.GlobalPosition;
				interactLight.GlobalPosition = lightPos;
				
			}
		}
		else
		{
			interactLight.Visible = false;
		}


		if (Input.IsActionJustPressed("interact"))
		{
			GD.Print("Interect pressed");
			if (targetIsInteractable)
			{
				GD.Print("Interacted with interatable.");
				targetCol.Call("Interact", playerInfo);



			}

		}

		if (Input.IsActionJustPressed("toggle_flashlight"))
		{
			if (!(flashLight.GetParent() as Node3D).Visible) return;
			flashLightAudio.Play(0.1f);
			if (playerInfo.flashLightEnergy > 0)
			{
				flashLight.Visible = !flashLight.Visible;
				ratBlaster.Visible = !ratBlaster.Visible;

			}
		}
		
		if(Input.IsActionJustPressed("Quit")){
			GetTree().Quit();
		}
	}


	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		if (@event is InputEventMouseMotion)
		{
			var inputMotion = (@event as InputEventMouseMotion);
			RotateY(-inputMotion.Relative.X * lookSensitivity);
			camera.RotateX(-inputMotion.Relative.Y * lookSensitivity);
			camera.Rotation = new Vector3(  Mathf.Clamp(camera.Rotation.X, -1.5f, 1.5f), camera.Rotation.Y,camera.Rotation.Z );


		}
	}

	bool inWater = false;
	bool isRFoot = true;
	bool footStepsPlaying;
	int clipIndex = 0;
	float clipCount = 6;
	void FootStepAudio()
	{
		footStepsPlaying = true; 
		if (isRFoot)
		{
			PlayFootStep(rFootAudio);
			isRFoot = false;
		}
		else
		{
			PlayFootStep(lFootAudio);
			isRFoot = true;
		}




	}

	void PlayFootStep(AudioStreamPlayer3D foot)
	{
		foot.PitchScale = (float)GD.RandRange(0.7f, 1f);
		foot.Play(clipIndex * 0.6f);
		if (clipIndex >= clipCount)
		{
			clipIndex = 0;
		}
		else
		{
			++clipIndex;
		}
		footStepTimer.Start();
	}
	private void _on_foot_step_timer_timeout()
	{
		rFootAudio.Stop();
		lFootAudio.Stop();
		footStepsPlaying = false;
	}

	public void EnteredWater()
	{
		clipIndex = 0;
		clipCount = 3;
		rFootAudio.Stream = waterStep;
		lFootAudio.Stream = waterStep;
		speed = 3;
	}
	public void ExitedWater()
	{
		clipIndex = 0;
		clipCount = 6;
		rFootAudio.Stream = groundStep;
		lFootAudio.Stream = groundStep;
		speed = 5;
	}
}



