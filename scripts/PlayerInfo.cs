using Godot;
using System;

public partial class PlayerInfo : Node
{
	public int keys = 0;
	[Export] public CharacterController controller;


	[ExportGroup("Flashlight")]
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
	public override void _Ready()
	{
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
		if (controller.flashLight.Visible && flashLightEnergy > 0)
		{
			flashLightEnergy -= flashlightDrainRate * (float)delta;

			if(flashLightEnergy <= 0)
			{
				controller.flashLight.Visible = false;
			}
			else if (flashLightEnergy < flashLightDimThreshold)
			{
				for(int i=0; i < lightArr.Length; i++)
				{
					lightArr[i].LightEnergy = Mathf.Lerp(controller.flashLight.LightEnergy, 0.5f, flashLightDimRate * (float)delta);
				}
				wideConeMat.AlbedoColor.Lerp(clear, flashLightDimRate * (float)delta);
				
			}
		}
	}
	public void RestoreLightLevels()
	{
		for (int i = 0; i < lightArr.Length; i++)
		{
			lightArr[i].LightEnergy = lightEnergyLevels[i];
		}
		wideConeMat.AlbedoColor = baseFlashColor;
	}
}
