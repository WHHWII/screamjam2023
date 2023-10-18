using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class Door : Node3D, iInteractable
{

	[Export] int keyRequirement = 1;

	public void Interact(PlayerInfo player)
	{
		if(player.keys >= keyRequirement)
		{
			player.keys -= keyRequirement;
			GD.Print(player.keys);
			GD.Print("door opened");
			GetParent().QueueFree();
		}
		else
		{
			GD.Print("not enough keys");
		}
	}
}
