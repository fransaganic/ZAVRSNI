using System;
using UnityEngine;

namespace ARudzbenik.ARContent
{
    public class AnimatedContent : MonoBehaviour
    {
        public static Action OnAnimatedContentShowAction = null;
        public static Action OnAnimatedContentHideAction = null;
        public static Action OnPlayAnimationAction = null;
        public static Action OnReplayAnimationAction = null;
        public static Action OnStopAnimationAction = null;

        private const string _DEFAULT_STATE = "Idle";

        [SerializeField] private bool _useFunctionality = false;

        private Animator _animator = null;
        private bool _isActive = false;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            if (_animator == null) Destroy(this);

            OnPlayAnimationAction += OnPlay;
            OnReplayAnimationAction += OnReplay;
            OnStopAnimationAction += OnStop;
        }

        private void OnDestroy()
        {
            OnPlayAnimationAction -= OnPlay;
            OnReplayAnimationAction -= OnReplay;
            OnStopAnimationAction -= OnStop;
        }

        private void OnEnable()
        {
            if (_useFunctionality)
            {
                _isActive = true;
                OnAnimatedContentShowAction?.Invoke();
            }
        }

        private void OnDisable()
        {
            _isActive = false;
            OnAnimatedContentHideAction?.Invoke();
        }

        private void OnPlay()
        {
            if (_isActive) _animator.enabled = true;
        }

        private void OnReplay()
        {
            if (_isActive) _animator.Play(_DEFAULT_STATE);
        }

        private void OnStop()
        {
            if (_isActive) _animator.enabled = false;
        }
    }
}