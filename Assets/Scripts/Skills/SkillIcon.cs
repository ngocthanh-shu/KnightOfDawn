using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;
using DG.Tweening;


public class SkillIcon : MonoBehaviour
{
    [SerializeField] private Image skillImage;
    [Header("Lock Group")]
    [SerializeField] private GameObject lockGroup;
    [Header("Unlock Group")]
    [SerializeField] private GameObject unlockGroup;
    [SerializeField] private Text numeberCooldown;
    [SerializeField] private Image imageCooldown;
    private float cooldown;
    private float currentCooldown;
    private SkillStatus _skillStatus;
    private OnScreenButton _onScreenButton;
    private void Awake()
    {
        lockGroup.SetActive(true);
        unlockGroup.SetActive(false);
        _onScreenButton = GetComponent<OnScreenButton>();
        _onScreenButton.enabled = false;
        _skillStatus = SkillStatus.Lock;
        imageCooldown.fillAmount = 0;
        numeberCooldown.text = "";
    }

    public void LockSkill()
    {
        lockGroup.SetActive(true);
        unlockGroup.SetActive(false);
        _onScreenButton.enabled = false;
        _skillStatus = SkillStatus.Unvailable;
    }

    public void UnlockSkill()
    {
        lockGroup.SetActive(false);
        unlockGroup.SetActive(true);
        _onScreenButton.enabled = true;
        _skillStatus = SkillStatus.Unlock;
    }
    
    public SkillStatus GetSkillStatus()
    {
        return _skillStatus;
    }
    
    public void SetSkillStatus(SkillStatus skillStatus)
    {
        if(skillStatus == SkillStatus.Lock)
            LockSkill();
        else if(skillStatus == SkillStatus.Unlock)
            UnlockSkill();
    }
    
    public void SetSkill(Sprite icon, float cooldownTime = 0)
    {
        skillImage.sprite = icon;
        cooldown = cooldownTime;
    }

    public void CooldownSkill()
    {
        imageCooldown.fillAmount = 1;
        float cooldownShow = cooldown;
        imageCooldown.DOFillAmount(0, cooldown).OnUpdate( () =>
        {
            cooldownShow -= Time.deltaTime;
            numeberCooldown.text = $"{(int)cooldownShow}";
        }
        ).OnComplete(() => numeberCooldown.text = "");
    }
}

public enum SkillStatus
{
    Lock,
    Unlock,
    Unvailable,
    Cooldown
}
