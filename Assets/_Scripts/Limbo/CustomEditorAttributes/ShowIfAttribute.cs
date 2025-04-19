using System;
using UnityEngine;

namespace Limbo.CustomEditorAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ShowIfAttribute : PropertyAttribute
    {
        public string ConditionName { get; }
        public object CompareValue { get; }
        public bool Invert { get; }

        public ShowIfAttribute(string conditionName, object compareValue = null, bool invert = false)
        {
            ConditionName = conditionName;
            CompareValue = compareValue;
            Invert = invert;
        }
    }
}