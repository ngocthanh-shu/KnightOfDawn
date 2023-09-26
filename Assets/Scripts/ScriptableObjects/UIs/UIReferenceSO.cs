using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "UIReference", menuName = "Data/UI/Manager")]
public class UIReferenceSO : ScriptableObject
{
    [TableList] public List<UIReference> listUIReferences;

    public UIReference GetUIReference(string key)
    {
        foreach(UIReference uiRef in listUIReferences)
        {
            if (key.Equals(uiRef.key))
            {
                return uiRef;
            }
        }
        return null;
    }
}

[Serializable]
public class UIReference
{
    public string key;
    public GameObject prefab;
    public bool isSingle;
}

