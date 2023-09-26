using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerGrowth", menuName = "Data/Player/PlayerGrowth")]
public class PlayerGrowth : ScriptableObject
{
    public float ScaleAttributeByLv;
    [TableList] public List<RequiredExpForLevel> RequiredTable;

}

[Serializable]
public class RequiredExpForLevel
{
    public int level;
    public int RequiredExp;
}
