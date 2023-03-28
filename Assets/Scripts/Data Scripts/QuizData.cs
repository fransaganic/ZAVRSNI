using System;

namespace ARudzbenik.Data
{
    [Serializable]
    public class QuizData
    {
        public Lesson Lesson = Lesson.NO_LESSON;
        public QuestionData[] Questions = null;
    }
}