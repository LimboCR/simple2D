using System;
using UnityEngine;

namespace Limbo.CustomEditorAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class FoldoutAttribute : PropertyAttribute
    {
        public string GroupName { get; }

        public FoldoutAttribute(string groupName)
        {
            GroupName = groupName;
        }
    }
}