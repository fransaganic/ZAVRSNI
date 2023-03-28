using ARudzbenik.Data;
using ARudzbenik.General;
using System.Collections.Generic;
using UnityEngine;

namespace ARudzbenik.Quiz
{
    public class QuizManager
    {
        private QuizData _quizData = null;
        private int _currentQuestionIndex = 0;
        private int _currentQuizLength = 0;

        public QuestionData CurrentQuestion => _currentQuestionIndex == _currentQuizLength ? null : _quizData.Questions[_currentQuestionIndex];
        public bool IsLastQuestion => _currentQuestionIndex == _currentQuizLength - 1;
        public int QuizLength => _quizData != null ? _quizData.Questions.Length : 0;
        public int Score { get; private set; }

        public void CheckAnswers(List<AnswerData> selectedAnswers)
        {
            int correctAnswersCount = 0;
            foreach (AnswerData answer in _quizData.Questions[_currentQuestionIndex].Answers)
            {
                if (answer.IsCorrect)
                {
                    if (!selectedAnswers.Contains(answer)) return;
                    correctAnswersCount++;
                }
            }
            if (correctAnswersCount != selectedAnswers.Count) return;
            Score++;
        }

        public void NextQuestion()
        {
            _currentQuestionIndex++;
        }

        public bool LoadQuiz(Lesson lesson)
        {
            TextAsset quizFile = Resources.Load(lesson.ToString() + Constants.QUIZ_FILE_PATH_SUFIX) as TextAsset;
            
            if (quizFile == null) return false;

            _quizData = JsonUtility.FromJson<QuizData>(quizFile.text);
            _currentQuestionIndex = 0;
            _currentQuizLength = QuizLength;
            Score = 0;
            Resources.UnloadUnusedAssets();
            return true;
        }
    }
}