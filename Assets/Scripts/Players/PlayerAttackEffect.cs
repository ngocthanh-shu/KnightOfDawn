using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAttackEffect : MonoBehaviour
{
    public List<ParticleSystem> attackEffects;
    public Action attackHitCallback;

    void NormalAttackHit()
    {
        int randomEffect = Random.Range(0, attackEffects.Count);
        attackEffects[randomEffect].transform.position = transform.position + transform.forward * 0.5f;
        attackEffects[randomEffect].transform.rotation = transform.rotation;
        attackEffects[randomEffect].Play();
        attackHitCallback?.Invoke();
    }
}
