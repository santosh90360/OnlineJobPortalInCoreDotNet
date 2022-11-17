using System.ComponentModel.DataAnnotations;

namespace JobSeeker.Models
{
    public class Job
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string JobTitle { get; set; }
        [Required]
        public string Location { get; set; }
        public string JobType { get; set; }
        public string Category { get; set; }
        public string? JobTags { get; set; }
        public float? Salary { get; set; }
        public string? Experience { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ClosingDate { get; set; }
        public DateTime? ExtendedDate { get; set; }
        public string CompanyName { get; set; }
        public string Website { get; set; }
        public string JobDescription { get; set; }
        public string? TwitterUsername { get; set; }
        public string? Logo { get; set; }        
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }        
        public DateTime EntryDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string? IPAddress { get; set; }

    }
}
