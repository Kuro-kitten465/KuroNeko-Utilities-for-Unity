using UnityEditor;
using UnityEngine;
using Kuro.UnityUtils.Security.Primitives;

namespace Kuro.UnityUtils.Editor
{
    [CustomPropertyDrawer(typeof(SecureInt32))]
    public class SecureInt32Drawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var modProp = property.FindPropertyRelative("_modifier");
            var obProp = property.FindPropertyRelative("_obfuscated");
            var keyProp = property.FindPropertyRelative("_key");

            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();

            var newValue = EditorGUI.IntField(position, label, modProp.intValue);

            if (EditorGUI.EndChangeCheck())
            {
                var newKey = Random.Range(int.MinValue, int.MaxValue);
                var obfuscated = newValue ^ newKey;

                modProp.intValue = newValue;
                obProp.intValue = obfuscated;
                keyProp.intValue = newKey;
            }

            EditorGUI.EndProperty();
        }
    }
}
