using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUI
{
    void SetDefault();
    void Initialize();
    void SetData(IUIData UIData = null);
    void Show(IUIData UIData = null);
    void Hide();
    bool IsInit();
}
public interface IUIData
{ 
}

public interface ICharacter {
    void Initialize(IData data = null);
    void SetData(IData data = null);
    void Move();
    void GetHit(int value = 0);
}

public interface IDamage
{
    void SetData(IData data = null);
    int GetHP();
    void HitDamage(int value = 0);
}

public interface IAttack
{
    void Attack();
}

public interface ISkill
{
    void Active(GameObject rightEnemy = null, PlayerController player = null, AudioClip audio = null);
    void SpecialEffect();
    void SetData();
    GameObject GetNearestEnemy(List<GameObject> enemies);
    GameObject[] GetNearestEnemies(List<GameObject> enemies);
}

public interface IAttackHandle
{
    void HandleAttack(IData data);
}

public interface IData
{
}