using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using MEC;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] public GameObject FemalePlayerPrefab;
    [SerializeField] public GameObject MalePlayerPrefab;
    [SerializeField] public CinemachineVirtualCamera virtualCamera;
    [SerializeField] public Camera mainCamera;
    [SerializeField] public EnemyManager enemyManager;
    [SerializeField] public PoolManager poolManager;
    [SerializeField] public AbilityManager abilityManager;
    
    public UIManager uiManager { get; private set; }
    
    public ResourceManager resourceManager { get; private set; }
    public ControllerManager controllerManager { get; private set; }
    public AudioManager audioManager { get; private set; }
    public DataMemoryManager dataMemoryManager { get; private set; }

    public PlayerController PC;
    
    public Action BackMenuAction;
    public Action ContinueAction;
    public Action PauseAction;
    public Action<List<EnemyController>> GetEnemyListAction;
    public Action ShowIndicatorAction;
    public Action UpdateHpAction;

    public int _playerExp;
    public Action<int> enemyDeadAction;
    public Action endGame;
    private float _startGameTime;
    private List<RequiredExpForLevel> _levelTable;
    private List<LevelGetAbility> _abilityList;
    private GameplayUI _gameplayUI;
    private AudioSource _audioSource;

    private void Start()
    {
        Intialize();
        uiManager.DestroyAllUI();
        GameStart(dataMemoryManager.saveData);
        LoadGameplayUI();
    }

    private void Update()
    {
        if (PC != null)
        {
            PC.OnUpdate();
        }

        if (enemyManager != null)
        {
            enemyManager.OnUpdate();
        }

        if (_gameplayUI != null)
        {
            _gameplayUI.UpdateHandle(Time.time - _startGameTime);
        }
    }

    public void Intialize()
    {
        if (uiManager == null)
        {
            uiManager = UIManager.Instance;
        }

        if (resourceManager == null)
        {
            resourceManager = ResourceManager.Instance;
        }

        if (controllerManager == null)
        {
            controllerManager = ControllerManager.Instance;
        }

        if (audioManager == null)
        {
            audioManager = AudioManager.Instance;
        }

        if(dataMemoryManager == null)
        {
            dataMemoryManager = DataMemoryManager.Instance;
        }

        _audioSource = GetComponent<AudioSource>();
        poolManager.Initialize();
        enemyManager.Initialize(this);

        InitializeAction();
    }

    private void InitializeAction()
    {
        enemyDeadAction += EnemyDead;
        endGame += GameEnd;
        PauseAction += PauseGame;
        BackMenuAction += BackToMenu;
        ContinueAction += ContinueGame;
        GetEnemyListAction += GetEnemyList;
        ShowIndicatorAction += ShowIndicator;
        UpdateHpAction += UpdateHp;
    }

    private void UpdateHp()
    { 
        _gameplayUI.UpdateHP();
    }

    private void ShowIndicator()
    {
        _gameplayUI.UpdateHPLowIndicator();
    }

    private void GetEnemyList(List<EnemyController> ECs)
    {
        foreach(EnemyController EC in ECs)
        {
            PC.model.AddEnemy(EC.view.gameObject);
        }
    }

    private void ContinueGame()
    {
        Time.timeScale = 1;
    }

    private void BackToMenu()
    {
        enemyManager.KillALLEnemy();
        Time.timeScale = 1;
        ChangeToMenuScene();
    }

    private void PauseGame()
    {
        if(Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
    }

    [Button("Start")]
    public void GameStart(SaveData saveData)
    {
        _levelTable = resourceManager.playerGrowth.RequiredTable;
        DisableAllObejct();
        _playerExp = saveData != null ? saveData.Exp :  0;
        InitializePlayer();
        abilityManager.Initialize(this, PC, _abilityList);
        var transformRootPlayer = PC.view.transform.GetChild(0);

        virtualCamera.m_Follow = transformRootPlayer;
        virtualCamera.m_LookAt = transformRootPlayer;
        _startGameTime = saveData != null ? Time.time - saveData.time : Time.time;
        uiManager.HideAllUI<EnemyHealthBar>();
        if (dataMemoryManager.saveData != null)
        {
            PC.model.maxHP = dataMemoryManager.saveData.maxHP;
            PC.model.HP = dataMemoryManager.saveData.HP;
            PC.model.Atk = dataMemoryManager.saveData.Atk;
            PC.model.Def = dataMemoryManager.saveData.Def;
        }
        enemyManager.startWave?.Invoke(saveData);
    }

    public void GameEnd()
    {
        _gameplayUI.EnableDeathPanel();
    }

    public void InitializePlayer()
    {
        PC = new PlayerController();

        //GameObject gameobj = (PlayerPrefs.GetInt(KeyPlayerPrefs.Gender, 0) > 0) ? MalePlayerPrefab: FemalePlayerPrefab;
        GameObject gameobj;
        if(PlayerPrefs.GetInt(KeyPlayerPrefs.Gender, 0) > 0)
        {
            gameobj = MalePlayerPrefab;
            _abilityList = resourceManager.dictionaryPlayer["MaleBaseState"].AbilityList;
        }
        else
        {
            gameobj = FemalePlayerPrefab;
            _abilityList = resourceManager.dictionaryPlayer["FemaleBaseState"].AbilityList;
        }
        
        GameObject player = poolManager.GetPooledObject(gameobj, new Vector3(4, 4, 0), Quaternion.identity);
        PlayerView view = player.GetComponent<PlayerView>();
        PlayerAttackEffect attackEffect = player.GetComponent<PlayerAttackEffect>();
        PlayerDamage dmg = player.GetComponent<PlayerDamage>();

        PC.Initialize(new PlayerInitializeData()
        {
            dmg = dmg,
            view = view,
            attackEffect = attackEffect,
            gameManager = this
        });
        
        PC.view.SetCamera(mainCamera);
   
        PC.SetData(new PlayerModelData()
        {
            data = resourceManager.dictionaryPlayer["MaleBaseState"]
        });  
    }
    

    private void EnemyDead(int exp)
    {
        
        _playerExp += exp;
        _gameplayUI.SetExp(_playerExp);
        enemyManager.checkWave?.Invoke();
        UpdateLevelForPlayer();
        _gameplayUI.UpdateEnemyRemainAndWave();
    }

    private void UpdateLevelForPlayer()
    {
        int level = GetPlayerLevelByExp();
        if (PC.model.GetLevel() != level)
        {
            audioManager.PlayAduioOneShot(_audioSource, resourceManager.dictionaryAudio["GetScore"]);
            PC.model.IncreaseAttribute(resourceManager.playerGrowth.ScaleAttributeByLv);
            PC.model.SetLevel(level);
            _gameplayUI.SetLevel(level);
            _gameplayUI.setMaxExp(_levelTable[level+1].RequiredExp);
            _gameplayUI.UpdateHP();
            CheckUnlockSkill(level);
        }
    }

    [Button("Test Disable All Pool Object")]
    private void DisableAllObejct()
    {
        poolManager.DisableAll();
    }


    [Button]
    public void ChangeToMenuScene()
    {
        PC.RemoveAction();
        Timing.KillCoroutines();
        audioManager.SaveAudioValue();
        Timing.RunCoroutine(LoadMainMenu("MainMenu"));
    }

    IEnumerator<float> LoadMainMenu(string sceneName, bool isContinue = false, float waitTime = 1.5f)
    {
        yield return Timing.WaitUntilDone(Timing.RunCoroutine(UIManager.Instance.LoadAndShowPrefab<LoadingUI>("LoadingUI", UIManager.Instance.CanvasOverlay)));

        LoadingUI loadingUI = UIManager.Instance.GetUI<LoadingUI>("LoadingUI");

        yield return Timing.WaitUntilTrue(() => SaveGameData());

        loadingUI.SetData(new LoadingUIData
        {
            percent = 0.5f
        });

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        asyncLoad.allowSceneActivation = false;

        loadingUI.SetData(new LoadingUIData
        {
            percent = 1f
        });

        yield return Timing.WaitForSeconds(waitTime);
        asyncLoad.allowSceneActivation = true;
    }

    [Button]
    private void LoadGameplayUI()
    {
        Timing.RunCoroutine(ShowAndGetGameplayUI());
    }

    IEnumerator<float> ShowAndGetGameplayUI()
    {
        yield return Timing.WaitUntilDone(Timing.RunCoroutine(uiManager.LoadAndShowPrefab<GameplayUI>("GameplayUI", uiManager.CanvasOverlay, new GameplayUIData
        {
            eM = enemyManager,
            pC = PC,
            gM = this
        })));

        _gameplayUI = uiManager.GetUI<GameplayUI>("GameplayUI");
        _gameplayUI.InitializeSkillIcon();
        _gameplayUI.SetIcon();
        if (dataMemoryManager.saveData != null)
        {
            _gameplayUI.SetStatusSkill(dataMemoryManager.saveData);
        }
    }

    public int GetPlayerLevelByExp()
    {
        foreach (var required in _levelTable)
        {
            if (_playerExp < required.RequiredExp)
            {
                return required.level - 1;
            }
            else if(_playerExp >= required.RequiredExp && required.level == _levelTable.Count)
            {
                return required.level;
            }
        }
        return 1;
    }

    public void CheckUnlockSkill(int level)
    {
        for(int i = 1; i < _abilityList.Count; i++)
        {
            if(level >= _abilityList[i].level)
            {
                if (_abilityList[i].ability != null)
                {
                    _gameplayUI.GetSkill(i).UnlockSkill();
                    if(level == _abilityList[i].level)
                    {
                        Timing.RunCoroutine(uiManager.LoadAndShowPrefab<UnlockSkillUI>("UnlockSkillUI", uiManager.CanvasOverlay, new UnlockSKillUIData
                        {
                            image = _abilityList[i].ability.GetComponent<Skill>().Data.Icon,
                            name = _abilityList[i].abilityName
                        }));
                        if(Time.timeScale == 1)
                        {
                            Time.timeScale = 0;
                        }
                    }
                }
            }
        }
    }

    private void OnApplicationQuit()
    {
        SaveGameData();
    }

    private bool SaveGameData()
    {
        if (PC.model.HP > 0)
        {
            dataMemoryManager.SaveData(new SaveData
            {
                Exp = _playerExp,
                numberWave = enemyManager.currentWave,
                HP = PC.model.HP,
                maxHP = PC.model.maxHP,
                Atk = PC.model.Atk,
                Def = PC.model.Def,
                time = _gameplayUI.GetPlayTime(),
                skill1Status = _gameplayUI.GetSkill(1).GetSkillStatus(),
                skill2Status = _gameplayUI.GetSkill(2).GetSkillStatus(),
                skill3Status = _gameplayUI.GetSkill(3).GetSkillStatus(),
                skill4Status = _gameplayUI.GetSkill(4).GetSkillStatus(),
            });
        }
        return true;
    }
    
    public List<RequiredExpForLevel> GetLevelTable()
    {
        return _levelTable;
    }
}

public static class KeyPlayerPrefs
{
    public static string Gender = "Gender";
    public static string Music = "Music";
    public static string SFX = "SFX";
}

[Serializable]
public class PlayerModelPrefab
{
    public string key;
    public GameObject value;
}

