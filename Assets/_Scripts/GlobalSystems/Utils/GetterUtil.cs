using System.Collections.Generic;
using UnityEngine;

public static class GetterUtil
{
    public struct ComponentMatchResult
    {
        public Component Component;
        public string TypeName;
    }

    /// <summary>
    /// Version 1: Returns the component directly (null if not found)
    /// </summary>
    /// <typeparam name="T">Script to look for</typeparam>
    /// <param name="includeInactive">Look through inactive game objects</param>
    /// <returns></returns>
    public static T TryFindScript<T>(bool includeInactive = false) where T : MonoBehaviour
    {
        if(!includeInactive) return GameObject.FindFirstObjectByType<T>(FindObjectsInactive.Exclude);
        else return GameObject.FindFirstObjectByType<T>(FindObjectsInactive.Include);
    }

    /// <summary>
    /// Version 2: Uses 'out' and returns success as bool
    /// </summary>
    /// <typeparam name="T">Script to look for</typeparam>
    /// <param name="foundScript">Returning required found script</param>
    /// <param name="includeInactive">Look through inactive game objects</param>
    /// <returns></returns>
    public static bool TryFindScript<T>(out T foundScript, bool includeInactive = false) where T : MonoBehaviour
    {
        if(!includeInactive) foundScript = GameObject.FindFirstObjectByType<T>(FindObjectsInactive.Exclude);
        else foundScript = GameObject.FindFirstObjectByType<T>(FindObjectsInactive.Include);
        return foundScript != null;
    }

    /// <summary>
    /// Version 3: Uses 'out' and returns success as bool. Additional parameter for simplified search by tag if many objects have specific script
    /// </summary>
    /// <typeparam name="T">Script to look for</typeparam>
    /// <param name="tag">Which game object tag to look for</param>
    /// <param name="foundScript">Returning required found script</param>
    /// <returns></returns>
    public static bool TryFindScript<T>(string tag, out T foundScript) where T : MonoBehaviour
    {
        GameObject obj = GameObject.FindGameObjectWithTag(tag);
        foundScript = obj != null ? obj.GetComponent<T>() : null;
        return foundScript != null;
    }

    public static bool TryFindScript<T>(out T foundScript, string objectName) where T : MonoBehaviour
    {
        GameObject obj = GameObject.Find(objectName);
        foundScript = obj != null ? obj.GetComponent<T>() : null;
        return foundScript != null;
    }


    public static bool TryGetKnownType(GameObject obj, out ComponentMatchResult result)
    {
        if (obj.TryGetComponent<NewPlayerController>(out var player))
        {
            result = new ComponentMatchResult { Component = player, TypeName = nameof(NewPlayerController) };
            return true;
        }
        if (obj.TryGetComponent<CloseCombatNPCBase>(out var combat))
        {
            result = new ComponentMatchResult { Component = combat, TypeName = nameof(CloseCombatNPCBase) };
            return true;
        }
        if (obj.TryGetComponent<NPCCombatNav>(out var nav))
        {
            result = new ComponentMatchResult { Component = nav, TypeName = nameof(NPCCombatNav) };
            return true;
        }
        if (obj.TryGetComponent<TraderNPCBase>(out var trader))
        {
            result = new ComponentMatchResult { Component = trader, TypeName = nameof(TraderNPCBase) };
            return true;
        }

        result = default;
        return false;
    }

    public static T TryFindSO<T>(string assetName) where T : ScriptableObject
    {
        return Resources.Load<T>(assetName);
    }

    public static List<T> FindAllScripts<T>() where T : MonoBehaviour
    {
        return new List<T>(GameObject.FindObjectsByType<T>(0));
    }
}
