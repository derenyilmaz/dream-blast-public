using System;
using DG.Tweening;
using UnityEngine;
using static Constants;

public class Tile : MonoBehaviour
{
    public int xIndex;
    public int yIndex;

    public bool reservedAsFallDestination;
    
    private GameItem _gameItem;
    private GenerationComponent _generationComponent;
    

    private void Start()
    {
        EventManager.OnGameItemBroken += GameItemBrokenEventHandler;
        
        _generationComponent = GetComponent<GenerationComponent>();
    }

    private void OnDestroy()
    {
        EventManager.OnGameItemBroken -= GameItemBrokenEventHandler;
    }

    private void GameItemBrokenEventHandler(object sender, EventManager.GameItemBrokenEventArgs args)
    {
        if (_gameItem == args.GameItem)
        {
            PurgeGameItem();
        }
    }

    public GameItem GetGameItem()
    {
        return _gameItem;
    }

    public void AttachGameItemToTile(GameItem gameItem)
    {
        _gameItem = gameItem;
        _gameItem.xIndex = xIndex;
        _gameItem.yIndex = yIndex;
        _gameItem.SetSortingOrder(yIndex);
        _gameItem.transform.position = transform.position;
        
        reservedAsFallDestination = false;
    }

    public void PurgeGameItem()
    {
        EventManager.GameItemReleasedFromTile(_gameItem, this);
            
        _gameItem = null;

        if (_generationComponent == null)
        {
            return;
        }

        GenerateRandomCube();
    }

    public void Tap()
    {
        if (_gameItem != null)
        { 
            _gameItem.Tap();
        }
    }

    public void FallToDestination(Tile destinationTile)
    {
        if (_gameItem == null)
        {
            return;
        }
        
        var gameItem = _gameItem;
        PurgeGameItem();
        
        destinationTile.reservedAsFallDestination = true;
        var duration = (yIndex - destinationTile.yIndex) * GameItemOneTileFallDurationInSeconds;
        gameItem.transform.DOMoveY(destinationTile.transform.position.y, duration).OnComplete(
            () => destinationTile.AttachGameItemToTile(gameItem)).SetEase(Ease.OutBack);
    }

    public bool HasFallingGameItem()
    {
        return _gameItem != null && _gameItem.canFallDown;
    }

    public bool HasBlockingGameItem()
    {
        return _gameItem != null && !_gameItem.canFallDown;
    }

    private void GenerateRandomCube()
    {
        var nextItem = _generationComponent.GetNextItem();

        nextItem.transform.DOMoveY(transform.position.y, GameItemOneTileFallDurationInSeconds).SetEase(Ease.OutBack);
            
        nextItem.xIndex = xIndex;
        nextItem.yIndex = yIndex;
        AttachGameItemToTile(nextItem);
    }
}
