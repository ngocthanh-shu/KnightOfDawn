using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStateData", menuName = "Data/Enemy/StateData")]
public class EnemyStateDataSO : ScriptableObject
{
    [TableList] public List<EnemyGrowthValue> EnemyGrowthData;

    public EnemyGrowthValue GetEnemyStateByLevel(int _level = 1)
    {
        foreach(EnemyGrowthValue growthValue in EnemyGrowthData)
        {
            if (growthValue.Level == _level)
            {
                return growthValue;
            }
        }
        return EnemyGrowthData[0];
    }

    public int GetFirstLevel()
    {
        return EnemyGrowthData[0].Level;
    }

    public int GetLastLevel()
    {
        return EnemyGrowthData[EnemyGrowthData.Count-1].Level;
    }
}


[Serializable]
public class EnemyGrowthValue
{
    public int Level;
    public int Atk;
    public int Def;
    public int HP;
    public int Exp;
}