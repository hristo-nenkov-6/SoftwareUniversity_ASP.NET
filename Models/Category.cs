using System.ComponentModel.DataAnnotations;
using SeminarHub.Common;

namespace SeminarHub.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required, Range(ValidationConstants.CategoryNameMinLength,
            ValidationConstants.CategoryNameMaxLength)]
        public string Name { get; set; } = null!;
        public ICollection<Seminar> Seminars { get; set; } = 
            new List<Seminar>();
    }
}