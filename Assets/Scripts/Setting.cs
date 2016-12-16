using UnityEngine;
using System.Collections;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Setting<T> : ScriptableObject where T : Setting<T>
{
    protected static readonly string assetName = "Settings/" + typeof(T).Name;
    #if UNITY_EDITOR
    protected static readonly string assetFolder = "Settings/Resources/Settings";
    protected static readonly string assetFile = typeof(T).Name + ".asset";
    #endif

    protected static T instance;

    public static T Instance {
        get {
            if (instance == null) {
                instance = Resources.Load (assetName) as T;
                if (instance == null) {
                    // If not found, autocreate the asset object.
                    instance = CreateInstance<T> ();
                    instance.OnCreated ();
                    #if UNITY_EDITOR
                        MakeFolderOnAssets(assetFolder);

                        string fullPath = Path.Combine(Path.Combine("Assets", assetFolder), assetFile);
                        AssetDatabase.CreateAsset(instance, fullPath);
                    #endif
                }
            }
            return instance;
        }
    }

    protected virtual void OnCreated ()
    {
        // override in subclass
    }

    #if UNITY_EDITOR
    private static void MakeFolderOnAssets(string path)
    {
        string[] folderNames = path.Split('/');
        string currentPath = "Assets";
        for(int i = 0; i < folderNames.Length; i++)
        {
            string folderPath = Path.Combine(currentPath, folderNames[i]);
            if (!Directory.Exists(folderPath))
            {
                AssetDatabase.CreateFolder(currentPath, folderNames[i]);
            }
            currentPath = folderPath;
        }
    }
    #endif
}

