using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(AnimationStateHandler))]
public class AnimationStateHandlerEditor : Editor
{
    public VisualTreeAsset VisualTree;

    private AnimationStateHandler _animSH;

    private void OnEnable()
    {
        _animSH = (AnimationStateHandler)target;
    }

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new();
        VisualTree.CloneTree(root);

        return root;
    }
}
