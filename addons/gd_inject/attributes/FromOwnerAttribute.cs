using System;

namespace wortal_v2.addons.gd_inject.attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class FromOwnerAttribute : Attribute
{
    public string? Name { get; set; }
    public bool FromSelf { get; set; } = false;

    public override bool Equals(object? obj)
    {
        return obj != null && obj is FromOwnerAttribute;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}