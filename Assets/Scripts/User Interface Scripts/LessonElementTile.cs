using ARudzbenik.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ARudzbenik.UserInterface
{
    public class LessonElementTile : MonoBehaviour
    {
        [SerializeField] private Image _image = null;
        [SerializeField] private TextMeshProUGUI _text = null;

        public void InitializeTile(LessonElementData lessonElement)
        {
            if (lessonElement.LessonImageFilePath == null) _image.gameObject.SetActive(false);
            else
            {
                Sprite sprite = Resources.Load<Sprite>(lessonElement.LessonImageFilePath);
                if (sprite == null) _image.gameObject.SetActive(false);
                else _image.sprite = sprite;
            }

            if (lessonElement.LessonText == null || lessonElement.LessonText.Length == 0) _text.gameObject.SetActive(false);
            else _text.text = lessonElement.LessonText; 
        }
    }
}