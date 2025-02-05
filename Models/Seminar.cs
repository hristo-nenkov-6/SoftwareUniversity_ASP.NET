using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using SeminarHub.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeminarHub.Models
{
    public class Seminar
    {
        [Key]
        public int Id { get; set; }
        [Required, Range(ValidationConstants.SeminarTopicMinLenght,
            ValidationConstants.SeminarTopicMaxLenght)]
        public string Topic { get; set; } = null!;

        [Required, Range(ValidationConstants.SeminarLecturerMinLenght,
            ValidationConstants.SeminarLecturerMaxLenght)]
        public string Lecturer { get; set; } = null!;

        [Required, Range(ValidationConstants.SeminarDetailsMinLenght,
            ValidationConstants.SeminarDetailsMaxLenght)]
        public string Details { get; set; } = null!;

        [Required]
        public string OrganiserId { get; set; } = null!;

        [Required]
        public IdentityUser Organiser { get; set; } = null!;

        [Required, DisplayFormat(DataFormatString = ValidationConstants.DateTimeFormat,
            ApplyFormatInEditMode = true)]
        public DateTime DateAndTime { get; set; }

        [Required, Range(ValidationConstants.SeminarDurationMin,
            ValidationConstants.SeminarDurationMin)]
        public int Duration { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;

        public ICollection<SeminarParticipant> SeminarsParticipants =
            new List<SeminarParticipant>();
    }
}
