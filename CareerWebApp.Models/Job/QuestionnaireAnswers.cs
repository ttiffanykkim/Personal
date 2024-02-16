using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerWebApp.Models.Job
{
    public class QuestionnaireAnswers
    {
        public int ID { get; set; }
        public int QuestionID { get; set; }
        public int QuestionOrder { get; set; }
        public string Answers { get; set; }
    }
}