using System;
using UnityEngine;

[Serializable]
public class SpriteRef : IEquatable<SpriteRef>
{
    public bool UseConstant = true;
    public Sprite ConstantValue;
    public SpriteVariable Variable;

    public Sprite Value
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

    public SpriteRef()
    {
        UseConstant = true;
        ConstantValue = default;
    }

    public SpriteRef(Sprite value)
    {
        UseConstant = true;
        ConstantValue = value;
    }

    public SpriteRef(SpriteVariable value)
    {
        UseConstant = false;
        Variable = value;
    }

    public static implicit operator Sprite(SpriteRef v)
    {
        if(v == null) return default;
        return v.Value;
    }

    public static bool operator ==(SpriteRef v1, SpriteRef v2)
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

    public static bool operator !=(SpriteRef v1, SpriteRef v2) => !(v1 == v2);

    public bool Equals(SpriteRef other)
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
        return (obj is SpriteRef @ref && Value.Equals(@ref.Value)) || (obj is Sprite @c && Value.Equals(@c));
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value);
    }
}