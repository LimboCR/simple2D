using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSectorRendererScript : MonoBehaviour
{
    public GameObject Player;

    public Vector2 Offset = new(0f, 0f);
    public Vector2 Size = new(48f, 60f);

    public List<GameObject> ObjectsInSector = new();
    public Bounds GetBounds()
    {
        return new Bounds(Offset, Size);
    }

    private void Start()
    {
        Coroutine routine = StartCoroutine(DetectObjectsInSector());
    }

    public IEnumerator DetectObjectsInSector()
    {
        yield return new WaitForSeconds(5f);
        Bounds sectorBounds = GetBounds();

        var allObjects = FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        int count = 0;
        foreach (var obj in allObjects)
        {
            if (sectorBounds.Contains(obj.transform.position))
            {
                if (sectorBounds.Contains(obj.transform.position))
                {
                    if (!ObjectsInSector.Contains(obj)) ObjectsInSector.Add(obj);
                }
                count++;
            }
        }

        Debug.Log("Found: " + count + " object(s) inside bounds.");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, Size);
    }
}
