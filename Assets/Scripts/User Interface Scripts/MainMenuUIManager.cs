using ARudzbenik.General;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ARudzbenik.UserInterface
{
    public class MainMenuUIManager : MonoBehaviour
    {
        [SerializeField] private Transform _mainMenuUIContainer = null;
        [SerializeField] private GameObject _raycastBlocker = null;
        [SerializeField] private AnimatedButton _arButton = null;
        [SerializeField] private AnimatedButton _lessonsButton = null;
        [SerializeField] private AnimatedButton _quizButton = null;
        [SerializeField] private AnimatedButton _exitButton = null;
        [Header("Slide Animation Values")]
        [SerializeField] private float _slideAnimationDuration = 0.0f;

        private Vector3 _positionOnScreen = Vector3.zero;
        private Vector3 _positionOffScreenUp = Vector3.zero;

        private void Awake()
        {
            _positionOnScreen = _mainMenuUIContainer.position;
            _positionOffScreenUp = _mainMenuUIContainer.position + Screen.height * Vector3.up;
            _mainMenuUIContainer.position = _positionOffScreenUp;
        }

        private void Start()
        {
            _arButton.InitializeOnClick(() => AnimateDisappear(() => SceneManager.LoadScene(Constants.AR_SCENE_BUILD_INDEX)));
            _lessonsButton.InitializeOnClick(() => AnimateDisappear(() => SceneManager.LoadScene(Constants.LESSON_SCENE_BUILD_INDEX)));
            _quizButton.InitializeOnClick(() => AnimateDisappear(() => SceneManager.LoadScene(Constants.QUIZ_SCENE_BUILD_INDEX)));
            _exitButton.InitializeOnClick(() => AnimateDisappear(() => Application.Quit()));

            AnimateAppear();
        }

        private void AnimateAppear()
        {
            _raycastBlocker.SetActive(true);
            _mainMenuUIContainer.DOMove(_positionOnScreen, _slideAnimationDuration).OnComplete(() => _raycastBlocker.SetActive(false));
        }

        private void AnimateDisappear(Action onDisappearAction)
        {
            _raycastBlocker.SetActive(true);
            _mainMenuUIContainer.DOMove(_positionOffScreenUp, _slideAnimationDuration).OnComplete(() => onDisappearAction?.Invoke());
        }
    }
}