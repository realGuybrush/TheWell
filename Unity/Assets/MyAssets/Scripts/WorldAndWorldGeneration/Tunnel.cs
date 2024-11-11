using System.Collections.Generic;
using UnityEngine;

public class Tunnel
{
    public Vector2 start;
    public Vector2 end;
    public int tunnelWidth;

    public Tunnel(Vector2 Start, Vector2 End, int TunnelWidth, List<List<TileType>> tiles, bool getNarrower = false)
    {
        start = Start;
        end = End;
        tunnelWidth = TunnelWidth;
        DigTunnel(tiles, getNarrower);
    }

    private void DigTunnel(List<List<TileType>> tiles, bool getNarrower)
    {
        int distance = (int) GlobalFuncs.Distance2D(start, end);
        if (distance == 0) return;
        int indexX, indexY;
        int width = tiles[0].Count;
        int height = tiles.Count;
        float stepX = (end.x - start.x) / distance;
        float stepY = (end.y - start.y) / distance;
        float currentStepX = -stepX;
        float currentStepY = -stepY;
        int halfWidth = tunnelWidth / 2;
        for (int i = 0; i < distance; i++)
        {
            currentStepX += stepX;
            currentStepY += stepY;
            if(getNarrower)
                halfWidth = tunnelWidth / 2 - (tunnelWidth * i) / (2 * distance);
            for(int digY = -halfWidth; digY < halfWidth; digY++)
            for(int digX = -halfWidth; digX < halfWidth; digX++)
            {
                indexX = (int)(start.x + currentStepX + digX);
                indexY = (int)(start.y + currentStepY + digY);
                if (GlobalFuncs.Distance2D(new Vector2Int(indexX, indexY),
                    new Vector2(start.x + currentStepX, start.y + currentStepY)) < halfWidth)
                    if(indexY >= 0 && indexY < height && indexX >= 0 && indexX < width)
                        tiles[indexY][indexX] = TileType.Empty;
            }
        }
    }
}
