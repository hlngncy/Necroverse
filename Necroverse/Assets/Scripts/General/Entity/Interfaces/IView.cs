public interface IView
{
    public void OnShoot(bool isShooting);
    public void OnDead();
}

public interface IUIView
{
    public void OnShoot(bool isShooting);
    public void OnDead();
}