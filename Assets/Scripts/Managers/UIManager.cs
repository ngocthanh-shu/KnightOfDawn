using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using MEC;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private UIReferenceSO UIReferenceSO;
    [SerializeField] public Transform CanvasOverlay;
    [SerializeField] public Transform CanvasScreenSpace;
    [SerializeField] public Transform CanvasScreenSpaceHealthBarGroup;

    public Dictionary<string, List<UIBasic>> dictionaryUI;

    [Button("Init")]
    public void Initialize()
    {
        dictionaryUI = new Dictionary<string, List<UIBasic>>();
    }

    public T GetUI<T>(string k) where T : UIBasic
    {
        if (dictionaryUI.ContainsKey(k) && dictionaryUI[k] != null && dictionaryUI[k].Count > 0)
        {

            foreach (var ui in dictionaryUI[k])
            {
                if (ui != null && ui.gameObject.activeInHierarchy)
                {
                    return (T) ui;
                }
            }
        }
        return null;
    }


    public IEnumerator<float> LoadAndShowPrefab<T>(string k, Transform parent = null, IUIData data = null) where T : UIBasic
    {
        UIBasic uiBasic = null;
        var uiRef = UIReferenceSO.GetUIReference(k);
        if (uiRef == null || uiRef.prefab == null)
        {
            Debug.LogError($"[UI] LoadAndShow [{k}] is not exits");
            yield break;
        }
        //check use in cache
        if (dictionaryUI.ContainsKey(k) && dictionaryUI[k] != null && dictionaryUI[k].Count > 0)
        {
            if (uiRef.isSingle)
            {
                uiBasic = dictionaryUI[k][0];
            }
            else
            {
                foreach (var ui in dictionaryUI[k])
                {
                    if (ui != null && !ui.gameObject.activeInHierarchy)
                    {
                        uiBasic = ui;
                        break;
                    }
                }
            }

            if (uiBasic != null)
            {
                uiBasic.SetData(data);
            }
        }

        //create prefab
        if (uiBasic == null)
        {
            yield return Timing.WaitForOneFrame;
            uiBasic = InstantiatePrefab(k, uiRef.prefab, parent);
            if (uiBasic == null)
            {
                Debug.LogError($"[UI] LoadAndShowPrefab [{k}] is not instantiate");
                yield break;
            }

            if (!uiBasic.IsInit())
            {
                uiBasic.Initialize();
            }

            yield return Timing.WaitUntilTrue(() => uiBasic.IsInit());
            uiBasic.SetData(data);
            yield return Timing.WaitForOneFrame;
        }

        uiBasic.Show(data);
    }


    private UIBasic InstantiatePrefab(string k, GameObject prefab, Transform parent)
    {
        try
        {
            var obj = Instantiate(prefab, parent);
            var baseUi = obj.GetComponent<UIBasic>();
            if (baseUi == null)
            {
                baseUi = obj.AddComponent<UIBasic>();
            }

            if (baseUi != null)
            {
                if (!dictionaryUI.ContainsKey(k))
                {
                    dictionaryUI[k] = new List<UIBasic>() { baseUi };
                }
                else
                {
                    dictionaryUI[k].Add(baseUi);
                }

                return baseUi;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"[UI] InstantiatePrefab [{k}] is exception = {e}");
        }

        return null;
    }

    public void DestroyUIOnParent(Transform parent)
    {
        int childCount = parent.childCount;
        Transform child;
        UIBasic uiComponent;
        for (int i = childCount - 1; i >= 0; i--)
        {
            child = parent.GetChild(i);
            uiComponent = child.GetComponent<UIBasic>();
            
            if (uiComponent != null)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void HideAllUI<T>() where T : UIBasic
    {
        var k = typeof(T).Name;
        if (!dictionaryUI.ContainsKey(k) || dictionaryUI[k] == null || dictionaryUI[k].Count <= 0) return;
        
        foreach (T ui in dictionaryUI[k])
        {
            if (ui != null && ui.gameObject.activeInHierarchy)
            {
                ui.Hide();
            }
        }
    }

    public void DestroyAllUI()
    {
        DestroyUIOnParent(CanvasOverlay);
        DestroyUIOnParent(CanvasScreenSpace);
        DestroyUIOnParent(CanvasScreenSpaceHealthBarGroup);
    }


}