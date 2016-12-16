using UnityEngine;
using System.Collections;

public abstract class Ability : ScriptableObject
{

    public string aName = "New Ability";
    public Sprite aSprite;
    public AudioClip aSound;
    public float aBaseCoolDown = 1f;

    public abstract void Initialize (GameObject obj);

    public abstract void TriggerAbility ();
}