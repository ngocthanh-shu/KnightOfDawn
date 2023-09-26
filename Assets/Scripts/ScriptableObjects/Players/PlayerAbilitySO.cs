using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Data/Skills/Data")]
public class PlayerAbilitySO : ScriptableObject
{
    public int BaseAtk;
    public float Duration;
    public float Cooldown;
    public float Speed;
    public float Range;
    public bool AOEAttack;
    public float AOERange;
    public Sprite Icon;
}
