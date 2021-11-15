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
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public Guid TeacherId { get; set; }
        public ICollection<Score> Scores { get; set; }
    }
}
