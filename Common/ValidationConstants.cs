namespace SeminarHub.Common
{
    public static class ValidationConstants
    {
        // Seminar validation constants
        public const int SeminarTopicMinLenght = 3;
        public const int SeminarTopicMaxLenght = 100;
        public const int SeminarLecturerMinLenght = 5;
        public const int SeminarLecturerMaxLenght = 60;
        public const int SeminarDetailsMinLenght = 10;
        public const int SeminarDetailsMaxLenght = 500;
        public const int SeminarDurationMin = 30;
        public const int SeminarDurationMax = 180;
        public const string DateTimeFormat = "MMMM yyyy";

        // Category validation constants
        public const int CategoryNameMinLength = 3;
        public const int CategoryNameMaxLength = 50;
    }
}
