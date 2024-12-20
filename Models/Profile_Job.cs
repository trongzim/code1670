using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FPTJOB.Models
{
    public class Profile_Job
    {
        [Key]
        public int Id { get; set; }
        public DateTime RegDate { get; set; }

        public int JobId { get; set; }
        [ForeignKey("JobId")]
        public virtual Job? Job { get; set; }

        public int ProfileId { get; set; }
        [ForeignKey("ProfileId")]
        public virtual Profile? Profile { get; set; }
    }
}
