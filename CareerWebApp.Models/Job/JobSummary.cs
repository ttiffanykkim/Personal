using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerWebApp.Models.Job
{
    public class JobSummary
    {
        public int ID { get; set; }
        public string JobTitle { get; set; }
        public string Department { get; set; }
        public string JobLocation { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}