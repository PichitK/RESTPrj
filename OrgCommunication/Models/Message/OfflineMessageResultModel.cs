using System.Collections.Generic;

namespace OrgCommunication.Models.Message
{
    public class OfflineMessageResultModel : ResultModel
    {
        public IList<OfflineMessageModel> OfflineMessages { get; set; }
    }
}