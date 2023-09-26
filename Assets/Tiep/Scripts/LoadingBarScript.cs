using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingBarScript : MonoBehaviour
{
    private Slider loadingBar;
    [SerializeField] private float loadTime;
    [SerializeField] private Text percentText;

    void Start() {
        loadingBar = GetComponent<Slider>();
        loadingBar.value = 0;
        StartCoroutine(LoadScene());
    }

    private IEnumerator FillLoadingBar(float targetProgress, float duration) {
        float initialProgress = loadingBar.value;
        float timer = 0f;

        while (timer < duration) {
            timer += Time.deltaTime;
            float progress = Mathf.Lerp(initialProgress, targetProgress, timer / duration);
            loadingBar.value = progress;
            percentText.text = (Mathf.Round(progress * 100)).ToString() + "%";
            yield return null;
        }

        loadingBar.value = targetProgress;
        percentText.text = targetProgress.ToString() + "%";
    }

    IEnumerator LoadScene() {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Main Menu");
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone) {
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f); // Normalize the progress value

            StartCoroutine(FillLoadingBar(progress, loadTime));
            yield return null;

            if(loadingBar.value >= 0.99) {
                asyncOperation.allowSceneActivation = true;
            }
        }
        
    }


}
