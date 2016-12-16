using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameSetting : Setting<GameSetting>
{
    [MenuItem ("Settings/GameSetting")]
    public static void Edit ()
    {
        Selection.activeObject = Instance;
    }

    public DifficultySetting difficultySetting;
}
