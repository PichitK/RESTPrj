using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Business.Exception
{
    public sealed class OrgException : System.Exception
    {
        public int? Code { get; private set; }
        public OrgException(string message)
            : base(message)
        {

        }

        public OrgException(string message, System.Exception innerException)
            : base(message, innerException)
        {

        }

        public OrgException(int code, string message)
            : base(message)
        {
            this.Code = code;
        }

        public OrgException(int code, string message, System.Exception innerException)
            : base(message + " (" + code.ToString() + ")", innerException)
        {
            this.Code = code;
        }
    }
}