using Godot;
using System;

public partial class PlayerInfo : Node
{
	public int keys = 0;
	[Export] public CharacterController controller;

	[ExportGroup("Myer")]
	[Export] RayCast3D myerRay;
	public bool hasBeenMyered;
	[Export] AudioStreamPlayer3D jumpScareAudio;

	[ExportGroup("Flashlight")]
	[Export] AnimationPlayer flickerAnim;
	[Export] public float flashLightEnergy = 100;
	[Export] private float flashlightDrainRate = 5;
	[Export] private float flashLightDimThreshold = 33.33f;
	private float flashLightDimRate = 4;
	[Export] public SpotLight3D[] lightArr;
	private float[] lightEnergyLevels;
	[Export] Material flashConeMat;
	BaseMaterial3D wideConeMat;
	Color baseFlashColor;
	Color clear = new Color(0, 0, 0, 0);
	Node3D flashlightParent;
	bool hasFlickered = false;
	
	public override void _Ready()
	{
		flashlightParent = controller.flashLight.GetParent() as Node3D;
		flashLightDimRate = controller.flashLight.LightEnergy / flashLightDimThreshold;
		lightEnergyLevels = new float[lightArr.Length]; 
		for(int i = 0; i < lightArr.Length; i++)
		{
			lightEnergyLevels[i] = lightArr[i].LightEnergy;
		}
		wideConeMat = flashConeMat as BaseMaterial3D;
		baseFlashColor = wideConeMat.AlbedoColor;
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (controller.flashLight.Visible && flashlightParent.Visible && flashLightEnergy > 0)
		{
			flashLightEnergy -= flashlightDrainRate * (float)delta;

			if(flashLightEnergy <= 0)
			{
				controller.flashLight.Visible = false;
			}
			else if (flashLightEnergy < flashLightDimThreshold)
			{
				if (!hasFlickered && !(flickerAnim.IsPlaying()))
				{
					hasFlickered = true;
					PlayFlicker();
				}
				for(int i=0; i < lightArr.Length; i++)
				{
					lightArr[i].LightEnergy = Mathf.Lerp(controller.flashLight.LightEnergy, 0.5f, flashLightDimRate * (float)delta);
				}
				wideConeMat.AlbedoColor.Lerp(clear, flashLightDimRate * (float)delta);


			}
		}

		var targetCol = myerRay.GetCollider(); 
		if(targetCol != null)
		{
			var colNode = targetCol as Node3D;
			Michael myerScript = colNode.GetParent() as Michael;
			bool myerClose = colNode.GlobalPosition.DistanceTo(controller.GlobalPosition) <= 20;
			if(myerScript != null)
			{
				if (!jumpScareAudio.Playing && !hasBeenMyered && (myerScript.onPlayerScreen && myerScript.playerInLOS))
				{
					jumpScareAudio.VolumeDb = (float)GD.RandRange(-5, 5);
					jumpScareAudio.PitchScale = (float)GD.RandRange(0.8, 1.2);
					jumpScareAudio.Play();
					hasBeenMyered = true;
					myerScript.hiding = true;
				}
			}

		}
	}

	public void RestoreLightLevels()
	{
		hasFlickered = false;
		for (int i = 0; i < lightArr.Length; i++)
		{
			lightArr[i].LightEnergy = lightEnergyLevels[i];
		}
		wideConeMat.AlbedoColor = baseFlashColor;
	}

	public void PlayFlicker()
	{
		flickerAnim.CurrentAnimation = "FlashlightFlicker";
		flickerAnim.SpeedScale = (float)GD.RandRange(0.8f,1.2f);
		flickerAnim.Play();

	}
}
