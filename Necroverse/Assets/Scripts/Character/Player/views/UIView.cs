using System;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIView : MonoBehaviour,IView, IUIView
{
    [SerializeField] private Slider _healthBar;
    [SerializeField] private List<GameObject> _ammo = new List<GameObject>();
    
    private Observable<int> maxHealth = new Observable<int>();
    private bool _isShooting;
    private int _activeAmmo;
    private int _lastAmmo = 0;
    public int MaxHealth { set { maxHealth.Value = value; } }

    public UIView()
    {
        maxHealth.OnValueChanged.AddListener(SetSlider);
    }

    private void Start()
    {
        _activeAmmo = _ammo.Count;
        Debug.Log(_activeAmmo);
    }

    private void SetSlider(int previous, int current)
    {
        _healthBar.maxValue = current;
    }
    public void OnHurt(HealtInfo healthInfo)
    {
        _healthBar.value = healthInfo.currentHealth - healthInfo.damage;
    }
    
    public void OnShoot(bool isShooting)
    {
        _isShooting = isShooting;
        //if (_isShooting && _ammo == null) StartCoroutine(Reload());
        if(_ammo.Last().activeInHierarchy)
            StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        int activeAmmoCount = _activeAmmo;
        for (int i = 0; i < activeAmmoCount; i++ , _lastAmmo++ ,_activeAmmo--)
        {
            _ammo[_lastAmmo].SetActive(false);
            if (!_isShooting) break;
            yield return new WaitForSeconds(.1f);
        }
    }

    private IEnumerator Reload()
    {
        if (_activeAmmo == 0)
        {
            _activeAmmo = _ammo.Count;
            _lastAmmo = 0;
        }
        _ammo[0].SetActive(true);
        Debug.Log("ammo reloaded");
        yield return new WaitForSeconds(.5f);
    }
    /*private async void Shoot()
    {
        await Task.Run(() =>
        {
            while (_isShooting && _ammo != null)
            {
                //_ammo[0].SetActive(false);
                Debug.Log("ammo wasted");
                Task.Delay(1000);
            }

            for (int i = 0; i < _ammo.Count; i++)
            {
                //_ammo[i].SetActive(true);
                Debug.Log("ammo reload");
            }
        });
    }*/

    public void OnDead()
    {
        GetComponent<CanvasGroup>().DOFade(.5F, .5F);
    }

    public void OnReload()
    {
        throw new System.NotImplementedException();
    }
}
