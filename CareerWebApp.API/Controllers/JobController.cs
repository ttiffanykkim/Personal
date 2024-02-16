using CareerWebApp.API.Services;
using CareerWebApp.Models.Job;
using Microsoft.AspNetCore.Mvc;

namespace CareerWeb.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobController : ControllerBase
    {
        private readonly JobServices _jobService;
        private readonly EmailService _emailService;

        public JobController(JobServices jobService, EmailService emailService)
        {
            _jobService = jobService;
            _emailService = emailService;
        }

        [HttpPost]
        [Route("jobs")]
        public async Task<IActionResult> GetJobList(JobSearchCondition searchCondition = null)
        {
            var result = await _jobService.GetJobList(searchCondition);
            return Ok(result);

        }

        [HttpGet]
        [Route("job/{id}")]
        public async Task<IActionResult> GetJobByID(int id)
        {
            var result = await _jobService.GetJobByID(id);
            return Ok(result);
        }

        [HttpPost]
        [Route("job")]
        public async Task<IActionResult> SubmitResume(ApplicationInfo applicationInfo)
        {
            var job = await _jobService.GetJobByID(applicationInfo.JobID);
            var submittedQuestionnaireAnswers = await _jobService.GetQuestionAnswers(applicationInfo.QuestionAnswers);
            await _emailService.SendEmail(job, applicationInfo, submittedQuestionnaireAnswers);
            return Ok();
        }
    }
}