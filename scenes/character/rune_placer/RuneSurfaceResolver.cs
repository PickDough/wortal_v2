using Godot;
using wortal_v2.addons.utils;

namespace wortal_v2.scenes.character.rune_placer;

public static class RuneSurfaceResolver
{
    internal static Vector3 ResolveSurface(RaycastResult raycastResult)
    {
        return raycastResult.Collider switch
        {
            CsgBox3D csgBox => CsgBoxType(raycastResult, csgBox),
            StaticBody3D staticBody => StaticBodyType(raycastResult, staticBody),
            _ => UnsupportedType(raycastResult),
        };
    }

    private static Vector3 CsgBoxType(RaycastResult raycastResult, CsgBox3D csgBox)
    {
        var local = csgBox.ToLocal(raycastResult.Point);
        if (raycastResult.Normal.X != 0)
        {
            if ((int)csgBox.Size.Y % 2 == 0)
                local.Y = Mathf.Floor(local.Y) + 0.5f;
            else
                local.Y = Mathf.Floor(local.Y) + 1;
            if ((int)csgBox.Size.Z % 2 == 0)
                local.Z = Mathf.Floor(local.Z) + 0.5f;
            else
                local.Z = Mathf.Floor(local.Z) + 1;
        }

        if (raycastResult.Normal.Y != 0)
        {
            if ((int)csgBox.Size.X % 2 == 0)
                local.X = Mathf.Floor(local.X) + 0.5f;
            else
                local.X = Mathf.Floor(local.X) + 1;
            if ((int)csgBox.Size.Z % 2 == 0)
                local.Z = Mathf.Floor(local.Z) + 0.5f;
            else
                local.Z = Mathf.Floor(local.Z) + 1;
        }

        if (raycastResult.Normal.Z != 0)
        {
            if ((int)csgBox.Size.X % 2 == 0)
                local.X = Mathf.Floor(local.X) + 0.5f;
            else
                local.X = Mathf.Floor(local.X) + 1;
            if ((int)csgBox.Size.Y % 2 == 0)
                local.Y = Mathf.Floor(local.Y) + 0.5f;
            else
                local.Y = Mathf.Floor(local.Y) + 1;
        }


        return csgBox.ToGlobal(local);
    }

    private static Vector3 StaticBodyType(RaycastResult raycastResult, StaticBody3D staticBody)
    {
        var local = staticBody.ToLocal(raycastResult.Point);
        var beforeLocal = local;
        GD.Print(raycastResult.Normal);

        if (raycastResult.Normal.X != 0)
        {
            local.Y = (int)local.Y + 0.5f  * Mathf.Sign(local.Y);
            local.Z = (int)local.Z + 0.5f * Mathf.Sign(local.Z);
        } 
        else if (raycastResult.Normal.Y != 0)
        {
            local.X = (int)(local.X) + 0.5f * Mathf.Sign(local.X);
            local.Z = (int)(local.Z) + 0.5f * Mathf.Sign(local.Z);
        }
        else if (raycastResult.Normal.Z != 0)
        {
            local.X =(int)(local.X) + 0.5f * Mathf.Sign(local.X);
            local.Y = (int)(local.Y) + 0.5f * Mathf.Sign(local.Y);
        }
        
        return staticBody.ToGlobal(local);
    }

    private static Vector3 UnsupportedType(RaycastResult raycastResult)
    {
        GD.PrintErr("Unsupported surface for rune placement");
        return raycastResult.Point;
    }
}