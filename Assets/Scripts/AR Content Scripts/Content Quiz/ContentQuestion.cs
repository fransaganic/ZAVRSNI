using System;

namespace ARudzbenik.ARContent.ContentQuiz
{
    [Serializable]
    public class ContentQuestion
    {
        public string QuestionText = string.Empty;
        public ContentAnswer[] Answers = null;
    }
}