using UnityEngine;
using UnityEngine.Pool;

public class BulletSpawner : Singleton<BulletSpawner>
{
    [SerializeField] private Bullet _ammo;
    private ObjectPool<Bullet> _pool;
    private Vector3 _position;
    private Vector3 _direction;
    private Quaternion _rotation;
    public int _damage;
    private Transform _t;


    void Start()
    {
        _pool = new ObjectPool<Bullet>(
            () => { return Instantiate(_ammo,_position, _rotation); },
            GetBulletFromPool,
            ReleaseBulletFromPool,
            fireball => { Destroy(fireball.gameObject); }, 
            false, 
            10,
            15);
    }

    private void GetBulletFromPool(Bullet obj)
    {
        _t = obj.transform;
        _t.position = _position;
        _t.rotation = _rotation;
        obj.Destination = _direction;
        obj.Damage = _damage;
        obj.gameObject.SetActive(true);
    }
    
    private void ReleaseBulletFromPool(Bullet obj)
    {
        obj.gameObject.SetActive(false);
    }

    public void GetBullet(Vector3 position, Quaternion rotation, int damage, Vector3 direction)
    {
        _position = position;
        _rotation = rotation;
        _damage = damage;
        _direction = direction;
        _pool.Get();
    }

    public void ReleaseBullet(Bullet bullet)
    {
        _pool.Release(bullet);
    }

}

