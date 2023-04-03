using ARudzbenik.Data;
using ARudzbenik.General;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ARudzbenik.UserInterface
{
    public class LessonsUIManager : MonoBehaviour
    {
        [SerializeField] private AnimatedButton _menuButton = null;
        [SerializeField] private GameObject _raycastBlocker = null;
        [Header("Lesson Picker Container")]
        [SerializeField] private SlidableContainer _lessonPickerContainer = null;
        [SerializeField] private LessonPickToggle _lessonPickTogglePrefab = null;
        [SerializeField] private ToggleGroup _lessonPickToggleGroup = null;
        [SerializeField] private Transform _lessonPickToggleContainer = null;
        [SerializeField] private AnimatedButton _continueButton = null;
        [Header("Lesson View Container")]
        [SerializeField] private SlidableContainer _lessonViewContainer = null;
        [SerializeField] private Scrollbar _lessonViewScrollbar = null;
        [SerializeField] private TextMeshProUGUI _lessonNameText = null;
        [SerializeField] private LessonElementTile _lessonElementTilePrefab = null;
        [SerializeField] private Transform _lessonElementContainer = null;
        [SerializeField] private AnimatedButton _backButton = null;
        [Header("Error Container")]
        [SerializeField] private SlidableContainer _errorContainer = null;
        [SerializeField] private TextMeshProUGUI _errorText = null; 
        [SerializeField] private AnimatedButton _errorBackButton = null;

        private ActiveContainer _activeContainer = ActiveContainer.NO_CONTAINER;
        private Lesson _chosenLesson = Lesson.NO_LESSON;
        private LessonData _lesson = null;
        private List<LessonElementTile> _lessonElementTiles = new List<LessonElementTile>();

        private void Start()
        {
            _menuButton.InitializeOnClick(() =>
            {
                GetActiveContainer().Slide(ContainerPosition.OFF_SCREEN_LEFT, () => SceneManager.LoadScene(Constants.MAIN_MENU_SCENE_BUILD_INDEX));
            });

            _lessonPickerContainer.Slide(ContainerPosition.OFF_SCREEN_LEFT, moveInstantly: true);
            _lessonViewContainer.Slide(ContainerPosition.OFF_SCREEN_LEFT, moveInstantly: true);
            _errorContainer.Slide(ContainerPosition.OFF_SCREEN_LEFT, moveInstantly: true);

            _continueButton.InitializeOnClick(() => GoToLessonView());
            _backButton.InitializeOnClick(() => GoToLessonPicker());

            InitializeLessonPicker();
            GoToLessonPicker();
        }

        private SlidableContainer GetActiveContainer()
        {
            SlidableContainer activeContainer = null;
            switch (_activeContainer)
            {
                case ActiveContainer.LESSON_PICKER_CONTAINER: activeContainer = _lessonPickerContainer; break;
                case ActiveContainer.LESSON_VIEW_CONTAINER: activeContainer = _lessonViewContainer; break;
                case ActiveContainer.ERROR_CONTAINER: activeContainer = _errorContainer; break;
            }
            return activeContainer;
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

        private void LoadLesson()
        {
            while (_lessonElementTiles.Count < _lesson.LessonElements.Length) _lessonElementTiles.Add(Instantiate(_lessonElementTilePrefab, _lessonElementContainer));

            for (int index = 0; index < _lessonElementTiles.Count; index++)
            {
                if (index >= _lesson.LessonElements.Length) _lessonElementTiles[index].gameObject.SetActive(false);
                else
                {
                    _lessonElementTiles[index].gameObject.SetActive(true);
                    _lessonElementTiles[index].InitializeTile(_lesson.LessonElements[index]);
                }
            }

            _lessonViewScrollbar.value = 1.0f;
            Canvas.ForceUpdateCanvases();
        }

        private void GoToLessonPicker()
        {
            _raycastBlocker.SetActive(true);
            _lessonPickerContainer.Slide(ContainerPosition.ON_SCREEN, () => _raycastBlocker.SetActive(false));

            if (_activeContainer != ActiveContainer.LESSON_PICKER_CONTAINER) _lessonViewContainer.Slide(ContainerPosition.OFF_SCREEN_LEFT);
            else _lessonViewContainer.Slide(ContainerPosition.OFF_SCREEN_LEFT, moveInstantly: true);

            _activeContainer = ActiveContainer.LESSON_PICKER_CONTAINER;
        }

        private void GoToLessonView()
        {
            _lessonNameText.text = Constants.GetLessonName(_chosenLesson);

            TextAsset lessonFile = Resources.Load(_chosenLesson.ToString() + Constants.LESSON_FILE_PATH_SUFIX) as TextAsset;
            if (lessonFile != null)
            {
                _lesson = JsonUtility.FromJson<LessonData>(lessonFile.text);
                LoadLesson();
            }

            bool isLessonUnavailable = lessonFile == null || _lesson.LessonElements.Length == 0;
            if (isLessonUnavailable)
            {
                GoToError("TRENUTNO NE POSTOJE PODACI ZA ODABRANU LEKCIJU.", _lessonPickerContainer, () =>
                {
                    _raycastBlocker.SetActive(true);
                    _errorContainer.Slide(ContainerPosition.OFF_SCREEN_LEFT);
                    _lessonPickerContainer.Slide(ContainerPosition.ON_SCREEN, () => _raycastBlocker.SetActive(false));
                    _activeContainer = ActiveContainer.LESSON_PICKER_CONTAINER;
                });
                return;
            }

            _raycastBlocker.SetActive(true);
            _lessonPickerContainer.Slide(ContainerPosition.OFF_SCREEN_RIGHT);
            _lessonViewContainer.Slide(ContainerPosition.ON_SCREEN, () => _raycastBlocker.SetActive(false));
            _activeContainer = ActiveContainer.LESSON_VIEW_CONTAINER;
        }

        private void GoToError(string errorMessage, SlidableContainer currentContainer, Action onErrorBackButtonPressed)
        {
            _errorText.text = errorMessage;
            _errorBackButton.InitializeOnClick(onErrorBackButtonPressed, removePreviousListeners: true);

            _raycastBlocker.SetActive(true);
            _errorContainer.Slide(ContainerPosition.ON_SCREEN, () => _raycastBlocker.SetActive(false));
            if (_activeContainer != ActiveContainer.NO_CONTAINER) GetActiveContainer().Slide(ContainerPosition.OFF_SCREEN_RIGHT);
            _activeContainer = ActiveContainer.ERROR_CONTAINER;
        }

        private enum ActiveContainer { NO_CONTAINER, LESSON_PICKER_CONTAINER, LESSON_VIEW_CONTAINER, ERROR_CONTAINER };
    }
}