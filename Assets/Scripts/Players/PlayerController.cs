using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : ICharacter, IAttack
{
    public PlayerModel model;
    public PlayerView view;
    public PlayerDamage dmg;
    public PlayerAttackEffect attackEffect;
    
    public Action<int> GetHitAction;

    public GameManager _gameManager;

    #region ICharacter Methods
    public void Initialize(IData data = null)
    {
        PlayerInitializeData initializeData = (PlayerInitializeData) data;

        if (initializeData != null)
        {
            dmg = initializeData.dmg;
            view = initializeData.view;
            attackEffect = initializeData.attackEffect;
            view.transform.rotation = Quaternion.Euler(0, -45f, 0);
            _gameManager = initializeData.gameManager;
        }

        InitializeCallbacks();
        InitializeActions();
    }

    public void SetData(IData data = null)
    {
        PlayerModelData modelData = (PlayerModelData)data;
        model = new PlayerModel(modelData.data, _gameManager.GetPlayerLevelByExp());
    }
    
    public void OnUpdate()
    {
        if(!model.isSkillCasting)
            Rotate();
        Move();
        //Attack();
        view.Animation().SetAttackAnimation(model.isAttacking);
    }

    public void Rotate()
    {
        view.Rotate(model.isAttacking,model.vision,model.enemyList);
    }
    
    public void Move()
    {
        if(model.isAttacking)
            model.speedModifier = 0.5f;
        else
            model.speedModifier = 1f;
        view.Move(model.baseSpeed * model.speedModifier);
        view.Animation().SetMovingAnimation(view.IsMoving());
    }

    #endregion

    private void InitializeCallbacks()
    {
        if (_gameManager == null) return;
        _gameManager.controllerManager.moveAction += ReadInputMovement;
        _gameManager.controllerManager.attackAction += ReadInputAttack;
        _gameManager.controllerManager.attackCancelAction += ReadInputAttackCancel;
        _gameManager.controllerManager.skill1Action += ReadInputSkill1;
        _gameManager.controllerManager.skill2Action += ReadInputSkill2;
        _gameManager.controllerManager.skill3Action += ReadInputSkill3;
        _gameManager.controllerManager.skill4Action += ReadInputSkill4;
    }

    private void InitializeActions()
    {
        dmg.hitDamageAction += GetHit;
        dmg.getHPFunc += GetHP;
        attackEffect.attackHitCallback += Attack;
    }

    public void RemoveAction()
    {
        _gameManager.controllerManager.moveAction -= ReadInputMovement;
        _gameManager.controllerManager.attackAction -= ReadInputAttack;
        _gameManager.controllerManager.attackCancelAction -= ReadInputAttackCancel;
        _gameManager.controllerManager.skill1Action -= ReadInputSkill1;
        _gameManager.controllerManager.skill2Action -= ReadInputSkill2;
        _gameManager.controllerManager.skill3Action -= ReadInputSkill3;
        _gameManager.controllerManager.skill4Action -= ReadInputSkill4;
    }

    #region IAttack Methods
    public void Attack()
    {
        _gameManager.abilityManager.NormalAttack();
    }

    public void GetHit(int value = 0)
    {
        value -= model.Def / model.DrC;
        model.HP -= value;
        GetHitAction?.Invoke(value);
        _gameManager.ShowIndicatorAction?.Invoke();
        if(model.HP <= 0)
            _gameManager.endGame?.Invoke();
    }
    #endregion
    
    public int GetHP()
    {
        return model.HP;
    }

    #region Main Methods
    public void ReadInputMovement(Vector2 directionInput)
    {
        model.isMoving = directionInput != Vector2.zero;
        view.SetDirection(directionInput);
        view.Animation().SetMovementAnimation(directionInput.x, directionInput.y);
    }
    
    private void ReadInputAttack()
    {
        model.isAttacking = true;
        model.isSkillCasting = false;
        view.Animation().SetDirection(view.GetDirectionX(), view.GetDirectionY());
    }
    
    private void ReadInputAttackCancel()
    {
        model.isAttacking = false;
        view.Animation().ResetDirection();
    }


    private void ReadInputSkill1()
    {
        _gameManager.abilityManager.Skill1();
        view.Animation().CastingSkillAnimation();
    }
    
    private void ReadInputSkill2()
    {
        _gameManager.abilityManager.Skill2();
        view.Animation().CastingSkillAnimation();
    }
    
    private void ReadInputSkill3()
    {
        _gameManager.abilityManager.Skill3();
        view.Animation().CastingSkillAnimation();
    }
    
    private void ReadInputSkill4()
    {
        _gameManager.abilityManager.Skill4();
        view.Animation().CastingSkillAnimation();
    }
    #endregion

    public IEnumerator StayRotate(GameObject target)
    {
        model.isSkillCasting = true;
        view.Rotate(model.isAttacking, model.vision, model.enemyList, target);
        yield return new WaitForSeconds(0.5f);
        model.isSkillCasting = false;
    }
}

public class PlayerInitializeData : IData
{
    public PlayerView view;
    public PlayerDamage dmg;
    public GameManager gameManager;
    public PlayerAttackEffect attackEffect;
}

public class PlayerModelData : IData
{
    public PlayerStateDataSO data;
}