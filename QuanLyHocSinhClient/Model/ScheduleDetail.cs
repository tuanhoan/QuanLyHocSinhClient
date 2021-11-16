using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyHocSinhClient.Models
{
    public class ScheduleDetail
    {
        public int ScheduleId { get; set; }
        public int LessonId { get; set; }
        public int SubjectId { get; set; }
        public virtual Schedule ScheduleNavigation { get; set; }
        public virtual Lesson LessonNavigation { get; set; }

    }
}
