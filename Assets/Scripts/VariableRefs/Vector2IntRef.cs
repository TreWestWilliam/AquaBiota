using System;
using UnityEngine;

[Serializable]
public class Vector2IntRef : IEquatable<Vector2IntRef>
{
    public bool UseConstant = true;
    public Vector2Int ConstantValue;
    public Vector2IntVariable Variable;

    public Vector2Int Value
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

    public Vector2IntRef()
    {
        UseConstant = true;
        ConstantValue = default;
    }

    public Vector2IntRef(Vector2Int value)
    {
        UseConstant = true;
        ConstantValue = value;
    }

    public Vector2IntRef(Vector2IntVariable value)
    {
        UseConstant = false;
        Variable = value;
    }

    public static implicit operator Vector2Int(Vector2IntRef v)
    {
        if(v == null) return default;
        return v.Value;
    }

    public static bool operator ==(Vector2IntRef v1, Vector2IntRef v2)
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

    public static bool operator !=(Vector2IntRef v1, Vector2IntRef v2) => !(v1 == v2);

    public bool Equals(Vector2IntRef other)
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
        return (obj is Vector2IntRef @ref && Value.Equals(@ref.Value)) || (obj is Vector2Int @c && Value.Equals(@c));
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value);
    }
}