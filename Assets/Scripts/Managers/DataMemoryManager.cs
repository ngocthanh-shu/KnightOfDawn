using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class DataMemoryManager : Singleton<DataMemoryManager>
{
    [SerializeField] public string fileName;

    public SaveData saveData;
    private string combiedPath;
    public void Initialize()
    {
        //combine path for mobile
        combiedPath = Application.persistentDataPath + "/" + fileName;
    }

    public void SaveData(SaveData data)
    {

        if (string.IsNullOrEmpty(combiedPath))
        {
            //Create a json file if not exist on mobile
            CreateJsonFile(combiedPath);
        }

        string jsonData = JsonConvert.SerializeObject(data);
        File.WriteAllText(combiedPath, jsonData);
    }

    private void CreateJsonFile(string s)
    {
        //Create a json file if not exist on mobile
        File.Create(s);
    }

    public void LoadData()
    {
        if (string.IsNullOrEmpty(combiedPath)) return;
        if (File.Exists(combiedPath))
        {
            string jsonData = File.ReadAllText(combiedPath);
            saveData = JsonConvert.DeserializeObject<SaveData>(jsonData);
        }
    }
    
    public void DeleteData()
    {
        if (string.IsNullOrEmpty(combiedPath)) return;
        if (File.Exists(combiedPath))
        {
            File.Delete(combiedPath);
        }
    }

    public bool IsGetSaveData()
    {
        return File.Exists(combiedPath);
    }
}

public class SaveData
{
    public int Exp;
    public int numberWave;
    public int HP;
    public int maxHP;
    public int Atk;
    public int Def;
    public float time;
    public SkillStatus skill1Status;
    public SkillStatus skill2Status;
    public SkillStatus skill3Status;
    public SkillStatus skill4Status;
}
