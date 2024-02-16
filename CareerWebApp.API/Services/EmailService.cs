using CareerWebApp.Models.Job;
using System.Net.Mail;
using System.Text;
using System.Net;

namespace CareerWebApp.API.Services
{
    public class EmailService
    {
        private const string smtpHost = "email-smtp.us-east-1.amazonaws.com";
        private const string smtpUserName = "AKIAV7THUGJKA7IFWTKO";
        private const string smtpPassword = "BBjH6+OyTliKSFlZ7ivTXMbd0ekkEKD33WtiPxLIYLtI";
        private const string fromEmail = "ttiffanykkim@gmail.com";
        private const string toEmail = "ttiffanykkim@gmail.com";

        public async Task SendEmail(JobPostingAPIReponse job, ApplicationInfo applicationInfo, IEnumerable<SubmittedQuestionnaireAnswers> submittedQuestionnaireAnswers)
        {
            string emailBody = GenerateHtmlEmailBody(job, applicationInfo, submittedQuestionnaireAnswers);
            await SendEmail(fromEmail, toEmail, emailBody, job, applicationInfo);
        }

        private string GenerateHtmlEmailBody(JobPostingAPIReponse job, ApplicationInfo applicationInfo, IEnumerable<SubmittedQuestionnaireAnswers> submittedQuestionnaireAnswers)
        {
            var emailBody = new StringBuilder();

            // Start of HTML email
            emailBody.Append("<html><body style='font-family: Arial, sans-serif; color: #333;'>");
            emailBody.Append("<div style='text-align: center; max-width: 600px; margin: auto;'>");

            // Basic Job Information
            emailBody.Append($"<h1 style='color: #4A90E2;'>{job.Detail.JobTitle}</h1>");
            emailBody.Append($"<h3>{job.Detail.Department} <small>Posted at {job.Detail.CreatedAt.ToShortDateString()}</small></h3>");

            // Default Information
            emailBody.Append("<hr/>");
            emailBody.Append("<div style='text-align: left; max-width: 100%'>");
            emailBody.Append("<ul style='list-style-type: none; padding: 0;'>");
            emailBody.Append("<li><strong>Name:</strong>" + applicationInfo.FirstName + " " + applicationInfo.LastName + "</li>");
            emailBody.Append("<li><strong>Email:</strong> " + applicationInfo.Email + "</li>");
            emailBody.Append("<li><strong>Phone:</strong> " + applicationInfo.PreferredPhoneNumber + "</li>");
            emailBody.Append("<li><strong>State:</strong> " + applicationInfo.SelectedState + "</li>");
            emailBody.Append("</ul>");

            // Questionnaire Answers
            emailBody.Append("<hr/>");
            foreach (var answer in submittedQuestionnaireAnswers)
            {
                emailBody.Append("<div style='margin-bottom: 10px;'>");
                emailBody.Append("<p style='font-weight: bold;'>Q: " + answer.Question + "</p>");
                emailBody.Append("<p style='color: #555; pading-left:5px;'>A: " + answer.Answer + "</p>");
                emailBody.Append("</div>");
            }

            // Attachments
            emailBody.Append("<hr/>");
            if (applicationInfo.ResumeFileContents != null && applicationInfo.ResumeFileName != null)
            {
                emailBody.Append($"<p><strong>Attached Resume</strong> {applicationInfo.ResumeFileName}</p>");
            }
            if (applicationInfo.OptionalFileContents != null && applicationInfo.OptionalFileName != null)
            {
                emailBody.Append($"<p><strong>Attached Optional File</strong> {applicationInfo.OptionalFileName}</p>");
            }
            emailBody.Append("</div>");

            // End of HTML email
            emailBody.Append("</div></body></html>");

            return emailBody.ToString();
        }

        private async Task SendEmail(string fromEmail, string toEmail, string emailBody, JobPostingAPIReponse job, ApplicationInfo applicationInfo)
        {
            using (var mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(fromEmail);
                mailMessage.To.Add(toEmail);
                mailMessage.Subject = $"Application Submission for {job.Detail.JobTitle}";
                mailMessage.Body = emailBody;
                mailMessage.IsBodyHtml = true;

                MemoryStream resumeStream = null;
                MemoryStream optionalFileStream = null;

                try
                {
                    // Attach Resume File
                    if (applicationInfo.ResumeFileContents != null && applicationInfo.ResumeFileName != null)
                    {
                        resumeStream = new MemoryStream(applicationInfo.ResumeFileContents);
                        var resumeAttachment = new Attachment(resumeStream, applicationInfo.ResumeFileName);
                        mailMessage.Attachments.Add(resumeAttachment);
                    }

                    // Attach Optional File
                    if (applicationInfo.OptionalFileContents != null && applicationInfo.OptionalFileName != null)
                    {
                        optionalFileStream = new MemoryStream(applicationInfo.OptionalFileContents);
                        var optionalFileAttachment = new Attachment(optionalFileStream, applicationInfo.OptionalFileName);
                        mailMessage.Attachments.Add(optionalFileAttachment);
                    }

                    var smtpClient = new SmtpClient(smtpHost)
                    {
                        Port = 587,
                        Credentials = new NetworkCredential(smtpUserName, smtpPassword),
                        EnableSsl = true,
                    };

                    await smtpClient.SendMailAsync(mailMessage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    resumeStream?.Dispose();
                    optionalFileStream?.Dispose();
                }
            }
        }

    }
}