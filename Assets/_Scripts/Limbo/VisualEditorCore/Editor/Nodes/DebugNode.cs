using UnityEngine;
using UnityEngine.UIElements;

namespace Limbo.NodeGraphEditorCore
{
    public class DebugNode : GraphNodeBase
    {
        public override void Initialize(Vector2 position)
        {
            base.Initialize(position);

            InputPorts = new Port[1] { new Port(PortType.Input) };
            var label = new Label("Debug");
            label.style.unityTextAlign = TextAnchor.MiddleCenter;
            label.style.marginTop = 10;
            label.style.fontSize = 14;

            var row = new VisualElement { style = { flexDirection = FlexDirection.Row } };
            row.Add(InputPorts[0]);

            Add(label);
            Add(row);
        }

        public override float Evaluate()
        {
            float value = InputPorts[0].ConnectedNode?.Evaluate() ?? 0;
            Debug.Log("[DebugNode] Value = " + value);
            return value;
        }
    }
}