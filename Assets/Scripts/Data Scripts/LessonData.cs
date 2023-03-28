using System;

namespace ARudzbenik.Data
{
    [Serializable]
    public class LessonData
    {
        public Lesson Lesson = Lesson.NO_LESSON;
        public LessonElementData[] LessonElements = null;
    }
}