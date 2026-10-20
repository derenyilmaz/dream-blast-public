
using System;
using System.Collections.Generic;
using UnityEngine;
using static Constants;
using Random = UnityEngine.Random;

[RequireComponent(typeof(TouchController))]
[RequireComponent(typeof(LevelManager))]
[RequireComponent(typeof(FallManager))]
public class Board : MonoBehaviour
{
    private TileMatrix _tileMatrix;
    
    private TouchController _touchController;
    private LevelManager _levelManager;
    private FallManager _fallManager;

    private PrefabLibrary _prefabLibrary;
    
    private void Awake()
    {
        _touchController = GetComponent<TouchController>();
        _levelManager = GetComponent<LevelManager>();
        _fallManager = GetComponent<FallManager>();
        
        _tileMatrix = new TileMatrix();
        
        _prefabLibrary = Resources.Load<PrefabLibrary>("PrefabLibrary");
    }

    private void Start()
    {
        EventManager.OnCubeTapped += CubeTappedEventHandler;
        EventManager.OnTntTapped += TntTappedEventHandler;
        
        _levelManager.Configure(_tileMatrix);
        _fallManager.Configure(_tileMatrix);
    }
    

    private void OnDestroy()
    {
        EventManager.OnCubeTapped -= CubeTappedEventHandler;
        EventManager.OnTntTapped -= TntTappedEventHandler;
    }


    private void CubeTappedEventHandler(object sender, EventManager.CubeTappedEventArgs eventArgs)
    {
        var tappedCube = eventArgs.Cube;
        
        if (tappedCube == null)
        {
            return;
        }

        var startTile = _tileMatrix[tappedCube.xIndex, tappedCube.yIndex];
        var predicate = new Func<Tile, bool>(tile => tile.GetGameItem() != null && 
                                                     tile.GetGameItem().gameItemType == tappedCube.gameItemType);
        
        var connectedTilesWithSameCubeType = _tileMatrix.GetConnectedTilesWithPredicate(startTile, predicate);
        
        if (connectedTilesWithSameCubeType.Count < MinConnectedCubeCountForLegalMove)
        {
            return;
        }

        EventManager.MoveSpent();
        
        var boundaryTiles = _tileMatrix.GetBoundaryTiles(connectedTilesWithSameCubeType);
        foreach (var tile in boundaryTiles)
        {
            tile.GetGameItem()?.TakeDamageFromBoundary();
        }

        if (connectedTilesWithSameCubeType.Count < MinConnectedCubeCountForTntCreation)
        {
            RegularMove(connectedTilesWithSameCubeType);
            return;
        }
        
        TntCreatingMove(connectedTilesWithSameCubeType, startTile);
    }

    private void RegularMove(HashSet<Tile> tileList)
    {
        foreach (var tile in tileList)
        {
            var connectedCube = tile.GetGameItem();
            if (connectedCube == null)
            {
                continue;
            }
            
            connectedCube.Break();
        }
    }

    private void TntCreatingMove(HashSet<Tile> tileList, Tile startTile)
    {
        foreach (var tile in tileList)
        {
            var connectedCube = tile.GetGameItem();
            if (connectedCube == null)
            {
                continue;
            }
            
            connectedCube.Break();
            tile.PurgeGameItem();
        }

        var tntPrefab = _prefabLibrary.GetGameItemPrefabFromType(GameItemType.Tnt);

        var tnt = Instantiate(tntPrefab) as Tnt;

        if (tnt == null)
        {
            return;
        }
        
        startTile.AttachGameItemToTile(tnt);
    }

    private void TntTappedEventHandler(object sender, EventManager.TntTappedEventArgs args)
    {
        var tappedTnt = args.Tnt;
        var startTile = _tileMatrix[tappedTnt.xIndex, tappedTnt.yIndex];
        var predicate = new Func<Tile, bool>(tile => tile.GetGameItem() != null && 
                                                     tile.GetGameItem().gameItemType == tappedTnt.gameItemType);
        
        var connectedTntTiles = _tileMatrix.GetConnectedTilesWithPredicate(startTile, predicate);
        if (connectedTntTiles.Count < 2)
        {
            tappedTnt.Explode(isCombo: false);
        }
        else
        {
            foreach (var tile in connectedTntTiles)
            {
                // todo: make them gather at tap position
                var neighborTnt = tile.GetGameItem();
                if (neighborTnt == tappedTnt)
                {
                    continue;
                }

                Destroy(neighborTnt.gameObject);
            }
            
            tappedTnt.Explode(isCombo: true);
        }
        
        EventManager.MoveSpent();
    }
}                                       
