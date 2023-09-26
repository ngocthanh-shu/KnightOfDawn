using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttack : Skill
{
    public override void Active(GameObject rightEnemy = null, PlayerController player = null, AudioClip audio = null)
    {
        base.Active(rightEnemy, player, audio);
        GameObject[] enemiesInRange = GetNearestEnemies(player.model.enemyList);
        if (enemiesInRange.Length > 0)
        {
            IDamage enemyDamage;
            foreach (var enemy in enemiesInRange)
            {
                enemyDamage = enemy.GetComponent<IDamage>();
                if (enemyDamage != null)
                {
                    if (player != null)
                    {
                        enemyDamage.HitDamage(BaseAtk + (int) player.model.Atk/2);
                    }
                    else
                    {
                        enemyDamage.HitDamage(BaseAtk);
                    }
                }
            }
            _currentCooldown = Time.time + Cooldown;
        }
    }
}
