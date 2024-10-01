using System;
using TMPro;

[Serializable]
public class IntRef : IEquatable<IntRef>, IComparable<IntRef>
{
    public bool UseConstant = true;
    public int ConstantValue;
    public IntVariable Variable;

    public int Value
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

    public IntRef()
    {
        UseConstant = true;
        ConstantValue = default;
    }

    public IntRef(int value)
    {
        UseConstant = true;
        ConstantValue = value;
    }

    public IntRef(IntVariable value)
    {
        UseConstant = false;
        Variable = value;
    }

    public static implicit operator int(IntRef v)
    {
        if(v == null) return default;
        return v.Value;
    }

    public static bool operator ==(IntRef v1, IntRef v2)
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

    public static bool operator !=(IntRef v1, IntRef v2) => !(v1 == v2);

    public static bool operator >(IntRef v1, IntRef v2)
    {
        if(v1 is null)
        {
            if(v2 is null)
            {
                return false;
            }
            return default(int) > v2.Value;
        }
        else if(v2 is null)
        {
            return v1.Value > default(int);
        }
        return v1.Value > v2.Value;
    }

    public static bool operator <(IntRef v1, IntRef v2)
    {
        if(v1 is null)
        {
            if(v2 is null)
            {
                return false;
            }
            return default(int) < v2.Value;
        }
        else if(v2 is null)
        {
            return v1.Value < default(int);
        }
        return v1.Value < v2.Value;
    }

    public static bool operator >=(IntRef v1, IntRef v2)
    {
        if(v1 is null)
        {
            if(v2 is null)
            {
                return true;
            }
            return default(int) >= v2.Value;
        }
        else if(v2 is null)
        {
            return v1.Value >= default(int);
        }
        return v1.Value >= v2.Value;
    }

    public static bool operator <=(IntRef v1, IntRef v2)
    {
        if(v1 is null)
        {
            if(v2 is null)
            {
                return true;
            }
            return default(int) <= v2.Value;
        }
        else if(v2 is null)
        {
            return v1.Value <= default(int);
        }
        return v1.Value <= v2.Value;
    }

    public bool Equals(IntRef other)
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
        return (obj is IntRef @ref && Value.Equals(@ref.Value)) || (obj is int @c && Value.Equals(@c));
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value);
    }

    public int CompareTo(IntRef other)
    {
        return Value.CompareTo(other.Value);
    }
}