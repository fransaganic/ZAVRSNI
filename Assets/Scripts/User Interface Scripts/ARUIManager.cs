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
        [SerializeField] private AnimatedButton _menuButton = null;
        [Header("Content Information Values")]
        [SerializeField] private SlidableContainer _contentInformationContainer = null;
        [SerializeField] private TextMeshProUGUI _contentNameText = null;
        [SerializeField] private TextMeshProUGUI _lessonNameText = null;

        private bool _isContentShown = false;
        private bool _isContentInformationShown = false;

        private void Awake()
        {
            ARContentContainer.OnContentShown += OnContentShown;
            ARContentContainer.OnContentHidden += OnContentHidden;
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

            _menuButton.InitializeOnClick(() => SceneManager.LoadScene(Constants.MAIN_MENU_SCENE_BUILD_INDEX));
        }

        private void OnDestroy()
        {
            ARContentContainer.OnContentShown -= OnContentShown;
            ARContentContainer.OnContentHidden -= OnContentHidden;
        }

        private void AnimateContentInformationShow()
        {
            if (!_isContentShown || _isContentInformationShown) return;
            _contentInformationContainer.Slide(ContainerPosition.ON_SCREEN, () => _isContentInformationShown = true);
        }

        private void AnimateContentInformationHide()
        {
            if (!_isContentInformationShown) return;
            _contentInformationContainer.Slide(ContainerPosition.OFF_SCREEN_DOWN, () => _isContentInformationShown = false);
        }

        private void OnContentShown(string contentName, string lessonName)
        {
            _contentNameText.text = contentName;
            _lessonNameText.text = lessonName;
            _isContentShown = true;
            _contentInformationButton.SetInteractable(true);
        }

        private void OnContentHidden(string contentName, string lessonName)
        {
            if (_contentNameText.text != contentName || _lessonNameText.text != lessonName) return;

            _isContentShown = false;
            _contentInformationButton.SetInteractable(false);
            if (_isContentInformationShown) AnimateContentInformationHide();
        }
    }
}