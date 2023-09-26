using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class Hole : MonoBehaviour
{
    private int _damage;
    private float _duration;
    private float _aoeRange;
    private float _suckForce;
    private LayerMask _enemyLayer;
    public GameObject holeEffect;
    private ParticleSystem _hole;
    
    private float _delayDamage = 0.5f;
    private float _delayDamageTimer = 0;
    
    
    private float value = 0;
    public void SetData(int damage, float duration, float aoeRange, float suckForce, LayerMask enemyLayer)
    {
        _damage = damage;
        _duration = duration;
        _aoeRange = aoeRange;
        _suckForce = suckForce;
        _enemyLayer = enemyLayer;
        _delayDamageTimer = Time.time;
        _hole = PoolManager.Instance.GetPooledObject(holeEffect, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
    }
    

    public void ActiveHole()
    {
        _hole.transform.position = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
        _hole.Play();
        transform.DOScale(value, _duration).OnUpdate(() =>
        {
            SuckEnemies();
        }).OnComplete(() =>
        {
            _hole.Stop();
            gameObject.SetActive(false);
        });
    }

  
    public void SuckEnemies()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, _aoeRange, _enemyLayer);
        Rigidbody rb;
        Vector3 directionToEnemy;
        IDamage enemyDamage;
        foreach (var enemy in enemies)
        {
            rb = enemy.GetComponent<Rigidbody>();
            directionToEnemy = transform.position - rb.position;
            rb.MovePosition(rb.position + directionToEnemy.normalized * (_suckForce * Time.deltaTime));
            
            if(Time.time >= _delayDamageTimer)
            {
                enemyDamage = enemy.GetComponent<IDamage>();
                if (enemyDamage != null)
                {
                    enemyDamage.HitDamage(_damage);
                }
                _delayDamageTimer = Time.time + _delayDamage;
            }
        }
    }
}
