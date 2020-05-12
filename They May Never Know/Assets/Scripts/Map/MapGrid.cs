using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGrid
{
    public int width { get; private set; }
    public int height { get; private set; }
    public int gridWidth { get; private set; }
    public int gridHeight { get; private set; }
    float xOffset;
    float yOffset;

    public Vector2[,] Map { get; private set; }

    public MapGrid(int width, int height, int gridWidth, int gridHeight)
    {
        this.width = width;
        this.height = height;
        this.gridWidth = gridWidth;
        this.gridHeight = gridHeight;
        xOffset = -width * gridWidth;
        yOffset = -height * gridHeight;
        Map = new Vector2[width, height];
        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                Map[w, h] = new Vector2((gridWidth * h) + w * gridWidth + xOffset, (-w * (gridHeight)) + h * gridHeight);
            }
        }
    }
}
