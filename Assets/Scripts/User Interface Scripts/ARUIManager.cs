using ARudzbenik.ARContent;
using ARudzbenik.General;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ARudzbenik.UserInterface
{
    public class ARUIManager : MonoBehaviour
    {
        [SerializeField] private AnimatedButton _contentInformationButton = null;
        [SerializeField] private AnimatedButton _contentQuizButton = null;
        [SerializeField] private AnimatedButton _menuButton = null;
        [Header("Content Information Values")]
        [SerializeField] private SlidableContainer _contentInformationContainer = null;
        [SerializeField] private TextMeshProUGUI _contentNameText = null;
        [SerializeField] private TextMeshProUGUI _lessonNameText = null;
        [Header("Content Quiz Values")]
        [SerializeField] private SlidableContainer _contentQuizContainer = null;
        [SerializeField] private AnimatedButton _nextQuestionButton = null;
        [SerializeField] private TextMeshProUGUI _questionText = null;

        private ARContentContainer _currentContent = null;

        private bool _isContentShown = false;
        private bool _isContentInformationShown = false;
        private bool _isContentQuizShown = false;

        private void Awake()
        {
            ARContentContainer.OnContentShownAction += OnContentShown;
            ARContentContainer.OnContentHiddenAction += OnContentHidden;
        }

        private void Start()
        {
            _contentInformationButton.InitializeOnClick(() =>
            {
                if (_isContentInformationShown) AnimateContentInformationHide();
                else AnimateContentInformationShow();
            });
            _contentInformationButton.SetInteractable(false);
            _contentInformationContainer.Slide(ContainerPosition.OFF_SCREEN_DOWN, moveInstantly: true);

            _contentQuizButton.InitializeOnClick(() => 
            {
                if (_isContentQuizShown) AnimateContentQuizHide();
                else AnimateContentQuizShow();
            });
            _contentQuizButton.SetInteractable(false);
            _contentQuizContainer.Slide(ContainerPosition.OFF_SCREEN_DOWN, moveInstantly: true);

            _menuButton.InitializeOnClick(() => SceneManager.LoadScene(Constants.MAIN_MENU_SCENE_BUILD_INDEX));

            _nextQuestionButton.InitializeOnClick(() =>
            {
                _currentContent.NextQuizQuestion();
                _questionText.text = _currentContent.QuizQuestionText;
            });
        }

        private void OnDestroy()
        {
            ARContentContainer.OnContentShownAction -= OnContentShown;
            ARContentContainer.OnContentHiddenAction -= OnContentHidden;
        }

        private void AnimateContentInformationShow()
        {
            if (!_isContentShown || _isContentInformationShown) return;
            if (_isContentQuizShown) AnimateContentQuizHide();

            _contentInformationContainer.Slide(ContainerPosition.ON_SCREEN, () => _isContentInformationShown = true);
        }

        private void AnimateContentInformationHide()
        {
            if (!_isContentInformationShown) return;
            _contentInformationContainer.Slide(ContainerPosition.OFF_SCREEN_DOWN, () => _isContentInformationShown = false);
        }

        private void AnimateContentQuizShow()
        {
            if (!_isContentShown || _isContentQuizShown) return;
            if (_isContentInformationShown) AnimateContentInformationHide();

            _contentQuizContainer.Slide(ContainerPosition.ON_SCREEN, () => _isContentQuizShown = true);
            _currentContent.ToggleQuiz(true);
        }

        private void AnimateContentQuizHide()
        {
            if (!_isContentQuizShown) return;
            _contentQuizContainer.Slide(ContainerPosition.OFF_SCREEN_DOWN, () => _isContentQuizShown = false);
            _currentContent.ToggleQuiz(false);
        }

        private void OnContentShown(ARContentContainer content)
        {
            _currentContent = content;

            _contentInformationButton.SetInteractable(content.ContentName != string.Empty || content.LessonName != string.Empty);
            _contentNameText.text = content.ContentName;
            _lessonNameText.text = content.LessonName;

            _contentQuizButton.SetInteractable(content.HasQuiz);
            _questionText.text = content.QuizQuestionText;

            _isContentShown = true;
        }

        private void OnContentHidden(ARContentContainer content)
        {
            if (content != _currentContent) return;

            _contentInformationButton.SetInteractable(false);
            _contentQuizButton.SetInteractable(false);
            _isContentShown = false;

            if (_isContentInformationShown) AnimateContentInformationHide();
            if (_isContentQuizShown) AnimateContentQuizHide();

            _currentContent = null;
        }
    }
}