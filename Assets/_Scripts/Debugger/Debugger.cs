using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    [SerializeField] private bool _debug;

    [Space]
    [Header("Debug list")]
    [SerializeField] private List<GameObject> objectsToDebug;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_debug)
        {
            foreach(GameObject obj in objectsToDebug)
            {
                if(obj.TryGetComponent(out Messanger messanger))
                {
                    if (messanger.isClear) continue;
                    DebugMsg(messanger.ReadMessage());
                    messanger.ClearQueue();
                }
            }
        }
    }

    private void DebugMsg(string msg)
    {
        Debug.Log(msg);
    }
}
