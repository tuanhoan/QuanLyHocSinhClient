using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyHocSinhClient.Models
{
    public class Subject
    {
        public Subject()
        {
            Scores = new HashSet<Score>();
            Teachers = new HashSet<Teacher>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
       
        public ICollection<Score> Scores { get; set; }
        public ICollection<Teacher> Teachers { get; set; }
    }
}
