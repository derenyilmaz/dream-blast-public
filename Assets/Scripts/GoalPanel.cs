using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPanel : MonoBehaviour
{
    [SerializeField] private Transform gridParent;
    [SerializeField] private GoalItem goalItemPrefab;

    private Dictionary<Constants.GameItemType, GoalItem> _goalType2GoalItem = new();
    private PrefabLibrary _prefabLibrary;

    private void Awake()
    {
        _prefabLibrary = Resources.Load<PrefabLibrary>("PrefabLibrary");

        EventManager.OnLevelStarted += LevelStartedEventHandler;
        EventManager.OnGameItemBroken += GameItemBrokenEventHandler;
    }

    private void GameItemBrokenEventHandler(object sender, EventManager.GameItemBrokenEventArgs args)
    {
        var brokenItemType = args.GameItem.gameItemType;

        if (!_goalType2GoalItem.ContainsKey(brokenItemType))
        {
            return;
        }
        
        _goalType2GoalItem[brokenItemType].Count--;

        if (_goalType2GoalItem[brokenItemType].Count == 0)
        {
            _goalType2GoalItem.Remove(brokenItemType);
        }

        if (_goalType2GoalItem.Count == 0)
        {
            EventManager.LevelWon();
        }
    }

    private void OnDestroy()
    {
        EventManager.OnLevelStarted -= LevelStartedEventHandler;
    }

    private void SetGoals(string[] levelGrid)
    {
        foreach (var itemShortName in levelGrid)
        {
            var itemType = Constants.ShortName2GameItemType[itemShortName];
            var itemPrefab = _prefabLibrary.GetGameItemPrefabFromType(itemType);

            if (itemPrefab is not Obstacle)
            {
                continue;
            }

            if (_goalType2GoalItem.ContainsKey(itemType))
            {
                _goalType2GoalItem[itemType].Count++;
            }
            else
            {
                var goalItem = Instantiate(goalItemPrefab, parent: gridParent);
                var sprite = _prefabLibrary.GetGameItemPrefabFromType(itemType).GetSprite();
                goalItem.Configure(1, sprite);
                _goalType2GoalItem.Add(itemType, goalItem);
            }
        }
    }

    private void LevelStartedEventHandler(object sender, EventManager.LevelStartedEventArgs args)
    {
        SetGoals(args.LevelRepresentation.grid);
    }
}
