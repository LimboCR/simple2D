using UnityEngine;
using UnityEngine.UIElements;

namespace Limbo.NodeGraphEditorCore
{
    public class MultiplyNode : GraphNodeBase
    {
        public override void Initialize(Vector2 position)
        {
            base.Initialize(position);

            InputPorts = new Port[2] { new Port(PortType.Input), new Port(PortType.Input) };
            OutputPort = new Port(PortType.Output);

            var header = new Label("Multiply");
            header.style.unityTextAlign = TextAnchor.MiddleCenter;
            header.style.marginTop = 6;
            header.style.marginBottom = 6;
            header.style.fontSize = 14;

            var portRow = new VisualElement { style = { flexDirection = FlexDirection.Row } };
            portRow.Add(InputPorts[0]);
            portRow.Add(InputPorts[1]);
            portRow.Add(OutputPort);

            Add(header);
            Add(portRow);
        }

        public override float Evaluate()
        {
            float a = InputPorts[0].ConnectedNode?.Evaluate() ?? 0;
            float b = InputPorts[1].ConnectedNode?.Evaluate() ?? 0;
            return a * b;
        }
    }
}

