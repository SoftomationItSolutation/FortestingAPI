using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using BAL;
//using FortestingAPI.Models;
using System.Data;
using System.Web.Http.Cors;
using System.Web;
using System.IO;
namespace FortestingAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AdminController : ApiController
    {
        AdminDetails AdminMster = new AdminDetails();

        [HttpPost]
        public object LoginMaster(JsonMember.UserDetails obj)
        {
            string Det = JsonConvert.SerializeObject(AdminMster.AdminLoginMaster(obj), Formatting.Indented);
            return Det.Replace("\r", "").Replace("\n", "");
        }
    }
}
