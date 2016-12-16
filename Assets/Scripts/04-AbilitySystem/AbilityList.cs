using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu (menuName = "Abilities/Ability List")]
public class AbilityList : Ability
{
    public List<Ability> abilityList;

    public override void Initialize (GameObject obj)
    {
    }

    public override void TriggerAbility ()
    {
    }

}