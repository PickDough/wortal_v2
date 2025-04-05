using System;
using Godot;
using wortal_v2.addons.utils;
using wortal_v2.scenes.runes;

namespace wortal_v2.scenes.character.rune_placer;

public static class RuneSurfaceResolver
{
    internal static Vector3 ResolveSurface(RaycastResult raycastResult)
    {
        return raycastResult.Collider switch
        {
            StaticBody3D staticBody => StaticBodyType(raycastResult, staticBody),
            RigidBody3D rigidBody3D => RigidBodyType(raycastResult, rigidBody3D),
            _ => UnsupportedType(raycastResult),
        };
    }

    public static bool IsOverlapping(RaycastResult raycastResult, Rune rune)
    {
        var rootChildren = raycastResult.Collider.GetChildren();
        foreach (var child in rootChildren)
        {
            if (child is Rune runeChild && runeChild.Position.IsEqualApprox(raycastResult.Collider.ToLocal(rune!.GlobalPosition)))
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsSurfaceTilted(Vector3 normal)
    {
        var isX = normal.X == 0 || Math.Abs(Math.Abs(normal.X) - 1f) < 0.001f;
        var isY = normal.Y == 0 || Math.Abs(Math.Abs(normal.Y) - 1f) < 0.001f;
        var isZ = normal.Z == 0 || Math.Abs(Math.Abs(normal.Z) - 1f) < 0.001f;
        return Convert.ToInt32(isX) + Convert.ToInt32(isY) + Convert.ToInt32(isZ) != 3;
    }

    private static Vector3 StaticBodyType(RaycastResult raycastResult, PhysicsBody3D staticBody)
    {
        if (IsSurfaceTilted(raycastResult.Normal))
        {
            var node3D = raycastResult.Collider as Node3D;
            node3D = node3D!.GetChild(1) as Node3D;
            var localTilted = node3D.ToLocal(raycastResult.Point);
            localTilted.X = (int)(localTilted.X) + 0.5f * Mathf.Sign(localTilted.X);
            localTilted.Z = (int)(localTilted.Z) + 0.5f * Mathf.Sign(localTilted.Z);

            return node3D.ToGlobal(localTilted);
        }

        var local = staticBody.ToLocal(raycastResult.Point);

        if (raycastResult.Normal.X != 0)
        {
            local.Y = (int)local.Y + 0.5f * Mathf.Sign(local.Y);
            local.Z = (int)local.Z + 0.5f * Mathf.Sign(local.Z);
        }

        if (raycastResult.Normal.Y != 0)
        {
            local.X = (int)(local.X) + 0.5f * Mathf.Sign(local.X);
            local.Z = (int)(local.Z) + 0.5f * Mathf.Sign(local.Z);
        }

        if (raycastResult.Normal.Z != 0)
        {
            local.X = (int)(local.X) + 0.5f * Mathf.Sign(local.X);
            local.Y = (int)(local.Y) + 0.5f * Mathf.Sign(local.Y);
        }


        return staticBody.ToGlobal(local);
    }

    // Just for a box with scale (1, 1, 1)
    private static Vector3 RigidBodyType(RaycastResult raycastResult, PhysicsBody3D body)
    {
        var local = body.ToLocal(raycastResult.Point);

        if (Mathf.Abs(raycastResult.Normal.X) > 0.5f)
        {
            local.Y = (int)local.Y;
            local.Z = (int)local.Z;
            local.X = raycastResult.Normal.X / 2f;
        }

        if (Mathf.Abs(raycastResult.Normal.Y) > 0.5f)
        {
            local.X = (int)(local.X);
            local.Z = (int)(local.Z);
            local.Y = raycastResult.Normal.Y / 2f;
        }

        if (Mathf.Abs(raycastResult.Normal.Z) > 0.5f)
        {
            local.X = (int)(local.X);
            local.Y = (int)(local.Y);
            local.Z = raycastResult.Normal.Z / 2f;
        }

        return body.ToGlobal(local);
    }

    private static Vector3 UnsupportedType(RaycastResult raycastResult)
    {
        GD.PrintErr("Unsupported surface for rune placement");
        return raycastResult.Point;
    }
}