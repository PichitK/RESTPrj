using System.Linq;
using OrgCommunication.Models.Message;
using System.Collections.Generic;
using OrgComm.Data;
using OrgCommunication.Business.Configs;
using OrgCommunication.Business.Exception;
using System;

namespace OrgCommunication.Business
{
    public class MessageBL
    {
        public MessageBL()
        {

        }

        public void AddOfflineMessage(MessageOfflineAddRequestModel model)
        {
            if (model == null)
                throw new OrgException("Invalid message data");

            if (!model.ToMemberId.HasValue)
                throw new OrgException("Require member id");

            if (String.IsNullOrWhiteSpace(model.Data))
                throw new OrgException("Require messaage data");

            if (!model.Type.HasValue)
                throw new OrgException("Require message type");

            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                if (!dbc.Members.Any(r => r.Id.Equals(model.ToMemberId.Value)))
                    throw new OrgException("Invalid member");

                OrgComm.Data.Models.OfflineMessage message = new OrgComm.Data.Models.OfflineMessage();

                message.MemberId = model.ToMemberId.Value;
                message.Type = model.Type.Value;
                message.GetFlag = false;
                message.Data = model.Data;
                message.CreatedDate = DateTime.Now;

                dbc.OfflineMessages.Add(message);

                dbc.SaveChanges();
            }
        }

        public void UpdateOfflineMessage(MessageOfflineUpdateRequestModel model)
        {
            if (model == null)
                throw new OrgException("Invalid message data");

            if (!model.messageId.HasValue)
                throw new OrgException("Require message id");

            if (String.IsNullOrWhiteSpace(model.Data) && !model.Type.HasValue)
                throw new OrgException("Require message data or message type");

            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                OrgComm.Data.Models.OfflineMessage message = dbc.OfflineMessages.SingleOrDefault(r => r.Id.Equals(model.messageId.Value));

                if (message == null)
                    throw new OrgException("Message not found");

                if (!String.IsNullOrWhiteSpace(model.Data))
                    message.Data = model.Data;

                if (model.Type.HasValue)
                    message.Type = model.Type.Value;

                dbc.SaveChanges();
            }
        }

        public OfflineMessageModel GetOfflineMessageById(int messageId)
        {
            OrgComm.Data.Models.OfflineMessage message = null;

            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                message = dbc.OfflineMessages.SingleOrDefault(r => r.Id.Equals(messageId));

                if (message == null)
                    throw new OrgException("Message not found");
            }

            return new OfflineMessageModel
            {
                Id = message.Id,
                Data = message.Data,
                Type = message.Type,
                CreatedDate = message.CreatedDate.ToString(AppConfigs.GeneralDateTimeFormat),
                Ticks = message.CreatedDate.Ticks
            };
        }

        public IList<OfflineMessageModel> GetOfflineMessageByMemberId(int memberId)
        {
            List<OfflineMessageModel> messageList = null;

            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                var messages = dbc.OfflineMessages.Where(r => r.MemberId.Equals(memberId) && !r.GetFlag).ToList();

                messages.ForEach(r => r.GetFlag = true);

                dbc.SaveChanges();

                messageList = messages.Select(r => new OfflineMessageModel
                {
                    Id = r.Id,
                    Data = r.Data,
                    Type = r.Type,
                    CreatedDate = r.CreatedDate.ToString(AppConfigs.GeneralDateTimeFormat),
                    Ticks = r.CreatedDate.Ticks
                }).ToList();
            }

            return messageList;
        }
    }
}