using Godot;
using System;

public partial class AudioTriggerButReal : Node3D
{
	[Export] AudioStreamPlayer3D speaker;
	[Export] float speakerSpeed;
	[Export] Node3D speakerDestination;
	[Export] bool fade;
	[Export] float fadeSpeed;
	bool hasBeenTriggered = false;
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(speakerDestination != null)
		{
			speaker.GlobalPosition = speaker.GlobalPosition.MoveToward(speakerDestination.GlobalPosition, speakerSpeed * (float)delta);
			if (speaker.GlobalPosition.IsEqualApprox(speakerDestination.GlobalPosition))
			{
				speakerDestination = null;
			}
		}
		if (fade)
		{
			Mathf.Lerp(speaker.VolumeDb,0, fadeSpeed * (float)delta);
		}
	}
	private void _on_trigger_body_entered(Node3D body)
	{
		if (hasBeenTriggered) return;
		hasBeenTriggered = true;
		speaker.Play();
	}
}



