using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TextureSetting : Setting<TextureSetting>
{
    [MenuItem ("Settings/TextureSetting")]
    public static void Edit ()
    {
        Selection.activeObject = Instance;
    }

    protected override void OnCreated ()
    {
        useFastLoad = true;
    }

    public bool useFastLoad = false;
    public bool useCompression = false;
}
