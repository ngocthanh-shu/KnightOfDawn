using MEC;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : UIBasic
{
    public Slider musicSlider;
    public Slider sfxSlider;
    public GameObject continueButton;
    
    public ToggleGroup genderGroup;
    public Toggle maleToggle;
    public Toggle femaleToggle;

    public override void SetData(IUIData UIData = null)
    {
        MainMenuUIData data = (MainMenuUIData) UIData;
        musicSlider.value = GetMappedAudioValue(data.MusicValue);
        sfxSlider.value = GetMappedAudioValue(data.SFXValue);

        if(data.Gender > 0)
        {
            maleToggle.isOn = true;
            femaleToggle.isOn = false;
        }
        else
        {
            maleToggle.isOn = false;
            femaleToggle.isOn = true;
        }
        
        if(DataMemoryManager.Instance.IsGetSaveData())
        {
            continueButton.SetActive(true);
        }
        else
        {
            continueButton.SetActive(false);
        }
    }

    public void OnClickNewGame()
    {
        Timing.RunCoroutine(LoadStartGame("Map"));
    }

    public void OnClickContinue()
    {
        Timing.RunCoroutine(LoadStartGame("Map", true));
    }

    public void MusicChangeValue(float value)
    {
        AudioManager.Instance.ChangeAudioValue("Music Volume", value);
    }

    public void SFXChangeValue(float value)
    {
        AudioManager.Instance.ChangeAudioValue("SFX Volume", value);
    }

    public void OnGenderChange()
    {
        Toggle toggle = genderGroup.ActiveToggles().FirstOrDefault();
        if (toggle == maleToggle)
        {
            PlayerPrefs.SetInt(KeyPlayerPrefs.Gender, 1);
        }
        else
        {
            PlayerPrefs.SetInt(KeyPlayerPrefs.Gender, 0);
        }
    }

    private float GetMappedAudioValue(float value)
    {
        float mappedValue = Mathf.Lerp(0f, 100f, (value + 80f) / 80f);
        return Mathf.Clamp(mappedValue, 0f, 100f);
    }

    IEnumerator<float> LoadStartGame(string sceneName, bool isContinue = false, float waitTime = 1.5f)
    {
        yield return Timing.WaitUntilDone(Timing.RunCoroutine(UIManager.Instance.LoadAndShowPrefab<LoadingUI>("LoadingUI", UIManager.Instance.CanvasOverlay)));

        LoadingUI loadingUI = UIManager.Instance.GetUI<LoadingUI>("LoadingUI");
        DataMemoryManager memoryManager = DataMemoryManager.Instance;

        if (isContinue)
        {
            if (memoryManager.IsGetSaveData())
            {
                memoryManager.LoadData();

                loadingUI.SetData(new LoadingUIData
                {
                    percent = 0.75f
                });
            }
        }
        else
        {
            memoryManager.saveData = null;
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        asyncLoad.allowSceneActivation = false;

        loadingUI.SetData(new LoadingUIData
        {
            percent = 1f
        });

        yield return Timing.WaitForSeconds(waitTime);
        asyncLoad.allowSceneActivation = true;
    }

    public void OnClickButtonSound()
    {
        AudioManager.Instance.PlayClickButton();
    }
    
    public void OnClickQuit()
    {
        Application.Quit();
    }
}

public class MainMenuUIData : IUIData
{
    public float MusicValue;
    public float SFXValue;
    public int Gender;
}
