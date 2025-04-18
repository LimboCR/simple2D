using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;


namespace Limbo.NodeGraphEditorCore
{
    public class Connection : VisualElement
    {
        private readonly Port output;
        private readonly Port input;
        private Vector2? tempTargetPosition;
        public void SetTemporaryTargetPosition(Vector2 pos) => tempTargetPosition = pos;

        public Connection(Port output, Port input)
        {
            this.output = output;
            this.input = input;

            pickingMode = PickingMode.Ignore;
            generateVisualContent += OnGenerateVisualContent;
        }

        private void OnGenerateVisualContent(MeshGenerationContext ctx)
        {
            if (output == null) return;

            Vector2 startPos = output.worldBound.center;
            Vector2 endPos = input != null
                ? input.worldBound.center
                : tempTargetPosition ?? startPos;

            Vector2 startTangent = startPos + Vector2.right * 50f;
            Vector2 endTangent = endPos + Vector2.left * 50f;

            var painter = ctx.painter2D;
            painter.strokeColor = Color.white;
            painter.lineWidth = 2f;
            painter.BeginPath();

            const int segments = 20;
            for (int i = 0; i <= segments; i++)
            {
                float t = i / (float)segments;
                Vector2 point = CalculateCubicBezierPoint(t, startPos, startTangent, endTangent, endPos);

                if (i == 0)
                    painter.MoveTo(point);
                else
                    painter.LineTo(point);
            }

            painter.Stroke();
        }

        private Vector2 CalculateCubicBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            return uuu * p0 +
                   3 * uu * t * p1 +
                   3 * u * tt * p2 +
                   ttt * p3;
        }
    }
}