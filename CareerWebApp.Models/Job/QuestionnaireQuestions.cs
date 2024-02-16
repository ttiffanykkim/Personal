using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CareerWebApp.Models.Job;

namespace CareerWebApp.Models.Job
{
    public class QuestionnaireQuestions
    {
        public int ID { get; set; }
        public string Question { get; set; }
        public List<QuestionnaireAnswers> Answers { get; set; }
    }
}