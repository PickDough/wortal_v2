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
    [FromOwner] private TextureRect crosshair = null!;

    private Rune? lastRune;

    public override void _Process(double delta)
    {
        var raycastResult = characterBody.RaycastFromCamera(distance, CollisionLayer.Rune, true, false);
        if (raycastResult == null)
        {
            if (lastRune == null) return;
            lastRune.State = new RuneState.Placed();
            lastRune.GlobalPosition += lastRune.Transform.Basis.Z * 0.05f;
            lastRune = null;
            crosshair.SelfModulate = Colors.White;
            return;
        }

        var rune = raycastResult.Collider as Rune;
        if (rune!.State != new RuneState.Placed())
        {
            if (lastRune == rune)
            {
                crosshair.SelfModulate = rune.Power.IsAffectingCharacter(characterBody) ? Colors.Orange : Colors.Green;
            }
            return;
        }


        if (lastRune != null)
        {
            lastRune.GlobalPosition += lastRune.Transform.Basis.Z * 0.05f;
            lastRune!.State = new RuneState.Placed();
        }
        lastRune = rune;
        lastRune.State = new RuneState.AimedAt();
        lastRune.GlobalPosition -= lastRune.Transform.Basis.Z * 0.05f;
        crosshair.SelfModulate = rune.Power.IsAffectingCharacter(characterBody) ? Colors.Orange : Colors.Green;

        GD.Print("Modulate: " + crosshair.SelfModulate);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButtonEvent && mouseButtonEvent.IsReleased() &&
            mouseButtonEvent.ButtonIndex == MouseButton.Left && lastRune != null)
        {
            lastRune.Power?.Invoke();
        }
    }
}