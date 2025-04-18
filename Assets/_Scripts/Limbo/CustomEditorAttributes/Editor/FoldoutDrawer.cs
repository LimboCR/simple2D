using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Limbo.CustomEditorAttributes
{
    [CustomPropertyDrawer(typeof(FoldoutAttribute))]
    public class FoldoutDrawer : PropertyDrawer
    {
        private static Dictionary<string, bool> foldoutStates = new();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            FoldoutAttribute foldoutAttribute = (FoldoutAttribute)attribute;
            string key = $"{property.serializedObject.targetObject.GetInstanceID()}_{foldoutAttribute.GroupName}";

            if (!foldoutStates.TryGetValue(key, out bool isExpanded))
                foldoutStates[key] = isExpanded = true;

            if (IsFirstInGroup(property))
            {
                foldoutStates[key] = EditorGUI.Foldout(position, isExpanded, foldoutAttribute.GroupName, true);
            }

            if (foldoutStates[key])
            {
                EditorGUI.indentLevel++;
                EditorGUI.PropertyField(position, property, label, true);
                EditorGUI.indentLevel--;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            FoldoutAttribute foldoutAttribute = (FoldoutAttribute)attribute;
            string key = $"{property.serializedObject.targetObject.GetInstanceID()}_{foldoutAttribute.GroupName}";

            if (!foldoutStates.TryGetValue(key, out bool isExpanded))
                isExpanded = true;

            return isExpanded ? EditorGUI.GetPropertyHeight(property, label, true) : 0f;
        }

        private bool IsFirstInGroup(SerializedProperty property)
        {
            return property.name == GetFirstPropertyInGroup(property)?.name;
        }

        private SerializedProperty GetFirstPropertyInGroup(SerializedProperty property)
        {
            var iterator = property.Copy();
            iterator.Reset();
            while (iterator.NextVisible(true))
            {
                if (iterator.name == property.name)
                    break;

                var attr = fieldInfo.DeclaringType
                    .GetField(iterator.name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                    ?.GetCustomAttributes(typeof(FoldoutAttribute), true);

                if (attr?.Length > 0 && ((FoldoutAttribute)attr[0]).GroupName == ((FoldoutAttribute)attribute).GroupName)
                    return iterator;
            }

            return property;
        }
    }
}