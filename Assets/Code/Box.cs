using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box
{
    private int indexX;
    private int indexY;
    private int number;
    private bool hasNumber;
    private bool isMerged;

    public int Number { get => number; set => number = value; }
    public int IndexX { get => indexX; set => indexX = value; }
    public int IndexY { get => indexY; set => indexY = value; }
    public bool HasNumber { get => hasNumber; set => hasNumber = value; }
    public bool IsMerged { get => isMerged; set => isMerged = value; }

    public Box()
    {
        this.HasNumber = false;
    }
    public Box(int indexX, int indexY)
    {
        this.indexX = indexX;
        this.indexY = indexY;
    }
    public Box(int x, int y, int number) : this(x, y)
    {
        this.number = number;
        this.HasNumber = true;
    }
}
