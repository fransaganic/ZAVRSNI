using ARudzbenik.ARContent;
using ARudzbenik.General;
using DG.Tweening;
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
        [SerializeField] private Transform _contentInformationContainer = null;
        [SerializeField] private TextMeshProUGUI _contentNameText = null;
        [SerializeField] private TextMeshProUGUI _lessonNameText = null;
        [Header("Slide Animation Values")]
        [SerializeField] private float _slideAnimationDuration = 0.0f;

        private bool _isContentShown = false;
        private bool _isContentInformationShown = false;
        private Vector3 _positionOnScreen = Vector3.zero;
        private Vector3 _positionOffScreenDown = Vector3.zero;

        private void Awake()
        {
            _positionOnScreen = _contentInformationContainer.position;
            _positionOffScreenDown = _contentInformationContainer.position + Screen.height * Vector3.down;
            _contentInformationContainer.position = _positionOffScreenDown;

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
            _contentInformationContainer.DOKill();
            _contentInformationContainer.DOMove(_positionOnScreen, _slideAnimationDuration).OnComplete(() => _isContentInformationShown = true);
        }

        private void AnimateContentInformationHide()
        {
            if (!_isContentInformationShown) return;
            _contentInformationContainer.DOKill();
            _contentInformationContainer.DOMove(_positionOffScreenDown, _slideAnimationDuration).OnComplete(() => _isContentInformationShown = false);
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