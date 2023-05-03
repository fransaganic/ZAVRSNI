using System;
using System.Collections.Generic;
using UnityEngine;

namespace ARudzbenik.ARContent.ContentQuiz
{
    public class ContentWithQuiz : MonoBehaviour
    {
        public static Action<SelectableObject> OnObjectSelectedAction = null;

        [SerializeField] private ContentQuestion _question = null;
        [SerializeField] private bool _useFunctionality = false;

        public string CurrentQuestionText => _question.QuestionText;
        public bool IsBeingDestroyed { get; private set; } = false;

        private List<ContentAnswer> _currentlySelectedAnswers = new List<ContentAnswer>();
        private bool _isQuizActive = false;

        private void Awake()
        {
            if (!_useFunctionality || _question == null)
            {
                IsBeingDestroyed = true;
                Destroy(this);
            }
        }

        private void SelectAnswer(SelectableObject selectedObject)
        {
            if (!_isQuizActive) return;

            foreach (ContentAnswer answer in _question.Answers)
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

                _currentlySelectedAnswers.Clear();
                OnObjectSelectedAction -= SelectAnswer;
            }
        }
    }
}