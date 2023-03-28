using ARudzbenik.Data;
using ARudzbenik.General;
using System;
using UnityEngine;

namespace ARudzbenik.ARContent
{
    public class ARContentContainer : MonoBehaviour
    {
        public static Action<string, string> OnContentShown = null;
        public static Action<string, string> OnContentHidden = null;

        [SerializeField] private GameObject _content = null;
        [SerializeField] private DefaultObserverEventHandler _target = null;
        [Header("AR Content Information")]
        [SerializeField] private Lesson _lesson = Lesson.NO_LESSON;
        [SerializeField] private string _contentName = null;

        private string _lessonName = null;

        private void Awake()
        {
            if (_target == null) _target = GetComponentInParent<DefaultObserverEventHandler>();
            if (_target == null) Destroy(this);

            _lessonName = Constants.GetLessonName(_lesson);

            _content.SetActive(false);
            _target.OnTargetFound.AddListener(ShowContent);
            _target.OnTargetLost.AddListener(HideContent);
        }

        private void OnDestroy()
        {
            _target.OnTargetFound.RemoveListener(ShowContent);
            _target.OnTargetLost.RemoveListener(HideContent);
        }

        private void ShowContent()
        {
            _content.SetActive(true);
            OnContentShown?.Invoke(_contentName, _lessonName);
        }

        private void HideContent()
        {
            _content.SetActive(false);
            OnContentHidden?.Invoke(_contentName, _lessonName);
        }
    }
}