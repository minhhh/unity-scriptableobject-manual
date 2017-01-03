# unity-test-scriptableobject

In the [Introduction to Scriptable Objects](https://unity3d.com/learn/tutorials/modules/beginner/live-training-archive/scriptable-objects) tutorial, they claim that `Scriptable Objects are amazing data containers`. However, it's not clear how useful Scriptable Objects really is, and what problems they're trying to solve. In this article we will look at all usecases of Scriptable Objects, what issues they're trying to solve, what the original solutions are, and what the pros and cons of each approach are.

## Overall

**What is a Scriptable Object**

* Data Container
* Can NOT be attached to GameObject/Prefab
* Can be serialized and inspected like MonoBehaviour
* Can be put into .asset file

<br/>

**Pros**

* Built into Unity
* Can be saved as assets
* Can save during runtime
* Can be referenced instead of copied like MonoBehaviour
* Internal solution (no files/parsing). Performance is quite fast.
* Add to structure as you go. No need to go through a large file/multiple files to replace schema.

<br/>

**Cons**

* Requires Editor Scripting
* Can't edit outside Unity
* Can't save once deployed
* Cannot optimize loading speed since this is Unity code


## Use case 1: Global Game Settings

Every game has global settings for various aspects such as: Sounds, Video, Game play. There are some options to store these settings:

1. Scattered throughout the code, or maybe centralized into one source file.
    1. Pros: Easy to code
    1. Cons: Designers cannot find these settings easily. Change cannot persist if you quit `Play` mode
2. Stored in config files: text/binary.
    1. Pros: Easy to code. Designers can look at them if they're text files.
    1. Cons: Cannot change settings while playing. Might be hard for designers to understand and change the settings files since there's no validation method.
3. Stored in Prefabs
    1. Pros: Prefab can be stored as assets
    1. Cons: It might be too heavy for pure data purpose. Duplicate memory if you create instance.

ScriptableObject can be used to store global settings in `.asset` files.

* Pros
    1. Easy to find them in Unity
    1. Easy to change and test
    1. Can change while playing. Changes persist after quitting play mode
    1. Can have custom editor so it's easy to note the meaning of each field
    1. Can be validated using custom editor code.

**Implementation**

1. Create a base class for all the global game Settings. It will have a static property `Instance`, which provides a way to create a singleton instance in a fixed location. This location will be customized on a project basis.

```
public class Setting<T> : ScriptableObject where T : Setting<T>
{
...
}
```
2. Create subclass the `Setting` class. Provide a MenuItem for accessing it.

```
[MenuItem ("Settings/TextureSetting")]
public static void Edit ()
{
    Selection.activeObject = Instance;
}
```

## Use case 2: Swappable Global Game Settings or Scene Settings
Sometimes we don't want to have to change some global settings back and forth between a set of parameters since it's very time-consuming. So we might want to store several pre-defined settings and swap between them quickly.

Another case is scene settings. These settings only affect the scene, not the whole game, and they usually change between scene reload, for instance, game mode settings: Easy, Medium, Difficult.

ScriptableObject can deal with this situation:

1. To be able to swap setting, create a wrapper setting which references the specific setting
2. To swap scene setting, simply load the specific setting in `Assets` then assign it to specific field in a `MonoBehaviour`

**Implementation**
* Create multiple settings
* Define a field in `MonoBehaviour` or parent ScriptableObject, referring to the settings ScriptableObject


## Use case 3: Scriptable Objects as MasterData or Item Database

For any sufficiently large game, the game data will be so big that you have to separate them from the code, and they will be referred to as `MasterData`.

Popular solutions for storing `MasterData` include:

1. Text/Binary files. E.g [Unity XML Tutorial](https://www.youtube.com/watch?v=nYWlB7HRNSE)
1. Database
1. Network

**Cons**

1. One common disadvantage with these approach is you will need an external viewer to be able to see and modify `masterdata` conveniently.
1. You will have to replicate the schema in code

ScriptableObject can offer a way to create and edit `masterdata` inside Unity Editor.

**ScriptableObject Pros**

1. Can have references to Resources
1. View, Add, Delete and Edit entities/items conveniently in Unity Editor
1. Use ScriptableObject directly in `MonoBehaviour` instead of having to create an additional layer of MonoBehaviour

**Implementation**

[Introduction to Scriptable Objects](https://unity3d.com/learn/tutorials/modules/beginner/live-training-archive/scriptable-objects) shows how you use Scriptable Object to create and edit Inventory Lists.

[Saving Data in Unity: ScriptableObjects](https://www.youtube.com/watch?v=ItZbTYO0Mnw) shows how you can create multiple `Enemy` ScriptableObject and use them in the `EnemyMove` MonoBehaviour.


## Use case 4: Scriptable Objects as Dynamic Behaviour

Another usecase of ScriptableObject is using them as game behaviour which has data that can be modified by designer. They have some instance functions, like `MonoBehaviour`, but unlike `MonoBehaviour` which cannot be saved independently without prefabs, ScriptableObject can be saved as if they're just pure data. Game programmers can change the code of the behaviour without affecting game designers.

**Implementation**

These ScriptableObjects are similar to the ones in Usecase 3, but they have functions which can act arbitrarily on `GameObject`, `MonoBehaviour` and so on.

[Ability System with Scriptable Objects](https://unity3d.com/learn/tutorials/topics/scripting/ability-system-scriptable-objects?playlist=17117) shows how you implement an Ability System, where `Ability` is ScriptableObject with its own behaviour functions.

The `Ability` class contains data and abstracts methods which act on `GameObject` based on those data.

```
public abstract class Ability : ScriptableObject
{
    public string aName = "New Ability";
    public Sprite aSprite;
    public AudioClip aSound;
    public float aBaseCoolDown = 1f;

    public abstract void Initialize (GameObject obj);
    public abstract void TriggerAbility ();
}
```

The `ProjectileAbility` is a concrete `Ability` subclass, which modifies the target `GameObject` as needed and trigger the projectile action.

```
using UnityEngine;
using System.Collections;

[CreateAssetMenu (menuName = "Abilities/ProjectileAbility")]
public class ProjectileAbility : Ability
{
    public float projectileForce = 500f;
    public Rigidbody projectile;

    private ProjectileShootTriggerable launcher;

    public override void Initialize (GameObject obj)
    {
        launcher = obj.GetComponent<ProjectileShootTriggerable> ();
        launcher.projectileForce = projectileForce;
        launcher.projectile = projectile;
    }

    public override void TriggerAbility ()
    {
        launcher.Launch ();
    }
}
```

[ScriptableObjectDemo](https://bitbucket.org/richardfine/scriptableobjectdemo) is a more comprehensive demo.

## Other notes

* MasterData can include global settings, constants and any kind of game data. This really depends on the developers/designers to choose.

* ScriptableObjects can help you to load some specific items in an Item Database quickly, if the items are stored as individual assets. Refactoring multiple ScriptableObject assets into a collection is simple.

**Other Methods to store data**

1. From source code
    1. A lot of code for storing data
    1. Large binary size
1. From GameObject
    1. We can store data in `Component` inside Prefabs, or in Scene.
    1. This way is mostly heavier than ScriptableObject
    1. However, you can pack multiple `Component` arbitrarily so it might be helpful.
1. From XML, JSON, CSV, Excel
    1. Cannot have resources reference
    1. Might be good to have a workflow to convert into ScriptableObject
    1. To update data in saved data, text/binary are more suitable than ScriptableObject, which cannot be saved once deployed.
1. PlayerPrefs
    1. Can be used to save data
    1. Not suitable for very large amount of data
    1. Cannot control the save process easily
1. From embedded DB
    1. Cannot have resources reference
    1. Might be good to have a workflow to convert into ScriptableObject
    1. To update data in saved data, database is very convenient. But we must care about performance.
    1. We must be aware of encryption capabilities of the DB.
1. From Network
    1. Cannot have resources reference
    1. Usually used in combination with a text/binary persisted in local storage
    1. To update data via Network with ScriptableObject, AssetBundle is needed. So the general-purpose text/binary format might be better.


## References

* [Game Settings with Scriptable Objects in Unity3D](https://medium.com/@mormo_music/game-settings-with-scriptable-objects-in-unity3d-6f753fe508fd#.tb9ahxiej)
* [Introduction to Scriptable Objects](https://unity3d.com/learn/tutorials/modules/beginner/live-training-archive/scriptable-objects)
* [Ability System with Scriptable Objects](https://unity3d.com/learn/tutorials/topics/scripting/ability-system-scriptable-objects?playlist=17117)
* [Saving Data in Unity: ScriptableObjects](https://www.youtube.com/watch?v=ItZbTYO0Mnw)
* [Overthrowing the MonoBehaviour tyranny in a glorious ScriptableObject revolution](https://www.youtube.com/watch?v=VBA1QCoEAX4)
* [Scriptable Object](http://tsubakit1.hateblo.jp/entry/2014/07/24/030607)
* [ScriptableObjectDemo](https://bitbucket.org/richardfine/scriptableobjectdemo)
* [Unity Serialization… behind the mistery of ScriptableObject](http://ivanozanchetta.com/gamedev/unity3d/unity-serialization-behind-scriptableobject/)
