using CareerWebApp.Models.Job;

namespace CareerWebApp.Models.Job
{
    public class JobPostingAPIReponse
    {
        public JobPosting Detail { get; set; }
        public List<QuestionnaireQuestions> Questions { get; set; }
    }
}