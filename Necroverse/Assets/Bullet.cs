using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    private LayerMask _enemy;
    private int _damage;
    private Vector3 _destination;
    public int Damage { set { _damage = value; } }
    public Vector3 Destination { set { _destination = value; } }
    
    private void Update()
    {
        transform.Translate((_destination - transform.position).normalized * (Time.deltaTime * 100));
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collision ");
        if(other.gameObject.layer == LayerMask.GetMask("Zombie")) DamageActionManager.Instance.DoDamage(other,_damage);
        BulletSpawner.Instance.ReleaseBullet(this);
    }
}
