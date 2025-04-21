using System.Collections.Generic;
using UnityEngine;

public static class LoopUtils
{
    public static IEnumerable<T> Component<T>(this IEnumerable<GameObject> gameObjects) where T : Component
    {
        foreach (var go in gameObjects)
        {
            if (go.TryGetComponent<T>(out var comp))
                yield return comp;
        }
    }

    public static IEnumerable<(GameObject go, T component)> ComponentGO<T>(this IEnumerable<GameObject> gameObjects) where T : Component
    {
        foreach (var go in gameObjects)
        {
            if (go.TryGetComponent<T>(out var comp))
                yield return (go, comp);
        }
    }
}
