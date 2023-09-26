using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private float speed;
    private int damage;
    private float distance;
    private float AOERange;
    private Vector3 parentPosition;
    private LayerMask enemyLayer;
    public GameObject RocketEffect;
    private AudioSource _audioSource;
    private ParticleSystem _rocketEffect;
    private AudioClip _clip;

    public void SetData(float speed, int damage, float distance, float AOERange, Vector3 parentPosition, LayerMask enemyLayer)
    {
        this.speed = speed;
        this.damage = damage;
        this.distance = distance;
        this.AOERange = AOERange;
        this.parentPosition = parentPosition;
        this.enemyLayer = enemyLayer;
        _audioSource = GetComponent<AudioSource>();
        _clip = ResourceManager.Instance.dictionaryAudio["RocketExplode"];
        _rocketEffect = PoolManager.Instance.GetPooledObject(RocketEffect, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
    }
    
    private void Update()
    {
        Shoot();
        if (Vector3.Distance(transform.position, parentPosition) >= distance)
        {
            Explode();
            if (_clip != null)
                AudioManager.Instance.PlayAduioOneShot(_audioSource, _clip);
            _rocketEffect.Play();
            gameObject.SetActive(false);
        }
    }

    public void Shoot()
    {
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));
    }

    public void Explode()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, AOERange, enemyLayer);
        IDamage enemyDamage;
        foreach (var enemy in enemies)
        {
            enemyDamage = enemy.GetComponent<IDamage>();
            if (enemyDamage != null)
            {
                enemyDamage.HitDamage(damage);
            }
        }
    }
}
