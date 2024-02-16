using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerWebApp.Models.Job
{
    public class JobSearchCondition
    {
        public string JobTitle { get; set; }
        public string Department { get; set; }
        public string JobLocation { get; set; }
        public string AllKeyword { get; set; }
    }
}