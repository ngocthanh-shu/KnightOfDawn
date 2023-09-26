#if UNITY_EDITOR

using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using Topebox.Tool.SceneSwitcher;
using UnityEditor;
using UnityEngine;

public class EditorSwitchScene : OdinEditorWindow
{
    [MenuItem("TOPEBOX/Tools/Editor Switch Scene")]
    public static void OpenScreen()
    {
        var window = GetWindow<EditorSwitchScene>();
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
        window.CreateEditor();
    }

    private void CreateEditor()
    {
        if (editor == null)
        {
            editor = Editor.CreateEditor(editorMenuSO);
        }
    }

    [SerializeField] private EditorMenuSO editorMenuSO;
    private Editor editor;

    protected override void OnGUI()
    {
        if (editorMenuSO == null)
        {
            GUILayout.Label("editorMenuSO is null, Please select EditorMenuSO.asset");
            base.OnGUI();
            return;
        }

        CreateEditor();
        editor.OnInspectorGUI();

        GUILayout.Space(10);
        if (GUILayout.Button("Build Menu", GUILayout.Height(30)))
        {
            editorMenuSO.BuildMenu();
        }
    }
}
#endif
