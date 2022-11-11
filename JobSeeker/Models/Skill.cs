using System.ComponentModel.DataAnnotations;

namespace JobSeeker.Models
{
    public class Skill
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        [Required]
        public string Name { get; set; }       
        public string? Description { get; set; }
        public string? Experience { get; set; }
        public string? Version { get; set; }
        public string? LastingWorking { get; set; }
        public DateTime? EntryDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

    }
}
