using UnityEngine;
using UnityEditor;
using Kuro.UnityUtils.Security.Primitives;
using System;

namespace Kuro.UnityUtils.Editor
{
    [CustomPropertyDrawer(typeof(SecureInt64))]
    public class SecureInt64Drawer : PropertyDrawer
    {
        private static readonly System.Random _random = new();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var modProp = property.FindPropertyRelative("_modifier");
            var obProp = property.FindPropertyRelative("_obfuscated");
            var keyProp = property.FindPropertyRelative("_key");

            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();

            /* double doubleValue = modProp.longValue;
            doubleValue = EditorGUI.DelayedDoubleField(position, label, doubleValue);
            var newValue = (long)doubleValue; */
            var strValue = modProp.longValue.ToString();
            strValue = EditorGUI.TextField(position, label, strValue, EditorStyles.textField);
            var _ = long.TryParse(strValue, out var newValue);

            if (EditorGUI.EndChangeCheck())
            {
                var key = BitConverter.Int32BitsToSingle(_random.Next());
                var keyBits = BitConverter.DoubleToInt64Bits(key);
                var obfuscated = newValue ^ keyBits;

                modProp.longValue = newValue;
                obProp.longValue = obfuscated;
                keyProp.longValue = keyBits;
            }

            EditorGUI.EndProperty();
        }
    }
}
