using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace stratex.Schedule.Models
{
    public class ScheduleModel
    {
        public string Name { get; set; }
        public int Priority { get; set; }
        public string Employee { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}