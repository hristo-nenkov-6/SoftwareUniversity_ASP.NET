using SeminarHub.Common;
using System.ComponentModel.DataAnnotations;
using System.Configuration;


namespace SeminarHub.Models.ViewModels
{
    public class AddSeminarViewModel
    {
        public AddSeminarViewModel(List<Category> passedCategories) 
        {
            Categories = passedCategories;
            DateAndTime = DateTime.Now;
        }
        public AddSeminarViewModel() { }

        [Required(ErrorMessage="Topic is required!")]
        [StringLength(ValidationConstants.SeminarTopicMaxLenght, MinimumLength = ValidationConstants.SeminarTopicMinLenght)]
        public string Topic { get; set; } = null!;

        [Required(ErrorMessage = "Lecturer is required!")]
        [StringLength(ValidationConstants.SeminarLecturerMaxLenght, MinimumLength = ValidationConstants.SeminarLecturerMinLenght)]
        public string Lecturer { get; set; } = null!;

        [Required(ErrorMessage = "Details are required!")]
        [StringLength(ValidationConstants.SeminarDetailsMaxLenght, MinimumLength = ValidationConstants.SeminarDetailsMinLenght)]
        public string Details { get; set; } = null!;

        [Required]
        public DateTime DateAndTime { get; set; }

        [Required(ErrorMessage = "Duration is required!")]
        [Range(ValidationConstants.SeminarDurationMin, ValidationConstants.SeminarDurationMax)]
        public int Duration { get; set; }

        [Required(ErrorMessage = "Category is required!")]
        public int CategoryId { get; set; }
        public List<Category>? Categories { get; set; }
    }
}
