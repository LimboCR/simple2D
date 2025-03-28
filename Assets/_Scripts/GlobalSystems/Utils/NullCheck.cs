using UnityEngine;

public static class NullCheck
{
    public static bool IsNull(GameObject obj)
    {
        return obj == null;
    }
    public static bool IsNull(Transform transform)
    {
        return transform == null;
    }
    public static bool IsNull(Vector3 targetVector3)
    {
        return targetVector3 == null;
    }
    public static bool IsNull(Vector2 targetVector2)
    {
        return targetVector2 == null;
    }
    public static bool IsNull(Vector2Int targetVector2Int)
    {
        return targetVector2Int == null;
    }
    public static bool IsNull(string str)
    {
        return str == null;
    }
    public static bool IsNull(int targetInt)
    {
        return targetInt == 0;
    }
    public static bool IsNull(float targetFloat)
    {
        return targetFloat == 0;
    }
    public static bool IsNull(double targetDouble)
    {
        return targetDouble == 0;
    }
    public static bool IsNull(Coroutine coroutine)
    {
        return coroutine == null;
    }
}
