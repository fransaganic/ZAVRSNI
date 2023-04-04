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
        private string _chosenLesson = string.Empty;
        private string _lastLesson = string.Empty;
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

            _continueButton.InitializeOnClick(() => LoadLesson());
            _backButton.InitializeOnClick(() => GoToLessonPicker());

            LoadAvailableLessonNames();
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

        private void LoadAvailableLessonNames()
        {
            RemoteDataFetcher.Instance.FetchFileNames(Constants.LESSON_REGEX_PATTERN, Constants.LESSON_REGEX_MATCH_GROUP, (isSuccessful, lessonNames) => 
            {
                if (isSuccessful && lessonNames.Length > 0)
                {
                    InitializeLessonPicker(lessonNames);
                    GoToLessonPicker();
                }
                else
                {
                    string message = isSuccessful ? "NEMA DOSTUPNIH LEKCIJA" : "NEUSPJEŠNO POVEZIVANJE NA POSLUŽITELJ";
                    GoToError(message, () =>
                    {
                        _raycastBlocker.SetActive(true);
                        _errorContainer.Slide(ContainerPosition.OFF_SCREEN_LEFT, () => SceneManager.LoadScene(Constants.MAIN_MENU_SCENE_BUILD_INDEX));
                        _activeContainer = ActiveContainer.NO_CONTAINER;
                    });
                }
            });
        }

        private void InitializeLessonPicker(string[] lessonNames)
        {
            bool optionSelected = false;
            foreach (string lessonName in lessonNames)
            {
                LessonPickToggle toggle = Instantiate(_lessonPickTogglePrefab, _lessonPickToggleContainer);
                toggle.InitializeOnValueChanged(lessonName, (lessonName) => _chosenLesson = lessonName, _lessonPickToggleGroup, isOn: !optionSelected);
                optionSelected = true;
            }
            Canvas.ForceUpdateCanvases();
        }

        private void GoToLessonPicker()
        {
            _raycastBlocker.SetActive(true);

            if (_activeContainer != ActiveContainer.LESSON_PICKER_CONTAINER) _lessonViewContainer.Slide(ContainerPosition.OFF_SCREEN_LEFT);
            else _lessonViewContainer.Slide(ContainerPosition.OFF_SCREEN_LEFT, moveInstantly: true);

            _lessonPickerContainer.Slide(ContainerPosition.ON_SCREEN, () => _raycastBlocker.SetActive(false));
            _activeContainer = ActiveContainer.LESSON_PICKER_CONTAINER;

        }

        private void LoadLesson()
        {
            if (_chosenLesson == _lastLesson)
            {
                GoToLessonView();
                return;
            }
            RemoteDataFetcher.Instance.FetchJSONFile(_chosenLesson + Constants.LESSON_FILE_PATH_SUFIX, (isSuccessful, JSON) => 
            {
                string message = string.Empty;
                if (isSuccessful)
                {
                    try
                    {
                        _lesson = JsonUtility.FromJson<LessonData>(JSON);
                        if (_lesson.LessonElements.Length > 0)
                        {
                            _lastLesson = _chosenLesson;
                            InitializeLessonView();
                            GoToLessonView();
                            return;
                        }
                        message = "LEKCIJA NEMA PODATAKA";
                    }
                    catch
                    {
                        message = "LEKCIJU NIJE MOGUÆE UÈITATI";
                    }
                }
                else message = "NEUSPJEŠNO POVEZIVANJE NA POSLUŽITELJ";

                GoToError(message, () =>
                {
                    _raycastBlocker.SetActive(true);
                    _errorContainer.Slide(ContainerPosition.OFF_SCREEN_LEFT);
                    _lessonPickerContainer.Slide(ContainerPosition.ON_SCREEN, () => _raycastBlocker.SetActive(false));
                    _activeContainer = ActiveContainer.LESSON_PICKER_CONTAINER;
                });
            });
        }

        private void InitializeLessonView()
        {
            _lessonNameText.text = _chosenLesson;
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

        private void GoToLessonView()
        {
            _raycastBlocker.SetActive(true);
            _lessonPickerContainer.Slide(ContainerPosition.OFF_SCREEN_RIGHT);
            _lessonViewContainer.Slide(ContainerPosition.ON_SCREEN, () => _raycastBlocker.SetActive(false));
            _activeContainer = ActiveContainer.LESSON_VIEW_CONTAINER;
        }

        private void GoToError(string errorMessage, Action onErrorBackButtonPressed)
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