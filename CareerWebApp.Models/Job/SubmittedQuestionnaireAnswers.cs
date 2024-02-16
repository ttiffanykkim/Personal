using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerWebApp.Models.Job
{
    public class SubmittedQuestionnaireAnswers
    {
        public int QuestionID { get; set; }
        public string Question { get; set; }
        public int AnswerID { get; set; }
        public string Answer { get; set; }
    }
}