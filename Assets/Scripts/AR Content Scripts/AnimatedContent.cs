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
            if (!_useFunctionality) Destroy(this);

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
            Debug.Log("Ucitao i pokrenuo");
            _animator.enabled = true;
            _animator.Play(_DEFAULT_STATE);
            _isActive = true;
            OnAnimatedContentShowAction?.Invoke();
        }

        private void OnDisable()
        {
            Debug.Log("NESTAO valjda");
            _isActive = false;
            OnAnimatedContentHideAction?.Invoke();
        }

        private void OnPlay()
        {
            Debug.Log("PLAY pritisnut");
            if (_isActive) _animator.enabled = true;
        }

        private void OnReplay()
        {
            if (_isActive)
            {
                Debug.Log("REPLAY pritisnut");
                _animator.enabled = true;
                _animator.Play(_DEFAULT_STATE);
            }
        }

        private void OnStop()
        {
            Debug.Log("DISABLE pritisnut");
            if (_isActive) _animator.enabled = false;
        }
    }
}