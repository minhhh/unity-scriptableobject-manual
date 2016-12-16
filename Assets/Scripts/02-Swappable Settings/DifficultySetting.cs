using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Settings/DifficultySetting")]
public class DifficultySetting : ScriptableObject
{
    [Header("Physics")]
    public float gravity;
    public float drag;

    [Header("Size")]
    public float frontSize;
    public float backSize;
}
