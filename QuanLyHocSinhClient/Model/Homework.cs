using QuanLyHocSinhClient.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyHocSinhClient.Models
{
    public class Homework
    {
        public Homework()
        {
            HomeworkSubmits = new HashSet<HomeworkSubmit>();
        }
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public string Image { get; set; }
        public DateTime CreateAt { get; set; }
        public Guid TeacherId { get; set; }
        public Teacher TeacherNavigation { get; set; }
        public ICollection<HomeworkSubmit> HomeworkSubmits { get; set; }
    }
}
