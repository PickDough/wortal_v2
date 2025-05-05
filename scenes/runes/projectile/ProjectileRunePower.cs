using Godot;
using System;
using wortal_v2.addons.gd_inject.attributes;
using wortal_v2.scenes.runes;
using wortal_v2.scenes.runes.projectile_rune;

public partial class ProjectileRunePower : RunePower
{
    [FromOwner] private Rune rune = null!;
    [Export] private PackedScene projectileScene = null!;
    [Export] private float Velocity { get; set; }
    [Export] private float TimeToLive { get; set; }

    public override void Invoke()
    {
        GD.Print("Projectile rune power invoked");
        var projectile = projectileScene.Instantiate<Projectile>();
        projectile.Velocity = -rune.Basis.Z * Velocity;
        rune.AddChild(projectile);
        GetTree().CreateTimer(TimeToLive).Timeout += () =>
        {
            if (projectile.IsInsideTree())
            {
                projectile.QueueFree();
            }
        };
    }
}