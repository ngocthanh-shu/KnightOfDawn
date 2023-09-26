using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : UIBasic
{
    [SerializeField] private Slider expBar;
    [SerializeField] private Slider hpBar;
    [SerializeField] private Button BackButton;
    [SerializeField] private Text monsterRemainText;
    [SerializeField] private Text currentWaveText;
    [SerializeField] private Text levelText;
    [SerializeField] private Text timeText;
    [SerializeField] private GameObject TransperentPanel;
    [SerializeField] private GameObject PausePanel;
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private Text GameOverText;
    [SerializeField] private GameObject SkillList;
    [SerializeField] private Sprite DefaultSkillIcon;
    [SerializeField] private Image DamageIndicator1;
    [SerializeField] private Image DamageIndicator2;
    [SerializeField] private Image HPLowIndicator;
    private float time;
    
    private SkillIcon _skill1Btn;
    private SkillIcon _skill2Btn;
    private SkillIcon _skill3Btn;
    private SkillIcon _skill4Btn;

    private EnemyManager _enemyManager;
    private PlayerController _playerController;
    private GameManager _gameManager;
        
    public override void Initialize()
    {
        base.Initialize();
        BackButton.onClick.AddListener(OnPauseButtonClicked);
    }

    public void InitializeSkillIcon()
    {
        _skill1Btn = SkillList.transform.GetChild(1).GetComponent<SkillIcon>();
        _skill2Btn = SkillList.transform.GetChild(2).GetComponent<SkillIcon>();
        _skill3Btn = SkillList.transform.GetChild(3).GetComponent<SkillIcon>();
        _skill4Btn = SkillList.transform.GetChild(4).GetComponent<SkillIcon>();
    }

    public void SetIcon()
    {
        Skill skill = _gameManager.abilityManager.skill1.GetComponent<Skill>();
        if(skill != null)
        {
            _skill1Btn.SetSkill(skill.GetIcon(), skill.Cooldown);
            skill.cooldownAction += () => _skill1Btn.CooldownSkill();
        }
        else
        {
            _skill1Btn.SetSkill(DefaultSkillIcon);
            _skill1Btn.LockSkill();
            return;
        }
        skill = _gameManager.abilityManager.skill2.GetComponent<Skill>();
        if(skill != null)
        {
            _skill2Btn.SetSkill(skill.GetIcon(), skill.Cooldown);
            skill.cooldownAction += () => _skill2Btn.CooldownSkill();
        }
        else
        {
            _skill2Btn.SetSkill(DefaultSkillIcon);
            _skill2Btn.LockSkill();
            return;
        }
        skill = _gameManager.abilityManager.skill3.GetComponent<Skill>();
        if(skill != null)
        {
            _skill3Btn.SetSkill(skill.GetIcon(), skill.Cooldown);
            skill.cooldownAction += () => _skill3Btn.CooldownSkill();
        }
        else
        {
            _skill3Btn.SetSkill(DefaultSkillIcon);
            _skill3Btn.LockSkill();
            return;
        }
        skill = _gameManager.abilityManager.skill4.GetComponent<Skill>();
        if(skill != null)
        {
            _skill4Btn.SetSkill(skill.GetIcon(), skill.Cooldown);
            skill.cooldownAction += () => _skill4Btn.CooldownSkill();
        }
        else
        {
            _skill4Btn.SetSkill(DefaultSkillIcon);
            _skill4Btn.LockSkill();
            return;
        }
    }
    
    public SkillIcon GetSkill(int index)
    {
        switch (index)
        {
            case 1:
                return _skill1Btn;
            case 2:
                return _skill2Btn;
            case 3:
                return _skill3Btn;
            case 4:
                return _skill4Btn;
            default:
                return null;
        }
    }
    
    public void SetStatusSkill(SaveData data)
    {
        _skill1Btn.SetSkillStatus(data.skill1Status);
        _skill2Btn.SetSkillStatus(data.skill2Status);
        _skill3Btn.SetSkillStatus(data.skill3Status);
        _skill4Btn.SetSkillStatus(data.skill4Status);
    }

    public void InitializeActions()
    {
        _playerController.GetHitAction += ChangeHealthBar;
    }

    public override void SetData(IUIData UIData = null)
    {
        if(UIData == null) return;
        GameplayUIData gameplayUIData = (GameplayUIData)UIData;
        _enemyManager = gameplayUIData.eM;
        _playerController = gameplayUIData.pC;
        _gameManager = gameplayUIData.gM;
        SetDataForGameplay();
        InitializeActions();
        UpdateHPLowIndicator();
    }

    public void SetDataForGameplay()
    {
        expBar.maxValue = _gameManager.GetLevelTable()[_gameManager.GetPlayerLevelByExp()].RequiredExp;
        expBar.value = _gameManager._playerExp;
        hpBar.maxValue = _playerController.model.maxHP;
        hpBar.value = _playerController.model.HP;
        currentWaveText.text = "Wave: " + _enemyManager.currentWave;
        monsterRemainText.text = "Zombies: " + (_enemyManager.enemyInWare - _enemyManager.numberEnmeyDeadInWave);
        levelText.text = "Level " + _playerController.model.GetLevel();
        timeText.text = "Time: 0s";
        HPLowIndicator.DOFade(0,0).SetSpeedBased();
        DamageIndicator1.DOFade(0,0).SetSpeedBased();
        DamageIndicator2.DOFade(0,0).SetSpeedBased();
        
    }

    public void ShowIndicator()
    {
        DamageIndicator1.DOFade(1, 0.5f).SetLoops(1, LoopType.Yoyo);
        DamageIndicator2.DOFade(1, 0.5f).SetLoops(1, LoopType.Yoyo);
    }
    
    public void ShowHPLowIndicator()
    {
        HPLowIndicator.DOKill();
        HPLowIndicator.DOFade(1f, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }
    
    public void HideHPLowIndicator()
    {
        HPLowIndicator.DOFade(0, 0.5f).SetLoops(1, LoopType.Yoyo).OnComplete(() => HPLowIndicator.DOKill());
    }
    
    public void UpdateHPLowIndicator()
    {
        if(hpBar.value <= hpBar.maxValue * 0.3f)
        {
            ShowHPLowIndicator();
        }
        else
        {
            HideHPLowIndicator();
        }
    }
    
    public void UpdateEnemyRemainAndWave()
    {
        currentWaveText.text = "Wave: " + _enemyManager.currentWave;
        monsterRemainText.text = "Zombies: " + (_enemyManager.enemyInWare - _enemyManager.numberEnmeyDeadInWave);
    }

    public void UpdateHandle(float time)
    {
        //convert time to s 
        timeText.text = "Time: " + (int)time + "s";
        this.time = time;
    }

    public float GetPlayTime()
    {
        return time;
    }

    private void ChangeHealthBar(int value)
    {
        hpBar.value = _playerController.model.HP;
        UpdateHPLowIndicator();
    }

    private void OnPauseButtonClicked()
    {
        HPLowIndicator.DOKill();
        TransperentPanel.SetActive(true);
        PausePanel.SetActive(true);
        _gameManager.PauseAction?.Invoke();
    }
    
    public void OnContinueButtonClicked()
    {
        UpdateHPLowIndicator();   
        TransperentPanel.SetActive(false);
        PausePanel.SetActive(false);
        _gameManager.ContinueAction?.Invoke();
    }
    
    public void OnExitButtonClicked()
    {
        HPLowIndicator.DOKill();
        _gameManager.BackMenuAction?.Invoke();
    }
    
    public void EnableDeathPanel()
    {
        _gameManager.enemyManager.KillALLEnemy();
        _gameManager.dataMemoryManager.DeleteData();
        TransperentPanel.SetActive(true);
        GameOverPanel.SetActive(true);
        GameOverText.text = "TIME: " + (int)time + "s";
    }

    public void SetLevel(int level)
    {
        levelText.text = "Level " + level;
    }

    public void SetExp(int playerExp)
    {
        expBar.value = playerExp;
    }

    public void setMaxExp(int requiredExp)
    {
        expBar.maxValue = requiredExp;
    }
    
    public void SetHP(int value)
    {
        hpBar.value = value;
    }

    public void UpdateHP()
    {
        hpBar.maxValue = _playerController.model.maxHP;
        hpBar.value = _playerController.model.HP;
        UpdateHPLowIndicator();
    }

    public void OnClickButtonSound()
    {
        AudioManager.Instance.PlayClickButton();
    }
}

public class GameplayUIData : IUIData
{
    public EnemyManager eM;
    public PlayerController pC;
    public GameManager gM;
}