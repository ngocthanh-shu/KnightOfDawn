using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketFire : Skill
{
    public GameObject RocketPrefab;
    private GameObject _rocket;
    private Rocket _rocketFire;
    
    public override void Active(GameObject rightEnemy = null, PlayerController player = null, AudioClip audio = null)
    {
        base.Active(rightEnemy, player, audio);
        var position = transform.position;
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.x = 0;
        transform.rotation = Quaternion.Euler(rotation);
        float distance = Range;
        if (rightEnemy != null)
        {
            var positionEnemy = rightEnemy.transform.position;
            distance = Vector3.Distance(position, positionEnemy);
            transform.rotation = Quaternion.LookRotation(positionEnemy - position);
        }
        _rocket = PoolManager.Instance.GetPooledObject(RocketPrefab, position, transform.rotation);
        _rocketFire = _rocket.GetComponent<Rocket>();
        _rocketFire.SetData(Speed, BaseAtk, distance, AOERange, transform.position, EnemyLayer);
        _currentCooldown = Time.time + Cooldown;
    }
}
