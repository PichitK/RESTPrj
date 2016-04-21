using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Business.Configs
{
    public class AppConfigs
    {
        public static string OAuthAuthenticationType
        {
            get
            {
                string message = System.Configuration.ConfigurationManager.AppSettings["OAuth.AuthenticationType"];

                return message ?? "Application";
            }
        }

        public static int OAuthTimespanHour
        {
            get
            {
                int day = 0;

                if (Int32.TryParse(System.Configuration.ConfigurationManager.AppSettings["OAuth.TimespanHour"], out day))
                    return day;
                else
                    return 1;
            }
        }

        public static bool DebugInternalMessage
        {
            get
            {
                bool debugInternalMessage = false;

                if (Boolean.TryParse(System.Configuration.ConfigurationManager.AppSettings["Debug.InternalMessage"], out debugInternalMessage))
                    return debugInternalMessage;
                else
                    return false;
            }
        }

        public static string InternalErrorMessage
        {
            get
            {
                string message = System.Configuration.ConfigurationManager.AppSettings["InternalError.Message"];

                return message ?? "";
            }
        }

        public static string GeneralDateTimeFormat
        {
            get
            {
                string format = System.Configuration.ConfigurationManager.AppSettings["General.DateTime.Format"];

                return format ?? "dd-MM-yyyy HH:mm:ss";
            }
        }
        
        public static int? MemberPhotoWidthMax
        {
            get
            {
                int width = 0;

                if (Int32.TryParse(System.Configuration.ConfigurationManager.AppSettings["Member.Photo.Width.Max"], out width))
                    return width;
                else
                    return null;
            }
        }

        public static int? MemberPhotoHeightMax
        {
            get
            {
                int height = 0;

                if (Int32.TryParse(System.Configuration.ConfigurationManager.AppSettings["Member.Photo.Height.Max"], out height))
                    return height;
                else
                    return null;
            }
        }

        public static int? GroupPhotoWidthMax
        {
            get
            {
                int width = 0;

                if (Int32.TryParse(System.Configuration.ConfigurationManager.AppSettings["Group.Photo.Width.Max"], out width))
                    return width;
                else
                    return null;
            }
        }

        public static int? GroupPhotoHeightMax
        {
            get
            {
                int height = 0;

                if (Int32.TryParse(System.Configuration.ConfigurationManager.AppSettings["Group.Photo.Height.Max"], out height))
                    return height;
                else
                    return null;
            }
        }

        public static bool MailSendMail
        {
            get
            {
                bool sendMail = false;

                if (Boolean.TryParse(System.Configuration.ConfigurationManager.AppSettings["Mail.SendMail"], out sendMail))
                    return sendMail;
                else
                    return false;
            }
        }

        public static string MailHost
        {
            get
            {
                string host = System.Configuration.ConfigurationManager.AppSettings["Mail.Host"];

                return host ?? "";
            }
        }

        public static int MailPort
        {
            get
            {
                int port = 0;

                if (Int32.TryParse(System.Configuration.ConfigurationManager.AppSettings["Mail.Port"], out port))
                    return port;
                else
                    return 25;
            }
        }

        public static string MailUsername
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["Mail.Username"];
            }
        }

        public static string MailPassword
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["Mail.Password"];
            }
        }

        public static bool MailEnableSsl
        {
            get
            {
                bool mailEnableSsl = false;

                if (Boolean.TryParse(System.Configuration.ConfigurationManager.AppSettings["Mail.EnableSsl"], out mailEnableSsl))
                    return mailEnableSsl;
                else
                    return false;
            }
        }

        public static string MailFrom
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["Mail.From"] ?? "";
            }
        }

        public static string MailTemplateResetPassword
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["Mail.Template.ResetPassword"] ?? "";
            }
        }

        public static string MailTemplateChangeLoginId
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["Mail.Template.ChangeLoginId"] ?? "";
            }
        }

        public static string MailTemplateActivate
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["Mail.Template.Activate"] ?? "";
            }
        }
    }
}