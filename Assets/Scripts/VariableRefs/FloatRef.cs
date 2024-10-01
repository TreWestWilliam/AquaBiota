using System;

[Serializable]
public class FloatRef : IEquatable<FloatRef>, IComparable<FloatRef>
{
    public bool UseConstant = true;
    public float ConstantValue;
    public FloatVariable Variable;

    public float Value
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

    public FloatRef()
    {
        UseConstant = true;
        ConstantValue = default;
    }

    public FloatRef(float value)
    {
        UseConstant = true;
        ConstantValue = value;
    }

    public FloatRef(FloatVariable value)
    {
        UseConstant = false;
        Variable = value;
    }

    public static implicit operator float(FloatRef v)
    {
        if(v == null) return default;
        return v.Value;
    }

    public static bool operator ==(FloatRef v1, FloatRef v2)
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

    public static bool operator >(FloatRef v1, FloatRef v2)
    {
        if(v1 is null)
        {
            if(v2 is null)
            {
                return false;
            }
            return default(float) > v2.Value;
        }
        else if(v2 is null)
        {
            return v1.Value > default(float);
        }
        return v1.Value > v2.Value;
    }

    public static bool operator <(FloatRef v1, FloatRef v2)
    {
        if(v1 is null)
        {
            if(v2 is null)
            {
                return false;
            }
            return default(float) < v2.Value;
        }
        else if(v2 is null)
        {
            return v1.Value < default(float);
        }
        return v1.Value < v2.Value;
    }

    public static bool operator >=(FloatRef v1, FloatRef v2)
    {
        if(v1 is null)
        {
            if(v2 is null)
            {
                return true;
            }
            return default(float) >= v2.Value;
        }
        else if(v2 is null)
        {
            return v1.Value >= default(float);
        }
        return v1.Value >= v2.Value;
    }

    public static bool operator <=(FloatRef v1, FloatRef v2)
    {
        if(v1 is null)
        {
            if(v2 is null)
            {
                return true;
            }
            return default(float) <= v2.Value;
        }
        else if(v2 is null)
        {
            return v1.Value <= default(float);
        }
        return v1.Value <= v2.Value;
    }

    public static bool operator !=(FloatRef v1, FloatRef v2) => !(v1 == v2);

    public bool Equals(FloatRef other)
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
        return (obj is FloatRef @ref && Value.Equals(@ref.Value)) || (obj is float @c && Value.Equals(@c));
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value);
    }

    public int CompareTo(FloatRef other)
    {
        return Value.CompareTo(other.Value);
    }
}