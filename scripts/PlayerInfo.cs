using Godot;
using System;

public partial class PlayerInfo : Node
{
	public int keys = 0;
	[Export] public float flashLightEnergy = 100;
	[Export] private float flashlightDrainRate = 1;
	[Export] private float flashLightDimThreshold = 33.33f;
	private float flashLightDimRate = 0;
	

	[Export] public CharacterController controller;




	public override void _Ready()
	{
		flashLightDimRate = controller.flashLight.LightEnergy / flashLightDimThreshold;
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
				controller.flashLight.LightEnergy = Mathf.Lerp(controller.flashLight.LightEnergy, 0.5f, flashLightDimRate * (float)delta);
			}
		}
	}
}
