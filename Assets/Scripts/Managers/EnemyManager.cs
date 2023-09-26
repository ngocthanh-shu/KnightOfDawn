using MEC;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [MinValue(1)] public int currentWave;
    public int numberEnmeyDeadInWave;
    public int enemyInWare;
    
    public bool isLoopWave;
    [MinValue(0.1f)]public float levelMultiplier = 0.2f;

    [Header("Circle Spawn Arond Layer")]
    [SerializeField] public LayerMask layerWall;
    private float angleBetweenEnemies = 360f;

    [TableList] public List<EnemyWave> enemyWave;
    private List<EnemyController> ECList;
    private GameManager _gameManager;

    private int numberWave;
    
    public Action<SaveData> startWave;
    public Action checkWave;

    public void Initialize(GameManager gameManager)
    {
        _gameManager = gameManager;
        ECList = new List<EnemyController>();

        numberWave = enemyWave.Count;
        currentWave = gameManager.dataMemoryManager.saveData != null? gameManager.dataMemoryManager.saveData.numberWave : 1;
        startWave += StartSpawnEnemyWave;
        checkWave += checkWaveEnemy;
    }
    

    public void SetDefault()
    {
        currentWave = 1;
        ResetWaveValue();
    }

    public void OnUpdate()
    {
        foreach(EnemyController EC in ECList.Where(ec => ec.view.gameObject.activeInHierarchy))
        {
            EC?.Update();
        }
    }

    private void checkWaveEnemy()
    {
        numberEnmeyDeadInWave++;

        if(numberEnmeyDeadInWave >= enemyInWare)
        {
            StartNewWave();
        }
    }

    private void StartNewWave()
    {
        currentWave++;

        if (currentWave > numberWave && isLoopWave)
        {
            currentWave = 1;
        }

        if (currentWave <= numberWave)
        {
            ResetWaveValue();
            SpawnWave();
            return;
        }
       
        _gameManager.endGame?.Invoke();
    }

    public void StartSpawnEnemyWave(SaveData saveData = null)
    {
        Timing.KillCoroutines();
        if(saveData == null)
            SetDefault();
        else
        {
            currentWave = saveData.numberWave;
        }
        SpawnWave();
    }

    private void SpawnWave()
    {
        if (currentWave > numberWave) return;

        foreach (EnemySpawnByTime enemySpawnByTime in enemyWave[currentWave -1].waveList)
        {
            enemyInWare += enemySpawnByTime.amount;
            Timing.RunCoroutine(SpawnEnemyWave(enemySpawnByTime));
        }
        
    }

    private void ResetWaveValue()
    {
        numberEnmeyDeadInWave = 0;
        enemyInWare = 0;
    }

    IEnumerator<float> SpawnEnemyWave(EnemySpawnByTime enemySpawn)
    {
        yield return Timing.WaitForOneFrame;
        int numberEnemy = enemySpawn.amount;
        int numberIntantiate = enemySpawn.amountInstantiate;
        yield return Timing.WaitForSeconds(enemySpawn.timeAfterStart);

        while(numberEnemy >= 0)
        {
            int numberLoop = numberIntantiate;
            if (numberEnemy < numberIntantiate)
            {
                numberLoop = numberEnemy;
            }
            LoopSpawnEnemy(enemySpawn.enemyView, numberLoop, enemySpawn);
            numberEnemy -= numberIntantiate;
            yield return Timing.WaitForSeconds(enemySpawn.timeDelaySpawn);
        } 
        yield break;
    }


    private void LoopSpawnEnemy(GameObject prefabs, int enemyCount, EnemySpawnByTime enemySpawn)
    {
        for(int i =0; i < enemyCount; i++)
        {
            Vector3 center = _gameManager.PC.view.transform.position;

            //Check if player near wall spawn enemy center map
            if (Physics.CheckSphere(center, enemySpawn.radiusSpawn, layerWall))
            {
                center = new Vector3(0f, center.y, 0f);
            }

            float angle = i * angleBetweenEnemies / enemyCount;
            float x = Mathf.Sin(angle * Mathf.Deg2Rad) * enemySpawn.radiusSpawn;
            float z = Mathf.Cos(angle * Mathf.Deg2Rad) * enemySpawn.radiusSpawn;

            Vector3 spawnPosition = center + new Vector3(x, 0f, z);

            SpawnEnemy(prefabs, spawnPosition, enemySpawn);
        }
    }

    private void SpawnEnemy(GameObject prefabs, Vector3 position, EnemySpawnByTime enemySpawn)
    {
        GameObject go = _gameManager.poolManager.GetPooledObject(prefabs, position, Quaternion.identity);
        
        EnemyView EV = go.GetComponent<EnemyView>();

        EnemyController getEC = ECList.FirstOrDefault(d => d.view == EV);

        if (getEC != null)
        {
            int currentEnemyLevel = GetScaleLevel(_gameManager.resourceManager.dictionaryEnemy[GetKeyEnemyResoucesByType(getEC.GetEnemyType())]);
            getEC.SetData(new EnemyData
            {
                Level = currentEnemyLevel < enemySpawn.minLevel ? enemySpawn.minLevel : currentEnemyLevel,
                dataSO = _gameManager.resourceManager.dictionaryEnemy[GetKeyEnemyResoucesByType(getEC.GetEnemyType())],
                target = _gameManager.PC.view.transform
            });

            Timing.RunCoroutine(SetHealthbar(getEC));
        }
        else
        {
            EnemyController newEC = new EnemyController();

            newEC.Initialize(new EnemyInitializeData
            {
                view = EV,
                gameManager = _gameManager
            });

            int currentEnemyLevel = GetScaleLevel(_gameManager.resourceManager.dictionaryEnemy[GetKeyEnemyResoucesByType(newEC.GetEnemyType())]);

            newEC.SetData(new EnemyData
            {
                Level = currentEnemyLevel < enemySpawn.minLevel ? enemySpawn.minLevel : currentEnemyLevel,
                dataSO = _gameManager.resourceManager.dictionaryEnemy[GetKeyEnemyResoucesByType(newEC.GetEnemyType())],
                target = _gameManager.PC.view.transform
            });

            Timing.RunCoroutine(SetHealthbar(newEC));
            
            ECList.Add(newEC);
        }
        _gameManager.GetEnemyListAction?.Invoke(ECList);
    }

    IEnumerator<float> SetHealthbar(EnemyController EC)
    {
        yield return Timing.WaitUntilDone(Timing.RunCoroutine(_gameManager.uiManager.LoadAndShowPrefab<EnemyHealthBar>(typeof(EnemyHealthBar).Name, _gameManager.uiManager.CanvasScreenSpaceHealthBarGroup, new UIEnemyHealthBarData
        {
            EC = EC
        })));
    }

    public int GetScaleLevel(EnemyStateDataSO dataSO)
    {
        return Mathf.Clamp(Mathf.FloorToInt(_gameManager.GetPlayerLevelByExp() * levelMultiplier), dataSO.GetFirstLevel(), dataSO.GetLastLevel());
    }
    
    public void KillALLEnemy()
    {
        foreach(EnemyController EC in ECList.Where(ec => ec.view.gameObject.activeInHierarchy))
        {
            EC.model.Atk = 0;
            EC.model.baseSpeed = 0;
            EC.view.gameObject.SetActive(false);
        }
    }

    private string GetKeyEnemyResoucesByType(EnemyType type)
    {
        switch (type)
        {
            case (EnemyType.Epic):
                return "EpicEnemy";
            case (EnemyType.Rare):
                return "RareEnemy";
        }
        return "CommonEnemy";
    }


}

[Serializable]
public class EnemySpawnByTime
{
    public GameObject enemyView;
    [MinValue(0)] public float timeAfterStart;
    [MinValue(0)] public float timeDelaySpawn;
    [MinValue(1)] public int amountInstantiate;
    [MinValue(1)] public int amount;
    [MinValue(1)] [MaxValue(9)] public int minLevel;
    [MinValue(0)] [MaxValue(15)] public float radiusSpawn;
}

[Serializable] 
public class EnemyWave
{
    public List<EnemySpawnByTime> waveList;
}