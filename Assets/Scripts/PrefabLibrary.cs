using System;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[CreateAssetMenu(fileName = "PrefabLibrary", menuName = "ScriptableObjects/PrefabLibrary", order = 1)]
public class PrefabLibrary : ScriptableObject
{
    public List<GameItemType2Prefab> gameItemType2PrefabList = new();


    private Dictionary<GameItemType, GameItem> _gameItemType2PrefabDict = new();

    private void Awake()
    {
        foreach (var gameItemType2Prefab in gameItemType2PrefabList)
        {
            _gameItemType2PrefabDict.Add(gameItemType2Prefab.gameItemType, gameItemType2Prefab.gameItemPrefab);
        }
    }


    public GameItem GetGameItemPrefabFromType(GameItemType gameItemType)
    {
        // todo: ???
        if (_gameItemType2PrefabDict.Count == 0)
        {
            Awake();
        }
        
        if (!_gameItemType2PrefabDict.ContainsKey(gameItemType))
        {
            return null;
        }
        
        return _gameItemType2PrefabDict[gameItemType];
    }
}

[Serializable]
public class GameItemType2Prefab
{
    public GameItemType gameItemType;
    public GameItem gameItemPrefab;
}
