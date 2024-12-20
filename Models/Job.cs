using System.ComponentModel.DataAnnotations;

namespace FPTJOB.Models
{
    public class Job
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Job Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Requirement")]
        public string Requirement { get; set; } = string.Empty;

        [Display(Name = "Industry")]
        public string Industry { get; set; } = string.Empty;

        [Display(Name = "Location")]
        public string Location { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Deadline")]
        public DateTime Deadline { get; set; }

        [Display(Name = "Salary")]
        [Range(0.01, 100000, ErrorMessage = "Salary must be between 0.01 and 100,000.")]
        public decimal Salary { get; set; } // Salary nằm trong khoảng từ 0.01 đến 100,000

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}
