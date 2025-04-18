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

            bool condition = false;

            switch (conditionProp.propertyType)
            {
                case SerializedPropertyType.Boolean:
                    condition = conditionProp.boolValue;
                    break;
                case SerializedPropertyType.Enum:
                    condition = conditionProp.enumValueIndex != 0;
                    break;
                default:
                    Debug.LogWarning($"[ShowIf] Unsupported property type: {conditionProp.propertyType}");
                    break;
            }

            return showIf.Invert ? !condition : condition;
        }
    }
}
