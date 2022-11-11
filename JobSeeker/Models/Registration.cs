using System.ComponentModel.DataAnnotations;

namespace JobSeeker.Models
{
    public class Registration
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        public Int64? Mobile { get; set; }
        public string? Gender { get; set; }
        public string? DOB { get; set; }
        [Required]
        public string UserType { get; set; }
        public string? ProfileImage { get; set; }
        public string? MaritalStatus { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public bool IsLocked { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string? IPAddress { get; set; }

    }
}
