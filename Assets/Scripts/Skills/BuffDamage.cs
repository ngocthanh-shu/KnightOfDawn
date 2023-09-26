using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffDamage : Skill
{
    private PlayerController _playerController;
    public GameObject buffEffect;
    private ParticleSystem _particleSystem;
    public override void Active(GameObject rightEnemy = null, PlayerController player = null, AudioClip audio = null)
    {
        base.Active(rightEnemy, player, audio);
        if (player != null)
        {
            _playerController = player;
            IncreasePlayerStat(_playerController);
            _currentCooldown = Time.time + Cooldown;
            Invoke(nameof(DecreasePlayerStat), Duration);
        }
    }
    
    public void IncreasePlayerStat(PlayerController player)
    {
        _particleSystem = Instantiate(buffEffect, player.view.transform.position, Quaternion.identity, player.view.transform).GetComponent<ParticleSystem>();
        _particleSystem.Play();
        player.model.Atk *= BaseAtk;
        player.model.speedModifier += Speed;
        if (player.model.HP < player.model.maxHP * 0.75)
        {
            player.model.HP = (int) (player.model.HP + player.model.maxHP * 0.25f);
        }
        else
        {
            player.model.HP = player.model.maxHP;
        }
        player._gameManager.UpdateHpAction?.Invoke();
    }

    public void DecreasePlayerStat()
    {
        _particleSystem.Stop();
        _playerController.model.Atk /= BaseAtk;
        _playerController.model.speedModifier -= Speed;
    }

    public override void SpecialEffect()
    {
        base.SpecialEffect();
    }

    public int GetDamageBonus()
    {
        return BaseAtk;
    }
}
