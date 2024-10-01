using System;
using UnityEngine;

[Serializable]
public class Vector3IntRef : IEquatable<Vector3IntRef>
{
    public bool UseConstant = true;
    public Vector3Int ConstantValue;
    public Vector3IntVariable Variable;

    public Vector3Int Value
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

    public Vector3IntRef()
    {
        UseConstant = true;
        ConstantValue = default;
    }

    public Vector3IntRef(Vector3Int value)
    {
        UseConstant = true;
        ConstantValue = value;
    }

    public Vector3IntRef(Vector3IntVariable value)
    {
        UseConstant = false;
        Variable = value;
    }

    public static implicit operator Vector3Int(Vector3IntRef v)
    {
        if(v == null) return default;
        return v.Value;
    }

    public static bool operator ==(Vector3IntRef v1, Vector3IntRef v2)
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

    public static bool operator !=(Vector3IntRef v1, Vector3IntRef v2) => !(v1 == v2);

    public bool Equals(Vector3IntRef other)
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
        return (obj is Vector3IntRef @ref && Value.Equals(@ref.Value)) || (obj is Vector3Int @c && Value.Equals(@c));
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value);
    }
}