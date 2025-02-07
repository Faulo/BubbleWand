using UnityEditor;
using UnityEngine;

namespace uMicrophoneWebGL {

    public static class EditorUtil {
        public static string GetKey(string title, string category) {
            return $"uMicrophoneWebGL-{title}-{category}";
        }

        static string GetFoldOutKey(string title) {
            return GetKey(title, "FoldOut");
        }

        public static bool EditorOnlyToggle(string title, string category, bool initialState) {
            string keyTitle = title.Replace(" ", "_");
            string key = GetKey(keyTitle, category);
            bool value = EditorPrefs.GetBool(key, initialState);
            bool newValue = EditorGUILayout.Toggle(title, value);
            if (newValue != value) {
                EditorPrefs.SetBool(key, newValue);
            }

            return newValue;
        }

        public static bool IsFoldOutOpened(string title) {
            return EditorPrefs.GetBool(GetFoldOutKey(title));
        }

        public static bool Foldout(string title, bool initialState) {
            var style = new GUIStyle("ShurikenModuleTitle") {
                font = new GUIStyle(EditorStyles.label).font,
                border = new RectOffset(15, 7, 4, 4),
                fixedHeight = 22,
                contentOffset = new Vector2(20f, -2f),
                margin = new RectOffset((EditorGUI.indentLevel + 1) * 16, 0, 0, 0)
            };

            string key = GetFoldOutKey(title);
            bool display = EditorPrefs.GetBool(key, initialState);

            var rect = GUILayoutUtility.GetRect(16f, 22f, style);
            GUI.Box(rect, title, style);

            var e = Event.current;

            var toggleRect = new Rect(rect.x + 4f, rect.y + 2f, 13f, 13f);
            if (e.type == EventType.Repaint) {
                EditorStyles.foldout.Draw(toggleRect, false, false, display, false);
            }

            if (e.type == EventType.MouseDown && rect.Contains(e.mousePosition)) {
                EditorPrefs.SetBool(key, !display);
                e.Use();
            }

            return display;
        }

        public static bool SimpleFoldout(string title, bool initialState, string additionalKey = "") {
            string key = GetFoldOutKey(title + additionalKey);
            bool display = EditorPrefs.GetBool(key, initialState);
            bool newDisplay = EditorGUILayout.Foldout(display, title, EditorStyles.foldoutHeader);
            if (newDisplay != display) {
                EditorPrefs.SetBool(key, newDisplay);
            }

            return newDisplay;
        }

        public static void DrawProperty(SerializedObject obj, string propName) {
            var prop = obj.FindProperty(propName);
            if (prop == null) {
                return;
            }

            EditorGUILayout.PropertyField(prop);
        }
    }
}