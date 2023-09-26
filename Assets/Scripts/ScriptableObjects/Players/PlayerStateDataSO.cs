using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStateData", menuName = "Data/Player/StateData")]
public class PlayerStateDataSO : ScriptableObject
{
    public int Atk;
    public int Def;
    public int HP;
    public int DrC;
    
    
    [TableList] public List<LevelGetAbility> AbilityList;
}

[Serializable]
public class LevelGetAbility
{
    public int level;
    public string abilityName;
    public GameObject ability;
}
