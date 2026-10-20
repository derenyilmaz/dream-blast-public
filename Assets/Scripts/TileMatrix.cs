
using System;
using System.Collections.Generic;

public class TileMatrix
{
    // todo: rename this
    private Tile[,] Matrix { get;  set; }

    public int width;
    public int height;

    public TileMatrix()
    {
        
    }
    
    public Tile this[int i, int j]
    {
        get => Matrix[i, j];
        set => Matrix[i, j] = value;
    }

    public void SetTiles(Tile[,] tiles)
    {
        Matrix = tiles;
        width = tiles.GetLength(0);
        height = tiles.GetLength(1);
    }
    
    public HashSet<Tile> GetConnectedTilesWithPredicate(Tile startingTile, Func<Tile, bool> predicate)
    {
        var connectedTiles = new HashSet<Tile> { startingTile };
        var tileQueue = new Queue<Tile>();
        var visited = new bool[width, height];
        
        tileQueue.Enqueue(startingTile);

        while (tileQueue.Count > 0)
        {
            var currentTile = tileQueue.Dequeue();

            if (visited[currentTile.xIndex, currentTile.yIndex] || !predicate(currentTile))
            {
                continue;
            }
            
            visited[currentTile.xIndex, currentTile.yIndex] = true;
            connectedTiles.Add(currentTile);
            
            foreach (var neighborTile in GetNeighborTiles(currentTile))
            {
                tileQueue.Enqueue(neighborTile);
            }
        }
        
        return connectedTiles;
    }
    
    public HashSet<Tile> GetBoundaryTiles(HashSet<Tile> tiles)
    {
        var boundaryTiles = new HashSet<Tile> { };

        foreach (var tile in tiles)
        {
            var neighbors = GetNeighborTiles(tile);
            
            foreach (var neighborTile in neighbors)
            {
                if (tiles.Contains(neighborTile))
                {
                    continue;
                }

                boundaryTiles.Add(neighborTile);
            }
        }
        
        return boundaryTiles;
    }

    private IEnumerable<Tile> GetNeighborTiles(Tile baseTile)
    {
        foreach (var (neighborX, neighborY) in GetValidNeighboringIndices(baseTile.xIndex, baseTile.yIndex))
        {
            yield return Matrix[neighborX, neighborY];
        }
    }

    private IEnumerable<(int, int)> GetValidNeighboringIndices(int x, int y)
    {
        if (x > 0)
        {
            yield return (x - 1, y);
        }

        if (x < width - 1)
        {
            yield return (x + 1, y);
        }
        
        if (y > 0)
        {
            yield return (x, y - 1);
        }

        if (y < height - 1)
        {
            yield return (x, y + 1);
        }
    }
}
