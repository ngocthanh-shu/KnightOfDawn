using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    public Dictionary<string, PlayerStateDataSO> dictionaryPlayer;
    public Dictionary<string, EnemyStateDataSO> dictionaryEnemy;
    public Dictionary<string, AudioClip> dictionaryAudio;

    public PlayerGrowth playerGrowth;
    public AudioSO audioSO;
    [FolderPath(ParentFolder = "Assets/Resources")]
    public string folderPathPlayer;
    [FolderPath(ParentFolder = "Assets/Resources")]
    public string folderPathPlayerGrowing;
    [FolderPath(ParentFolder = "Assets/Resources")]
    public string folderPathEnemy;
    



    public void Initialize()
    {
        dictionaryPlayer = new Dictionary<string, PlayerStateDataSO>();
        dictionaryEnemy = new Dictionary<string, EnemyStateDataSO>();
        dictionaryAudio = new Dictionary<string, AudioClip>();
        playerGrowth = Resources.LoadAll<PlayerGrowth>(folderPathPlayerGrowing)[0];
    }

    public void LoadPlayerData()
    {
        LoadData<PlayerStateDataSO>(dictionaryPlayer, folderPathPlayer);
    }

    public void LoadEnemyData()
    {
        LoadData<EnemyStateDataSO>(dictionaryEnemy, folderPathEnemy);
    }

    public void LoadAduioData()
    {
        foreach(var audioRef in audioSO.audioReferences)
        {
            dictionaryAudio.Add(audioRef.key, audioRef.audio);
        }
    }

    public void LoadData<T>(Dictionary<string, T> dictionary, string folderPath) where T : ScriptableObject
    {
        if (dictionary == null | string.IsNullOrEmpty(folderPath)) return;

        var listDataSo = Resources.LoadAll<T>(folderPath);
        foreach (var t in listDataSo)
        {
            dictionary.Add(t.name, t);
        }
    }
}
