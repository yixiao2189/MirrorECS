using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Rendering;
using UnityEditor.Build.Reporting;
using System.Linq;

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
    public const string SERVER_MACRO_DEFINE = "SERVER_MODE";
    public const string CLIENT_TEST = "CLIENT_TEST_MODE";
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
        var strs =   symbols.Split(new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries);
        bool hasClientTest = strs.Any(t => t == CLIENT_TEST);
        if(EditorUserBuildSettings.enableHeadlessMode && !hasClientTest)
            HeadlessMarcroDefine();
    }

    void HeadlessMarcroDefine()
    {
        var newSymbols = symbols + $";{SERVER_MACRO_DEFINE}";
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, newSymbols);
    }
}
