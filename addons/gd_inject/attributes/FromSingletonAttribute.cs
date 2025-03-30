using System;

namespace wortal_v2.addons.gd_inject.attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class FromSingletonAttribute : Attribute
{
    public override bool Equals(object? obj)
    {
        return obj != null && obj is FromSingletonAttribute;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}