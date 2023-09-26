using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockSkillUI : UIBasic
{
    [SerializeField] private Image skillImage;
    [SerializeField] private Text skillName;

    public override void Show(IUIData uIData = null)
    {
        base.Show(uIData);
        UnlockSKillUIData showData = (UnlockSKillUIData) uIData;
        skillImage.sprite = showData.image;
        skillName.text = showData.name;
       
    }

    public void OnclickCloseBtn()
    {
        Time.timeScale = 1;
        Hide();
    }
}

public class UnlockSKillUIData : IUIData
{
    public Sprite image;
    public string name;
}