using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : ICharacter, IAttack
{
    public EnemyModel model;
    public EnemyView view;
    public EnemyDamage dame;

    public Action<int> GetHitAction;
    private Action ResetSpeedAction;
    private GameManager _gameManager;

    private float delayAttackTime;
    
    private IAttackHandle AttackHandle;

    #region ICharacter Methods
    public void Initialize(IData data = null)
    {
        if (data == null) return;

        EnemyInitializeData initializeData = (EnemyInitializeData)data;
        model = new EnemyModel();
        view = initializeData.view;
        dame = view.gameObject.AddComponent<EnemyDamage>();

        dame.SetData(new EnemyControllerData
        {
            EC = this
        });

        _gameManager = initializeData.gameManager;
        model.ChangeState(EnemyState.Idle);
        view.Initialize(model);

        AttackHandle = view.AddComponent<EnemyMeeleAttack>();

        ResetSpeedAction += ResetSpeed;
    }

    public void SetData(IData data = null)
    {
        if (data == null) return;

        EnemyData enemyData = (EnemyData) data;

        model.SetData(enemyData.dataSO, enemyData.Level, enemyData.target);

        ResetSpeedAction?.Invoke();
        delayAttackTime = Time.time;
    }

    public void Move()
    {
        view.MoveToTarget(model.target.position);
    }

    #endregion

    #region IAttack Methods
    public void Attack()
    {
        view.SetSpeed(0);
        AttackHandle.HandleAttack(new EnemyAttackData()
        {
            damage = model.Atk,
            range = model.attackRange,
            callbackAction = ResetSpeedAction
        });
        delayAttackTime = Time.time + model.delayAttack;
        model.ChangeState(EnemyState.Idle);
    }

    public void GetHit(int value = 0)
    {
        value -= model.Def / model.DrC;
        model.HP -= value;
        GetHitAction?.Invoke(value);
        _gameManager.audioManager.PlayAduioOneShot(view.audioSource, _gameManager.resourceManager.dictionaryAudio["ZombieHit"]);
        if (model.HP <= 0)
        {
            _gameManager.enemyDeadAction?.Invoke(model.Exp);
            view.gameObject.SetActive(false);
            return;
        }
    }
    #endregion

    #region Main Methods
    public void Update()
    {
        switch(model.state)
        { 
            case EnemyState.Attack:          
                    Attack();
                break;
            default:
                if(!view.navMeshAgent.isStopped)
                    Move();
                if (delayAttackTime < Time.time && view.CanAttack(model.attackRange))
                    model.ChangeState(EnemyState.Attack);
                break;
        }

        view.RotationToTarget(model.target.position);
    }

    public int GetHP()
    {
        return model.HP;
    }

    public void ResetSpeed()
    {
        view.SetSpeed(model.baseSpeed);
    }
    public float GetPercentHP()
    {
        return (float)model.HP / model.maxHP;
    }

    public EnemyType GetEnemyType()
    {
        return view.type;
    }
    #endregion
}

public class EnemyInitializeData : IData
{
    public EnemyView view;
    public GameManager gameManager;
}

public class EnemyData: IData
{
    public int Level;
    public EnemyStateDataSO dataSO;
    public Transform target;
}

public class EnemyControllerData : IData
{
    public EnemyController EC;
}