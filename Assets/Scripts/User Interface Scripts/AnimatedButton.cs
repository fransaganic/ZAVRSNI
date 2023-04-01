using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ARudzbenik.UserInterface
{
    [RequireComponent(typeof(Button))]
    public class AnimatedButton : MonoBehaviour
    {
        [SerializeField] private Image _icon = null;
        [SerializeField] private Image _outline = null;
        [SerializeField] private TextMeshProUGUI _text = null;
        [Header("Button Color Values")]
        [SerializeField] private Color _normalColor = Color.white;
        [SerializeField] private Color _pressedColor = Color.white;
        [Header("Click Animation Values")]
        [SerializeField] private float _clickAnimationDuration = 0.0f;
        [SerializeField] private float _clickAnimationScaleFactor = 0.0f;

        private Button _button = null;
        private Sequence _animationSequence = null;

        private void Awake()
        {
            _button = GetComponent<Button>();
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
                .OnComplete(() =>
                {
                    _animationSequence = null;
                    _outline.color = _normalColor;
                    if (_icon != null) _icon.color = _normalColor;
                    if (_text != null) _text.color = _normalColor;
                    onClickAction?.Invoke();
                });
            _outline.color = _pressedColor;
            if (_icon != null) _icon.color = _pressedColor;
            if (_text != null) _text.color = _pressedColor;
        }

        public void InitializeOnClick(Action onClickAction)
        {
            if (_button == null) _button = GetComponent<Button>();
            _button.onClick.AddListener(() => AnimateClick(onClickAction));
        }

        public void SetInteractable(bool isInteractable)
        {
            _button.interactable = isInteractable;
        }

        public void SetText(string text)
        {
            if (_text != null) _text.text = text;
        }
    }
}