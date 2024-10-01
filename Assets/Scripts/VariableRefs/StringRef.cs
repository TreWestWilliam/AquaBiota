using System;
using Unity.VisualScripting;

[Serializable]
public class StringRef : IEquatable<StringRef>, IComparable<StringRef>
{
    public bool UseConstant = true;
    public string ConstantValue;
    public StringVariable Variable;

    public string Value
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

    public StringRef()
    {
        UseConstant = true;
        ConstantValue = default;
    }

    public StringRef(string value)
    {
        UseConstant = true;
        ConstantValue = value;
    }

    public StringRef(StringVariable value)
    {
        UseConstant = false;
        Variable = value;
    }

    public static implicit operator string(StringRef v)
    {
        if(v == null) return default;
        return v.Value;
    }

    public static bool operator ==(StringRef v1, StringRef v2)
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

    public static bool operator !=(StringRef v1, StringRef v2) => !(v1 == v2);

    public bool Equals(StringRef other)
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
        return (obj is StringRef @ref && Value.Equals(@ref.Value)) || (obj is string @c && Value.Equals(@c));
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value);
    }

    public int CompareTo(StringRef other)
    {
        return Value.CompareTo(other.Value);
    }
}