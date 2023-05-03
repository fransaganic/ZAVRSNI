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

        private int _currentContentID = -1;

        private bool _isContentShown = false;
        private bool _isContentInformationShown = false;

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

            _menuButton.InitializeOnClick(() => SceneManager.LoadScene(Constants.MAIN_MENU_SCENE_BUILD_INDEX));
        }

        private void OnDestroy()
        {
            ARContentContainer.OnContentShownAction -= OnContentShown;
            ARContentContainer.OnContentHiddenAction -= OnContentHidden;
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

        private void OnContentShown(int contentID, string contentName, string lessonName)
        {
            _currentContentID = contentID;

            _contentNameText.text = contentName;
            _lessonNameText.text = lessonName;
            _contentInformationButton.SetInteractable(contentName != string.Empty || lessonName != string.Empty);

            _isContentShown = true;
        }

        private void OnContentHidden(int contentID)
        {
            if (contentID != _currentContentID) return;

            _contentInformationButton.SetInteractable(false);
            _isContentShown = false;

            if (_isContentInformationShown) AnimateContentInformationHide();
        }
    }
}