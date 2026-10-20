using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FallManager : MonoBehaviour
{
    private TileMatrix _tileMatrix;

    private int _fallTriggerCounter;
    private Coroutine _arrangeFallRoutine;

    private void Start()
    {
        // EventManager.OnGameItemBroken += GameItemBrokenEventHandler;
        EventManager.OnGameItemReleasedFromTile += GameItemReleasedFromTileEventHandler;
    }

    private void Update()
    {
        if (_fallTriggerCounter > 0 && _arrangeFallRoutine == null)
        {
            _arrangeFallRoutine = StartCoroutine(ArrangeFall());
        }
    }

    private void OnDestroy()
    {
        // EventManager.OnGameItemBroken -= GameItemBrokenEventHandler;
        EventManager.OnGameItemReleasedFromTile -= GameItemReleasedFromTileEventHandler;
    }

    public void Configure(TileMatrix tileMatrix)
    {
        _tileMatrix = tileMatrix;
    }

    private IEnumerator ArrangeFall()
    {
        var width = _tileMatrix.width;
        var height = _tileMatrix.height;

        var source2Destination = new Dictionary<Tile, Tile>();

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var destinationTile = _tileMatrix[i, j];

                if (destinationTile.GetGameItem() != null || destinationTile.reservedAsFallDestination)
                {
                    continue;
                }
                
                for (int k = j; k < height; k++)
                {
                    var sourceTile = _tileMatrix[i, k];

                    if (sourceTile.HasBlockingGameItem())
                    {
                        // there's an obstacle, nothing falls down
                        break;
                    }
                    
                    if (sourceTile.HasFallingGameItem() && 
                        !source2Destination.ContainsKey(sourceTile) &&
                        !source2Destination.ContainsValue(destinationTile))
                    {
                        source2Destination.Add(sourceTile, destinationTile);
                        yield return null;
                    }
                }
            }
        }

        foreach (var (source, destination) in source2Destination)
        {
            if (destination.reservedAsFallDestination)
            {
                continue;
            }
            // Debug.Log($"({source.xIndex}, {source.yIndex}) -> ({destination.xIndex}, {destination.yIndex})");
            source.FallToDestination(destination);
        }

        _fallTriggerCounter--;
        _arrangeFallRoutine = null;
    }
    
    private void GameItemBrokenEventHandler(object sender, EventManager.GameItemBrokenEventArgs args)
    {
        if (_fallTriggerCounter < 5)
        {
            _fallTriggerCounter++;
        }
        
        // ArrangeFall();
    }

    private void GameItemReleasedFromTileEventHandler(
        object sender, EventManager.GameItemReleasedFromTileEventArgs args)
    {
        if (_fallTriggerCounter < 5)
        {
            _fallTriggerCounter++;
        }
    }
}
