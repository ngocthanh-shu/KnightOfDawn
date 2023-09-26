using MEC;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : UIBasic
{
    [SerializeField] private TMP_Text enmeyText;
    [SerializeField] private Image healthBar;
    [SerializeField] private float positionOffset = 2f;

    private EnemyController _ec;

    public override void SetData(IUIData data)
    {
        UIEnemyHealthBarData controllerData = (UIEnemyHealthBarData) data;
        _ec = controllerData.EC;

        enmeyText.text = $"{_ec.GetEnemyType()} - LV:{_ec.model.Level}";
        healthBar.fillAmount = 1;

        _ec.GetHitAction += ChangeHealthBar;
    }
    
    public void LateUpdate()
    {
        transform.position = Camera.main.WorldToScreenPoint(_ec.view.transform.position + Vector3.up * positionOffset);
    }

    public void ChangeHealthBar(int value)
    {
        Timing.RunCoroutine(UIManager.Instance.LoadAndShowPrefab<PopupDamageUI>(typeof(PopupDamageUI).Name, UIManager.Instance.CanvasScreenSpaceHealthBarGroup, new PopupDamageUIData {
            position = transform.position,
            value = value
        }));

        if(_ec.GetPercentHP() <= 0)
        {
            Hide();
            return;
        }
        healthBar.fillAmount = _ec.GetPercentHP();
    }

    public override void Hide()
    {
        _ec.GetHitAction -= ChangeHealthBar;
        base.Hide();
    }
}

public class UIEnemyHealthBarData : IUIData
{
    public EnemyController EC;
}