namespace JobSeeker.Models.Dto
{
    public class RegistrationDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public Int64 Mobile { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string UserType { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public bool IsLocked { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string IPAddress { get; set; }
    }
}
