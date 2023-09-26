using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour, IDamage
{
    public event Action<int> hitEvent;
    public event Func<int> getHPEvent;

    public void SetData(IData data = null)
    {
        EnemyControllerData controllerData = (EnemyControllerData) data;
        hitEvent += controllerData.EC.GetHit;
        getHPEvent += controllerData.EC.GetHP;
    }

    public int GetHP()
    {
        return getHPEvent.Invoke();
    }

    public void HitDamage(int value = 0)
    {
        hitEvent?.Invoke(value);
    }
}

