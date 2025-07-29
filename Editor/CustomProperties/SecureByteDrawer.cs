using UnityEditor;
using UnityEngine;
using Kuro.UnityUtils.Security.Primitives;

namespace Kuro.UnityUtils.Editor
{
    [CustomPropertyDrawer(typeof(SecureByte))]
    public class SecureByteDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var modProp = property.FindPropertyRelative("_modifier");
            var obProp = property.FindPropertyRelative("_obfuscated");
            var keyProp = property.FindPropertyRelative("_key");

            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();

            byte newValue = (byte)EditorGUI.IntField(position, label, modProp.intValue);

            if (EditorGUI.EndChangeCheck())
            {
                var newKey = (byte)Random.Range(byte.MinValue, byte.MaxValue);
                var obfuscated = (byte)(newValue ^ newKey);

                modProp.intValue = newValue;
                obProp.intValue = obfuscated;
                keyProp.intValue = newKey;
            }

            EditorGUI.EndProperty();
        }
    }
}
