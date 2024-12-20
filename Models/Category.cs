using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FPTJOB.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool Status { get; set; }
        [InverseProperty("Category")]
        public virtual ICollection<Job>? Jobs { get; set;}
    }
}
