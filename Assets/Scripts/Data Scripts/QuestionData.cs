using System;

namespace ARudzbenik.Data
{
    [Serializable]
    public class QuestionData
    {
        public string QuestionText = null;
        public AnswerData[] Answers = null;
    }
}