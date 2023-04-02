using ARudzbenik.Data;
using ARudzbenik.General;
using ARudzbenik.Quiz;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ARudzbenik.UserInterface
{
    public class QuizUIManager : MonoBehaviour
    {
        [SerializeField] private AnimatedButton _menuButton = null;
        [SerializeField] private GameObject _raycastBlocker = null;
        [Header("Lesson Picker Container")]
        [SerializeField] private SlidableContainer _lessonPickerContainer = null;
        [SerializeField] private LessonPickToggle _lessonPickTogglePrefab = null;
        [SerializeField] private ToggleGroup _lessonPickToggleGroup = null;
        [SerializeField] private Transform _lessonPickToggleContainer = null;
        [SerializeField] private AnimatedButton _continueToPreQuizButton = null;
        [Header("Pre-Quiz Container")]
        [SerializeField] private SlidableContainer _preQuizContainer = null;
        [SerializeField] private TextMeshProUGUI _lessonNameText = null;
        [SerializeField] private GameObject _quizUnavailableContainer = null;
        [SerializeField] private GameObject _quizAvailableContainer = null;
        [SerializeField] private TextMeshProUGUI _numberOfQuestionsText = null;
        [SerializeField] private AnimatedButton _backToLessonPickerButton = null;
        [SerializeField] private AnimatedButton _continueToQuizButton = null;
        [Header("Quiz Container")]
        [SerializeField] private SlidableContainer _quizContainer = null;
        [SerializeField] private TextMeshProUGUI _questionText = null;
        [SerializeField] private AnswerButton _answerButtonPrefab = null;
        [SerializeField] private Transform _answerButtonContainer = null;
        [SerializeField] private VerticalLayoutGroup _answerButtonLayoutGroup = null;
        [SerializeField] private float _answerResultDisplayDuration = 0.0f;
        [SerializeField] private Slider _progressBar = null;
        [SerializeField] private float _progressBarFillDuration = 0.0f;
        [SerializeField] private AnimatedButton _nextButton = null;
        [Header("Post-Quiz Container")]
        [SerializeField] private SlidableContainer _postQuizContainer = null;
        [SerializeField] private TextMeshProUGUI _resultText = null;
        [SerializeField] private AnimatedButton _exitResultViewButton = null;

        private ActiveContainer _activeContainer = ActiveContainer.NO_CONTAINER;
        private Lesson _chosenLesson = Lesson.NO_LESSON;
        private QuizManager _quizManager = null;
        private List<AnswerButton> _answerButtons = new List<AnswerButton>();
        private List<AnswerData> _selectedAnswers = new List<AnswerData>();

        private void Awake()
        {
            _quizManager = new QuizManager();
        }

        private void Start()
        {
            _menuButton.InitializeOnClick(() => 
            {
                SlidableContainer activeContainer = null;
                switch (_activeContainer)
                {
                    case ActiveContainer.LESSON_PICKER_CONTAINER: activeContainer = _lessonPickerContainer; break;
                    case ActiveContainer.PRE_QUIZ_CONTAINER: activeContainer = _preQuizContainer; break;
                    case ActiveContainer.QUIZ_CONTAINER: activeContainer = _quizContainer; break;
                    case ActiveContainer.POST_QUIZ_CONTAINER: activeContainer = _postQuizContainer; break;
                }
                activeContainer.Slide(ContainerPosition.OFF_SCREEN_RIGHT, () => SceneManager.LoadScene(Constants.MAIN_MENU_SCENE_BUILD_INDEX));
            });

            _continueToPreQuizButton.InitializeOnClick(() => GoToPreQuiz());

            _backToLessonPickerButton.InitializeOnClick(() => GoToLessonPicker());
            _continueToQuizButton.InitializeOnClick(() => GoToQuiz());

            _nextButton.InitializeOnClick(() => CheckAnswers(() => 
            {
                _selectedAnswers.Clear();
                _quizManager.NextQuestion();
                ShowQuestion();
            }));

            _exitResultViewButton.InitializeOnClick(() => GoToLessonPicker());

            InitializeLessonPicker();
            GoToLessonPicker();

            _lessonPickerContainer.Slide(ContainerPosition.OFF_SCREEN_RIGHT, moveInstantly: true);
            _preQuizContainer.Slide(ContainerPosition.OFF_SCREEN_RIGHT, moveInstantly: true);
            _quizContainer.Slide(ContainerPosition.OFF_SCREEN_RIGHT, moveInstantly: true);
            _postQuizContainer.Slide(ContainerPosition.OFF_SCREEN_RIGHT, moveInstantly: true);
        }

        private IEnumerator InvokeActionAfterDelay(Action delayedAction, float delay)
        {
            yield return new WaitForSeconds(delay);
            delayedAction?.Invoke();
        }

        private void InitializeLessonPicker()
        {
            foreach (Lesson lesson in Enum.GetValues(typeof(Lesson)))
            {
                if (lesson == Lesson.NO_LESSON) continue;

                LessonPickToggle toggle = Instantiate(_lessonPickTogglePrefab, _lessonPickToggleContainer);
                toggle.InitializeOnValueChanged(lesson, (lesson) => _chosenLesson = lesson, _lessonPickToggleGroup);
            }
        }

        private void OnAnswerSelected(AnswerData answer)
        {
            if (_selectedAnswers.Contains(answer)) _selectedAnswers.Remove(answer);
            else _selectedAnswers.Add(answer);
        }

        private void CheckAnswers(Action onCheckFinishAction)
        {
            foreach (AnswerButton answerButton in _answerButtons)
            {
                if (answerButton.gameObject.activeSelf) answerButton.ShowResult();
            }

            _nextButton.SetInteractable(false);
            _quizManager.CheckAnswers(_selectedAnswers);
            StartCoroutine(InvokeActionAfterDelay(() => 
            {
                _nextButton.SetInteractable(true);
                onCheckFinishAction?.Invoke();
            }, _answerResultDisplayDuration));
        }

        private void ShowQuestion()
        {
            QuestionData question = _quizManager.CurrentQuestion;
            if (question == null)
            {
                GoToPostQuiz();
                return;
            }

            while (_answerButtons.Count < question.Answers.Length) _answerButtons.Add(Instantiate(_answerButtonPrefab, _answerButtonContainer));

            for (int index = 0; index < _answerButtons.Count; index++)
            {
                if (index >= question.Answers.Length)
                {
                    _answerButtons[index].gameObject.SetActive(false);
                    continue;
                }
                _answerButtons[index].gameObject.SetActive(true);
                _answerButtons[index].InitializeButton(question.Answers[index], OnAnswerSelected);
            }

            Canvas.ForceUpdateCanvases();
            _answerButtonLayoutGroup.enabled = false;
            _answerButtonLayoutGroup.enabled = true;

            _questionText.text = question.QuestionText;
            _nextButton.SetText(_quizManager.IsLastQuestion ? Constants.QUIZ_BUTTON_END_QUIZ_TEXT : Constants.QUIZ_BUTTON_NEXT_QUESTION_TEXT);
            _progressBar.DOValue(_progressBar.value + 1.0f / _quizManager.QuizLength, _progressBarFillDuration);
        }

        private void GoToLessonPicker()
        {
            _raycastBlocker.SetActive(true);
            _lessonPickerContainer.Slide(ContainerPosition.ON_SCREEN, () => _raycastBlocker.SetActive(false));
            _quizContainer.Slide(ContainerPosition.OFF_SCREEN_RIGHT, moveInstantly: true);

            if (_activeContainer == ActiveContainer.PRE_QUIZ_CONTAINER) _preQuizContainer.Slide(ContainerPosition.OFF_SCREEN_RIGHT);
            else _preQuizContainer.Slide(ContainerPosition.OFF_SCREEN_RIGHT, moveInstantly: true);

            if (_activeContainer == ActiveContainer.POST_QUIZ_CONTAINER) _postQuizContainer.Slide(ContainerPosition.OFF_SCREEN_RIGHT);
            else _postQuizContainer.Slide(ContainerPosition.OFF_SCREEN_RIGHT, moveInstantly: true);

            _activeContainer = ActiveContainer.LESSON_PICKER_CONTAINER;
        }

        private void GoToPreQuiz()
        {
            _lessonNameText.text = Constants.GetLessonName(_chosenLesson);

            _raycastBlocker.SetActive(true);
            _lessonPickerContainer.Slide(ContainerPosition.OFF_SCREEN_LEFT);
            _preQuizContainer.Slide(ContainerPosition.ON_SCREEN, () => _raycastBlocker.SetActive(false));
            _activeContainer = ActiveContainer.PRE_QUIZ_CONTAINER;

            if (_quizManager.LoadQuiz(_chosenLesson))
            {
                _quizUnavailableContainer.SetActive(false);
                _quizAvailableContainer.SetActive(true);
                _numberOfQuestionsText.text = _quizManager.QuizLength.ToString();
                _continueToQuizButton.SetInteractable(true);
            }
            else
            {
                _quizUnavailableContainer.SetActive(true);
                _quizAvailableContainer.SetActive(false);
                _continueToQuizButton.SetInteractable(false);
            }
        }

        private void GoToQuiz()
        {
            _progressBar.value = 0;
            _selectedAnswers.Clear();
            ShowQuestion();

            _raycastBlocker.SetActive(true);
            _preQuizContainer.Slide(ContainerPosition.OFF_SCREEN_LEFT);
            _quizContainer.Slide(ContainerPosition.ON_SCREEN, () => _raycastBlocker.SetActive(false));
            _activeContainer = ActiveContainer.QUIZ_CONTAINER;
        }

        private void GoToPostQuiz()
        {
            _resultText.text = _quizManager.Score.ToString() + " / " + _quizManager.QuizLength.ToString();

            _raycastBlocker.SetActive(true);
            _quizContainer.Slide(ContainerPosition.OFF_SCREEN_LEFT);
            _postQuizContainer.Slide(ContainerPosition.ON_SCREEN, () => _raycastBlocker.SetActive(false));
            _activeContainer = ActiveContainer.POST_QUIZ_CONTAINER;
        }

        private enum ActiveContainer { NO_CONTAINER, LESSON_PICKER_CONTAINER, PRE_QUIZ_CONTAINER, QUIZ_CONTAINER, POST_QUIZ_CONTAINER };
    }
}