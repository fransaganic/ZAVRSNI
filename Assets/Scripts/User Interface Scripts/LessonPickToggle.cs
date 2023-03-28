using ARudzbenik.Data;
using ARudzbenik.General;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ARudzbenik.UserInterface
{
    [RequireComponent(typeof(Toggle))]
    public class LessonPickToggle : MonoBehaviour
    {
        [SerializeField] private Image _outline = null;
        [SerializeField] private TextMeshProUGUI _text = null;
        [Header("Toggle Colors")]
        [SerializeField] private Color _normalColor = Color.white;
        [SerializeField] private Color _selectedColor = Color.white;
        [Header("Click Animation Values")]
        [SerializeField] private float _clickAnimationDuration = 0.0f;
        [SerializeField] private float _clickAnimationScaleFactor = 0.0f;

        private Toggle _toggle = null;
        private Lesson _lesson = Lesson.NO_LESSON;
        private Sequence _animationSequence = null;

        private void Awake()
        {
            _toggle = GetComponent<Toggle>();
        }

        private void AnimateClick(Action onClickAction)
        {
            if (_animationSequence != null)
            {
                _animationSequence.Kill();
                _animationSequence = null;
            }

            _animationSequence = DOTween.Sequence()
                .Append(transform.DOScale(_clickAnimationScaleFactor, _clickAnimationDuration / 2.0f))
                .Append(transform.DOScale(1.0f, _clickAnimationDuration / 2.0f))
                .OnComplete(() => onClickAction?.Invoke());
        }

        public void InitializeOnValueChanged(Lesson lesson, Action<Lesson> onToggleOnAction, ToggleGroup toggleGroup = null)
        {
            if (toggleGroup != null) _toggle.group = toggleGroup;

            _lesson = lesson;
            _text.text = Constants.GetLessonName(lesson);
            _toggle.onValueChanged.AddListener((isOn) =>
            {
                _outline.color = isOn ? _selectedColor : _normalColor;
                _text.color = isOn ? _selectedColor : _normalColor;
                if (isOn) AnimateClick(() => onToggleOnAction?.Invoke(_lesson));
            });
        }
    }
}