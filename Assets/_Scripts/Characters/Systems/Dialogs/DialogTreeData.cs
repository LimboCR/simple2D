using System.Collections.Generic;
using UnityEngine;

namespace Limbo.DialogSystem
{
    [System.Serializable]
    public class DialogNodeData
    {
        public string guid;
        public Vector2 position;
        public DialogNode dialogNode;
        public List<string> childGuids = new();
    }

    [System.Serializable]
    public class DialogTreeData : ScriptableObject
    {
        public List<DialogNodeData> nodes = new();
    }
}