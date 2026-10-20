using System;

public static class EventManager
{
    public class CubeTappedEventArgs : EventArgs
    {
        public Cube Cube;
    }
    
    public class GameItemBrokenEventArgs : EventArgs
    {
        public GameItem GameItem;
    }

    public class GameItemReleasedFromTileEventArgs : EventArgs
    {
        public GameItem GameItem;
        public Tile Tile;
    }

    public class TntTappedEventArgs : EventArgs
    {
        public Tnt Tnt;
    }
    
    public class LevelStartedEventArgs : EventArgs
    {
        public LevelRepresentation LevelRepresentation;
    }
    
    public static event EventHandler<CubeTappedEventArgs> OnCubeTapped;
    public static event EventHandler<GameItemBrokenEventArgs> OnGameItemBroken;
    public static event EventHandler<GameItemReleasedFromTileEventArgs> OnGameItemReleasedFromTile;
    public static event EventHandler<TntTappedEventArgs> OnTntTapped;
    public static event EventHandler OnMoveSpent;
    public static event EventHandler<LevelStartedEventArgs> OnLevelStarted;

    public static event EventHandler OnLevelFailed;
    public static event EventHandler OnLevelWon;
    

    public static void CubeTapped(Cube cube)
    {
        OnCubeTapped?.Invoke(null, new CubeTappedEventArgs { Cube = cube});
    }

    public static void GameItemBroken(GameItem gameItem)
    {
        OnGameItemBroken?.Invoke(null, new GameItemBrokenEventArgs { GameItem = gameItem});
    }
    
    public static void GameItemReleasedFromTile(GameItem gameItem, Tile tile)
    {
        OnGameItemReleasedFromTile?.Invoke(
            null, new GameItemReleasedFromTileEventArgs { GameItem = gameItem, Tile = tile});
    }

    public static void TntTapped(Tnt tnt)
    {
        OnTntTapped?.Invoke(null, new TntTappedEventArgs {Tnt = tnt});
    }

    public static void MoveSpent()
    {
        OnMoveSpent?.Invoke(null, EventArgs.Empty);
    }
   
    public static void LevelStarted(LevelRepresentation levelRepresentation)
    {
        OnLevelStarted?.Invoke(null, new LevelStartedEventArgs {LevelRepresentation = levelRepresentation});
    }

    public static void LevelFailed()
    {
        OnLevelFailed?.Invoke(null, EventArgs.Empty);
    }
    
    public static void LevelWon()
    {
        OnLevelWon?.Invoke(null, EventArgs.Empty);
    }
}
