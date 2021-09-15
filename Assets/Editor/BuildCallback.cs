using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Rendering;
using UnityEditor.Build.Reporting;

[InitializeOnLoad]
static class AutoClearMarcroDefine
{
    //[InitializeOnLoadMethod]
    static AutoClearMarcroDefine()
    {
        EditorApplication.delayCall += BindFunc;
    }

    static void BindFunc()
    {
        var fieldInfo = typeof(EditorApplication).GetField("focusChanged", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
        System.Action<bool> action = fieldInfo.GetValue(null) as System.Action<bool>;
        if (action == null)
            fieldInfo.SetValue(null, (System.Action<bool>)OnFocus);
        else
            action += OnFocus;
    }

    public static void OnFocus(bool focus)
    {
        if (!focus)
            return;
        if (EditorApplication.isPlaying || EditorApplication.isPlayingOrWillChangePlaymode || BuildCallback.isBuilding)
            return;
        var syms = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
        var array = syms.Split(new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries);
        var newArray = System.Array.FindAll(array, t => t != BuildCallback.SERVER_MACRO_DEFINE);

        if (newArray.Length < array.Length)
        {
            syms = string.Join(";", newArray);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, syms);
        }
    }
}

public class BuildCallback : IPreprocessBuildWithReport, IPostprocessBuildWithReport
{
    public const string SERVER_MACRO_DEFINE = "UNITY_SERVER";
    public  static bool isBuilding = false;

    public int callbackOrder => 0;

    string symbols = null;

    public void OnPostprocessBuild(BuildReport report)
    {
        isBuilding = false;
        if (symbols != null)
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, symbols);
        symbols = null;
    }

    public void OnPreprocessBuild(BuildReport report)
    {
        isBuilding = true;
        symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
        HeadlessMarcroDefine(EditorUserBuildSettings.enableHeadlessMode);
    }

    void HeadlessMarcroDefine(bool define)
    {
        if (!define) return;
        var newSymbols = symbols + $";{SERVER_MACRO_DEFINE}";
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, newSymbols);
    }
}
