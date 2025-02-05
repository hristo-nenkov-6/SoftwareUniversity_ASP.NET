using SeminarHub.Common;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace SeminarHub.Models.ViewModels
{
    public class JoinSeminarViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(ValidationConstants.SeminarTopicMaxLenght, MinimumLength = ValidationConstants.SeminarTopicMinLenght)]
        public string Topic { get; set; } = null!;

        [Required]
        [StringLength(ValidationConstants.SeminarLecturerMaxLenght, MinimumLength = ValidationConstants.SeminarLecturerMinLenght)]
        public string Lecturer { get; set; } = null!;

        [Required]
        [StringLength(ValidationConstants.SeminarDetailsMaxLenght, MinimumLength = ValidationConstants.SeminarDetailsMinLenght)]
        public string Details { get; set; } = null!;

        [Required]
        public DateTime DateAndTime { get; set; }
        
        [Required]
        public int OrganiserId { get; set; }

        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }
    }
}
