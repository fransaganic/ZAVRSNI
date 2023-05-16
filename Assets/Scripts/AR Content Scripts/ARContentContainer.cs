using ARudzbenik.ARContent.ContentQuiz;
using ARudzbenik.Data;
using ARudzbenik.General;
using System;
using UnityEngine;

namespace ARudzbenik.ARContent
{
    public class ARContentContainer : MonoBehaviour
    {
        public static Action<ARContentContainer> OnContentShownAction = null;
        public static Action<ARContentContainer> OnContentHiddenAction = null;

        [SerializeField] private GameObject _content = null;
        [SerializeField] private DefaultObserverEventHandler _target = null;
        [Header("AR Content Information")]
        [SerializeField] private Lesson _lesson = Lesson.NO_LESSON;
        [SerializeField] private string _contentName = null;

        private string _lessonName = null;
        private ContentWithQuiz _quizComponent = null;

        public string ContentName => _contentName;
        public string LessonName => _lessonName;
        public bool HasQuiz => _quizComponent != null && !_quizComponent.IsBeingDestroyed;
        public string QuizQuestionText => _quizComponent.CurrentQuestionText;

        private void Awake()
        {
            if (_target == null) _target = GetComponentInParent<DefaultObserverEventHandler>();
            if (_target == null) Destroy(this);

            _lessonName = Constants.GetLessonName(_lesson);

            _content.SetActive(false);
            _target.OnTargetFound.AddListener(ShowContent);
            _target.OnTargetLost.AddListener(HideContent);
        }

        private void Start()
        {
            _quizComponent = _content.GetComponent<ContentWithQuiz>();
        }

        private void OnDestroy()
        {
            _target.OnTargetFound.RemoveListener(ShowContent);
            _target.OnTargetLost.RemoveListener(HideContent);
        }

        private void ShowContent()
        {
            _content.SetActive(true);
            OnContentShownAction?.Invoke(this);
        }

        private void HideContent()
        {
            _content.SetActive(false);
            OnContentHiddenAction?.Invoke(this);
        }

        public void NextQuizQuestion()
        {
            _quizComponent.NextQuestion();
        }    

        public void ToggleQuiz(bool isActive)
        {
            _quizComponent.ToggleQuiz(isActive);
        }
    }
}