﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyHocSinhClient.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        public string TimeStart { get; set; }
        public virtual ICollection<ScheduleDetail> ScheduleDetails { get; set; }
        public Lesson()
        {
            ScheduleDetails = new HashSet<ScheduleDetail>();
        }
    }
}
