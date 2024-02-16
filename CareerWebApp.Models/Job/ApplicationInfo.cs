namespace CareerWebApp.Models.Job
{
    public class ApplicationInfo
    {
        public int JobID { get; set; }
        public Dictionary<int, int> QuestionAnswers { get; set; } = new Dictionary<int, int>();
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PreferredPhoneNumber { get; set; }
        public string SelectedState { get; set; }
        public byte[] ResumeFileContents { get; set; }
        public string ResumeFileName { get; set; }
        public byte[] OptionalFileContents { get; set; }
        public string OptionalFileName { get; set; }
    }
}