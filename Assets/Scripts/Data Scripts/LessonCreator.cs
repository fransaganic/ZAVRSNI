using ARudzbenik.General;
using System.IO;
using UnityEngine;

namespace ARudzbenik.Data
{
    public class LessonCreator : MonoBehaviour
    {
        [SerializeField] private Lesson _lesson = Lesson.NO_LESSON;
        [SerializeField] private LessonElementData[] _lessonElements = null;

        private string _path = null;

        private void Awake()
        {
            _path = Application.dataPath + "/Resources/";
        }

        public void LoadLesson()
        {
#if UNITY_EDITOR
            TextAsset lessonFile = Resources.Load(_lesson.ToString() + Constants.LESSON_FILE_PATH_SUFIX) as TextAsset;

            if (lessonFile == null)
            {
                Debug.LogError("There is no file for that lesson!");
                return;
            }

            LessonData lessonData = JsonUtility.FromJson<LessonData>(lessonFile.text);
            _lessonElements = lessonData.LessonElements;
#endif
        }

        public void SaveLesson()
        {
#if UNITY_EDITOR
            if (_lesson == Lesson.NO_LESSON)
            {
                Debug.LogError("Please choose a lesson!");
                return;
            }

            LessonData lessonData = new LessonData
            {
                Lesson = _lesson,
                LessonElements = _lessonElements
            };
            File.WriteAllText(_path + _lesson.ToString() + Constants.LESSON_FILE_PATH_SUFIX + ".json", JsonUtility.ToJson(lessonData));
#endif
        }
    }
}