using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModel
{
    public int maxHP;
    public int HP;
    public int Atk;
    public int Def;
    public int DrC;
    public int Exp;
    public float baseSpeed = 3f;
    public float attackRange = 1.3f;
    public float delayAttack = 4f;
    public Transform target;
    public int Level;

    public EnemyState state;

    public void SetData(EnemyStateDataSO dataSO, int level, Transform _target) 
    {
        EnemyGrowthValue growthValue = dataSO.GetEnemyStateByLevel(level);
        
        maxHP = HP = growthValue.HP;
        Atk = growthValue.Atk;
        Def = growthValue.Def;
        DrC = 100;
        Exp = growthValue.Exp;
        Level = level; 

        target = _target;
    }

    public void ChangeState(EnemyState _state)
    {
        state = _state;
    }
}

public enum EnemyState
{
    Idle,
    Run,
    Attack,
    Dead,
    Hit
}