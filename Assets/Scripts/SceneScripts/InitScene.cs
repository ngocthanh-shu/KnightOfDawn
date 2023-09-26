using MEC;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitScene : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = 60;
        InitializeManager();
        ShowLoadingUI();
    }

    private void InitializeManager()
    {
        ResourceManager.Instance.Initialize();
        UIManager.Instance.Initialize();
        AudioManager.Instance.Initialize();
        ControllerManager.Instance.Initialize();
        DataMemoryManager.Instance.Initialize();
    }

    private void ShowLoadingUI()
    { 
        Timing.RunCoroutine(InitAll());
    }


    IEnumerator<float> InitAll()
    {
        yield return Timing.WaitUntilDone(Timing.RunCoroutine(UIManager.Instance.LoadAndShowPrefab<LoadingUI>("LoadingUI", UIManager.Instance.CanvasOverlay)));
        
        LoadingUI loadingUI = UIManager.Instance.GetUI<LoadingUI>("LoadingUI");
        
        yield return Timing.WaitUntilDone(Timing.RunCoroutine(LoadResource(loadingUI)));
        
        yield return Timing.WaitUntilDone(Timing.RunCoroutine(LoadAudio(loadingUI)));

        yield return Timing.WaitUntilDone(Timing.RunCoroutine(LoadMenuScene(loadingUI, "MainMenu")));
        yield break;
    }

    IEnumerator<float> LoadResource(LoadingUI loadingUI)
    {
        ResourceManager.Instance.LoadPlayerData();
        ResourceManager.Instance.LoadEnemyData();
        LoadingBarVaule(loadingUI, 0.4f);
        yield break;
    }

    IEnumerator<float> LoadAudio(LoadingUI loadingUI)
    {
        ResourceManager.Instance.LoadAduioData();
        LoadingBarVaule(loadingUI, 0.8f);
        yield break;
    }

    IEnumerator<float> LoadMenuScene(LoadingUI loadingUI, string sceneName, float waitTime = 1.5f)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
       
        LoadingBarVaule(loadingUI, 1f);
        asyncLoad.allowSceneActivation = false;
        yield return Timing.WaitForSeconds(waitTime);
        asyncLoad.allowSceneActivation = true;
    }

    private void LoadingBarVaule(LoadingUI loadingUI, float value)
    {
        loadingUI.SetData(new LoadingUIData()
        {
            percent = value
        });
    }
}
