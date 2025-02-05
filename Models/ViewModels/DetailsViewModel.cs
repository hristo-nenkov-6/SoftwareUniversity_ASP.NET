namespace SeminarHub.Models.ViewModels
{
    public class DetailsViewModel
    {
        public int Id { get; set; }
        public string Topic { get; set; } = null!;
        public string Lecturer { get; set; } = null!;
        public DateTime DateAndTime { get; set; }
        public int Duration { get; set; }
        public string Category { get; set; } = null!;
        public string Organizer { get; set; } = null!;
        public string Details { get; set; } = null!;
    }
}
