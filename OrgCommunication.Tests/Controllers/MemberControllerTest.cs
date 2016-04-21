using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;

namespace OrgCommunication.Tests.Controllers
{
    [TestClass]
    public class MemberControllerTest
    {
        public string Host { get; set; }

        public MemberControllerTest()
        {
            this.Host = System.Configuration.ConfigurationManager.AppSettings["Host"];
        }

        [TestMethod]
        public void Register()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.Host);
                //client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string FirstName = "Pichit";

                using (var mdc = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(System.Globalization.CultureInfo.InvariantCulture)))
                {
                    //string filepath = "D:\\member_photo.jpg";
                    //byte[] file = File.ReadAllBytes(filepath);
                    //mdc.Add(new StreamContent(new MemoryStream(file)), "Photo", "member_photo.jpg");
                    mdc.Add(new StringContent("TrenXIII"), "LoginId");
                    mdc.Add(new StringContent("hello"), "Password");
                    mdc.Add(new StringContent("hamlet1310@gmail.com"), "Email");
                    mdc.Add(new StringContent(FirstName), "FirstName");
                    mdc.Add(new StringContent("Kongchoke"), "LastName");
                    mdc.Add(new StringContent("Tern"), "NickName");
                    mdc.Add(new StringContent("M"), "Gender");
                    mdc.Add(new StringContent("71.2"), "Weight");
                    mdc.Add(new StringContent("172"), "Height");
                    mdc.Add(new StringContent("1"), "CompanyId");
                    mdc.Add(new StringContent("1"), "DepartmentId");
                    mdc.Add(new StringContent("3"), "PositionId");
                    mdc.Add(new StringContent("1868"), "EmployeeId");
                    mdc.Add(new StringContent("0899673230"), "Phone");

                    using (HttpResponseMessage response = client.PostAsync("member/register", mdc).Result)
                    {
                        string message = response.Content.ReadAsStringAsync().Result;
                        OrgCommunication.Models.Member.ProfileResultModel result = Newtonsoft.Json.JsonConvert.DeserializeObject<OrgCommunication.Models.Member.ProfileResultModel>(message);

                        Assert.AreEqual(FirstName, result.Member.FirstName);
                    }
                }
            }
        }

        [TestMethod]
        public void RegisterAPI()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.Host);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string FirstName = "Pichit";

                using (var mdc = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(System.Globalization.CultureInfo.InvariantCulture)))
                {
                    //mdc.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    string filepath = "D:\\me.jpg";
                    byte[] file = File.ReadAllBytes(filepath);
                    mdc.Add(new StreamContent(new MemoryStream(file)), "Photo", "me.jpg");
                    mdc.Add(new StringContent("inTren"), "LoginId");
                    mdc.Add(new StringContent("hello"), "Password");
                    mdc.Add(new StringContent("hamlet1310@hotmail.com"), "Email");
                    mdc.Add(new StringContent(FirstName), "FirstName");
                    mdc.Add(new StringContent("Kongchoke"), "LastName");
                    mdc.Add(new StringContent("Tern"), "NickName");
                    mdc.Add(new StringContent("M"), "Gender");
                    mdc.Add(new StringContent("71.2"), "Weight");
                    //mdc.Add(new StringContent("172"), "Height");
                    mdc.Add(new StringContent("15/413"), "Address");
                    mdc.Add(new StringContent("Bangkapi"), "City");
                    mdc.Add(new StringContent("10240"), "ZipCode");
                    mdc.Add(new StringContent("TH"), "CountryCode");
                    mdc.Add(new StringContent("0809671230"), "Phone");

                    using (HttpResponseMessage response = client.PostAsync("api/member/register", mdc).Result)
                    {
                        string message = response.Content.ReadAsStringAsync().Result;
                        OrgCommunication.Models.Member.ProfileResultModel result = Newtonsoft.Json.JsonConvert.DeserializeObject<OrgCommunication.Models.Member.ProfileResultModel>(message);

                        Assert.AreEqual(FirstName, result.Member.FirstName);
                    }
                }
            }
        }
    }
}
