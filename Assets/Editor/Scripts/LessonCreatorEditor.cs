using UnityEditor;
using UnityEngine;

namespace ARudzbenik.Data
{
#if UNITY_EDITOR
    [CustomEditor(typeof(LessonCreator))]
    public class LessonCreatorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("Load")) ((LessonCreator)target).LoadLesson();
            if (GUILayout.Button("Save")) ((LessonCreator)target).SaveLesson();
        }
    }
#endif
}