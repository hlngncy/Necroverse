using System;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIView : MonoBehaviour,IView, IUIView
{
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Slider _magazinBar;

    
    private Observable<int> maxHealth = new Observable<int>();
    private ushort _activeAmmo;
    private ushort _lastAmmo = 0;
    private bool _isShooting;
    public int MaxHealth { set { maxHealth.Value = value; } }

    public UIView()
    {
        maxHealth.OnValueChanged.AddListener(SetSlider);
    }

    private void Start()
    {
        _magazinBar.maxValue = 19;
        _magazinBar.value = 19;
    }

    private void SetSlider(int previous, int current)
    {
        _healthBar.maxValue = current;
        _healthBar.value = current;
    }
    public void OnHurt(HealtInfo healthInfo)
    {
        _healthBar.value = healthInfo.currentHealth - healthInfo.damage;
    }
    
    public void OnShoot(bool isShooting)
    {
        //TODO Add Vignet effect
        _isShooting = isShooting;
    }
    
    public void OnDead()
    {
        GetComponent<CanvasGroup>().DOFade(.5F, .5F);
    }

    public void OnFire()
    {
        _magazinBar.value -= 1;
    }

    public void OnReload()
    {
        _magazinBar.value = _magazinBar.maxValue;
    }
}
