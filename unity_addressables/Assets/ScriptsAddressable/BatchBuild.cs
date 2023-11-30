
#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Build.Reporting;
using UnityEngine;

internal class BatchBuild
{
    public static string build_script
        = "Assets/AddressableAssetsData/DataBuilders/BuildScriptPackedMode.asset";

    public static string profile_name = "Default";

    public static void ChangeSettings()
    {
        string defines = "";
        string[] args = Environment.GetCommandLineArgs();

        foreach (var arg in args)
            if (arg.StartsWith("-defines=", System.StringComparison.CurrentCulture))
                defines = arg.Substring(("-defines=".Length));

        var buildSettings = EditorUserBuildSettings.selectedBuildTargetGroup;
        PlayerSettings.SetScriptingDefineSymbolsForGroup(buildSettings, defines);
    }

    [MenuItem("Window/Asset Management/Addressables/Build Addressables and Player2")]
    public static void BuildContentAndPlayer()
    {
        AddressableAssetSettings settings
            = AddressableAssetSettingsDefaultObject.Settings;

        settings.activeProfileId
            = settings.profileSettings.GetProfileId(profile_name);

        IDataBuilder builder
            = AssetDatabase.LoadAssetAtPath<ScriptableObject>(build_script) as IDataBuilder;

        settings.ActivePlayerDataBuilderIndex
            = settings.DataBuilders.IndexOf((ScriptableObject)builder);

        AddressableAssetSettings.BuildPlayerContent(out AddressablesPlayerBuildResult result);

        Debug.Log("AddressablesPlayerBuildResult:" + (string.IsNullOrEmpty(result.Error) ? "Ok" : "Error"));
        if (!string.IsNullOrEmpty(result.Error))
            throw new Exception("AddressablesPlayerBuildResult error:" + result.Error);

        BuildReport buildReport
            = BuildPipeline.BuildPlayer(EditorBuildSettings.scenes,
                "E:/zengfeng/githubs/learn.unity_addressable/unity_addressables/_Build/app.apk", EditorUserBuildSettings.activeBuildTarget,
                BuildOptions.None);

        Debug.Log("buildReport.summary.result:" + buildReport.summary.result);
        if (buildReport.summary.result != BuildResult.Succeeded)
            throw new Exception("buildReport error: " + buildReport.summary.ToString());
    }
}
#endif