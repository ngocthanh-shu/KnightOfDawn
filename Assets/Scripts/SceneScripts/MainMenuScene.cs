using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScene : MonoBehaviour
{
    private void Awake()
    {
        AudioManager.Instance.PlayAduido(GetComponent<AudioSource>(), ResourceManager.Instance.dictionaryAudio["MenuMusic"]);
    }

    void Start()
    {
        UIManager.Instance.DestroyAllUI();
        Timing.RunCoroutine(UIManager.Instance.LoadAndShowPrefab<MainMenuUI>("MainMenuUI", UIManager.Instance.CanvasOverlay, new MainMenuUIData { 
            MusicValue = PlayerPrefs.GetFloat(KeyPlayerPrefs.Music, 0),
            SFXValue = PlayerPrefs.GetFloat(KeyPlayerPrefs.SFX, 0),
            Gender = PlayerPrefs.GetInt(KeyPlayerPrefs.Gender, 1),
        }));
    }
}
