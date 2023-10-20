using Godot;
using System;

public partial class Key : Node3D, iInteractable
{
	public void Interact(PlayerInfo player)
	{
		++player.keys;
		GD.Print(player.keys);
		GetParent().QueueFree();
	}
}
