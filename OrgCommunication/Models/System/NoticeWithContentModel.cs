using System.Collections.Generic;

namespace OrgCommunication.Models.System
{
    public class NoticeWithContentModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string CreatedDate { get; set; }
    }
}