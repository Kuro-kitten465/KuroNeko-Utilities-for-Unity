using System;
using UnityEngine;
using UnityEditor;
using Kuro.UnityUtils.Security.Primitives;

namespace Kuro.UnityUtils
{
    [CustomPropertyDrawer(typeof(SecureFloat))]
    public class SecureFloatDrawer : PropertyDrawer
    {
        private readonly System.Random _random = new();
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var modProp = property.FindPropertyRelative("_modifier");
            var obProp = property.FindPropertyRelative("_obfuscated");
            var keyProp = property.FindPropertyRelative("_key");

            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();

            var newValue = EditorGUI.FloatField(position, label, modProp.floatValue);

            if (EditorGUI.EndChangeCheck())
            {
                var newKey = BitConverter.Int32BitsToSingle(_random.Next());

                var valueBits = BitConverter.SingleToInt32Bits(newValue);
                var keyBits = BitConverter.SingleToInt32Bits(newKey);
                var obfuscated = BitConverter.Int32BitsToSingle(valueBits ^ keyBits);

                modProp.floatValue = newValue;
                obProp.floatValue = obfuscated;
                keyProp.floatValue = newKey;
            }

            EditorGUI.EndProperty();
        }
    }
}
