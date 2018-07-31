using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using BAL;
using FortestingAPI.Models;
using System.Data;
using System.Web.Http.Cors;
using System.Web;
using System.IO;
//using System.Web.Script.Serialization;
using System.Threading.Tasks;


namespace FortestingAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : ApiController
    {
        UserDetails UserMaster = new UserDetails();
        DataTable dt = new DataTable();

        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [HttpPost]
        public object LoginMaster(JsonMember.UserDetails obj)
        {
            string Det = JsonConvert.SerializeObject(UserMaster.LoginMaster(obj), Formatting.Indented);
            return Det.Replace("\r", "").Replace("\n", "");
        }

        [HttpPost]
        public object GetProfileByLogin(JsonMember.UserDetails obj)
        {
            string Det = JsonConvert.SerializeObject(UserMaster.GetProfileByLogin(obj), Formatting.Indented);
            return Det.Replace("\r", "").Replace("\n", "");
        }

        [HttpPost]
        public object GetAllProfileDetails(JsonMember.UserDetails obj)
        {
            string Det = JsonConvert.SerializeObject(UserMaster.GetAllProfileDetails(obj), Formatting.Indented);
            return Det.Replace("\r", "").Replace("\n", "");
        }

        [HttpPost]
        public object GenerateUser(JsonMember.UserDetails obj)
        {
            string Det = JsonConvert.SerializeObject(UserMaster.GenerateUser(obj), Formatting.Indented);
            return Det.Replace("\r", "").Replace("\n", "");
        }

        [HttpPost]
        public object UpdatePersonalDetails()
        {
            bool flag = false;
            string Det;
            JsonMember.UserDetails obj = new JsonMember.UserDetails();
            JsonMember.LoginReturn Lobj = new JsonMember.LoginReturn();
            try
            {
                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
                    {
                        var hpf = HttpContext.Current.Request.Files[i];
                        if (hpf != null)
                        {
                            obj.LoginId = HttpContext.Current.Request.Form.Get("LoginId");
                            obj.EmailId = HttpContext.Current.Request.Form.Get("EmailId");
                            obj.FirstName = HttpContext.Current.Request.Form.Get("FirstName");
                            obj.LastName = HttpContext.Current.Request.Form.Get("LastName");
                            obj.MobileNo = HttpContext.Current.Request.Form.Get("MobileNo");
                            obj.UserId = Int64.Parse(HttpContext.Current.Request.Form.Get("UserId"));
                            string rootPath = System.Web.Hosting.HostingEnvironment.MapPath("~/UserProfilePictures/");
                            string fileName = Path.GetFileName(hpf.FileName);
                            fileName = obj.LoginId + ".png";
                            string fpath = rootPath + fileName;
                            hpf.SaveAs(fpath);
                            flag = true;
                            obj.ProfilePicPath = "/UserProfilePictures/" + fileName;
                        }
                        else
                        {
                            Lobj = new JsonMember.LoginReturn()
                            {
                                flag = "false",
                                Message = "Unable to process",
                                EmailId = "",
                                MobileNo = "",
                                Name = "",
                                UserId = "",
                                FirstName = "",
                                LastName = "",
                                ProfilePicPath = "",
                            };

                        }

                    }
                }
                else
                {
                    obj.LoginId = HttpContext.Current.Request.Form.Get("LoginId");
                    obj.EmailId = HttpContext.Current.Request.Form.Get("EmailId");
                    obj.FirstName = HttpContext.Current.Request.Form.Get("FirstName");
                    obj.LastName = HttpContext.Current.Request.Form.Get("LastName");
                    obj.MobileNo = HttpContext.Current.Request.Form.Get("MobileNo");
                    obj.UserId = Int64.Parse(HttpContext.Current.Request.Form.Get("UserId"));
                    obj.ProfilePicPath = "";
                    flag = true;
                }
            }
            catch (Exception ex)
            {

                Lobj = new JsonMember.LoginReturn()
                {
                    flag = "false",
                    Message = ex.Message.ToString(),
                    EmailId = "",
                    MobileNo = "",
                    Name = "",
                    UserId = "",
                    FirstName = "",
                    LastName = "",
                    ProfilePicPath = "",
                };
            }
            if (flag)
                Det = JsonConvert.SerializeObject(UserMaster.UpdatePersonalDetails(obj), Formatting.Indented);
            else
                Det = JsonConvert.SerializeObject(Lobj, Formatting.Indented);
            return Det.Replace("\r", "").Replace("\n", "");


        }

        [HttpPost]
        public string ValidateOTP(JsonMember.UserDetails obj)
        {
            string Det = JsonConvert.SerializeObject(UserMaster.ValidateOTP(obj), Formatting.Indented);
            return Det.Replace("\r", "").Replace("\n", "");
        }

        [HttpPost]
        public string ResendOTP(JsonMember.UserDetails obj)
        {
            string Det = JsonConvert.SerializeObject(UserMaster.ResendOTP(obj), Formatting.Indented);
            return Det.Replace("\r", "").Replace("\n", "");
        }

        [HttpPost]
        public string ForgetPassword(JsonMember.UserDetails obj)
        {
            string Det = JsonConvert.SerializeObject(UserMaster.ForgetPassword(obj), Formatting.Indented);
            return Det.Replace("\r", "").Replace("\n", "");
        }

        [HttpPost]
        public string ChangePassword(JsonMember.UserDetails obj)
        {
            string Det = JsonConvert.SerializeObject(UserMaster.ChangePassword(obj), Formatting.Indented);
            return Det.Replace("\r", "").Replace("\n", "");
        }

        [HttpPost]
        public string TranscationManagement(JsonMember.TranscationManagement obj)
        {
            string Det = JsonConvert.SerializeObject(UserMaster.TranscationManagement(obj), Formatting.Indented);
            return Det.Replace("\r", "").Replace("\n", "");
        }

        [HttpPost]
        public string GetTranscationManagement(JsonMember.TranscationManagement obj)
        {
            string Det = JsonConvert.SerializeObject(UserMaster.GetTranscationManagement(obj), Formatting.Indented);
            return Det.Replace("\r", "").Replace("\n", "");
        }

        [HttpPost]
        public string GetAvailableBalance(JsonMember.TranscationManagement obj)
        {
            string Det = JsonConvert.SerializeObject(UserMaster.GetAvailableBalance(obj), Formatting.Indented);
            return Det.Replace("\r", "").Replace("\n", "");
        }

        [HttpPost]
        public string UpdateTranscationStatus(JsonMember.TranscationManagement obj)
        {
            string Det = JsonConvert.SerializeObject(UserMaster.UpdateTranscationStatus(obj), Formatting.Indented);
            return Det.Replace("\r", "").Replace("\n", "");
        }
        [HttpPost]
        public object GetTranscationDetails(JsonMember.TranscationManagement obj)
        {
            string Det = JsonConvert.SerializeObject(UserMaster.GetTranscationDetails(obj), Formatting.Indented);
            return Det.Replace("\r", "").Replace("\n", "");
        }

        [HttpPost]
        public object GetNotification(JsonMember.TranscationManagement obj)
        {
            string Det = JsonConvert.SerializeObject(UserMaster.GetNotification(obj), Formatting.Indented);
            return Det.Replace("\r", "").Replace("\n", "");
        }

        [HttpPost]
        public object RewardManagementInsertUpdate(JsonMember.RewardManagement obj)
        {
            string Det = JsonConvert.SerializeObject(UserMaster.RewardManagementInsertUpdate(obj), Formatting.Indented);
            return Det.Replace("\r", "").Replace("\n", "");
        }

        [HttpPost]
        public object GetRewardManagement(JsonMember.RewardManagement obj)
        {
            string Det = JsonConvert.SerializeObject(UserMaster.GetRewardManagement(obj), Formatting.Indented);
            return Det.Replace("\r", "").Replace("\n", "");
        }

        [HttpPost]
        public object ValidateRewardCode(JsonMember.RewardManagement obj)
        {
            string Det = JsonConvert.SerializeObject(UserMaster.ValidateRewardCode(obj), Formatting.Indented);
            return Det.Replace("\r", "").Replace("\n", "");
        }

        [HttpPost]
        public object RequestMoney(JsonMember.RequestMoney obj)
        {
            string Det = JsonConvert.SerializeObject(UserMaster.RequestMoney(obj), Formatting.Indented);
            return Det.Replace("\r", "").Replace("\n", "");
        }



    }
}
