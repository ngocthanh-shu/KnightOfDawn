#if UNITY_EDITOR
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Topebox.Tool.SceneSwitcher
{
    [System.Serializable]
    public class Menu
    {
        [VerticalGroup("1")] [Sirenix.OdinInspector.FilePath]
        public string scenePath;

        [VerticalGroup("1")] public string menuPath;
        [VerticalGroup("1")] public string tip;

        [VerticalGroup("2")] public bool firstScene;
        [VerticalGroup("2")] public string subMenu;
        [VerticalGroup("2")] public string shortCut;
    }

    [CreateAssetMenu(fileName = "EditorMenuSO", menuName = "TOPEBOX/Tools/EditorMenuSO")]
    public class EditorMenuSO : ScriptableObject
    {
        [Title("Scene List")] [TableList] public Menu[] menus;
        [Title("Start Scene")] public string startSceneMenu;

        private const string TemplateRoot = "Tools/EditorMenu/Editor/Template";
        private const string OutPutContent = "Tools/EditorMenu/Editor";

        public void BuildMenu()
        {
            BuildCustomMenu();
            BuildSceneSwitcher();
            AssetDatabase.Refresh();
        }

        private void BuildCustomMenu()
        {
            var templatePath = Path.Combine(Application.dataPath, TemplateRoot, "CustomMenu_template.txt");

            //check file is exist
            if (!File.Exists(templatePath)) return;

            //load file content
            var content = File.ReadAllText(templatePath);

            var class_name = "GenerateCustomMenu";
            var main_content = "";
            var start_scene_path = "";

            //build content
            Menu firstScene = default;
            if (menus != null)
            {
                for (var i = 0; i < menus.Length; i++)
                {
                    var m = menus[i];
                    if (string.IsNullOrEmpty(m.scenePath)) continue;
                    if (m.firstScene && firstScene == null)
                    {
                        firstScene = m;
                    }

                    main_content += $"\t\t//============== {m.menuPath} - {m.tip} =================\n";
                    main_content += $"\t\tprivate static string scene_{i}_path = \"{m.scenePath}\";\n";
                    main_content += $"\t\t[MenuItem(\"{m.menuPath} Scene {m.shortCut}\")]\n";
                    main_content += $"\t\tpublic static void quick{m.menuPath.Replace("/", "_").Replace(" ", "")}()\n";
                    main_content += "\t\t{\n";
                    main_content += $"\t\t\topenSceneWithSaveConfirm(scene_{i}_path);\n";
                    main_content += "\t\t}\n\n";
                }

                firstScene ??= menus[0];
                start_scene_path = $"private static string start_scene_path = \"{firstScene.scenePath}\";";
            }

            //replace content
            content = content.Replace("<class_name>", class_name);
            content = content.Replace("<start_scene_menu>", startSceneMenu);
            content = content.Replace("<start_scene_path>", start_scene_path);
            content = content.Replace("<main_content>", main_content);

            //write file new content
            var outputPath = Path.Combine(Application.dataPath, OutPutContent, $"{class_name}.cs");
            if (!Directory.Exists(Path.Combine(Application.dataPath, OutPutContent)))
            {
                Directory.CreateDirectory(Path.Combine(Application.dataPath, OutPutContent));
            }

            File.WriteAllText(outputPath, content);
        }

        private void BuildSceneSwitcher()
        {
            var templatePath = Path.Combine(Application.dataPath, TemplateRoot, "SceneSwitcher_template.txt");

            //check file is exist
            if (!File.Exists(templatePath)) return;

            //load file content
            var content = File.ReadAllText(templatePath);

            var main_content = "";

            if (menus != null)
            {
                foreach (var m in menus)
                {
                    if (string.IsNullOrEmpty(m.scenePath)) continue;
                    main_content +=
                        $"\t\t\tif(GUILayout.Button(new GUIContent(\"{m.subMenu}\", \"{m.tip}\"), ToolbarStyles.commandButtonStyle))\n";
                    main_content += "\t\t\t{\n";
                    main_content += $"\t\t\t\tGenerateCustomMenu.quick{m.menuPath.Replace("/", "_").Replace(" ", "")}();\n";
                    main_content += "\t\t\t}\n";
                }
            }

            content = content.Replace("<main_content>", main_content);

            //replace content
            var outputPath = Path.Combine(Application.dataPath, OutPutContent, "GenerateSceneSwitcher.cs");
            if (!Directory.Exists(Path.Combine(Application.dataPath, OutPutContent)))
            {
                Directory.CreateDirectory(Path.Combine(Application.dataPath, OutPutContent));
            }

            //write file new content
            File.WriteAllText(outputPath, content);
        }
    }
}
#endif
