﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyHocSinhClient.Models
{
    public class Semester
    {
        public Semester()
        {
            Scores = new HashSet<Score>();
        }
        public int Id { get; set; }
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public ICollection<Score> Scores { get; set; }
    }
}
