using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Rendering;
using UnityEditor.Build.Reporting;

public class BuildCallback : IPreprocessBuildWithReport, IPostprocessBuildWithReport
{
    const string SERVER_MACRO_DEFINE = "UNITY_SERVER";
    public int callbackOrder => 0;

    string symbols = null;

    public void OnPostprocessBuild(BuildReport report)
    {

        if (symbols != null)
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, symbols);
        symbols = null;
    }

    public void OnPreprocessBuild(BuildReport report)
    {

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
