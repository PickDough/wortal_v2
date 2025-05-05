using System;
using Godot;
using wortal_v2.addons.node_selector;
using wortal_v2.addons.physics_character_body;
using wortal_v2.scenes.props.volume.water;
using wortal_v2.scenes.runes.ice.body_states;
using wortal_v2.scenes.runes.projectile_rune;

namespace wortal_v2.scenes.runes.ice;

public partial class IceProjectile: Projectile
{
    [Export] private PackedScene icePlaneScene = null!;
    protected override void OnBodyEntered(Node3D body)
    {
        Node? state = body switch
        {
            PhysicsCharacterBody characterBody => new IceCharacterBody(characterBody),
            _ => null
        };

        if (state == null)
        {
            GD.PrintErr($"Ice projectile cannot affect {body}");
            return;
        }
        
        var states = body.FindUnder<Node>("States");
        states!.AddChild(state);
        GD.Print($"Ice projectile affected {body}");
    }

    protected override void OnAreaEntered(Area3D area)
    {
        Node? state = area switch
        {
            WaterVolume => new IceWaterVolume(icePlaneScene.Instantiate<IcePlane>(), this),
            _ => null
        };

        if (state == null)
        {
            GD.PrintErr($"Ice projectile cannot affect {area}");
            return;
        }
        
        var states = area.FindUnder<Node>("States");
        states!.AddChild(state);
        GD.Print($"Ice projectile affected {area}");
    }
}