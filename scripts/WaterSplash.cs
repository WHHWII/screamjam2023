using Godot;
using System;

public partial class WaterSplash : CollisionShape3D
{
	[Export] AudioStreamPlayer3D audioPlayer;
	private bool hasPlayed = false;
	private void _on_head_collision_body_entered(Node3D body)
	{
		if (hasPlayed) return;
		audioPlayer.Play();
		hasPlayed = true;
	}
}
