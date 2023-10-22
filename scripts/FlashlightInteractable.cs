using Godot;
using System;

public partial class FlashlightInteractable : Node3D, iInteractable
{

	[Export] AnimationPlayer flickerAnim;
	[Export] Timer flickerTimer;
	[Export] Timer startTimer;
	public void Interact(PlayerInfo player)
	{
		GD.Print("Flashlight interacted with");
		(player.controller.flashLight.GetParent() as Node3D).Visible = true;
		GetParent().QueueFree();
	}

	private void _on_flicker_timer_timeout()
	{
		float diceRoll = GD.Randf();
		if (diceRoll <= .2)
		{
			PlayFlicker();
		}
	}
	private void _on_start_timer_timeout()
	{
		PlayFlicker();
		flickerTimer.Start();
	}

	public void PlayFlicker()
	{
		flickerAnim.CurrentAnimation = "FlashlightFlicker";
		flickerAnim.SpeedScale = (float)GD.RandRange(0.5f, 1.5f);
		flickerAnim.Play();

	}
	

}






