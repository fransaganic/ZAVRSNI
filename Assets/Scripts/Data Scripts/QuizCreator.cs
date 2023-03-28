using ARudzbenik.General;
using System.IO;
using UnityEngine;

namespace ARudzbenik.Data
{
    public class QuizCreator : MonoBehaviour
    {
        [SerializeField] private Lesson _lesson = Lesson.NO_LESSON;
        [SerializeField] private QuestionData[] _questions = null;

        private string _path = null;

        private void Awake()
        {
            _path = Application.dataPath + "/Resources/";
        }

        public void LoadQuiz()
        {
#if UNITY_EDITOR
            TextAsset quizFile = Resources.Load(_lesson.ToString() + Constants.QUIZ_FILE_PATH_SUFIX) as TextAsset;
            
            if (quizFile == null)
            {
                Debug.LogError("There is no file under that name!");
                return;
            }

            QuizData quizData = JsonUtility.FromJson<QuizData>(quizFile.text);
            _questions = quizData.Questions;
#endif
        }

        public void SaveQuiz()
        {
#if UNITY_EDITOR
            if (_lesson == Lesson.NO_LESSON)
            {
                Debug.LogError("Please choose a lesson!");
                return;
            }

            QuizData quizData = new QuizData
            {
                Lesson = _lesson,
                Questions = _questions
            };
            File.WriteAllText(_path + _lesson.ToString() + Constants.QUIZ_FILE_PATH_SUFIX + ".json", JsonUtility.ToJson(quizData));
#endif
        }
    }
}