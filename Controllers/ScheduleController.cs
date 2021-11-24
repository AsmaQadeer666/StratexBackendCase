using stratex.Schedule.Entity;
using stratex.Schedule.Models;
using stratex.Schedule.Repository.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace stratex.Schedule.Controllers
{
    public class ScheduleController : ApiController
    {
        private readonly ScheduleRepo scheduleRepo = new ScheduleRepo();

        public List<ScheduleModel> Get(DateTime date)
        {
            return scheduleRepo.GetActivities(date);
        }
    }
}