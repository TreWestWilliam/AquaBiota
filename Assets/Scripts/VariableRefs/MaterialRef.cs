using System;
using UnityEngine;

[Serializable]
public class MaterialRef : IEquatable<MaterialRef>
{
    public bool UseConstant = true;
    public Material ConstantValue;
    public MaterialVariable Variable;

    public Material Value
    {
        get { return UseConstant ? ConstantValue : (Variable != null) ? Variable.Value : default; }
        set
        {
            if(UseConstant)
                ConstantValue = value;
            else
                Variable.Value = value;
        }
    }

    public MaterialRef()
    {
        UseConstant = true;
        ConstantValue = default;
    }

    public MaterialRef(Material value)
    {
        UseConstant = true;
        ConstantValue = value;
    }

    public MaterialRef(MaterialVariable value)
    {
        UseConstant = false;
        Variable = value;
    }

    public static implicit operator Material(MaterialRef v)
    {
        if(v == null) return default;
        return v.Value;
    }

    public static bool operator ==(MaterialRef v1, MaterialRef v2)
    {
        if(v1 is null)
        {
            if(v2 is null)
            {
                return true;
            }
            return false;
        }
        else if(v2 is null)
        {
            return false;
        }
        return v1.Value == v2.Value;
    }

    public static bool operator !=(MaterialRef v1, MaterialRef v2) => !(v1 == v2);

    public bool Equals(MaterialRef other)
    {
        if(this is null)
        {
            if(other is null)
            {
                return true;
            }
            return false;
        }

        return Value.Equals(other.Value);
    }

    public override bool Equals(object obj)
    {
        return (obj is MaterialRef @ref && Value.Equals(@ref.Value)) || (obj is Material @c && Value.Equals(@c));
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value);
    }
}