using UnityEngine;
using System.Collections;
using UnityEditor;

public class MakeDialogSO
{
    [MenuItem("Assets/Create/My Scriptable Object")]
    public static void CreateMyAsset()
    {
        DialogSO asset = ScriptableObject.CreateInstance<DialogSO>();

        AssetDatabase.CreateAsset(asset, "Assets/NewDialogObject.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}