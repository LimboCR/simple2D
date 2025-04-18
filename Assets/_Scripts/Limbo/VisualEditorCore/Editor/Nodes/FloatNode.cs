using UnityEngine;
using UnityEngine.UIElements;

namespace Limbo.NodeGraphEditorCore
{
    public class FloatNode : GraphNodeBase
    {
        private FloatField floatField;

        public override void Initialize(Vector2 position)
        {
            base.Initialize(position);

            var container = new VisualElement { style = { flexDirection = FlexDirection.Row } };
            var label = new Label("float:");
            floatField = new FloatField();
            floatField.value = 1.0f;

            OutputPort = new Port(PortType.Output);

            container.Add(label);
            container.Add(floatField);
            container.Add(OutputPort);
            Add(container);
        }

        public override float Evaluate()
        {
            return floatField.value;
        }
    }
}
