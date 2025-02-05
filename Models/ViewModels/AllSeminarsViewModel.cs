using System.Drawing;

namespace SeminarHub.Models.ViewModels
{
    public class AllSeminarsViewModel
    {
        public int Id { get; set; }
        public string Topic { get; set; } = null!;
        public string Lecturer { get; set; } = null!;
        public DateTime DateAndTime { get; set; }
        public string Category { get; set; } = null!;
        public string Organizer { get; set; } = null!;
    }
}
