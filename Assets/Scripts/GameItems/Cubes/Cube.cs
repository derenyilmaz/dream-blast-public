public class Cube : GameItem
{
    public override void Tap()
    {
        EventManager.CubeTapped(this);
    }
}
