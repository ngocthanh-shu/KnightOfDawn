using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField]
    private AudioMixer audioMixer;
    [SerializeField]
    private AudioClip clickButton;
    private AudioSource _audioSource;
    public void Initialize()
    {
        LoadAudioValue();
        _audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void ChangeAudioValue(string groupName, float value)
    {
        float mappedValue = Mathf.Lerp(-80f, 0f, value / 100f);
        mappedValue = Mathf.Clamp(mappedValue, -80f, 0f);
        audioMixer.SetFloat(groupName, mappedValue);
    }

    public void LoadAudioValue()
    {
        audioMixer.SetFloat("Music Volume", PlayerPrefs.GetFloat(KeyPlayerPrefs.Music, 20f));
        audioMixer.SetFloat("SFX Volume", PlayerPrefs.GetFloat(KeyPlayerPrefs.SFX, 20f));
    }

    public void PlayAduido(AudioSource source, AudioClip clip)
    {
        if (source.clip == clip) return;
        source.clip = clip;
        source.Play();
    }

    public void PlayAduioOneShot(AudioSource source, AudioClip clip)
    {
        source.PlayOneShot(clip);
    }

    
    [Button("Save Aduio")]
    public void SaveAudioValue()
    {
        float musicValue;
        float sfxValue;

        audioMixer.GetFloat("Music Volume", out musicValue);
        audioMixer.GetFloat("SFX Volume", out sfxValue);

        PlayerPrefs.SetFloat(KeyPlayerPrefs.Music, musicValue);
        PlayerPrefs.SetFloat(KeyPlayerPrefs.SFX, sfxValue);
        
    }
    
    public void PlayClickButton()
    {
        PlayAduioOneShot(_audioSource, clickButton);
    }

    public void OnApplicationQuit()
    {
        SaveAudioValue();
    }
}