public interface IUIView
{
    public int MaxHealth { set; }
    public void OnShoot(bool isShooting);
    public void OnDead();
    public void OnFire();
}