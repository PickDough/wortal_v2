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
        var raycastResult = characterBody.RaycastFromCamera(distance,
            CollisionLayer.World + CollisionLayer.Item + CollisionLayer.Rune, true, true);

        if (raycastResult == null || !raycastResult.Collider.GetCollisionLayerValue(CollisionLayer.RuneLayer))
        {
            HandleNoRaycastResult();
            return;
        }

        if (raycastResult.Collider is not Rune rune)
            return;

        if (TryGetColor(rune, out var color, out var shouldReturn))
        {
            crosshair.SelfModulate = color;
            if (shouldReturn) return;
        }
        else
        {
            return;
        }

        UpdateLastRune(rune);
        crosshair.SelfModulate = rune.Power.IsAffectingCharacter(characterBody) ? Colors.Orange : Colors.Green;
    }

    private void HandleNoRaycastResult()
    {
        if (lastRune == null) return;

        lastRune.State = new RuneState.Placed();
        lastRune.GlobalPosition += lastRune.Transform.Basis.Z * 0.05f;
        lastRune = null;
        crosshair.SelfModulate = Colors.White;
    }

    private bool TryGetColor(Rune rune, out Color color, out bool shouldReturn)
    {
        shouldReturn = true;

        if (rune.State == new RuneState.OnCooldown())
        {
            color = Colors.Gray;
            return true;
        }

        if (rune.State == new RuneState.AimedAt() && lastRune == rune)
        {
            color = rune.Power.IsAffectingCharacter(characterBody) ? Colors.Orange : Colors.Green;
            return true;
        }

        if (rune.State == new RuneState.Placed())
        {
            color = Colors.White;
            shouldReturn = false;
            return true;
        }

        color = default;
        return false;
    }

    private void UpdateLastRune(Rune rune)
    {
        if (lastRune != null)
        {
            lastRune.GlobalPosition += lastRune.Transform.Basis.Z * 0.05f;
            lastRune.State = new RuneState.Placed();
        }

        lastRune = rune;
        lastRune.State = new RuneState.AimedAt();
        lastRune.GlobalPosition -= lastRune.Transform.Basis.Z * 0.05f;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButtonEvent && mouseButtonEvent.IsReleased() &&
            mouseButtonEvent.ButtonIndex == MouseButton.Left && lastRune != null)
        {
            lastRune.Power?.Invoke();
            lastRune.State = new RuneState.OnCooldown();
        }
    }
}