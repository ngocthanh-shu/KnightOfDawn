using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorModule : MonoBehaviour
{
    [Header("Animation")]
    [TableList] public List<AnimationInfo> animationInfors;

    public Dictionary<string, AnimationInfo> dictionaryAnimationInfo;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        dictionaryAnimationInfo = new Dictionary<string, AnimationInfo>();
    }

    private void Start()
    {
        foreach(AnimationInfo info in animationInfors)
        {
            dictionaryAnimationInfo[info.name] = info;
        }
    }

    public void ChangeAnimation(string key, float timeTransitive)
    {
        if (!dictionaryAnimationInfo.ContainsKey(key) || dictionaryAnimationInfo[key] == null) return;

        animator.CrossFade(dictionaryAnimationInfo[key].name, timeTransitive);
    }

    public void SetBooleanAttribute(string name, bool value = true)
    {
        animator.SetBool(name, value);
    }

    public float GetEndTime(string key)
    {
        if (!dictionaryAnimationInfo.ContainsKey(key) || dictionaryAnimationInfo[key] == null) return 0;
        return dictionaryAnimationInfo[key].endTime;
    }

    public float GetTransitionTime(string key)
    {
        if (!dictionaryAnimationInfo.ContainsKey(key) || dictionaryAnimationInfo[key] == null) return 0;

        
        return dictionaryAnimationInfo[key].endTime;
    }

    public bool IsCurrentAnimation(string key)
    { 
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        return currentState.IsName(dictionaryAnimationInfo[key].name);
    }
}

[Serializable]
public class AnimationInfo
{
    public string name;
    public float endTime;
    public float transitionTime;
}