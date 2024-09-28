public enum Rotation
{
    Up,
    Left,
    Down,
    Right,
    FlipUp,
    FlipLeft,
    FlipDown,
    FlipRight
}

static class RotationFunctions
{
    public static Rotation clockwise(this Rotation initial)
    {
        switch(initial)
        {
            case Rotation.Up:
                return Rotation.Right;
            case Rotation.FlipUp:
                return Rotation.FlipRight;
            default:
                return (Rotation)((int)initial - 1);
        }
    }
    public static Rotation widdershins(this Rotation initial)
    {
        switch(initial)
        {
            case Rotation.Right:
                return Rotation.Up;
            case Rotation.FlipRight:
                return Rotation.FlipUp;
            default:
                return (Rotation)((int)initial + 1);
        }
    }

    public static Rotation oneEighty(this Rotation initial)
    {
        int value = (int)initial;
        bool flipped = value > 3;

        return (Rotation)((value + (flipped ? -2 : 2)) % 4 + (flipped ? 4 : 0));
    }

    public static Rotation flipHorizontal(this Rotation initial)
    {
        switch(initial)
        {
            case Rotation.Up:
                return Rotation.FlipDown;
            case Rotation.Left:
                return Rotation.FlipLeft;
            case Rotation.Down:
                return Rotation.FlipUp;
            case Rotation.Right:
                return Rotation.FlipRight;
            case Rotation.FlipUp:
                return Rotation.Down;
            case Rotation.FlipLeft:
                return Rotation.Left;
            case Rotation.FlipDown:
                return Rotation.Up;
            default:
                return Rotation.Right;
        }
    }

    public static Rotation flipVertical(this Rotation initial)
    {
        switch(initial)
        {
            case Rotation.Up:
                return Rotation.FlipUp;
            case Rotation.Left:
                return Rotation.FlipRight;
            case Rotation.Down:
                return Rotation.FlipDown;
            case Rotation.Right:
                return Rotation.FlipLeft;
            case Rotation.FlipUp:
                return Rotation.Up;
            case Rotation.FlipLeft:
                return Rotation.Right;
            case Rotation.FlipDown:
                return Rotation.Down;
            default:
                return Rotation.Left;
        }
    }

    public static Rotation relativeRotation(this Rotation reference, Rotation compared)
    {
        switch(reference)
        {
            case Rotation.Up:
                return compared;
            case Rotation.Left:
                return compared.clockwise();
            case Rotation.Down:
                return compared.oneEighty();
            case Rotation.Right:
                return compared.widdershins();
            case Rotation.FlipUp:
                return compared.flipVertical();
            case Rotation.FlipLeft:
                return compared.flipVertical().widdershins();
            case Rotation.FlipDown:
                return compared.flipHorizontal();
            default:
                return compared.flipHorizontal().widdershins();
        }
    }

    public static bool onSide(this Rotation rotation)
    {
        return (int)rotation % 2 == 1;
    }

    public static bool flipped(this Rotation rotation)
    {
        return (int)rotation > 3;
    }

    public static bool flipX(this Rotation rotation)
    {
        switch(rotation)
        {
            case Rotation.Up:
                return false;
            case Rotation.Left:
                return false;
            case Rotation.FlipUp:
                return false;
            case Rotation.FlipRight:
                return false;
            default:
                return true;
        }
    }

    public static bool flipY(this Rotation rotation)
    {
        switch(rotation)
        {
            case Rotation.Up:
                return false;
            case Rotation.Right:
                return false;
            case Rotation.FlipDown:
                return false;
            case Rotation.FlipRight:
                return false;
            default:
                return true;
        }
    }

    public static Rotation unFlippedRotation(this Rotation rotation)
    {
        if((int)rotation > 3)
        {
            return rotation.flipVertical();
        }
        return rotation;
    }
}