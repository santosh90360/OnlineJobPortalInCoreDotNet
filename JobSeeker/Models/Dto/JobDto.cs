using System.ComponentModel.DataAnnotations;

namespace JobSeeker.Models.Dto
{
    public class JobDto
    {       
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }
        [Required]
        public string Location { get; set; }
        [Display(Name = "Job Type")]
        public string JobType { get; set; }
        public float? Salary { get; set; }
        public string? Experience { get; set; }
        public string Category { get; set; }
        [Display(Name = "Job Tags")]
        public string? JobTags { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ClosingDate { get; set; }
        public DateTime? ExtendedDate { get; set; }
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        public string? AboutCompany { get; set; }
        public string Website { get; set; }
        [Display(Name = "Job Description")]
        public string JobDescription { get; set; }
        public string? TwitterUsername { get; set; }
        public string? Logo { get; set; }        
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }        
        public DateTime EntryDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string? IPAddress { get; set; }
        public ResultStatus Result { get; set; }
        public List<JobDto> JobList { get; set; }

    }
}
