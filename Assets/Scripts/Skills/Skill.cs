using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Skill : MonoBehaviour, ISkill
{
    public PlayerAbilitySO Data;
    public int BaseAtk;
    public float Duration;
    public float Speed;
    public float Cooldown;
    public float Range;
    public float AOERange;
    public bool AOEAttack;
    public LayerMask EnemyLayer;
    protected float _currentCooldown;
    public Sprite Icon;
    public ParticleSystem ParticleSystem;
    private AudioSource _audioSource;

    public Action cooldownAction;

    private void Awake()
    {
        _currentCooldown = Time.time;
        _audioSource = GetComponent<AudioSource>();
        SetData();
    }

    public void SetData()
    {
        BaseAtk = Data.BaseAtk;
        Duration = Data.Duration;
        Speed = Data.Speed;
        Cooldown = Data.Cooldown;
        Range = Data.Range;
        AOERange = Data.AOERange;
        AOEAttack = Data.AOEAttack;
        Icon = Data.Icon;
    }

    public GameObject GetNearestEnemy(List<GameObject> enemies)
    {
        GameObject[] enemiesInRange = Array.FindAll(enemies.ToArray(), enemy => Vector3.Distance(transform.position, enemy.transform.position) <= Range && enemy.activeSelf);
        GameObject nearestEnemy = null;
        float minDistance = Mathf.Infinity;
        foreach (var enemy in enemiesInRange)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = enemy;
            }
        }
        return nearestEnemy;
    }

    public GameObject[] GetNearestEnemies(List<GameObject> enemies)
    {
        return Array.FindAll(enemies.ToArray(), enemy => Vector3.Distance(transform.position, enemy.transform.position) <= Range && enemy.activeSelf);
    }


    public virtual void Active(GameObject rightEnemy = null, PlayerController player = null, AudioClip audioClip = null)
    {
        cooldownAction?.Invoke();
        ActiveParticle(player);
        if(_audioSource != null && audioClip != null)
            AudioManager.Instance.PlayAduioOneShot(_audioSource, audioClip);
    }

    public virtual void SpecialEffect()
    {
        
    }

    public bool CooldownCalculate()
    {
        if (Time.time >= _currentCooldown)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public Sprite GetIcon()
    {
        return Icon;
    }

    public virtual void ActiveParticle(PlayerController player)
    {
        if (ParticleSystem != null)
        {
            ParticleSystem.transform.position = player.view.transform.position + player.view.transform.forward * 0.5f;
            ParticleSystem.transform.rotation = player.view.transform.rotation;
            ParticleSystem.Play();
        }
    }
}
