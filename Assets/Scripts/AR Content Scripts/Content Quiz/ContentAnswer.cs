using System;

namespace ARudzbenik.ARContent.ContentQuiz
{
    [Serializable]
    public class ContentAnswer
    {
        public SelectableObject AnswerObject = null;
        public bool IsCorrect = false;
    }
}