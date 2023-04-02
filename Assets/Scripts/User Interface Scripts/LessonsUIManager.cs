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
        [SerializeField] private TextMeshProUGUI _lessonNameText = null;
        [SerializeField] private LessonElementTile _lessonElementTilePrefab = null;
        [SerializeField] private Transform _lessonElementContainer = null;
        [SerializeField] private GameObject _lessonUnavailableContainer = null;
        [SerializeField] private AnimatedButton _backButton = null;

        private Lesson _chosenLesson = Lesson.NO_LESSON;
        private bool _isInLessonPicker = true;
        private LessonData _lesson = null;
        private List<LessonElementTile> _lessonElementTiles = new List<LessonElementTile>();

        private void Start()
        {
            _menuButton.InitializeOnClick(() =>
            {
                SlidableContainer activeContainer = _isInLessonPicker ? _lessonPickerContainer : _lessonViewContainer;
                activeContainer.Slide(ContainerPosition.OFF_SCREEN_LEFT, () => SceneManager.LoadScene(Constants.MAIN_MENU_SCENE_BUILD_INDEX));
            });

            _lessonPickerContainer.Slide(ContainerPosition.OFF_SCREEN_LEFT, moveInstantly: true);
            _lessonViewContainer.Slide(ContainerPosition.OFF_SCREEN_LEFT, moveInstantly: true);

            _continueButton.InitializeOnClick(() => GoToLessonView());
            _backButton.InitializeOnClick(() => GoToLessonPicker());

            InitializeLessonPicker();
            GoToLessonPicker();
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
        }

        private void GoToLessonPicker()
        {
            _raycastBlocker.SetActive(true);
            _lessonPickerContainer.Slide(ContainerPosition.ON_SCREEN, () => _raycastBlocker.SetActive(false));

            if (!_isInLessonPicker) _lessonViewContainer.Slide(ContainerPosition.OFF_SCREEN_LEFT);
            else _lessonViewContainer.Slide(ContainerPosition.OFF_SCREEN_LEFT, moveInstantly: true);

            _isInLessonPicker = true;
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
                foreach (LessonElementTile lessonElementTile in _lessonElementTiles) lessonElementTile.gameObject.SetActive(false);
            }

            _lessonUnavailableContainer.SetActive(isLessonUnavailable);

            _raycastBlocker.SetActive(true);
            _lessonPickerContainer.Slide(ContainerPosition.OFF_SCREEN_RIGHT);
            _lessonViewContainer.Slide(ContainerPosition.ON_SCREEN, () => _raycastBlocker.SetActive(false));
            _isInLessonPicker = false;
        }
    }
}