using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIView : MonoBehaviour,IView, IUIView
{
    public void OnHurt(HealtInfo healthInfo)
    {
        
    }

    public void OnShoot(bool isShooting)
    {
        throw new System.NotImplementedException();
    }


    public void OnDead()
    {
        throw new System.NotImplementedException();
    }
}
