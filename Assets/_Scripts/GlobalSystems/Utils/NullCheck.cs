using UnityEngine;

public static class NullCheck
{
    public static bool HasValue<T>() where T : Object
    {
        return typeof(T) != null;
    }
    public static bool HasScript<T>() where T : Object
    {
        return typeof(T) != null;
    }
}
