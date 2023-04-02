using ARudzbenik.General;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ARudzbenik.UserInterface
{
    public class MainMenuUIManager : MonoBehaviour
    {
        [SerializeField] private SlidableContainer _mainMenuContainer = null;
        [SerializeField] private GameObject _raycastBlocker = null;
        [SerializeField] private AnimatedButton _arButton = null;
        [SerializeField] private AnimatedButton _lessonsButton = null;
        [SerializeField] private AnimatedButton _quizButton = null;
        [SerializeField] private AnimatedButton _exitButton = null;

        private void Start()
        {
            _arButton.InitializeOnClick(() => AnimateDisappear(() => SceneManager.LoadScene(Constants.AR_SCENE_BUILD_INDEX)));
            _lessonsButton.InitializeOnClick(() => AnimateDisappear(() => SceneManager.LoadScene(Constants.LESSON_SCENE_BUILD_INDEX)));
            _quizButton.InitializeOnClick(() => AnimateDisappear(() => SceneManager.LoadScene(Constants.QUIZ_SCENE_BUILD_INDEX)));
            _exitButton.InitializeOnClick(() => AnimateDisappear(() => Application.Quit()));

            _mainMenuContainer.Slide(ContainerPosition.OFF_SCREEN_UP, moveInstantly: true);

            AnimateAppear();
        }

        private void AnimateAppear()
        {
            _raycastBlocker.SetActive(true);
            _mainMenuContainer.Slide(ContainerPosition.ON_SCREEN, () => _raycastBlocker.SetActive(false));
        }

        private void AnimateDisappear(Action onDisappearAction)
        {
            _raycastBlocker.SetActive(true);
            _mainMenuContainer.Slide(ContainerPosition.OFF_SCREEN_UP, () => onDisappearAction?.Invoke());
        }
    }
}