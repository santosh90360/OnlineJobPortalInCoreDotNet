namespace JobSeeker.Models.Dto
{
    public class SkillDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }       
        public string Name { get; set; }   
        public string Description { get; set; }
        public string? Experience { get; set; }
        public string? Version { get; set; }
        public string? LastingWorking { get; set; }
        public DateTime? EntryDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public ResultStatus Result { get; set; }
        public List<SkillDto> Skills { get; set; }
    }
}
