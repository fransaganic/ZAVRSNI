using System;
using System.Collections.Generic;
using UnityEngine;

namespace ARudzbenik.ARContent.ContentQuiz
{
    public class ContentWithQuiz : MonoBehaviour
    {
        public static Action<SelectableObject> OnObjectSelectedAction = null;

        [SerializeField] private ContentQuestion[] _questions = null;
        [SerializeField] private bool _useFunctionality = false;

        public string CurrentQuestionText => _questions[_currentQuestionIndex].QuestionText;
        public bool IsBeingDestroyed { get; private set; } = false;

        private int _currentQuestionIndex = 0;
        private List<ContentAnswer> _currentlySelectedAnswers = new List<ContentAnswer>();
        private bool _isQuizActive = false;

        private void Awake()
        {
            if (!_useFunctionality || _questions.Length == 0)
            {
                IsBeingDestroyed = true;
                Destroy(this);
            }
        }

        public void NextQuestion()
        {
            _currentQuestionIndex++;
            if (_currentQuestionIndex >= _questions.Length) _currentQuestionIndex = 0;

            foreach (ContentAnswer answer in _currentlySelectedAnswers) answer.AnswerObject.UpdateSelectionVisual(isSelected: false);
            _currentlySelectedAnswers.Clear();
        }

        private void SelectAnswer(SelectableObject selectedObject)
        {
            if (!_isQuizActive) return;

            ContentQuestion currentQuestion = _questions[_currentQuestionIndex];
            foreach (ContentAnswer answer in currentQuestion.Answers)
            {
                if (answer.AnswerObject.Equals(selectedObject))
                {
                    if (_currentlySelectedAnswers.Contains(answer))
                    {
                        _currentlySelectedAnswers.Remove(answer);
                        answer.AnswerObject.UpdateSelectionVisual(isSelected: false);
                    }
                    else
                    {
                        _currentlySelectedAnswers.Add(answer);
                        answer.AnswerObject.UpdateSelectionVisual(isSelected: true);
                    }
                    break;
                }
            }
            CheckAnswer();
        }
        
        private void CheckAnswer()
        {
            if (!_isQuizActive) return;
            foreach (ContentAnswer answer in _currentlySelectedAnswers) answer.AnswerObject.UpdateCorrectnessVisual(answer.IsCorrect);
        }

        public void ToggleQuiz(bool isActive)
        {
            _isQuizActive = isActive;

            if (isActive) OnObjectSelectedAction += SelectAnswer;
            else
            {
                foreach (ContentAnswer answer in _currentlySelectedAnswers) answer.AnswerObject.UpdateSelectionVisual(isSelected: false);

                _currentQuestionIndex = 0;
                _currentlySelectedAnswers.Clear();
                OnObjectSelectedAction -= SelectAnswer;
            }
        }
    }
}