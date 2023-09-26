using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeeleAttack : MonoBehaviour, IAttackHandle
{
    public Vector3 offset;
    private AnimatorModule _animatorController;
    private EnemyView _view;
    private int _damage;
    private float _range;
    private Action action;
    private void Awake()
    {
        _view = GetComponent<EnemyView>();
        _animatorController = GetComponent<AnimatorModule>();
    }

    public void HandleAttack(IData data)
    {
        _animatorController.ChangeAnimation("ZombieAttack", 0.1f);
        EnemyAttackData attackData = (EnemyAttackData) data;
        _damage = attackData.damage;
        _range = attackData.range;
        action = attackData.callbackAction;
    }

    public void TriggerEventAttack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _range, _view.layerCanAttack);
        foreach (var hitCollider in hitColliders)
        {
            IDamage damage = hitCollider.GetComponent<IDamage>();

            if (damage != null && damage.GetHP() > 0)
            {
                damage.HitDamage(_damage);
            }
        }

        
    }

    public void TriggerEventEnd()
    {
        action?.Invoke();
    }
}

public class EnemyAttackData : IData
{
    public int damage;
    public float range;
    public Action callbackAction;
}
