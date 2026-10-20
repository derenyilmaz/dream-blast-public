using System.Collections.Generic;

public static class Constants
{
    public const int MinConnectedCubeCountForLegalMove = 1;
    public const int MinConnectedCubeCountForTntCreation = 5;
    public const float GameItemOneTileFallDurationInSeconds = 0.1f;

    public const string PLayerPrefsLevelKey = "PlayerLevel";
    
    public const string LevelSceneName = "LevelScene";
    public const string MainSceneName = "MainScene";

    
    public enum GameItemType
    {
        None = 0,
        
        // Cubes
        BlueCube = 1,
        GreenCube = 2,
        RedCube = 3,
        YellowCube = 4,
        RandomCube = 5,

        // SpecialItems
        Tnt = 101,
        
        // Obstacles
        Box = 1001,
        Stone = 1002,
        Vase = 1003,
    }
    
    public static readonly List<GameItemType> AllCubeTypes = new ()
    {
        GameItemType.BlueCube,
        GameItemType.GreenCube,
        GameItemType.RedCube,
        GameItemType.YellowCube,
    };

    public static readonly Dictionary<string, GameItemType> ShortName2GameItemType = new()
    {
        {"r", GameItemType.RedCube},
        {"g", GameItemType.GreenCube},
        {"b", GameItemType.BlueCube},
        {"y", GameItemType.YellowCube},
        {"rand", GameItemType.RandomCube},
        
        {"t", GameItemType.Tnt},
        
        {"bo", GameItemType.Box},
        {"s", GameItemType.Stone},
        {"v", GameItemType.Vase},
    };
}
