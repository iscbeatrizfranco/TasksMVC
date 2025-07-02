using Microsoft.EntityFrameworkCore;

namespace TasksMVC.Entities
{
    public class Attachment
    {
        public Guid Id { get; set; }
        public int AssignmentId { get; set; }
        public Assignment Assignment { get; set; }

        [Unicode(false)]
        public string Url { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
