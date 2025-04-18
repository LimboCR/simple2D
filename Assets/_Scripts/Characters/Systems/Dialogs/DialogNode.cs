using System.Collections.Generic;
using UnityEngine;

namespace Limbo.DialogSystem
{
    [CreateAssetMenu(fileName ="DialogNode", menuName = "Dialog System/Create Dialog Node")]
    public class DialogNode : ScriptableObject
    {
        [TextArea] public string LineText;
        public List<DialogOptionVariant> Options;
    }
}