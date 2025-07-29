using UnityEditor;
using UnityEngine;
using Kuro.UnityUtils.Security.Primitives;

namespace Kuro.UnityUtils
{
    [CustomPropertyDrawer(typeof(SecureBool))]
    public class SecureBoolDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var modProp = property.FindPropertyRelative("_modifier");
            var obProp = property.FindPropertyRelative("_obfuscated");
            var keyProp = property.FindPropertyRelative("_key");

            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();

            var newValue = EditorGUI.Toggle(position, label, modProp.boolValue);

            if (EditorGUI.EndChangeCheck())
            {
                var newKey = (byte)Random.Range(byte.MinValue, byte.MaxValue);
                var pattern = (byte)(newValue ? 0b01010101 : 0b10101010);
                var obfuscated = (byte)(pattern ^ newKey);

                modProp.boolValue = newValue;
                obProp.intValue = obfuscated;
                keyProp.intValue = newKey;
            }

            EditorGUI.EndProperty();
        }
    }
}
