using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyHocSinhClient.Models
{
    public class Teacher
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime? BirthDay { get; set; }
        public string Address { get; set; }
        public Boolean Sex { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Image { get; set; }
        public string CMND { get; set; }
        public string PlaceOfIssue { get; set; }
        public DateTime? DateOfIssue { get; set; }
        public string Nationality { get; set; }
        public string Folk { get; set; }
        public string Religion { get; set; }
        public int SubjectId { get; set; }
        public virtual Subject SubjectNavigation { get; set; }
        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<Homework> Homeworks { get; set; }
        public Teacher()
        {
            Classes = new HashSet<Class>();
            Homeworks = new HashSet<Homework>();
        }
    }
}
