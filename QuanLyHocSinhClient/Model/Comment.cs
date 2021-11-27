using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLyHocSinhClient.Models;

namespace QuanLyHocSinhClient.Models
{
    public class Comment
    {
        public Guid StudentId { get; set; }
        public DateTime CreatedAt { get; set; }
        public  int NewsFeedId { get; set; }
        public  string Content { get; set; }
        public virtual Student StudentNavigation { get; set; }
        public virtual NewsFeed NewsFeedNavigation { get; set; } 
    }
}
