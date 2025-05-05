using Godot;

namespace wortal_v2.scenes.runes.projectile_rune;

public partial class Projectile: Area3D
{
    public Vector3 Velocity { get; set; }

    public override void _Ready()
    {   
        BodyEntered += OnBodyEntered;
        AreaEntered += OnAreaEntered;
    }

    protected virtual void OnAreaEntered(Area3D area)
    {
        GD.Print("area entered: " + area.Name);
    }

    protected virtual void OnBodyEntered(Node3D body)
    {
        GD.Print("body entered: " + body.Name);
    }

    public override void _PhysicsProcess(double delta)
    {
        GlobalPosition += Velocity * (float)delta;
    }
}