using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventorySpace
{
    [SerializeField] private Vector2Int Size;
    public Vector2Int size
    {
        get
        {
            return Size;
        }
        set
        {
            resize(value);
        }
    }

    [SerializeField] private bool[] occupiedSpots;

    public InventorySpace()
    {
        Size = new Vector2Int(1, 1);
        occupiedSpots = new bool[1];
    }

    public InventorySpace(Vector2Int size)
    {
        Size = size;
        occupiedSpots = new bool[size.x * size.y];
    }

    public InventorySpace(Vector2Int size, bool filled)
    {
        Size = size;
        occupiedSpots = new bool[size.x * size.y];
        if(filled)
        {
            for(int i = 0; i < occupiedSpots.Length; i++)
            {
                occupiedSpots[i] = true;
            }
        }
    }

    public InventorySpace(InventorySpace original)
    {
        Size = original.size;
        occupiedSpots = original.occupiedSpots;
    }

    public InventorySpace(InventorySpace original, Rotation rotation)
    {
        Size = new Vector2Int(rotation.onSide() ? original.size.y : original.size.x,
            rotation.onSide() ? original.size.x : original.size.y);

        occupiedSpots = new bool[size.x * size.y];
        addOther(original, Vector2Int.zero, rotation);
    }

    public InventorySpace(InventorySpace original, Vector2Int size, Vector2Int offset, Rotation rotation = Rotation.Up)
    {
        Size = size;
        occupiedSpots = new bool[size.x * size.y];

        addOther(original, offset, rotation);
    }

    private int index(int x, int y)
    {
        return x + (size.x * y);
    }

    public bool checkOccupied(int x, int y)
    {
        return occupiedSpots[index(x, y)];
    }

    #region Adjust Values
    public void setState(bool value, int x, int y)
    {
        occupiedSpots[index(x, y)] = value;
    }

    public void setEmpty(int x, int y)
    {
        setState(false, x, y);
    }

    public void setOccupied(int x, int y)
    {
        setState(true, x, y);
    }

    private void resize(Vector2Int newSize)
    {
        resize(newSize.x, newSize.y);
    }

    public void resizeX(int x)
    {
        resize(x, Size.y);
    }

    public void resizeY(int y)
    {
        resize(Size.x, y);
    }

    public void resize(int newX, int newY)
    {
        bool[,] grid = lineToGrid(occupiedSpots, Size);

        bool[,] resizedGrid = new bool[newX, newY];
        for(int y = 0; y < newX; y++)
        {
            for(int x = 0; x < newX; x++)
            {
                if(x < grid.GetLength(0) && y < grid.GetLength(1))
                {
                    resizedGrid[x, y] = grid[x, y];
                }
                else
                {
                    resizedGrid[x, y] = false;
                }
            }
        }

        occupiedSpots = gridToLine(resizedGrid);
        Size.x = newX;
        Size.y = newY;
    }

    private bool[,] lineToGrid(bool[] line, Vector2Int size)
    {
        bool[,] grid = new bool[size.x, size.y];

        int index = 0;
        for(int y = 0; y < size.y; y++)
        {
            for(int x = 0; x < size.x; x++)
            {
                if(index < line.Length)
                {
                    grid[x, y] = line[index];
                }
                else
                {
                    grid[x, y] = false;
                }
                index++;
            }
        }

        return grid;
    }

    private bool[] gridToLine(bool[,] grid)
    {
        int x = grid.GetLength(0);
        int y = grid.GetLength(1);
        bool[] line = new bool[x * y];

        for(int i = 0; i < line.Length; i++)
        {
            line[i] = grid[i % x, i / x];
        }

        return line;
    }
    #endregion

    public void addOther(InventorySpace otherGrid, Vector2Int offset, Rotation rotation)
    {
        setOccupiedAtOverlap(true, otherGrid, offset, rotation);
    }

    public void subtractOther(InventorySpace otherGrid, Vector2Int offset, Rotation rotation)
    {
        setOccupiedAtOverlap(false, otherGrid, offset, rotation);
    }

    private void setOccupiedAtOverlap(bool newOccupied, InventorySpace otherGrid, Vector2Int offset, Rotation rotation)
    {
        if(offset.x > size.x || offset.y > size.y)
        {
            return;
        }

        int minX = offset.x < 0 ? 0 : offset.x;
        int minY = offset.y < 0 ? 0 : offset.y;

        int otherX = rotation.onSide() ? otherGrid.size.y : otherGrid.size.x;
        int otherY = rotation.onSide() ? otherGrid.size.x : otherGrid.size.y;

        int maxX = offset.x + otherX > size.x ? size.x : offset.x + otherX;
        int maxY = offset.y + otherY > size.y ? size.y : offset.y + otherY;

        for(int y = minY; y < maxY; y++)
        {
            for(int x = minX; x < maxX; x++)
            {
                int oX = rotation.flipX() ? otherX - x - 1 : x;
                int oY = rotation.flipY() ? otherY - y - 1 : y;

                bool otherOccupied = rotation.onSide() ? otherGrid.checkOccupied(oY, oX) : otherGrid.checkOccupied(oX, oY);

                if(otherOccupied)
                {
                    setState(newOccupied, x, y);
                }
            }
        }
    }

    public Vector2Int[] addOtherCoord(InventorySpace otherGrid, Vector2Int offset, Rotation rotation)
    {
        return setOccupiedAtOverlapAndCoord(true, otherGrid, offset, rotation);
    }

    public Vector2Int[] subtractOtherCoord(InventorySpace otherGrid, Vector2Int offset, Rotation rotation)
    {
        return setOccupiedAtOverlapAndCoord(false, otherGrid, offset, rotation);
    }

    private Vector2Int[] setOccupiedAtOverlapAndCoord(bool newOccupied, InventorySpace otherGrid, Vector2Int offset, Rotation rotation)
    {
        List<Vector2Int> coordinates = new List<Vector2Int>();
        if(offset.x > size.x || offset.y > size.y)
        {
            return coordinates.ToArray();
        }

        int minX = offset.x < 0 ? 0 : offset.x;
        int minY = offset.y < 0 ? 0 : offset.y;

        int otherX = rotation.onSide() ? otherGrid.size.y : otherGrid.size.x;
        int otherY = rotation.onSide() ? otherGrid.size.x : otherGrid.size.y;

        int maxX = offset.x + otherX > size.x ? size.x : offset.x + otherX;
        int maxY = offset.y + otherY > size.y ? size.y : offset.y + otherY;
        
        for(int y = minY; y < maxY; y++)
        {
            for(int x = minX; x < maxX; x++)
            {
                int oX = rotation.flipX() ? otherX - x - 1 : x;
                int oY = rotation.flipY() ? otherY - y - 1 : y;
                Debug.Log(oX + " " + oY);
                Debug.Log(minX + " " + minY);
                bool otherOccupied = rotation.onSide() ? otherGrid.checkOccupied(oY - minY, oX - minX) : otherGrid.checkOccupied(oX - minX, oY - minY);
                
                if (otherOccupied)
                {
                    setState(newOccupied, x, y);
                    coordinates.Add(new Vector2Int(x, y));
                }
            }
        }
        
        return coordinates.ToArray();
    }

    public Vector2Int[] getOverlapCoordinates(InventorySpace otherGrid, Vector2Int offset, Rotation rotation, bool onlyOccupied = false)
    {
        List<Vector2Int> coordinates = new List<Vector2Int>();

        if(offset.x > size.x || offset.y > size.y)
        {
            return coordinates.ToArray();
        }

        int minX = offset.x < 0 ? 0 : offset.x;
        int minY = offset.y < 0 ? 0 : offset.y;

        int otherX = rotation.onSide() ? otherGrid.size.y : otherGrid.size.x;
        int otherY = rotation.onSide() ? otherGrid.size.x : otherGrid.size.y;

        int maxX = offset.x + otherX > size.x ? size.x : offset.x + otherX;
        int maxY = offset.y + otherY > size.y ? size.y : offset.y + otherY;

        for(int y = minY; y < maxY; y++)
        {
            for(int x = minX; x < maxX; x++)
            {
                if(!onlyOccupied || checkOccupied(x, y))
                {
                    int oX = rotation.flipX() ? otherX - x - 1 : x;
                    int oY = rotation.flipY() ? otherY - y - 1 : y;
                    
                    bool otherOccupied = rotation.onSide() ? otherGrid.checkOccupied(oY, oX) : otherGrid.checkOccupied(oX, oY);

                    if(otherOccupied)
                    {
                        coordinates.Add(new Vector2Int(x, y));
                    }
                }
            }
        }

        return coordinates.ToArray();
    }

    public bool checkFitsWithin(InventorySpace otherGrid, Vector2Int offset, Rotation rotation)
    {
        if(offset.x < 0)
            return false;
        if(offset.y < 0)
            return false;

        int compare = offset.x;
        if(!rotation.onSide())
        {
            compare += otherGrid.size.x;
        }
        else
        {
            compare += otherGrid.size.y;
        }

        if(compare > size.x)
            return false;

        compare = offset.y;
        if(!rotation.onSide())
        {
            compare += otherGrid.size.y;
        }
        else
        {
            compare += otherGrid.size.x;
        }

        if(compare > size.y)
            return false;
        return true;
    }

    public bool checkNoOverlap(InventorySpace otherGrid, Vector2Int offset, Rotation rotation)
    {
        if(offset.x > size.x || offset.y > size.y)
        {
            return true;
        }

        int minX = offset.x < 0 ? 0 : offset.x;
        int minY = offset.y < 0 ? 0 : offset.y;

        int otherX = rotation.onSide() ? otherGrid.size.y : otherGrid.size.x;
        int otherY = rotation.onSide() ? otherGrid.size.x : otherGrid.size.y;

        int maxX = offset.x + otherX > size.x ? size.x : offset.x + otherX;
        int maxY = offset.y + otherY > size.y ? size.y : offset.y + otherY;

        for(int y = minY; y < maxY; y++)
        {
            for(int x = minX; x < maxX; x++)
            {
                if(checkOccupied(x, y))
                {
                    int oX = rotation.flipX() ? otherX - x - 1 : x;
                    int oY = rotation.flipY() ? otherY - y - 1 : y;

                    bool otherOccupied = rotation.onSide() ? otherGrid.checkOccupied(oY, oX) : otherGrid.checkOccupied(oX, oY);

                    if(otherOccupied)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    public bool checkAnyOpen(InventorySpace otherGrid, Vector2Int offset, Rotation rotation)
    {
        if(offset.x > size.x || offset.y > size.y)
        {
            return false;
        }

        int minX = offset.x < 0 ? 0 : offset.x;
        int minY = offset.y < 0 ? 0 : offset.y;

        int otherX = rotation.onSide() ? otherGrid.size.y : otherGrid.size.x;
        int otherY = rotation.onSide() ? otherGrid.size.x : otherGrid.size.y;

        int maxX = offset.x + otherX > size.x ? size.x : offset.x + otherX;
        int maxY = offset.y + otherY > size.y ? size.y : offset.y + otherY;

        for(int y = minY; y < maxY; y++)
        {
            for(int x = minX; x < maxX; x++)
            {
                if(!checkOccupied(x, y))
                {
                    int oX = rotation.flipX() ? otherX - x - 1 : x;
                    int oY = rotation.flipY() ? otherY - y - 1 : y;

                    bool otherOccupied = rotation.onSide() ? otherGrid.checkOccupied(oY, oX) : otherGrid.checkOccupied(oX, oY);

                    if(otherOccupied)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public bool checkFitAndOverlap(InventorySpace otherGrid, Vector2Int offset, Rotation rotation)
    {
        return checkFitsWithin(otherGrid, offset, rotation) && checkNoOverlap(otherGrid, offset, rotation);
    }
}
