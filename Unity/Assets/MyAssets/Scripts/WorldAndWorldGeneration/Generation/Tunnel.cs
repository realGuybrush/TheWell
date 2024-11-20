using System.Collections.Generic;
using UnityEngine;

public class Tunnel
{
    public Vector2 start;
    public Vector2 end;
    public int tunnelWidth;

    public Tunnel(Vector2 Start, Vector2 End, int TunnelWidth, List<List<TileShape>> tiles, bool getNarrower = false)
    {
        start = Start;
        end = End;
        tunnelWidth = TunnelWidth;
        DigTunnel(tiles, getNarrower);
    }

    private void DigTunnel(List<List<TileShape>> tiles, bool getNarrower)
    {
        int distance = (int) GlobalFuncs.Distance2D(start, end);
        if (distance == 0) return;
        float stepX = (end.x - start.x) / distance;
        float stepY = (end.y - start.y) / distance;
        float currentShiftX = -stepX;
        float currentShiftY = -stepY;
        int halfWidth = tunnelWidth / 2;
        for (int i = 0; i < distance; i++)
        {
            currentShiftX += stepX;
            currentShiftY += stepY;
            if(getNarrower)
                halfWidth = tunnelWidth / 2 - (tunnelWidth * i) / (2 * distance);
            DigRoundHole(halfWidth, new Vector2Int((int)(start.x + currentShiftX), (int)(start.y + currentShiftY)) , tiles);
        }
    }

    private void DigRoundHole(int halfWidth, Vector2Int center, List<List<TileShape>> tiles)
    {
        int endY = center.y + halfWidth;
        int endX = center.x + halfWidth;
        for(int y = center.y - halfWidth; y < endY; y++)
        for(int x = center.x - halfWidth; x < endX; x++)
        {
            if (GlobalFuncs.Distance2D(new Vector2Int(x, y), center) < halfWidth)
                if(y >= 0 && y < tiles.Count && x >= 0 && x < tiles[0].Count)
                    tiles[y][x] = TileShape.Empty;
        }
    }
}
