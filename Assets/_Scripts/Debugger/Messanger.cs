using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Messanger : MonoBehaviour
{
    [SerializeField] private List<string> _messagesQueue;
    public bool isClear;

    private void Awake()
    {
        isClear = true;
    }

    public void AddMessage(string message)
    {
        string finalMessage = this.gameObject.name + " debug: " + message;
        _messagesQueue.Add(message);
        isClear = false;
    }

    public string ReadMessage()
    {
        return _messagesQueue.ElementAt(0);
    }

    public void ClearQueue()
    {
        _messagesQueue.Clear();
        isClear = true;
    }
}
