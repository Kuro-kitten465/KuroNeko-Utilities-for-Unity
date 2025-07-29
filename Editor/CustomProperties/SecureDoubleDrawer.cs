using UnityEngine;
using UnityEditor;
using Kuro.UnityUtils.Security.Primitives;
using System;

namespace Kuro.UnityUtils.Editor
{
    [CustomPropertyDrawer(typeof(SecureDouble))]
    public class SecureDoubleDrawer : PropertyDrawer
    {
        private readonly System.Random _random = new(); 

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var modProp = property.FindPropertyRelative("_modifier");
            var obProp = property.FindPropertyRelative("_obfuscated");
            var keyProp = property.FindPropertyRelative("_key");

            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();

            var newValue = EditorGUI.DoubleField(position, label, modProp.doubleValue);

            if (EditorGUI.EndChangeCheck())
            {
                var newKey = BitConverter.Int32BitsToSingle(_random.Next());

                var valueBits = BitConverter.DoubleToInt64Bits(newValue);
                var keyBits = BitConverter.DoubleToInt64Bits(newKey);
                var obfuscated = BitConverter.Int64BitsToDouble(valueBits ^ keyBits);

                modProp.doubleValue = newValue;
                obProp.doubleValue = obfuscated;
                keyProp.doubleValue = newKey;
            }

            EditorGUI.EndProperty();
        }
    }
}
