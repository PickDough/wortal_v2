using Godot;

namespace wortal_v2.scenes.props.volume;

[Tool]
public partial class Volume: Area3D
{
    [Export]
    private Vector3 Size
    {
        get;
        set
        {
            field = value;
            OnSizeSet(value);
        }
    }
    
    protected virtual void OnSizeSet(Vector3 size)
    {
        var collisionShape = GetNodeOrNull<CollisionShape3D>("CollisionShape3D");
        if (collisionShape is not { Shape: BoxShape3D boxShape }) return;
        
        boxShape.Size = size;
    }
}