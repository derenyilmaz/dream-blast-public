using System;
using UnityEngine;
using static Constants;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform tileParent;
    [SerializeField] private Transform pieceParent;
    [SerializeField] private Tile tilePrefab;
    
    
    private PrefabLibrary _prefabLibrary;
    private LevelLibrary _levelLibrary;
    private TileMatrix _tileMatrix;

    
    private int _moveCount;

    
    private void Start()
    {
        EventManager.OnMoveSpent += MoveSpentEventHandler;
    }

    private void OnDestroy()
    {
        EventManager.OnMoveSpent -= MoveSpentEventHandler;
    }

    public void Configure(TileMatrix tileMatrix)
    {
        _tileMatrix = tileMatrix;

        _prefabLibrary = Resources.Load<PrefabLibrary>("PrefabLibrary");
        _levelLibrary = Resources.Load<LevelLibrary>("LevelLibrary");
        
        LoadLevelByIndex(2);
    }
    
    private void LoadLevelByIndex(int index)
    {
        if(index < 0 || index >= _levelLibrary.levelList.Count)
        {
            return;
        }
        
        var levelAsset = _levelLibrary.GetLevelByIndex(index);
        var levelRepresentation = JsonUtility.FromJson<LevelRepresentation>(levelAsset.ToString());
        
        CreateLevelByLevelRepresentation(levelRepresentation);
    }

    private void CreateLevelByLevelRepresentation(LevelRepresentation levelRepresentation)
    {
        _moveCount = levelRepresentation.move_count;
        
        var width = levelRepresentation.grid_width;
        var height = levelRepresentation.grid_height;
        var grid = levelRepresentation.grid;
        
        var tiles = new Tile[width, height];

        var vertOffset = height / 2f - 0.5f;
        var horOffset = width / 2f - 0.5f;
        
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var tile = Instantiate(tilePrefab, parent: transform);
                tile.transform.localPosition = new Vector3(i - horOffset, j - vertOffset);
                tile.xIndex = i;
                tile.yIndex = j;

                tiles[i, j] = tile;
                if (j == height - 1)
                {
                    tile.gameObject.AddComponent<GenerationComponent>();
                }

                var index = width * j + i;
                var shortName = grid[index];

                var itemType = ShortName2GameItemType[shortName];

                if (itemType == GameItemType.RandomCube)
                {
                    itemType = (GameItemType) Random.Range(1, 5);
                }
                
                var randomItemPrefab = _prefabLibrary.GetGameItemPrefabFromType(itemType);
                var item = Instantiate(randomItemPrefab, parent: transform);
                
                tile.AttachGameItemToTile(item);   
            }
            _tileMatrix.SetTiles(tiles);
        }
        
        EventManager.LevelStarted(levelRepresentation);
    }

    private void MoveSpentEventHandler(object sender, EventArgs args)
    {
        _moveCount--;

        if (_moveCount == 0)
        {
            EventManager.LevelFailed();
        }
    }
}

[Serializable]
public class LevelRepresentation
{
    public int level_number;
    public int grid_width;
    public int grid_height;
    public int move_count;
    public string[] grid;
}
