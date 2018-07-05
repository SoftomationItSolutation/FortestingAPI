using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
using System.Configuration;

namespace BAL
{
    public class JsonMember
    {
        DBManager Sqldbmanager = new DBManager(DataProvider.SqlServer, ConfigurationManager.ConnectionStrings["E_wallet"].ConnectionString);
        public class UserDetails
        {
            public Int64 Id { get; set; }
            public Int64 OTP { get; set; }
            public Int64 OTPId { get; set; }
            public string Name { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public string EmailId { get; set; }
            public string MobileNo { get; set; }
            public string Status { get; set; }
            public string LoginId { get; set; }
        }
        public class TranscationManagement
        {
            public Int64 UserId { get; set; }
            public Int64 PatnerUserId { get; set; }
            public Int64 TranscationSourceId { get; set; }
            public Decimal Amount { get; set; }
        }
        public class TranscationReturn
        {
            public string flag { get; set; }
            public string Message { get; set; }
        }
        public class registrationReturn {
            public string flag { get; set; }
            public string Message { get; set; }
            public string OTPId { get; set; }
            public string OTP { get; set; }
            public string UserId { get; set; }
        }
        public class LoginReturn
        {
            public string flag { get; set; }
            public string Message { get; set; }
            public string Name { get; set; }
            public string EmailId { get; set; }
            public string MobileNo { get; set; }
            public string UserId { get; set; }
            public string UserName { get; set; }
        }
    }
}