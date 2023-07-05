public interface IView
{
    public void OnShoot(bool isShooting);
    public void OnDead();
    public void OnReload();
}

public interface IUIView
{
    public int MaxHealth { set; }
    public void OnShoot(bool isShooting);
    public void OnDead();
}