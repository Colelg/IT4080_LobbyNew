using UnityEditor;
using UnityEngine;


[InitializeOnLoadAttribute]
// -----------------------------------------------------------------------------
// These menu'ed things have been adapted from:
// https://forum.unity.com/threads/can-i-stop-auto-compile-after-edit-create-or-remove-a-script.1268315/
// -----------------------------------------------------------------------------
public static class ButchRefreshMenu {
    private const string AUTO_REFRESH_PREF = "butchAutoRefresh";

    static ButchRefreshMenu(){
        EditorApplication.playModeStateChanged += PlayRefresh;
    }


    private static void PlayRefresh(PlayModeStateChange state) {
        if (state == PlayModeStateChange.ExitingEditMode) {
            AssetDatabase.Refresh();
        }
    }


    [InitializeOnLoadMethod]
    private static void Initialize() {
        Debug.Log("Script realoded!");
        AssetDatabase.SaveAssets();
        EditorApplication.LockReloadAssemblies();
    }


    // -----------------------------------------
    // Menu Items
    // -----------------------------------------
    [MenuItem("IT4080/BUTCH/Auto Refresh", false, 500)]
    private static void MnuAutoRefreshToggle() {
        var status = EditorPrefs.GetInt(AUTO_REFRESH_PREF);
        EditorPrefs.SetInt(AUTO_REFRESH_PREF, status == 1 ? 0 : 1);
    }


    [MenuItem("IT4080/BUTCH/Auto Refresh", true, 500)]
    private static bool MnuAutoRefreshToggleValidation() {
        var status = EditorPrefs.GetInt(AUTO_REFRESH_PREF);
        Menu.SetChecked("BUTCH/Auto Refresh", status == 1);
        return true;
    }


    [MenuItem("IT4080/BUTCH/Refresh %#R", false, 500)]
    private static void MnuRefresh() {
        Debug.Log("Request script reload.");

        EditorApplication.UnlockReloadAssemblies();
        AssetDatabase.Refresh();
        EditorUtility.RequestScriptReload();
    }
}