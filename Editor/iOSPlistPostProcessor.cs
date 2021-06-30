#if UNITY_IOS
using UnityEngine;

using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

using System.IO;
using System.Collections.Generic;

public class iOSPlistPostProcessor
{
    const string _resource_folder = "Packages/com.unity.ios-plist-postprocessor/Resources";
    const string _asset_name = "IOSPlistScriptable.asset";

    static iOSPlistScriptable _plist_scriptable;
    
    static iOSPlistScriptable PlistScriptable
    {
        get
        {
            if (!_plist_scriptable)
            {
                var asset_path = $"{_resource_folder}/{_asset_name}";
                _plist_scriptable = AssetDatabase.LoadAssetAtPath<iOSPlistScriptable>(asset_path);
            }

            return _plist_scriptable;
        }
    }

    [MenuItem("Tools/PostProcessing/iOS/Select Plist Scriptable")]
    public static void SelectPlistScriptable()
    {
        Selection.activeObject = PlistScriptable;
    }

    [PostProcessBuild(1)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuildProject)
    {
        if (target == BuildTarget.iOS)
        {
            Debug.Log($"<b>iOSPostProcessor.OnPostprocessBuild</b> Removing [{PlistScriptable.plist_items_to_remove.Length}] (<color=green>{string.Join(", ", PlistScriptable.plist_items_to_remove)}</color>) from plist. Path: {pathToBuildProject}");

            var proj_path = PBXProject.GetPBXProjectPath(pathToBuildProject);
            var project = new PBXProject();

            project.ReadFromFile(proj_path);

#if UNITY_2020_1_OR_NEWER
            var targetGUID = project.GetUnityMainTargetGuid();
            var targetName = project.GetUnityMainTargetGuid();
#else
            var targetName = PBXProject.GetUnityTargetName();
            var targetGUID = project.TargetGuidByName(targetName);
#endif

            var plist_path = Path.Combine(pathToBuildProject, "Info.plist");
            var plist = new PlistDocument();

            plist.ReadFromFile(plist_path);

            Debug.Log($"<b>iOSPostProcessor.OnPostprocessBuild</b> targetName: {targetName}, targetGUID: {targetGUID}, plist_path: {plist_path}, \n plist:\n {string.Join("\n", plist.root.values)}");

            foreach (var item in PlistScriptable.plist_items_to_remove)
            {
                plist.root.values.Remove(item);

            }

            foreach (var item in PlistScriptable.plist_items_to_add)
            {
                if (!plist.root.values.ContainsKey(item.Key))
                {
                    plist.root.values.Add(item.Key, new PlistElementString(item.Value));
                }
            }
            plist.WriteToFile(plist_path);
        }
    }
}
#endif
