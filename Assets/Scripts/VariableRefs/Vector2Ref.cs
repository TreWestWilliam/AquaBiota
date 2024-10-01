using System;
using UnityEngine;

[Serializable]
public class Vector2Ref : IEquatable<Vector2Ref>
{
    public bool UseConstant = true;
    public Vector2 ConstantValue;
    public Vector2Variable Variable;

    public Vector2 Value
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

    public Vector2Ref()
    {
        UseConstant = true;
        ConstantValue = default;
    }

    public Vector2Ref(Vector2 value)
    {
        UseConstant = true;
        ConstantValue = value;
    }

    public Vector2Ref(Vector2Variable value)
    {
        UseConstant = false;
        Variable = value;
    }

    public static implicit operator Vector2(Vector2Ref v)
    {
        if(v == null) return default;
        return v.Value;
    }

    public static bool operator ==(Vector2Ref v1, Vector2Ref v2)
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

    public static bool operator !=(Vector2Ref v1, Vector2Ref v2) => !(v1 == v2);

    public bool Equals(Vector2Ref other)
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
        return (obj is Vector2Ref @ref && Value.Equals(@ref.Value)) || (obj is Vector2 @c && Value.Equals(@c));
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value);
    }
}