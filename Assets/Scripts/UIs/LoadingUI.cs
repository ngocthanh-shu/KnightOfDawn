using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingUI : UIBasic
{
    [SerializeField] private Slider loadingBar;
    [SerializeField] private Text percentText;
    [SerializeField] private float loadTime = 0.1f;
    public override void Initialize()
    {
        base.Initialize();
        loadingBar.value = 0;
    }

    public override void Show(IUIData uIData = null)
    {
        base.Show(uIData);
        Time.timeScale = 1;
    }

    public override void SetData(IUIData UIData = null)
    {
        if (UIData == null) return;
        LoadingUIData loadingUIData = (LoadingUIData) UIData;
        SetPercent(loadingUIData.percent);
    }

    private void SetPercent(float value)
    {
        loadingBar.DOValue(value, loadTime).OnUpdate(() => percentText.text = $"{value * 100}%");
    }
}

public class LoadingUIData : IUIData
{
    public float percent;
}

