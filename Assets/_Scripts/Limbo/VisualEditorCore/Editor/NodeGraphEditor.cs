using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace Limbo.NodeGraphEditorCore
{
    public class NodeGraphEditor : EditorWindow
    {
        private VisualElement graphView;
        private Button executeButton;
        private List<GraphNodeBase> allNodes = new();

        private VisualElement zoomContainer;
        private float zoomLevel = 1f;
        private Vector2 panStart;
        private bool isPanning = false;

        [MenuItem("Window/Custom/Node Graph Editor")]
        public static void ShowEditor()
        {
            var window = GetWindow<NodeGraphEditor>();
            window.titleContent = new GUIContent("Node Graph Editor");
        }

        public void CreateGUI()
        {
            var toolbar = new VisualElement();
            toolbar.style.flexDirection = FlexDirection.Row;
            toolbar.style.paddingLeft = 10;
            toolbar.style.paddingTop = 5;
            toolbar.style.paddingBottom = 5;
            toolbar.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f);

            var executeButton = new Button(OnExecuteClicked) { text = "Execute" };
            toolbar.Add(executeButton);
            rootVisualElement.Add(toolbar);

            // Zoom container
            zoomContainer = new VisualElement();
            zoomContainer.style.flexGrow = 1;
            zoomContainer.style.backgroundColor = new Color(0.13f, 0.13f, 0.13f);

            // Important: make it pickable!
            zoomContainer.pickingMode = PickingMode.Position;

            // Register pointer event
            zoomContainer.RegisterCallback<PointerDownEvent>(OnPointerDown);
            // Enable zoom/pan
            zoomContainer.RegisterCallback<WheelEvent>(OnZoom);
            zoomContainer.RegisterCallback<MouseDownEvent>(OnMouseDownPan);
            zoomContainer.RegisterCallback<MouseMoveEvent>(OnMouseMovePan);
            zoomContainer.RegisterCallback<MouseUpEvent>(OnMouseUpPan);

            rootVisualElement.Add(zoomContainer);
        }

        private void OnPointerDown(PointerDownEvent evt)
        {
            if (evt.button == 1) // Right click
            {
                Vector2 localMousePos = evt.localPosition;

                // Convert local to world, then to screen
                Vector2 screenMousePos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);

                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("Create/Float"), false, () => CreateNode<FloatNode>(localMousePos));
                menu.AddItem(new GUIContent("Create/Multiply"), false, () => CreateNode<MultiplyNode>(localMousePos));
                menu.AddItem(new GUIContent("Create/Debug"), false, () => CreateNode<DebugNode>(localMousePos));
                menu.ShowAsContext();

                evt.StopPropagation();
            }
        }

        private void CreateNode<T>(Vector2 position) where T : GraphNodeBase, new()
        {
            var node = new T();
            node.Initialize(position);
            zoomContainer.Add(node);
            allNodes.Add(node);
        }

        private void OnExecuteClicked()
        {
            foreach (var node in allNodes)
            {
                if (node is DebugNode debugNode)
                {
                    debugNode.Evaluate();
                }
            }
        }

        private void OnZoom(WheelEvent evt)
        {
            float delta = evt.delta.y > 0 ? -0.1f : 0.1f;
            zoomLevel = Mathf.Clamp(zoomLevel + delta, 0.5f, 2f);
            zoomContainer.transform.scale = Vector3.one * zoomLevel;
            evt.StopPropagation();
        }

        private void OnMouseDownPan(MouseDownEvent evt)
        {
            if (evt.button == 2) // Middle mouse
            {
                isPanning = true;
                panStart = evt.mousePosition;
                evt.StopPropagation();
            }
        }

        private void OnMouseMovePan(MouseMoveEvent evt)
        {
            if (isPanning)
            {
                var delta = evt.mousePosition - panStart;
                panStart = evt.mousePosition;
                var position = zoomContainer.transform.position;
                zoomContainer.transform.position = position + (Vector3)delta;
                evt.StopPropagation();
            }
        }

        private void OnMouseUpPan(MouseUpEvent evt)
        {
            if (evt.button == 2)
            {
                isPanning = false;
                evt.StopPropagation();
            }
        }
    }
}