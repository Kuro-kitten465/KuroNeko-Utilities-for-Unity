using UnityEditor;
using UnityEngine;
using Kuro.UnityUtils.Security.Primitives;
using System;

namespace Kuro.UnityUtils
{
    [Obsolete("The core of SecureDecimal still not finish due to Unity can't serialized Decimal value.")]
    [CustomPropertyDrawer(typeof(SecureDecimalDrawer))]
    public class SecureDecimalDrawer : PropertyDrawer
    {
        private static readonly System.Random _random = new();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var modProp = property.FindPropertyRelative("_modifier");
            var obProp = property.FindPropertyRelative("_obfuscated");
            var keyProp = property.FindPropertyRelative("_key");

            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();

            var newStrValue = EditorGUI.TextField(position, label, modProp.doubleValue.ToString());

            EditorGUI.EndProperty();
        }
    }
}
