using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Audios", fileName = "AudioSO")]
public class AudioSO : ScriptableObject
{
    [TableList] public List<AudioReference> audioReferences;
}

[Serializable]
public class AudioReference
{
    public string key;
    public AudioClip audio;
}