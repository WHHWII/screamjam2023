using Godot;
using System;

public partial class Lock : StaticBody3D, iInteractable
{
	[Export] AnimationPlayer anim;
	[Export] AudioStreamPlayer3D aud;
	public void Interact(PlayerInfo player)
	{
		if (!anim.IsPlaying())
		{
			anim.CurrentAnimation = "LockRattle";
			anim.Play();
			aud.Play();
		}
	}
}
