using CareerWebApp.Models.Job;
using Dapper;
using Npgsql;

namespace CareerWebApp.API.Services
{
    public class JobServices
    {
        private readonly string _connectionString;

        public JobServices(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("PostgreSQL");
        }

        public async Task<IEnumerable<JobSummary>> GetJobList(JobSearchCondition searchCondition)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                string sql = "SELECT id, jobtitle, department, joblocation, createdat FROM jobpostings";

                if (searchCondition != null)
                {
                    List<string> conditions = new List<string>();
                    DynamicParameters parameters = new DynamicParameters();
                    if (!string.IsNullOrWhiteSpace(searchCondition.JobTitle))
                    {
                        conditions.Add("jobtitle ILIKE @JobTitle");
                        parameters.Add("@JobTitle", "%" + searchCondition.JobTitle + "%");
                    }

                    if (!string.IsNullOrWhiteSpace(searchCondition.Department))
                    {
                        conditions.Add("department ILIKE @Department");
                        parameters.Add("@Department", "%" + searchCondition.Department + "%");
                    }

                    if (!string.IsNullOrWhiteSpace(searchCondition.JobLocation))
                    {
                        conditions.Add("joblocation ILIKE @JobLocation");
                        parameters.Add("@JobLocation", "%" + searchCondition.JobLocation + "%");
                    }

                    if (!string.IsNullOrWhiteSpace(searchCondition.AllKeyword))
                    {
                        conditions.Add("(jobtitle ILIKE @AllKeyword OR department ILIKE @AllKeyword OR joblocation ILIKE @AllKeyword)");
                        parameters.Add("@AllKeyword", "%" + searchCondition.AllKeyword + "%");
                    }

                    if (conditions.Any())
                    {
                        sql += " WHERE " + string.Join(" AND ", conditions);
                    }
                    return await connection.QueryAsync<JobSummary>(sql, parameters);
                }

                return await connection.QueryAsync<JobSummary>(sql);
            }
        }

        public async Task<JobPostingAPIReponse> GetJobByID(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var parameter = new { jobID = id };

                string sql = "SELECT * FROM jobpostings WHERE id = @jobID;";
                var jobPosting = connection.Query<JobPosting>(sql, parameter).SingleOrDefault();

                sql = "SELECT qq.* FROM jobpostings jp INNER JOIN questionnairequestions qq ON qq.id = ANY(jp.questionnaireid) WHERE jp.id = @jobID;";
                var questions = connection.Query<QuestionnaireQuestions>(sql, parameter).ToList();

                sql = "SELECT qa.* FROM jobpostings jp\r\n\tINNER JOIN questionnaireanswers qa ON qa.questionid = ANY(jp.questionnaireid) WHERE jp.id = @jobID;";
                var answers = await connection.QueryAsync<QuestionnaireAnswers>(sql, parameter);

                JobPostingAPIReponse jobDetailReponse = new JobPostingAPIReponse();
                jobDetailReponse.Detail = jobPosting;
                jobDetailReponse.Questions = questions;

                foreach (var question in jobDetailReponse.Questions)
                {
                    question.Answers = answers.Where(o => o.QuestionID == question.ID).ToList();
                }

                return jobDetailReponse;
            }
        }

        public async Task<IEnumerable<SubmittedQuestionnaireAnswers>> GetQuestionAnswers(Dictionary<int, int> questionAnswers)
        {
            var parameters = new DynamicParameters();

            //kvp: KeyValuePair<TKey, TValue> generated from Dictionary<int, int> questionAnswers
            //index: zero based index
            var pairs = questionAnswers.Select((kvp, index) =>
            {
                var paramName1 = $"questionId{index}";
                var paramName2 = $"answerId{index}";
                parameters.Add(paramName1, kvp.Key);
                parameters.Add(paramName2, kvp.Value);
                return $"(@{paramName1}, @{paramName2})";
            });

            string sql = $@"
SELECT q.id AS QuestionID, q.question AS Question, 
       a.id AS AnswerID, a.answers AS Answer
FROM questionnairequestions q
INNER JOIN questionnaireanswers a ON q.id = a.questionid
WHERE (q.id, a.id) IN (VALUES {string.Join(", ", pairs)})";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var result = await connection.QueryAsync<SubmittedQuestionnaireAnswers>(sql, parameters);
                return result;
            }
        }
    }
}