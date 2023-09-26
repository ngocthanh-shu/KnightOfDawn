using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : Skill 
{
    public GameObject boomPrefab;

    // ReSharper disable Unity.PerformanceAnalysis
    public override void Active(GameObject rightEnemy = null, PlayerController player = null, AudioClip audio = null)
    {
        base.Active(rightEnemy, player, audio);
        
        GameObject boom = PoolManager.Instance.GetPooledObject(boomPrefab, transform.position, Quaternion.identity);
        BoomObject boomObject = boom.GetComponent<BoomObject>();
        int atk = (int) player.model.Atk/2 + BaseAtk;
        boomObject.SetData(atk, AOERange, Duration, EnemyLayer);
        boomObject.ActiveExplode();
        _currentCooldown = Time.time + Cooldown;
    }

    public override void SpecialEffect()
    {
        base.SpecialEffect();
    }
}
