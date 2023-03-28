using UnityEditor;
using UnityEngine;

namespace ARudzbenik.Data
{
#if UNITY_EDITOR
    [CustomEditor(typeof(QuizCreator))]
    public class QuizCreatorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("Load")) ((QuizCreator)target).LoadQuiz();
            if (GUILayout.Button("Save")) ((QuizCreator)target).SaveQuiz();
        }
    }
#endif
}