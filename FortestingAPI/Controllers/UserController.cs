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
        public object GenerateUser(JsonMember.UserDetails obj)
        {
            string Det = JsonConvert.SerializeObject(UserMaster.GenerateUser(obj), Formatting.Indented);
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
    }
}
