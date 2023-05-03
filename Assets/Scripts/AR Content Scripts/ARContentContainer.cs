using ARudzbenik.Data;
using ARudzbenik.General;
using System;
using UnityEngine;

namespace ARudzbenik.ARContent
{
    public class ARContentContainer : MonoBehaviour
    {
        public static Action<int, string, string> OnContentShownAction = null;
        public static Action<int> OnContentHiddenAction = null;

        [SerializeField] private GameObject _content = null;
        [SerializeField] private DefaultObserverEventHandler _target = null;
        [Header("AR Content Information")]
        [SerializeField] private Lesson _lesson = Lesson.NO_LESSON;
        [SerializeField] private string _contentName = null;

        private string _lessonName = null;

        private int _ID = -1;

        private void Awake()
        {
            if (_target == null) _target = GetComponentInParent<DefaultObserverEventHandler>();
            if (_target == null) Destroy(this);

            _lessonName = Constants.GetLessonName(_lesson);

            _content.SetActive(false);
            _target.OnTargetFound.AddListener(ShowContent);
            _target.OnTargetLost.AddListener(HideContent);

            _ID = GetInstanceID();
        }

        private void OnDestroy()
        {
            _target.OnTargetFound.RemoveListener(ShowContent);
            _target.OnTargetLost.RemoveListener(HideContent);
        }

        private void ShowContent()
        {
            _content.SetActive(true);
            OnContentShownAction?.Invoke(_ID, _contentName, _lessonName);
        }

        private void HideContent()
        {
            _content.SetActive(false);
            OnContentHiddenAction?.Invoke(_ID);
        }
    }
}