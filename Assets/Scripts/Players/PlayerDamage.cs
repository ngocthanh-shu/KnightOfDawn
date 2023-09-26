using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDamage : MonoBehaviour, IDamage
{
    public event Action<int> hitDamageAction;
    public event Func<int> getHPFunc;

    public void SetData(IData data = null)
    {
        throw new System.NotImplementedException();
    }

    public int GetHP()
    {
        return getHPFunc.Invoke();
    }

    public void HitDamage(int value = 0)
    {
        hitDamageAction?.Invoke(value);
    }
}

