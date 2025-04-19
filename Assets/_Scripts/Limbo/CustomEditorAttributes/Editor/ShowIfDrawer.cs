using System;
using UnityEditor;
using UnityEngine;

namespace Limbo.CustomEditorAttributes
{
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (ShouldShow(property))
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return ShouldShow(property)
                ? EditorGUI.GetPropertyHeight(property, label, true)
                : 0f;
        }

        private bool ShouldShow(SerializedProperty property)
        {
            ShowIfAttribute showIf = (ShowIfAttribute)attribute;
            SerializedProperty conditionProp = property.serializedObject.FindProperty(showIf.ConditionName);

            if (conditionProp == null)
            {
                Debug.LogWarning($"[ShowIf] Can't find condition field: {showIf.ConditionName}");
                return true;
            }

            bool result = true;

            switch (conditionProp.propertyType)
            {
                case SerializedPropertyType.Boolean:
                    result = conditionProp.boolValue;
                    break;

                case SerializedPropertyType.Enum:
                case SerializedPropertyType.Integer:
                    if (showIf.CompareValue != null)
                    {
                        int target = Convert.ToInt32(showIf.CompareValue);
                        result = conditionProp.intValue == target;
                    }
                    else
                    {
                        result = conditionProp.intValue != 0;
                    }
                    break;

                case SerializedPropertyType.ObjectReference:
                    result = conditionProp.objectReferenceValue != null;
                    break;

                default:
                    Debug.LogWarning($"[ShowIf] Unsupported property type: {conditionProp.propertyType}");
                    break;
            }

            return showIf.Invert ? !result : result;
        }
    }
}