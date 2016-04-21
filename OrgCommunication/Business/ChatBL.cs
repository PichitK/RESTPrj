using OrgCommunication.Business.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Business
{
    public class ChatBL
    {
        public enum ParticipationType
        {
            Member = 1,
            Group
        }

        public ChatBL()
        {

        }

        public static string GetChatRoomId(int[] participatedMemberId, ParticipationType type)
        {
            if (!Enum.IsDefined(typeof(ParticipationType), type))
                throw new OrgException("Invalid participation type");
            
            int[] sortedId = participatedMemberId.OrderBy(r => r).ToArray();
            string chatroomId = ((int)type).ToString();

            foreach(int i in sortedId)
            {
                string memberId = i.ToString();

                chatroomId += memberId.Length.ToString() + memberId;
            }

            return chatroomId;
        }
    }
}