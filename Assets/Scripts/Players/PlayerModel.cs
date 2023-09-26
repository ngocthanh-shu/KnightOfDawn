using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel
{
    private int level;
    public int maxHP;
    public int HP;
    public int Atk;
    public int Def;
    public int DrC;
    public float baseSpeed;
    public float speedModifier;
    public float vision;
    
    public bool isMoving;
    public bool isAttacking;
    public bool isSkillCasting;
    public bool isDead;

    public List<GameObject> enemyList;

    public PlayerModel(PlayerStateDataSO playerState, int level)
    {
        SetData(playerState);
        SetData(level);
        SetState();
        enemyList = new List<GameObject>();
    }

    public void SetData(PlayerStateDataSO playerState)
    {
        maxHP = playerState.HP;
        HP = playerState.HP;
        Atk = playerState.Atk;
        Def = playerState.Def;
        DrC = playerState.DrC;
    }

    public void SetData(int level)
    {
        this.level = level;
        baseSpeed = 10;
        speedModifier = 1;
        vision = 3;
    }

    public void SetState()
    {
        isMoving = false;
        isAttacking = false;
        isSkillCasting = false;
        isDead = false;
    }

    public void SetLevel(int level)
    {
        this.level = level;
    }

    public int GetLevel()
    {
        return level;
    }

    public void IncreaseAttribute(float scale)
    {
        int maxHPTemp = maxHP;
        maxHP = (int) (maxHP * Random.Range(1f, scale));
        maxHPTemp = maxHP - maxHPTemp;
        HP += maxHPTemp;
        Atk = (int) (Atk * Random.Range(1f, scale));
        Def = (int) (Def * Random.Range(1f, scale));
    }
    
    public void SetEnemyList(List<GameObject> enemyList)
    {
        foreach (GameObject enemy in enemyList)
        {
            if(!this.enemyList.Contains(enemy))
                this.enemyList.Add(enemy);
        }
    }
    
    public void AddEnemy(GameObject enemy)
    {
        if(!enemyList.Contains(enemy))
            enemyList.Add(enemy);
    }
}
