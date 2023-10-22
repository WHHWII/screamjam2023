using Godot;
using System;

public partial class FlashlightInteractable : Node3D, iInteractable
{
	public void Interact(PlayerInfo player)
	{
		GD.Print("Flashlight interacted with");
		(player.controller.flashLight.GetParent() as Node3D).Visible = true;
		GetParent().QueueFree();
	}
}
