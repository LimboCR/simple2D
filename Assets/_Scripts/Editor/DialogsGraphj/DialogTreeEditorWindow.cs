using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Limbo.DialogSystem.Editor
{
    public class DialogTreeEditorWindow : EditorWindow
    {
        private class DialogEditorNode
        {
            public Rect rect;
            public DialogNode dialogNode;
            public List<DialogEditorNode> children = new();
            public bool isDragged;
            public bool isSelected;

            public DialogEditorNode(Vector2 position)
            {
                rect = new Rect(position.x, position.y, 400, 400);
                dialogNode = ScriptableObject.CreateInstance<DialogNode>();
                dialogNode.Options = new List<DialogOptionVariant>();
            }

            public void Draw()
            {
                GUILayout.BeginArea(rect, GUI.skin.window);
                dialogNode.LineText = EditorGUILayout.TextArea(dialogNode.LineText, GUILayout.MinWidth(200), GUILayout.MinHeight(100), GUILayout.Width(300), GUILayout.Height(200));

                GUILayout.Label("Options:");
                for (int i = 0; i < dialogNode.Options.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    dialogNode.Options[i].OptionText = EditorGUILayout.TextField(dialogNode.Options[i].OptionText);
                    if (GUILayout.Button("X", GUILayout.Width(20)))
                    {
                        dialogNode.Options.RemoveAt(i);
                    }
                    if(GUILayout.RepeatButton(">", GUILayout.Width(20)))
                    {

                    }
                    EditorGUILayout.EndHorizontal();
                }

                if (GUILayout.Button("Add Option"))
                {
                    dialogNode.Options.Add(new DialogOptionVariant());
                }

                GUILayout.EndArea();
            }

            public void Drag(Vector2 delta)
            {
                rect.position += delta;
            }
        }

        private List<DialogEditorNode> nodes = new();
        private DialogEditorNode selectedNode;
        private Vector2 dragOffset;

        private Vector2 mousePosition;
        private bool isConnecting;
        private DialogEditorNode startNode;

        [MenuItem("Tools/Dialog Tree Editor")]
        public static void OpenWindow()
        {
            GetWindow<DialogTreeEditorWindow>("Dialog Tree Editor");
        }

        private void OnGUI()
        {
            Event e = Event.current;
            mousePosition = e.mousePosition;

            DrawConnections();
            DrawNodes();
            ProcessEvents(e);

            if (GUI.changed) Repaint();
        }

        private void DrawNodes()
        {
            foreach (var node in nodes)
            {
                node.Draw();
            }
        }

        private void DrawConnections()
        {
            foreach (var node in nodes)
            {
                foreach (var child in node.children)
                {
                    Handles.DrawBezier(
                        node.rect.center,
                        child.rect.center,
                        node.rect.center + Vector2.left * 50f,
                        child.rect.center - Vector2.left * 50f,
                        Color.white,
                        null,
                        2f);
                }
            }
        }

        private void ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        selectedNode = GetNodeAtPosition(e.mousePosition);
                        if (selectedNode != null)
                        {
                            dragOffset = e.mousePosition - selectedNode.rect.position;
                            selectedNode.isDragged = true;
                            GUI.changed = true;
                        }
                        else
                        {
                            ClearNodeSelection();
                        }
                    }

                    if (e.button == 1)
                    {
                        ShowContextMenu(e.mousePosition);
                    }
                    break;

                case EventType.MouseDrag:
                    if (e.button == 0 && selectedNode != null && selectedNode.isDragged)
                    {
                        selectedNode.Drag(e.delta);
                        GUI.changed = true;
                    }
                    break;

                case EventType.MouseUp:
                    if (selectedNode != null)
                    {
                        selectedNode.isDragged = false;
                    }
                    break;
            }
        }

        private void ShowContextMenu(Vector2 position)
        {
            GenericMenu menu = new();
            menu.AddItem(new GUIContent("Add Node"), false, () => AddNode(position));

            if (selectedNode != null)
            {
                menu.AddItem(new GUIContent("Remove Node"), false, () => RemoveNode(selectedNode));
                menu.AddItem(new GUIContent("Start Connection"), false, () => BeginConnection(selectedNode));
                if (isConnecting && startNode != null && startNode != selectedNode)
                    menu.AddItem(new GUIContent("Connect To This"), false, () => ConnectNodes(startNode, selectedNode));
            }

            menu.ShowAsContext();
        }

        private void AddNode(Vector2 position)
        {
            nodes.Add(new DialogEditorNode(position));
        }

        private void RemoveNode(DialogEditorNode node)
        {
            foreach (var n in nodes)
            {
                n.children.Remove(node);
            }
            nodes.Remove(node);
        }

        private DialogEditorNode GetNodeAtPosition(Vector2 position)
        {
            return nodes.LastOrDefault(n => n.rect.Contains(position));
        }

        private void ClearNodeSelection()
        {
            selectedNode = null;
            foreach (var node in nodes)
            {
                node.isSelected = false;
            }
        }

        private void BeginConnection(DialogEditorNode node)
        {
            startNode = node;
            isConnecting = true;
        }

        private void ConnectNodes(DialogEditorNode from, DialogEditorNode to)
        {
            if (!from.children.Contains(to))
                from.children.Add(to);

            isConnecting = false;
            startNode = null;
        }

        // Saving & Loading logic here later...
    }


}