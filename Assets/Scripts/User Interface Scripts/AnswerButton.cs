using ARudzbenik.Data;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ARudzbenik.UserInterface
{
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(VerticalLayoutGroup))]
    public class AnswerButton : MonoBehaviour
    {
        [SerializeField] private Image _outline = null;
        [SerializeField] private TextMeshProUGUI _text = null;
        [Header("Button Colors")]
        [SerializeField] private Color _normalColor = Color.white;
        [SerializeField] private Color _selectedColor = Color.white;
        [SerializeField] private Color _correctAnswerColor = Color.white;
        [SerializeField] private Color _correctNonSelectedAnswerColor = Color.white;
        [SerializeField] private Color _incorrectAnswerColor = Color.white;
        [Header("Click Animation Values")]
        [SerializeField] private float _clickAnimationDuration = 0.0f;
        [SerializeField] private float _clickAnimationScaleFactor = 0.0f;

        private Button _button = null;
        private VerticalLayoutGroup _verticalLayoutGroup = null;
        private AnswerData _answer = null;
        private Sequence _animationSequence = null;
        private bool _isSelected = false;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
        }

        private void AnimateClick(Action onClickAction)
        {
            if (_animationSequence != null)
            {
                _animationSequence.Kill();
                _animationSequence = null;
            }

            if (_isSelected)
            {
                _animationSequence = DOTween.Sequence()
                    .Append(transform.DOScale(_clickAnimationScaleFactor, _clickAnimationDuration / 2.0f))
                    .Append(transform.DOScale(1.0f, _clickAnimationDuration / 2.0f))
                    .OnComplete(() => onClickAction?.Invoke());
            }
            else onClickAction?.Invoke();
            _outline.color = _isSelected ? _selectedColor : _normalColor;
            _text.color = _isSelected ? _selectedColor : _normalColor;
        }

        private void RefreshLayoutGroup()
        {
            Canvas.ForceUpdateCanvases();
            _verticalLayoutGroup.enabled = false;
            _verticalLayoutGroup.enabled = true;
        }

        public void InitializeButton(AnswerData answer, Action<AnswerData> onClickAction)
        {
            _answer = answer;

            _text.text = answer.AnswerText;
            RefreshLayoutGroup();

            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(() => 
            {
                _isSelected = !_isSelected;
                AnimateClick(() => onClickAction?.Invoke(_answer));
            });
            _button.interactable = true;

            _isSelected = false;
            _outline.color = _normalColor;
            _text.color = _normalColor;
        }

        public void ShowResult()
        {
            if (_isSelected)
            {
                _outline.color = _answer.IsCorrect ? _correctAnswerColor : _incorrectAnswerColor;
                _text.color = _answer.IsCorrect ? _correctAnswerColor : _incorrectAnswerColor;
            }
            else if (_answer.IsCorrect)
            {
                _outline.color = _correctNonSelectedAnswerColor;
                _text.color = _correctNonSelectedAnswerColor;
            }
            _button.interactable = false;
        }
    }
}