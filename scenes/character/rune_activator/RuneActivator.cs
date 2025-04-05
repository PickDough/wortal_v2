using Godot;
using wortal_v2.addons.gd_inject.attributes;
using wortal_v2.addons.physics_character_body;
using wortal_v2.addons.utils;
using wortal_v2.scenes.runes;

namespace wortal_v2.scenes.character.rune_activator;

public partial class RuneActivator : Node
{
	[Export] private float distance = 10f;
	
	[FromOwner] private PhysicsCharacterBody characterBody = null!;

	private Rune? lastRune; 
	public override void _Process(double delta)
	{
		
		var raycastResult = characterBody.RaycastFromCamera(distance, CollisionLayer.Rune, true, false);
		if (raycastResult == null)
		{
			if (lastRune == null) return;
			lastRune.State = new RuneState.Placed();
			lastRune = null;
			return;
		}
		
		var rune = raycastResult.Collider as Rune;
		if (rune!.State != new RuneState.Placed() || lastRune == rune) return;
		
		rune.State = new RuneState.AimedAt();
		if (lastRune != null) lastRune!.State = new RuneState.Placed();
		lastRune = rune;
	}
	
	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseButtonEvent && mouseButtonEvent.IsReleased() && lastRune != null)
		{
			lastRune.Power?.Invoke();
		}
	}
}