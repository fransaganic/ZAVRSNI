using ARudzbenik.ARContent;
using UnityEngine;

namespace ARudzbenik.UserInterface
{
    public class AnimatedContentUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject _buttonContainer = null;
        [SerializeField] private AnimatedButton _playButton = null;
        [SerializeField] private AnimatedButton _replayButton = null;
        [SerializeField] private AnimatedButton _stopButton = null;

        private void Awake()
        {
            AnimatedContent.OnAnimatedContentShowAction += Activate;
            AnimatedContent.OnAnimatedContentHideAction += Deactivate;
        }

        private void Start()
        {
            _playButton.InitializeOnClick(() => AnimatedContent.OnPlayAnimationAction?.Invoke());
            _replayButton.InitializeOnClick(() => AnimatedContent.OnReplayAnimationAction?.Invoke());
            _stopButton.InitializeOnClick(() => AnimatedContent.OnStopAnimationAction?.Invoke());

            Deactivate();
        }

        private void OnDestroy()
        {
            AnimatedContent.OnAnimatedContentShowAction -= Activate;
            AnimatedContent.OnAnimatedContentHideAction -= Deactivate;
        }

        private void Activate()
        {
            _buttonContainer.SetActive(true);
        }

        private void Deactivate()
        {
            _buttonContainer.SetActive(false);
        }
    }
}