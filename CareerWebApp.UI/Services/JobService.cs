using CareerWebApp.Models.Job;
using System.Net.Http.Json;

namespace CareerWebApp.UI.Services
{
    public class JobService
    {
        private readonly HttpClient _httpClient;

        public JobService(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }

        public async Task<List<JobSummary>> GetJobList(JobSearchCondition searchCondition = null)
        {
            JsonContent? jsonContent = searchCondition == null ? null : JsonContent.Create(searchCondition);
            var response = await _httpClient.PostAsync("Job/jobs", jsonContent);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<JobSummary>>();
        }

        public async Task<JobPostingAPIReponse> GetJobPosting(int id)
        {
            var response = await _httpClient.GetAsync($"Job/job/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<JobPostingAPIReponse>();
        }

        public async Task SubmitResume(ApplicationInfo applicationInfo)
        {
            var response = await _httpClient.PostAsJsonAsync($"Job/job", applicationInfo);
        }
    }
}