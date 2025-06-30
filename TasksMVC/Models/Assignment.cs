using System.ComponentModel.DataAnnotations;

namespace TasksMVC.Models
{
    public class Assignment
    {
        public int Id { get; set; }
        
        [StringLength(250)]
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public DateTime CreationDate { get; set; }
        public List<SubAssignment> SubAssignments { get; set; }
        public List<Attachment> AttachedFiles { get; set; }
    }
}
