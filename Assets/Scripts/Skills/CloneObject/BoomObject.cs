using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BoomObject : MonoBehaviour
{
    private int damage;
    private float AOERange;
    private int duration;
    private LayerMask enemyLayer;
    public GameObject boomEffect;
    public GameObject boomView;
    private Renderer _boomViewRenderer;
    private AudioSource _audioSource;
    private AudioClip _clip;
    private ParticleSystem _boom;

    public void SetData(int damage, float AOERange, float duration, LayerMask enemyLayer)
    {
        this.damage = damage;
        this.AOERange = AOERange;
        this.duration = (int) duration;
        this.enemyLayer = enemyLayer;
        boomView.SetActive(true);
        _audioSource = GetComponent<AudioSource>();
        _boomViewRenderer = boomView.GetComponent<Renderer>();
        _boomViewRenderer.materials[0].DOColor(Color.white, 0).SetSpeedBased();
        _clip = ResourceManager.Instance.dictionaryAudio["Bomb"];
        _boom = PoolManager.Instance.GetPooledObject(boomEffect, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
    }
    
    public void ActiveExplode()
    {
        _boomViewRenderer.materials[0].DOColor(Color.black, 0.5f).SetLoops(duration, LoopType.Yoyo).OnComplete(() =>
        {
            _boomViewRenderer.materials[0].DOColor(Color.white, 0).SetSpeedBased();
            Boomm();
            Invoke("DisableObject", 2f);
        });
    }
    
    public void Boomm()
    {
        if (_clip != null)
        {
            AudioManager.Instance.PlayAduioOneShot(_audioSource, _clip);
        }

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
        
        _boom.Play();
        boomView.SetActive(false);
    }
    
    private void DisableObject()
    {
        gameObject.SetActive(false);
    }
}
