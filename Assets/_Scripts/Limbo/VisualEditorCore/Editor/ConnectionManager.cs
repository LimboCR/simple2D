using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace Limbo.NodeGraphEditorCore
{
    public class ConnectionManager : VisualElement
    {
        public static ConnectionManager Instance;
        private List<Connection> connections = new();
        private Connection tempConnection;
        private Port tempStartPort;
        private Vector2 currentMousePos;

        public ConnectionManager()
        {
            Instance = this;
            style.position = Position.Absolute;
            pickingMode = PickingMode.Ignore;
        }

        public void BeginConnection(Port startPort)
        {
            tempStartPort = startPort;
            tempConnection = new Connection(startPort, null);
            Add(tempConnection);
            RegisterCallback<MouseMoveEvent>(OnMouseMove);
            RegisterCallback<MouseUpEvent>(OnMouseUp);
        }

        private void OnMouseMove(MouseMoveEvent evt)
        {
            currentMousePos = evt.localMousePosition;
            tempConnection?.SetTemporaryTargetPosition(currentMousePos);
            tempConnection?.MarkDirtyRepaint();
        }

        public void MarkDirtyRepaint()
        {
            foreach (var c in connections)
                c.MarkDirtyRepaint();
        }

        private void OnMouseUp(MouseUpEvent evt)
        {
            var picked = panel.Pick(evt.mousePosition) as Port;

            if (picked != null && picked != tempStartPort &&
                tempStartPort.Type == PortType.Output && picked.Type == PortType.Input)
            {
                var connection = new Connection(tempStartPort, picked);
                picked.ConnectedNode = tempStartPort.parent as GraphNodeBase;
                connections.Add(connection);
                Add(connection);
            }

            Remove(tempConnection);
            UnregisterCallback<MouseMoveEvent>(OnMouseMove);
            UnregisterCallback<MouseUpEvent>(OnMouseUp);
        }
    }
}
