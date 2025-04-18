using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Limbo.NodeGraphEditorCore
{
    public enum PortType { Input, Output }

    public class Port : VisualElement
    {
        public GraphNodeBase ConnectedNode;
        public PortType Type;
        public Action<Port> OnConnect;

        public Port(PortType type)
        {
            Type = type;

            style.width = 12;
            style.height = 12;
            style.backgroundColor = type == PortType.Input ? Color.green : Color.cyan;
            style.borderBottomLeftRadius = 6;
            style.borderBottomRightRadius = 6;
            style.borderTopLeftRadius = 6;
            style.borderTopRightRadius = 6;
            style.marginLeft = 4;
            style.marginRight = 4;
            pickingMode = PickingMode.Position;

            RegisterCallback<PointerDownEvent>(OnPointerDown);
        }

        private void OnPointerDown(PointerDownEvent evt)
        {
            if (evt.button == 0 && Type == PortType.Output)
            {
                ConnectionManager.Instance?.BeginConnection(this);
                evt.StopPropagation();
            }
        }
    }
}


