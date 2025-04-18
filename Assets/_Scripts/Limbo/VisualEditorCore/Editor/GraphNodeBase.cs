using UnityEngine;
using UnityEngine.UIElements;

namespace Limbo.NodeGraphEditorCore
{
    public abstract class GraphNodeBase : VisualElement
    {
        public Vector2 vPosition;
        public Port OutputPort;
        public Port[] InputPorts;

        private Vector2 dragStartPos;
        private Vector2 mouseStartPos;
        private bool isDragging;

        public GraphNodeBase()
        {
            var title = new Label("Title");
            style.position = Position.Absolute;
            style.width = 150;
            style.height = 60;
            style.backgroundColor = new Color(0.2f, 0.2f, 0.2f);
            style.borderTopLeftRadius = 4;
            style.borderTopRightRadius = 4;
            style.borderBottomLeftRadius = 4;
            style.borderBottomRightRadius = 4;
            style.borderBottomWidth = 1;
            style.borderTopWidth = 1;
            style.borderLeftWidth = 1;
            style.borderRightWidth = 1;
            style.borderTopColor = Color.black;
            style.borderBottomColor = Color.black;
            style.borderLeftColor = Color.black;
            style.borderRightColor = Color.black;
            Add(title);

            title.RegisterCallback<MouseDownEvent>(OnMouseDown);
            title.RegisterCallback<MouseMoveEvent>(OnMouseMove);
            title.RegisterCallback<MouseUpEvent>(OnMouseUp);
        }

        public virtual void Initialize(Vector2 position)
        {
            vPosition = position;
            style.position = Position.Absolute;
            style.left = vPosition.x;
            style.top = vPosition.y;

            style.backgroundColor = new Color(0.25f, 0.25f, 0.25f);
            style.borderTopLeftRadius = 4;
            style.borderBottomRightRadius = 4;
            style.marginBottom = 4;
            style.paddingBottom = 4;
            style.width = 150;
            style.minHeight = 60;

            pickingMode = PickingMode.Position;
        }

        public abstract float Evaluate();

        private void OnMouseDown(MouseDownEvent evt)
        {
            if (evt.button == 0)
            {
                isDragging = true;
                dragStartPos = this.transform.position;
                mouseStartPos = evt.mousePosition;
                evt.StopPropagation();
            }
        }

        private void OnMouseMove(MouseMoveEvent evt)
        {
            if (isDragging)
            {
                Vector2 offset = evt.mousePosition - mouseStartPos;
                transform.position = dragStartPos + offset;

                // Repaint connections when moving
                ConnectionManager.Instance?.MarkDirtyRepaint();
                evt.StopPropagation();
            }
        }

        private void OnMouseUp(MouseUpEvent evt)
        {
            if (evt.button == 0 && isDragging)
            {
                isDragging = false;
                evt.StopPropagation();
            }
        }
    }
}