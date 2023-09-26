using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class EditorTool
{
    [MenuItem("Tools/Application/Data Path")]
    public static void dataPath()
    {
        EditorUtility.RevealInFinder(Application.dataPath);
    }
    
    [MenuItem("Tools/Application/Persistent Data Path")]
    public static void persistentDataPath()
    {
        EditorUtility.RevealInFinder(Application.persistentDataPath);
    }
    
    [MenuItem("Tools/Application/Streaming Assets Path")]
    public static void streamingAssetsPath()
    {
        EditorUtility.RevealInFinder(Application.streamingAssetsPath);
    }
    
    [MenuItem("Tools/Application/Temporary Cache Path")]
    public static void temporaryCachePath()
    {
        EditorUtility.RevealInFinder(Application.temporaryCachePath);
    }
    
    [MenuItem("Tools/Application/Console Log Path")]
    public static void consoleLogPath()
    {
        EditorUtility.RevealInFinder(Application.consoleLogPath);
    }
}