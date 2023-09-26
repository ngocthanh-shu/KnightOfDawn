using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class PopupDamageUI : UIBasic
{
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private float moveY = 1.5f;
    [SerializeField] private float timeExist = 0.5f;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public override void Show(IUIData uIData = null)
    {
        base.Show(uIData);
        PopupDamageUIData data = (PopupDamageUIData)uIData;

        transform.position = data.position;
        damageText.text = $"-{data.value}";

        Vector2 currentPosition = rectTransform.anchoredPosition;
        Vector2 newPosition = currentPosition + new Vector2(0f, moveY);
        
        rectTransform.DOAnchorPos(newPosition, timeExist).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            Hide();
        });
    }
}

public class PopupDamageUIData : IUIData
{
    public Vector3 position;
    public int value;
}

