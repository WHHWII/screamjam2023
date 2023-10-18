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

	[Export] public SpotLight3D flashLight; 

	public const float Speed = 5.0f;

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
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
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
			//display prompt
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

		if (Input.IsActionJustPressed("toggle_flashlight") && playerInfo.flashLightEnergy > 0)
		{
			flashLight.Visible = !flashLight.Visible;
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
			//camera.Rotation = Mathf.Clamp(camera.Rotation.X, -Math.PI / 2, Math.PI / 2);

		}
	}

}
