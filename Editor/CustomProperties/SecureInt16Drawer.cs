using UnityEditor;
using UnityEngine;
using Kuro.UnityUtils.Security.Primitives;

namespace Kuro.UnityUtils
{
    [CustomPropertyDrawer(typeof(SecureInt16))]
    public class SecureInt16Drawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var modProp = property.FindPropertyRelative("_modifier");
            var obProp = property.FindPropertyRelative("_obfuscated");
            var keyProp = property.FindPropertyRelative("_key");

            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();

            var newValue = (short)EditorGUI.IntField(position, label, modProp.intValue);

            if (EditorGUI.EndChangeCheck())
            {
                var newKey = (short)Random.Range(short.MinValue, short.MaxValue);
                var obfuscated = newValue ^ newKey;

                modProp.intValue = newValue;
                obProp.intValue = obfuscated;
                keyProp.intValue = newKey;
            }

            EditorGUI.EndProperty();
        }
    }
}
