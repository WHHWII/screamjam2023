using Godot;
using System;

public partial class BatteryPickup : Node, iInteractable
{
	[Export] private float energy = 50;

	public void Interact(PlayerInfo player)
	{
		player.flashLightEnergy += energy;
	}
}
