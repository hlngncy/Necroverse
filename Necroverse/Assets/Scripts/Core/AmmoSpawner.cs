using UnityEngine;
using UnityEngine.Pool;

public class AmmoSpawner : Singleton<AmmoSpawner>
{
    [SerializeField] private Ammo _ammo;
    private ObjectPool<Ammo> _pool;
    private Vector3 _position;
    private Quaternion _rotation;
    public int _damage;


    void Start()
    {
        _pool = new ObjectPool<Ammo>(
            () => { return Instantiate(_ammo,_position, _rotation); },
            GetFireballFromPool,
            ReleaseFireballFromPool,
            fireball => { Destroy(fireball.gameObject); }, 
            false, 
            10,
            15);
    }

    private void GetFireballFromPool(Ammo obj)
    {
        obj.transform.position = _position;
        obj.transform.rotation = _rotation;
        obj.gameObject.SetActive(true);
    }
    
    private void ReleaseFireballFromPool(Ammo obj)
    {
        obj.gameObject.SetActive(false);
    }

    public void GetFireball(Vector3 position, Quaternion rotation, int damage)
    {
        _position = position;
        _rotation = rotation;
        _damage = damage;
        _pool.Get();
    }

    public void ReleaseFireball(Ammo fireball, Collider2D player = null)
    {
        if (player != null)
        {
            DamageActionManager.Instance.DoDamage(player, _damage);
        }
        _pool.Release(fireball);
    }

}

public class Ammo : MonoBehaviour
{
    
}
