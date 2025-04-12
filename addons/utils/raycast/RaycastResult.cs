using Godot;

namespace wortal_v2.addons.utils;

public record RaycastResult(CollisionObject3D Collider, Vector3 Normal, Vector3 Point);

public static class CollisionLayer
{
    public const int World = 1 << 0;
    public const int WorldLayer = 1;

    public const int Player = 1 << 1;
    public const int PlayerLayer = 2;
    
    public const int RuneSurface = 1 << 2;
    public const int RuneSurfaceLayer = 3;
    
    public const int Rune = 1 << 3;
    public const int RuneLayer = 4;
    
    public const int Item = 1 << 4;
    public const int ItemLayer = 5;
}