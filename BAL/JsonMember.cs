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
        public class UserDetails
        {
            public Int64 Id { get; set; }
            public Int64 OTP { get; set; }
            public Int64 OTPId { get; set; }
            public Int64 UserId { get; set; }
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
            public Int64 RequestId { get; set; }
            public Int64 RewardId { get; set; }
            public String MsgDescription { get; set; }
            public String TranscationId { get; set; }
            public String ProcessId { get; set; }
            public String ProcessStatus { get; set; }
            public String TranscationSource { get; set; }
            public Decimal Amount { get; set; }
            
            public List<TranscationDetails> lstTranscationDetails;
        }
      
        public class TranscationDetails
        {
            public Int64 UserId { get; set; }
            public Int64 TranscationSourceId { get; set; }
            public Int64 PartnerUserId { get; set; }
            public Int64 RewardId { get; set; }
            public string UserLoginId { get; set; }
            public string EmailId { get; set; }
            public string MobileNo { get; set; }
            public string TranscationId { get; set; }
            public Decimal Amount { get; set; }
            public Decimal AvailableBalance { get; set; }
            public string TranscationStatus { get; set; }
            public string TranscationSource { get; set; }
            public string TranscationDetail { get; set; }
            public string PartnerLoginId { get; set; }
            public string PartnerEmailId { get; set; }
            public string PartnerMobileNo { get; set; }
            public string Ldate { get; set; }
            public string LTime { get; set; }
            public string flag { get; set; }
            public string Message { get; set; }
        }

        public class TranscationReturn
        {
            public string flag { get; set; }
            public string Message { get; set; }
            public string TranscationId { get; set; }
            public Decimal AvailableBalance { get; set; }
            public Decimal RewardBalance { get; set; }
            public Decimal transferMoney { get; set; }
            public Decimal reciverMoney { get; set; }
            public Decimal lastmonth { get; set; }
            public Decimal lastmonthCredit { get; set; }
            public Decimal lastmonthDebit { get; set; }
        }

        public class registrationReturn
        {
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
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string ProfilePicPath { get; set; }
        }

        public class NotificationManagement
        {
            public Int64 NotificationCount { get; set; }
            public Int64 RequestMoneyNotificationCount { get; set; }
            public Decimal AvailableBalance { get; set; }
            public string flag { get; set; }
            public string Message { get; set; }
            public List<NotificationManagementDetails> lstNotificationManagementDetails;
            public List<MoneyRequestNotificationDetails> lstMoneyRequestNotificationDetails;
        }

        public class NotificationManagementDetails
        {
            public string TranscationId { get; set; }
            public string ShortMsg { get; set; }
            public string MsgDescription { get; set; }
            public string LdateTime { get; set; }
        }
        public class MoneyRequestNotificationDetails
        {
            public Int64 RequestId { get; set; }
            public Int64 PartnerId { get; set; }
            public Decimal Amount { get; set; }
            public string MobileNo { get; set; }
            public string UserName { get; set; }
            public string Ldate { get; set; }
            public string LTime { get; set; }
        }

        public class RewardManagement {
            public Int64 RewardId { get; set; }
            public Int64 ValidDay { get; set; }
            public Int64 UserId { get; set; }
            public Decimal RewardAmount { get; set; }
            public string flag { get; set; }
            public string Message { get; set; }
            public string RewardCode { get; set; }
            public List<RewardManagementDetails> lstRewardManagementDetails;
        }

        public class RewardManagementDetails
        {
            public Int64 RewardId { get; set; }
            public Decimal RewardAmount { get; set; }
            public string RewardCode { get; set; }
            public string ValidFrom { get; set; }
            public string ValidTill { get; set; }
        }

        public class RequestMoney
        {
            public Int64 RequestId { get; set; }
            public Int64 RequesterId { get; set; }
            public Int64 RequestToId { get; set; }
            public decimal Amount { get; set; }
            public string flag { get; set; }
            public string Message { get; set; }
        }

    }


}