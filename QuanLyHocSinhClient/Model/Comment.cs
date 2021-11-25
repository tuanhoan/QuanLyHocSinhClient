using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLyHocSinhClient.Models;

namespace QuanLyHocSinhClient.Model
{
    public class Comment
    {
        public Guid studentId { get; set; }
        public DateTime createdAt { get; set; }
        public  int newsFeedId { get; set; }
        public  string Content { get; set; }
        public virtual Student StudentNavigation { get; set; }
        public virtual NewsFeed NewsFeedNavigation { get; set; } 
    }
}
