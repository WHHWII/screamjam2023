using Godot;
using System;

public partial class AudioTrigger : Node3D
{

	[Export] AudioStreamPlayer3D audioPlayer;

	private bool hasPlayed = false;

	private void _on_area_3d_body_entered(Node3D body)
	{
		if (hasPlayed) return;
		
		audioPlayer.Play();
		hasPlayed = true;
	}
}

