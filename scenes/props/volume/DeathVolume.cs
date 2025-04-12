using Godot;
using wortal_v2.addons.physics_character_body;
using wortal_v2.scenes.singletons;
using wortal_v2.ui.screens;
using wortal_v2.ui.screens.game_over;

namespace wortal_v2.scenes.props.volume;

public partial class DeathVolume : Area3D
{
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node3D body)
    {
        if (body is not PhysicsCharacterBody) return;

        ScreenManager.Instance(this).SetScreen<GameOverScreen>();
    }
}