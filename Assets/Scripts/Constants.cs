using ARudzbenik.Data;
using System.Collections.Generic;

namespace ARudzbenik.General
{
    public static class Constants
    {
        public const int MAIN_MENU_SCENE_BUILD_INDEX = 0;
        public const int AR_SCENE_BUILD_INDEX = 1;
        public const int LESSON_SCENE_BUILD_INDEX = 2;
        public const int QUIZ_SCENE_BUILD_INDEX = 3;

        public static readonly string LESSON_FILE_PATH_SUFIX = "_LESSON";
        public static readonly string QUIZ_FILE_PATH_SUFIX = "_QUIZ";
        public static readonly string QUIZ_BUTTON_END_QUIZ_TEXT = "ZAVRŠI KVIZ";
        public static readonly string QUIZ_BUTTON_NEXT_QUESTION_TEXT = "SLJEDEÆE PITANJE";

        private static Dictionary<Lesson, string> _lessonEnumToNameDictionary = new Dictionary<Lesson, string>()
        {
            { Lesson.SUSTAV_VIDEOIGARA_ZA_VISE_IGRACA, "Sustav videoigara za više igraèa" },
            { Lesson.OPTIMIZACIJA_I_PROGRAMIRANJE_VIDEOIGRE_ZA_VISE_IGRACA_NA_ISTOM_UREDAJU, "Optimizacija i programiranje videoigre za više igraèa na istom ureðaju" },
            { Lesson.OPTIMIZACIJA_I_PROGRAMIRANJE_VIDEOIGRE_ZA_VISE_IGRACA_PUTEM_LOKALNE_MREZE, "Optimizacija i programiranje videoigre za više igraèa putem lokalne mreže" },
            { Lesson.OPTIMIZACIJA_I_PROGRAMIRANJE_VIDEOIGRE_ZA_VISE_IGRACA_PUTEM_INTERNETA, "Optimizacija i programiranje videoigre za više igraèa putem Interneta" },
        };

        public static string GetLessonName(Lesson lesson)
        {
            if (_lessonEnumToNameDictionary.ContainsKey(lesson)) return _lessonEnumToNameDictionary[lesson];

            string name = string.Empty;
            foreach (string part in lesson.ToString().Split("_")) name += part.ToLower() + " ";
            return char.ToUpper(name[0]) + name[1..];
        }
    }
}