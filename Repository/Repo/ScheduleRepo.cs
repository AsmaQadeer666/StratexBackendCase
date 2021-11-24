using stratex.Schedule.Entity;
using stratex.Schedule.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace stratex.Schedule.Repository.Repo
{
    public class ScheduleRepo
    {
        private readonly string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["backend_connectionstring"].ConnectionString;

        public List<ScheduleModel> GetActivities(DateTime date)
        {
            List<ScheduleModel> scheduleList = new List<ScheduleModel>();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("[dbo].[GetActivities]", sqlConnection))
            {
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@Date", SqlDbType.DateTime2));
                sqlDataAdapter.SelectCommand.Parameters["@Date"].Value = date;

                DataTable dt = new DataTable();

                sqlDataAdapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    scheduleList = (from DataRow dr in dt.Rows
                                    select new ScheduleModel()
                                    {
                                        Priority = (int)(dr["Priority"]),
                                        Name = dr["Name"].ToString(),
                                        Employee = dr["Employee"].ToString(),
                                        StartDateTime = DateTime.Parse(dr["StartDateTime"].ToString()),
                                        EndDateTime = DateTime.Parse(dr["EndDateTime"].ToString())
                                    }).ToList();
                }
                return Adjust_Overlapping(scheduleList);
            }
        }

        private List<ScheduleModel> Adjust_Overlapping(List<ScheduleModel> scheduleList)
        {
            List<ScheduleModel> listSchedule = new List<ScheduleModel>();

            var listLeaves = scheduleList.Where(p => p.Priority == 1);
            var listBreaks = scheduleList.Where(p => p.Priority == 2);
            var listShift = scheduleList.Where(p => p.Priority == 3);

            bool leaveFlag, breakFlag = true;

            //Get Employee
            foreach (var emp in scheduleList.Select(p => new { p.Employee }).Distinct().ToList())
            {
                //Full Day Leave
                if (listShift.Count()==0 && listLeaves.Count()==1)
                {
                    foreach (var leave in listLeaves.Where(x => x.Employee == emp.Employee).ToList())
                    {
                        listSchedule.Add(leave);
                    }
                }
             
                //retrieve employee shift
                foreach (var shift in listShift.Where(x => x.Employee == emp.Employee).ToList())
                {
                    ScheduleModel scheduleModel = new ScheduleModel()
                    {
                        Name = shift.Name,
                        Employee = shift.Employee,
                        Priority = shift.Priority,
                        StartDateTime = shift.StartDateTime,
                        EndDateTime = shift.EndDateTime,
                    };

                    breakFlag = true;

                    //retrieve employee breaks and check break overlapping with shift
                    // If overlapping is found, then we split.
                    foreach (var breaks in listBreaks.Where(x => x.Employee == emp.Employee).ToList())
                    {
                        if (breakFlag)
                        {
                            scheduleModel.EndDateTime = breaks.StartDateTime;

                            var ScheduleModelOne = new ScheduleModel()
                            {
                                Name = shift.Name,
                                Employee = shift.Employee,
                                Priority = shift.Priority,
                                StartDateTime = scheduleModel.StartDateTime,
                                EndDateTime = scheduleModel.EndDateTime,
                            };

                            if ((ScheduleModelOne.StartDateTime >= shift.StartDateTime) &&
                                (scheduleModel.EndDateTime <= shift.EndDateTime))
                            {
                                listSchedule.Add(ScheduleModelOne);
                            }

                            breakFlag = false;
                        }

                        if (scheduleModel.EndDateTime == breaks.StartDateTime)
                        {
                            if ((breaks.StartDateTime >= shift.StartDateTime) &&
                                (breaks.EndDateTime <= shift.EndDateTime))
                            {
                                listSchedule.Add(breaks);
                            }

                            scheduleModel.StartDateTime = breaks.StartDateTime;
                            scheduleModel.EndDateTime = breaks.EndDateTime;
                        }

                        else
                        {
                            var scheduleModelOne = new ScheduleModel()
                            {
                                Name = shift.Name,
                                Employee = shift.Employee,
                                Priority = shift.Priority,
                                StartDateTime = scheduleModel.EndDateTime,
                                EndDateTime = breaks.StartDateTime,
                            };

                            if ((scheduleModelOne.StartDateTime >= shift.StartDateTime) &&
                                (scheduleModelOne.EndDateTime <= shift.EndDateTime))
                            {
                                listSchedule.Add(scheduleModelOne);
                            }

                            scheduleModel.StartDateTime = breaks.StartDateTime;
                            scheduleModel.EndDateTime = breaks.EndDateTime;

                            if ((breaks.StartDateTime >= shift.StartDateTime) &&
                                (breaks.EndDateTime <= shift.EndDateTime))
                            {
                                listSchedule.Add(breaks);
                            }
                        }
                    }

                    if (listLeaves.Where(x => x.Employee == emp.Employee).ToList().Count == 0)
                    {
                        var scheduleModelOne = new ScheduleModel()
                        {
                            Name = shift.Name,
                            Employee = shift.Employee,
                            Priority = shift.Priority,
                            EndDateTime = shift.EndDateTime,
                            StartDateTime = scheduleModel.EndDateTime,
                        };

                        if ((scheduleModelOne.StartDateTime >= shift.StartDateTime) &&
                            (scheduleModelOne.EndDateTime <= shift.EndDateTime))
                        {
                            listSchedule.Add(scheduleModelOne);
                        }
                    }

                    leaveFlag = listBreaks.Where(x => x.Employee == emp.Employee).ToList().Count == 0;

                    //retrieve employee leaves and check leave overlapping with shift
                    // If overlapping is found, then we split.
                    foreach (var leaves in listLeaves.Where(x => x.Employee == emp.Employee).ToList())
                    {
                        if (leaveFlag)
                        {
                            scheduleModel.EndDateTime = leaves.StartDateTime;

                            var scheduleModelOne = new ScheduleModel()
                            {
                                Name = shift.Name,
                                Employee = shift.Employee,
                                Priority = shift.Priority,
                                StartDateTime = scheduleModel.StartDateTime,
                                EndDateTime = scheduleModel.EndDateTime,
                            };

                            if ((scheduleModelOne.StartDateTime >= shift.StartDateTime) &&
                                (scheduleModelOne.EndDateTime <= shift.EndDateTime))
                            {
                                listSchedule.Add(scheduleModelOne);
                            }

                            leaveFlag = false;
                        }

                        if (scheduleModel.EndDateTime == leaves.StartDateTime)
                        {
                            if ((leaves.StartDateTime >= shift.StartDateTime) &&
                                (leaves.EndDateTime <= shift.EndDateTime))
                            {
                                listSchedule.Add(leaves);
                            }

                            scheduleModel.StartDateTime = leaves.StartDateTime;
                            scheduleModel.EndDateTime = leaves.EndDateTime;
                        }

                        else
                        {
                            var ScheduleModelOne = new ScheduleModel()
                            {
                                Name = shift.Name,
                                Employee = shift.Employee,
                                Priority = shift.Priority,
                                StartDateTime = scheduleModel.EndDateTime,
                                EndDateTime = leaves.StartDateTime,
                            };

                            if ((ScheduleModelOne.StartDateTime >= shift.StartDateTime) &&
                                (ScheduleModelOne.EndDateTime <= shift.EndDateTime))
                            {
                                listSchedule.Add(ScheduleModelOne);
                            }

                            scheduleModel.StartDateTime = leaves.StartDateTime;
                            scheduleModel.EndDateTime = leaves.EndDateTime;

                            if ((leaves.StartDateTime >= shift.StartDateTime) &&
                                (leaves.EndDateTime <= shift.EndDateTime))
                            {
                                listSchedule.Add(leaves);
                            }
                        }
                    }
                }
            }
            return listSchedule;
        }
    }
}