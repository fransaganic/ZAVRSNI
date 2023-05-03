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

        private List<ContentAnswer> _currentlySelectedAnswers = new List<ContentAnswer>();
        private bool _isQuizActive = false;

        private void Awake()
        {
            if (!_useFunctionality || _question == null) Destroy(this);
        }

        private void OnEnable()
        {
            _isQuizActive = true;
            // TODO: update question text
            OnObjectSelectedAction += SelectAnswer;
        }

        private void OnDisable()
        {
            foreach (ContentAnswer answer in _currentlySelectedAnswers) answer.AnswerObject.UpdateSelectionVisual(isSelected: false);
            
            _currentlySelectedAnswers.Clear();
            _isQuizActive = false;
            // TODO: clear question text
            OnObjectSelectedAction -= SelectAnswer;
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
    }
}