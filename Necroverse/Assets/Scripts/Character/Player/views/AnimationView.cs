using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationView : MonoBehaviour, IView
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private ShootVFX _vfx;

    private void Update()
    {
        ArrangeMovementAnimations();
    }

    private void ArrangeMovementAnimations()
    {
        _animator.SetFloat("Speed", Mathf.Clamp(Mathf.Abs(_rb.velocity.x)+Mathf.Abs(_rb.velocity.z), 0, 1));
        _animator.SetFloat("X", transform.InverseTransformDirection(_rb.velocity).x);
        _animator.SetFloat("Y", transform.InverseTransformDirection(_rb.velocity).z);
    }
    

    public void OnShoot(bool isShooting)
    {
        _animator.SetBool("Aiming", isShooting);
        _vfx.EnableVFX(isShooting);
    }

    public void OnDead()
    {
        _animator.SetBool("Dead", true);
    }

    public void OnReload()
    {
        _animator.SetTrigger("Reload");
    }

}
