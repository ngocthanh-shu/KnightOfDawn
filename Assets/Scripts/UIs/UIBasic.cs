using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBasic : MonoBehaviour, IUI
{
    [SerializeField] protected bool destroyOnHide = false;
    protected bool _isInit = false;
    public virtual void Initialize()
    {
        _isInit = true;
    }
    public virtual void SetDefault()
    {
    }

    public virtual void SetData(IUIData UIData = null)
    {
    }

    public virtual void Show(IUIData uIData = null)
    {
        gameObject.SetActive(true);
    }
    public virtual void Hide()
    {
        if (destroyOnHide) Destroy(gameObject);

        gameObject.SetActive(false);
    }

    public virtual bool IsInit()
    {
        return _isInit;
    }
}
