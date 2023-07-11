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
    public void OnHurt(HealthInfo healthInfo)
    {
        _healthBar.value = healthInfo.currentHealth - healthInfo.damage;
    }
    
    public void OnShoot(bool isShooting)
    {
        //TODO Add Vignet effect
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
        //DOValue(float to, float duration, bool snapping = false);
        _magazinBar.DOValue(_magazinBar.maxValue, 2F).SetEase(Ease.Linear);
        //_magazinBar.value = _magazinBar.maxValue;
    }
}
