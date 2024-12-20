using System.ComponentModel.DataAnnotations.Schema;

namespace FPTJOB.Models
{
    public class Profile
    {
        public int Id { get; set; }
        public string UserID { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Skill { get; set; }
        public string Education { get; set; }
        public string MyFile { get; set; }

        [NotMapped]
        public IFormFile UploadFile { get; set; }

        [InverseProperty("Profile")]
        public virtual ICollection<Profile_Job>? Profile_Jobs { get; set; }
    }
}
