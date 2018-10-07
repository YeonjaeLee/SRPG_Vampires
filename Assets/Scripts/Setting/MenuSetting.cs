using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(MenuSetting))]
public class MenuSettingEditor : Editor {

    public override void OnInspectorGUI()
    {
        MenuSetting s = new MenuSetting();
        EditorUtility.SetDirty(s);

        if (GUILayout.Button("Clear AssetBundle Cache"))
        {
            Caching.ClearCache();
        }

        if(GUILayout.Button("Delete SaveFile"))
        {
            PlayerPrefs.DeleteAll();
            //ImageCache.RemoveAllCache();
        }

        DrawDefaultInspector();
    }
}
#endif

[System.Serializable]
public class MenuSetting : ScriptableObject
{
    private static MenuSetting instance; 

    public static MenuSetting st
    {
        get
        {
            if(instance == null)
            {
                instance = Resources.Load("MenuSetting") as MenuSetting;
                if(instance == null)
                {
                    // 에셋이 없으면 생성
                    instance = CreateInstance<MenuSetting>();
#if UNITY_EDITOR
                    string properPath = Path.Combine(Application.dataPath, "Resources");
                    if(!Directory.Exists(properPath))
                    {
                        AssetDatabase.CreateFolder("Assets", "Resources");
                    }

                    string fullPath = Path.Combine(Path.Combine("Assets", "Resources"), "MenuSetting.asset");
                    AssetDatabase.CreateAsset(instance, fullPath);
#endif
                }
            }
            return instance;
        }
    }

#if UNITY_EDITOR
    [MenuItem("Game Setting/Menu Setting")]
    public static void Edit()
    {
        Selection.activeObject = st;
    }
#endif

    [HideInInspector]
    public enum GameType
    {
        TYPE1,
        TYPE2,
        TYPE3,
    }

    public GameType gameType;
    public bool isLobbyPopup;
    public bool isProfilePopup;
    public bool isProfileEditPopup;

}