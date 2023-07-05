using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootVFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem _vfx;
    public void EnableVFX(bool isEnable)
    {
        if(isEnable) _vfx.Play();
        else _vfx.Stop();
    }
    
}
