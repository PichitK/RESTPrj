using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OrgCommunication.Tests.Controllers
{
    [TestClass]
    public class GroupControllerTest
    {
        public string Host { get; set; }

        public GroupControllerTest()
        {
            this.Host = System.Configuration.ConfigurationManager.AppSettings["Host"];
        }

        [TestMethod]
        public void AddGroupAPI()
        {
            using (var client = new HttpClient())
            {
                string accesstoken = "";
                client.BaseAddress = new Uri(this.Host);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
               
                using (HttpContent hc = new StringContent("{ UserName: \"inTren\", Password: \"hello\"}"))
                {
                    hc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    using (HttpResponseMessage response = client.PostAsync("api/member/signin", hc).Result)
                    {
                        string message = response.Content.ReadAsStringAsync().Result;
                        OrgCommunication.Models.Member.ProfileResultModel result = Newtonsoft.Json.JsonConvert.DeserializeObject<OrgCommunication.Models.Member.ProfileResultModel>(message);

                        accesstoken = result.Member.AccessToken;
                    }
                }



                ////client.DefaultRequestHeaders.Accept.Clear();
                ////client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //string FirstName = "Pichit";

                using (var mdc = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(System.Globalization.CultureInfo.InvariantCulture)))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
                    //mdc.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    string title = "CS15";

                    string filepath = "D:\\me.jpg";
                    byte[] file = File.ReadAllBytes(filepath);
                    mdc.Add(new StreamContent(new MemoryStream(file)), "Logo", "me.jpg");
                    mdc.Add(new StringContent("15"), "MemberId");
                    mdc.Add(new StringContent(title), "Title");
                    mdc.Add(new StringContent("Com-Sci"), "SubTitle");

                    using (HttpResponseMessage response = client.PostAsync("api/group/addgroup", mdc).Result)
                    {
                        string message = response.Content.ReadAsStringAsync().Result;
                        OrgCommunication.Models.Group.GroupResultModel result = Newtonsoft.Json.JsonConvert.DeserializeObject<OrgCommunication.Models.Group.GroupResultModel>(message);

                        Assert.AreEqual(title, result.Group.Title);
                    }
                }
            }
        }
    }
}
