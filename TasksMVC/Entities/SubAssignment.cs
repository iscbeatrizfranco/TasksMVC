namespace TasksMVC.Entities
{
    public class SubAssignment
    {
        public Guid Id { get; set; }
        public int AssignmentId { get; set; }
        public Assignment Assignment { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }
        public int Order { get; set; }

    }
}
