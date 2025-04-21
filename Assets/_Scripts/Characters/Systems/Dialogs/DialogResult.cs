using UnityEngine;

namespace Limbo.DialogSystem
{
    public class DialogResult : ScriptableObject
    {
        public virtual void ApplyResult(GameObject target) { }
    }
}