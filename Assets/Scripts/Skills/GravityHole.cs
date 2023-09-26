using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityHole : Skill
{
    public float suckForce;
    public GameObject hole;
    public override void Active(GameObject rightEnemy = null, PlayerController player = null, AudioClip audio = null)
    {
        base.Active(rightEnemy, player, audio);
        Transform holeTransform;
        if(rightEnemy != null)
        {
            holeTransform = rightEnemy.transform;
        }
        else
        {
            holeTransform = player.view.transform;
        }
        holeTransform.position = new Vector3(holeTransform.position.x, player.view.transform.position.y, holeTransform.position.z);
        GameObject holeClone = PoolManager.Instance.GetPooledObject(hole, holeTransform.position, Quaternion.identity);
        Hole holeObject = holeClone.GetComponent<Hole>();
        holeClone.gameObject.transform.localScale = transform.localScale = new Vector3(5, 1, 5);
        holeObject.SetData(BaseAtk, Duration, AOERange, suckForce, EnemyLayer);
        holeObject.ActiveHole();
        _currentCooldown = Time.time + Cooldown;
    }
}
