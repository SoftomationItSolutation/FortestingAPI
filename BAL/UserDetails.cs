using DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;

using static BAL.JsonMember;

namespace BAL
{
    public class UserDetails
    {

        DBManager Sqldbmanager = new DBManager(DataProvider.SqlServer, ConfigurationManager.ConnectionStrings["E_wallet"].ConnectionString);
        DataSet DS = new DataSet();
        DataTable dt = new DataTable();
        Email objEMail = new Email();
        
        IDataReader idr;

        public DataSet LogError(string ModuleName, string ErrorSource, string Description)
        {
            try
            {
                Sqldbmanager.Open();
                Sqldbmanager.CreateParameters(3);
                Sqldbmanager.AddParameters(0, "@ModuleName", ModuleName.Trim());
                Sqldbmanager.AddParameters(1, "@ErroSource", ErrorSource.Trim());
                Sqldbmanager.AddParameters(2, "@Description", Description.Trim());
                DS = Sqldbmanager.ExecuteDataSet(CommandType.StoredProcedure, "usp_LogError");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Sqldbmanager.Close();
            }
            return DS;
        }

        public Object LoginMaster(JsonMember.UserDetails obj)
        {
            LoginReturn Lobj = new LoginReturn();
            DataTable Logindt = new DataTable();
            try
            {
                Sqldbmanager.Open();
                Sqldbmanager.CreateParameters(1);
                Sqldbmanager.AddParameters(0, "@LoginId", obj.LoginId);
                DS = Sqldbmanager.ExecuteDataSet(CommandType.StoredProcedure, "USP_LoginMaster");
                if (DS.Tables[1].Rows.Count > 0)
                {
                    if (DS.Tables[0].Rows[0]["UserPassword"].ToString() == obj.Password)
                    {
                        
                        Lobj = new LoginReturn()
                        {
                            flag = "true",
                            Message = "Success",
                            EmailId = DS.Tables[1].Rows[0]["EmailId"].ToString(),
                            MobileNo = DS.Tables[1].Rows[0]["MobileNo"].ToString(),
                            Name = DS.Tables[1].Rows[0]["Name"].ToString(),
                            UserName = DS.Tables[1].Rows[0]["UserName"].ToString(),
                            UserId = DS.Tables[1].Rows[0]["UserId"].ToString()
                        };
                        
                    }
                    else
                    {
                        Lobj = new LoginReturn()
                        {
                            flag = "false",
                            Message = "Invalid Password",
                            EmailId = "",
                            MobileNo = "",
                            Name = "",
                            UserId=""
                        };
                        
                    }
                }
                else
                {
                    Lobj = new LoginReturn()
                    {
                        flag = "false",
                        Message = "User not register with us.",
                        EmailId = "",
                        MobileNo = "",
                        Name = "",
                        UserId = ""
                    };
                    
                }
            }
            catch (Exception Ex)
            {
                Lobj = new LoginReturn()
                {
                    flag = "false",
                    Message = "Invalid Email/Mobile",
                    EmailId = "",
                    MobileNo = "",
                    Name = "",
                    UserId = ""
                };
                
            }
            finally
            {
                Sqldbmanager.Close();
            }
            return Lobj;
        }

        public object GenerateUser(JsonMember.UserDetails obj)
        {
            registrationReturn obj1 = new registrationReturn();
            try
            {
                Sqldbmanager.Open();
                Sqldbmanager.CreateParameters(5);
                Sqldbmanager.AddParameters(0, "@Name", obj.Name);
                Sqldbmanager.AddParameters(1, "@UserName", obj.UserName);
                Sqldbmanager.AddParameters(2, "@UserPassword", obj.Password);
                Sqldbmanager.AddParameters(3, "@EmailId", obj.EmailId);
                Sqldbmanager.AddParameters(4, "@MobileNo", obj.MobileNo);
                DS = Sqldbmanager.ExecuteDataSet(CommandType.StoredProcedure, "USP_GenerateUser");
                if (Convert.ToBoolean(DS.Tables[0].Rows[0]["flag"]) == true)
                {
                    objEMail.SendSMSbyTillio(obj.MobileNo, "Flipprr Verification Code " + DS.Tables[0].Rows[0]["OTP"].ToString());
                    //Thread thrdSms = new Thread(() => SmsResult = (new Email()).SendSMS(obj.MobileNo, DS.Tables[0].Rows[0]["OTP"].ToString() + " is your flipprr verification code."));
                    //thrdSms.Start();

                    //Thread thrdMail = new Thread(() => MailResult = (new Email()).sendMail(obj.EmailId, "", "Flipprr Verification Code", DS.Tables[0].Rows[0]["OTP"].ToString(), ""));
                    //thrdMail.Start();

                }
                obj1 = new registrationReturn()
                {
                    flag = DS.Tables[0].Rows[0]["flag"].ToString(),
                    Message = DS.Tables[0].Rows[0]["Message"].ToString(),
                    OTPId = DS.Tables[0].Rows[0]["OTPId"].ToString(),
                    OTP = DS.Tables[0].Rows[0]["OTP"].ToString(),
                    UserId = DS.Tables[0].Rows[0]["UserId"].ToString()
                };
            }
            catch (Exception Ex)
            {
                DS = LogError("Generate User", Ex.Message.ToString(), "SP Name: USP_GenerateUser");
                obj1 = new registrationReturn()
                {
                    flag = "false",
                    Message = DS.Tables[0].Rows[0]["Meaasge"].ToString(),
                    OTPId = "",
                    OTP = "",
                    UserId = ""
                };
            }
            finally
            {
                Sqldbmanager.Close();
            }
           
            return obj1;
        }

        public object ValidateOTP(JsonMember.UserDetails obj)
        {
            registrationReturn obj1 = new registrationReturn();
            try
            {
                Sqldbmanager.Open();
                Sqldbmanager.CreateParameters(2);
                Sqldbmanager.AddParameters(0, "@OTPId", obj.OTPId);
                Sqldbmanager.AddParameters(1, "@OTP", obj.OTP);
                DS = Sqldbmanager.ExecuteDataSet(CommandType.StoredProcedure, "USP_ValidateOTP");
                obj1 = new registrationReturn()
                {
                    flag = DS.Tables[0].Rows[0]["flag"].ToString(),
                    Message = DS.Tables[0].Rows[0]["Message"].ToString(),
                    OTPId = DS.Tables[0].Rows[0]["OTPId"].ToString(),
                    OTP = DS.Tables[0].Rows[0]["OTP"].ToString(),
                    UserId = DS.Tables[0].Rows[0]["UserId"].ToString()
                };
            }
            catch (Exception Ex)
            {
                DS = LogError("Validate OTP", Ex.Message.ToString(), "SP Name: USP_ValidateOTP");
                obj1 = new registrationReturn()
                {
                    flag = "false",
                    Message = DS.Tables[0].Rows[0]["Meaasge"].ToString(),
                    OTPId = "",
                    OTP = "",
                    UserId = ""
                };
            }
            finally
            {
                Sqldbmanager.Close();
            }
           
            return obj1;
        }

        public object ResendOTP(JsonMember.UserDetails obj)
        {
            registrationReturn obj1 = new registrationReturn();
            try
            {
                Sqldbmanager.Open();
                Sqldbmanager.CreateParameters(1);
                Sqldbmanager.AddParameters(0, "@OTPId", obj.OTPId);
                DS = Sqldbmanager.ExecuteDataSet(CommandType.StoredProcedure, "USP_ResendOTP");
                if (Convert.ToBoolean(DS.Tables[0].Rows[0]["flag"]) == true)
                {
                    objEMail.SendSMSbyTillio(DS.Tables[0].Rows[0]["MobileNo"].ToString(), "Flipprr Verification Code " + DS.Tables[0].Rows[0]["OTP"].ToString());

                }
                obj1 = new registrationReturn()
                {
                    flag = "true",
                    Message = "",
                    OTPId = DS.Tables[0].Rows[0]["OTPId"].ToString(),
                    OTP = DS.Tables[0].Rows[0]["OTP"].ToString(),
                    UserId = DS.Tables[0].Rows[0]["UserId"].ToString()
                };
            }
            catch (Exception Ex)
            {
                DS = LogError("Resend OTP", Ex.Message.ToString(), "SP Name: USP_ResendOTP");
                obj1 = new registrationReturn()
                {
                    flag = "false",
                    Message = DS.Tables[0].Rows[0]["Meaasge"].ToString(),
                    OTPId = "",
                    OTP = "",
                    UserId = ""
                };
            }
            finally
            {
                Sqldbmanager.Close();
            }
           
            return obj1;
        }

        public object ForgetPassword(JsonMember.UserDetails obj)
        {
            registrationReturn obj1 = new registrationReturn();
            try
            {
                Sqldbmanager.Open();
                Sqldbmanager.CreateParameters(1);
                Sqldbmanager.AddParameters(0, "@LoginId", obj.LoginId);
                DS = Sqldbmanager.ExecuteDataSet(CommandType.StoredProcedure, "usp_ForgetPassword");
                if (Convert.ToBoolean(DS.Tables[0].Rows[0]["flag"]) == true)
                {
                    //objEMail.SendSMS(obj.MobileNo, DS.Tables[0].Rows[0]["MobileNo"].ToString() + " is your flipprr verification code.");
                    objEMail.SendSMSbyTillio(DS.Tables[0].Rows[0]["MobileNo"].ToString(), "Flipprr Verification Code " + DS.Tables[0].Rows[0]["OTP"].ToString());

                }
                obj1 = new registrationReturn()
                {
                    flag = DS.Tables[0].Rows[0]["flag"].ToString(),
                    Message = DS.Tables[0].Rows[0]["Message"].ToString(),
                    OTPId = DS.Tables[0].Rows[0]["OTPId"].ToString(),
                    OTP = DS.Tables[0].Rows[0]["OTP"].ToString(),
                    UserId = DS.Tables[0].Rows[0]["UserId"].ToString()
                };
            }
            catch (Exception Ex)
            {
                DS = LogError("Forget Password", Ex.Message.ToString(), "SP Name: usp_ForgetPassword");
                obj1 = new registrationReturn()
                {
                    flag = "false",
                    Message = DS.Tables[0].Rows[0]["Meaasge"].ToString(),
                    OTPId = "",
                    OTP = "",
                    UserId = ""
                };
            }
            finally
            {
                Sqldbmanager.Close();
            }
           
            return obj1;
        }

        public object ChangePassword(JsonMember.UserDetails obj)
        {
            registrationReturn obj1 = new registrationReturn();
            try
            {
                Sqldbmanager.Open();
                Sqldbmanager.CreateParameters(4);
                Sqldbmanager.AddParameters(0, "@UserId", obj.UserId);
                Sqldbmanager.AddParameters(1, "@OTPId", obj.OTPId);
                Sqldbmanager.AddParameters(2, "@OTP", obj.OTP);
                Sqldbmanager.AddParameters(3, "@UserPassword", obj.Password);
                DS = Sqldbmanager.ExecuteDataSet(CommandType.StoredProcedure, "USP_ChangePassword");
                obj1 = new registrationReturn()
                {
                    flag = DS.Tables[0].Rows[0]["flag"].ToString(),
                    Message = DS.Tables[0].Rows[0]["Message"].ToString(),
                    OTPId = "",
                    OTP = "",
                    UserId = DS.Tables[0].Rows[0]["UserId"].ToString()
                };
            }
            catch (Exception Ex)
            {
                DS = LogError("Change Password", Ex.Message.ToString(), "SP Name: USP_ChangePassword");
                obj1 = new registrationReturn()
                {
                    flag = "false",
                    Message = DS.Tables[0].Rows[0]["Meaasge"].ToString(),
                    OTPId = "",
                    OTP = "",
                    UserId = ""
                };
            }
            finally
            {
                Sqldbmanager.Close();
            }
            
            return obj1;
        }

        public object GetProfileByLogin(JsonMember.UserDetails obj)
        {
            List<JsonMember.LoginReturn> lstDetails = new List<JsonMember.LoginReturn>();

            try
            {
                Sqldbmanager.Open();
                Sqldbmanager.CreateParameters(1);
                Sqldbmanager.AddParameters(0, "@LoginId", obj.LoginId);
                idr = Sqldbmanager.ExecuteReader(CommandType.StoredProcedure, "USP_LoginMaster");
                if (idr.NextResult())
                {
                    while (idr.Read())
                    {
                        lstDetails.Add(new JsonMember.LoginReturn()
                        {
                            flag = "true",
                            Message = "Success",
                            EmailId = Convert.ToString(idr["EmailId"]),
                            MobileNo = Convert.ToString(idr["MobileNo"]),
                            Name = Convert.ToString(idr["Name"]),
                            UserName = Convert.ToString(idr["UserName"]),
                            UserId = Convert.ToString(idr["UserId"])
                        });
                    }
                }
                else
                {
                    lstDetails.Add(new JsonMember.LoginReturn()
                    {
                        flag = "false",
                        Message = "No User Found",
                        EmailId = "",
                        MobileNo = "",
                        Name = "",
                        UserId = ""
                    });

                }
            }
            catch (Exception Ex)
            {
                lstDetails.Add(new JsonMember.LoginReturn()
                {
                    flag = "false",
                    Message = Ex.Message.ToString(),
                    EmailId = "",
                    MobileNo = "",
                    Name = "",
                    UserId = ""
                });

            }
            finally
            {
                Sqldbmanager.Close();
            }
            return lstDetails;
        }

        public object GetAllProfileDetails(JsonMember.UserDetails obj)
        {
            
            List<JsonMember.LoginReturn> lstDetails = new List<JsonMember.LoginReturn>();
            try
            {
                Sqldbmanager.Open();
                Sqldbmanager.CreateParameters(1);
                Sqldbmanager.AddParameters(0, "@UserId", obj.UserId);
                idr = Sqldbmanager.ExecuteReader(CommandType.StoredProcedure, "USP_GetCompletePoflieDetails");
                if (idr.Read()) {
                    while (idr.Read())
                    {
                        lstDetails.Add(new JsonMember.LoginReturn()
                        {
                            flag = "true",
                            Message = "Success",
                            EmailId = Convert.ToString(idr["EmailId"]),
                            MobileNo = Convert.ToString(idr["MobileNo"]),
                            Name = Convert.ToString(idr["Name"]),
                            UserName = Convert.ToString(idr["UserName"]),
                            UserId = Convert.ToString(idr["UserId"])
                        });
                    }
                }
                else
                {
                    lstDetails.Add(new JsonMember.LoginReturn()
                    {
                        flag = "false",
                        Message = "No User Found",
                        EmailId = "",
                        MobileNo = "",
                        Name = "",
                        UserId = ""
                    });

                }
            }
            catch (Exception Ex)
            {
                lstDetails.Add(new JsonMember.LoginReturn()
                {
                    flag = "false",
                    Message = Ex.Message.ToString(),
                    EmailId = "",
                    MobileNo = "",
                    Name = "",
                    UserId = ""
                });

            }
            finally
            {
                Sqldbmanager.Close();
            }
            return lstDetails;
        }

        public object TranscationManagement(JsonMember.TranscationManagement obj)
        {
            TranscationReturn obj1 = new TranscationReturn();
            try
            {
                Sqldbmanager.Open();
                Sqldbmanager.CreateParameters(7);
                Sqldbmanager.AddParameters(0, "@UserId", obj.UserId);
                Sqldbmanager.AddParameters(1, "@TranscationSourceId", obj.TranscationSourceId);
                Sqldbmanager.AddParameters(2, "@Amount", obj.Amount);
                Sqldbmanager.AddParameters(3, "@PartnerUserId", obj.PatnerUserId);
                Sqldbmanager.AddParameters(4, "@MsgDescription", obj.MsgDescription);
                Sqldbmanager.AddParameters(5, "@RequestId", obj.RequestId);
                Sqldbmanager.AddParameters(6, "@RewardId", obj.RewardId);
                DS = Sqldbmanager.ExecuteDataSet(CommandType.StoredProcedure, "USP_TranscationManagement");
                obj1 = new TranscationReturn()
                {
                    flag = DS.Tables[0].Rows[0]["flag"].ToString(),
                    Message = DS.Tables[0].Rows[0]["Message"].ToString(),
                    TranscationId = DS.Tables[0].Rows[0]["TranscationId"].ToString(),
                    AvailableBalance = Convert.ToDecimal(DS.Tables[0].Rows[0]["AvailableBalance"].ToString())
                };
            }
            catch (Exception Ex)
            {
                DS = LogError("Transcation Management", Ex.Message.ToString(), "SP Name: USP_TranscationManagement");
                obj1 = new TranscationReturn()
                {
                    flag = "false",
                    Message = DS.Tables[0].Rows[0]["Meaasge"].ToString(),
                    TranscationId=""
                };
            }
            finally
            {
                Sqldbmanager.Close();
            }
           
            return obj1;
        }

        public object GetAvailableBalance(JsonMember.TranscationManagement obj)
        {
            TranscationReturn obj1 = new TranscationReturn();
            try
            {
                Sqldbmanager.Open();
                Sqldbmanager.CreateParameters(1);
                Sqldbmanager.AddParameters(0, "@UserId", obj.UserId);
                DS = Sqldbmanager.ExecuteDataSet(CommandType.StoredProcedure, "USP_GetAvailableBalance");
                obj1 = new TranscationReturn()
                {
                    flag = "true",
                    Message = "success",
                    TranscationId = "",
                    AvailableBalance = Convert.ToDecimal(DS.Tables[0].Rows[0]["AvailableBalance"].ToString()),
                    RewardBalance = Convert.ToDecimal(DS.Tables[0].Rows[0]["RewardBalance"].ToString()),
                    transferMoney = Convert.ToDecimal(DS.Tables[0].Rows[0]["transferMoney"].ToString()),
                    reciverMoney = Convert.ToDecimal(DS.Tables[0].Rows[0]["reciverMoney"].ToString()),
                    lastmonth = Convert.ToDecimal(DS.Tables[0].Rows[0]["lastmonth"].ToString()),
                    lastmonthCredit = Convert.ToDecimal(DS.Tables[0].Rows[0]["lastmonthCredit"].ToString()),
                    lastmonthDebit = Convert.ToDecimal(DS.Tables[0].Rows[0]["lastmonthCredit"].ToString()),
                };
            }
            catch (Exception Ex)
            {
                DS = LogError("Get Available Balance", Ex.Message.ToString(), "SP Name: USP_GetAvailableBalance");
                obj1 = new TranscationReturn()
                {
                    flag = "false",
                    Message = DS.Tables[0].Rows[0]["Meaasge"].ToString(),
                    TranscationId = "",
                    AvailableBalance = 0,
                    transferMoney=0,
                    reciverMoney=0,
                    lastmonth=0,
                    lastmonthCredit=0,
                    lastmonthDebit=0
                };
            }
            finally
            {
                Sqldbmanager.Close();
            }
            

            
            return obj1;
        }

        public object GetNotification(JsonMember.TranscationManagement obj)
        {
            NotificationManagement obj1 = new NotificationManagement();
            try
            {
                Sqldbmanager.Open();
                Sqldbmanager.CreateParameters(1);
                Sqldbmanager.AddParameters(0, "@UserId", obj.UserId);
                idr = Sqldbmanager.ExecuteReader(CommandType.StoredProcedure, "USP_GetNotification");
                List<JsonMember.NotificationManagementDetails> lstDetails = new List<JsonMember.NotificationManagementDetails>();
                List<JsonMember.MoneyRequestNotificationDetails> lstMoneyDetails = new List<JsonMember.MoneyRequestNotificationDetails>();
                while (idr.Read())
                {
                    lstDetails.Add(new JsonMember.NotificationManagementDetails()
                    {
                        TranscationId = Convert.ToString(idr["TranscationId"]),
                        ShortMsg = Convert.ToString(idr["ShortMsg"]),
                        MsgDescription = Convert.ToString(idr["MsgDescription"]),
                        LdateTime = Convert.ToString(idr["LdateTime"]),
                    });
                }
                if (idr.NextResult())
                {
                    while (idr.Read())
                    {
                        lstMoneyDetails.Add(new JsonMember.MoneyRequestNotificationDetails()
                        {
                            RequestId = Convert.ToInt64(idr["RequestId"]),
                            Amount = Convert.ToDecimal(idr["Amount"]),
                            Ldate = Convert.ToString(idr["Ldate"]),
                            LTime = Convert.ToString(idr["LTime"]),
                            UserName = Convert.ToString(idr["UserName"]),
                            MobileNo = Convert.ToString(idr["MobileNo"]),
                            PartnerId = Convert.ToInt64(idr["RequestToId"])
                        });
                    }
                }
                if (idr.NextResult())
                {
                    while (idr.Read())
                    {
                        obj1 = new NotificationManagement()
                        {
                            flag = "true",
                            Message = "success",
                            AvailableBalance = Convert.ToDecimal(idr["AvailableBalance"].ToString()),
                            NotificationCount = Convert.ToInt64(idr["NotificationCount"].ToString()),
                            RequestMoneyNotificationCount = Convert.ToInt64(idr["RequestMoneyNotificationCount"].ToString()),
                            lstNotificationManagementDetails = lstDetails,
                            lstMoneyRequestNotificationDetails=lstMoneyDetails
                        };
                    }
                }


            }
            catch (Exception Ex)
            {
                DS = LogError("Get Notification", Ex.Message.ToString(), "SP Name: USP_GetNotification");
                obj1 = new NotificationManagement()
                {
                    flag = "false",
                    Message = DS.Tables[0].Rows[0]["Meaasge"].ToString(),
                    AvailableBalance = 0,
                    NotificationCount = 0,
                    RequestMoneyNotificationCount=0
                };
            }
            finally
            {
                idr.Close();
                Sqldbmanager.Close();
            }
            return obj1;
        }

        public object GetTranscationDetails(JsonMember.TranscationManagement obj)
        {
            List<JsonMember.TranscationDetails> lstTranscationDetails = new List<JsonMember.TranscationDetails>();
            try
            {
                Sqldbmanager.Open();
                Sqldbmanager.CreateParameters(2);
                Sqldbmanager.AddParameters(0, "@UserId", obj.UserId);
                Sqldbmanager.AddParameters(1, "@for", obj.TranscationSource);
                idr = Sqldbmanager.ExecuteReader(CommandType.StoredProcedure, "USP_GetTranscationDetails");
                while (idr.Read())
                {
                    lstTranscationDetails.Add(new JsonMember.TranscationDetails()
                    {
                        flag = "true",
                        Message = "success",
                        UserId = Convert.ToInt64(idr["UserId"]),
                        TranscationSourceId = Convert.ToInt64(idr["TranscationSourceId"]),
                        PartnerUserId = Convert.ToInt64(idr["PartnerUserId"]),
                        UserLoginId = Convert.ToString(idr["UserLoginId"]),
                        EmailId = Convert.ToString(idr["EmailId"]),
                        MobileNo = Convert.ToString(idr["MobileNo"]),
                        TranscationId = Convert.ToString(idr["TranscationId"]),
                        Amount = Convert.ToDecimal(idr["Amount"]),
                        AvailableBalance = Convert.ToDecimal(idr["AvailableBalance"]),
                        TranscationSource = Convert.ToString(idr["TranscationSource"]),
                        TranscationDetail = Convert.ToString(idr["TranscationDetails"]),
                        PartnerLoginId = Convert.ToString(idr["PartnerLoginId"]),
                        PartnerEmailId = Convert.ToString(idr["PartnerEmailId"]),
                        PartnerMobileNo = Convert.ToString(idr["PartnerMobileNo"]),
                        Ldate = Convert.ToString(idr["Ldate"]),
                        LTime = Convert.ToString(idr["LTime"]),
                    });
                }
                
            }
            catch (Exception Ex)
            {
                DS = LogError("Get Available Balance", Ex.Message.ToString(), "SP Name: USP_GetTranscationDetails");
                lstTranscationDetails.Add(new JsonMember.TranscationDetails()
                {
                    flag= "false",
                    Message = DS.Tables[0].Rows[0]["Meaasge"].ToString()
                });
                
            }
            finally
            {
                idr.Close();
                Sqldbmanager.Close();
            }



            return lstTranscationDetails;
        }

        public object RewardManagementInsertUpdate(JsonMember.RewardManagement obj)
        {
            RewardManagement obj1 = new RewardManagement();
            try
            {
                Sqldbmanager.Open();
                Sqldbmanager.CreateParameters(4);
                Sqldbmanager.AddParameters(0, "@UserId", obj.UserId);
                Sqldbmanager.AddParameters(1, "@RewardAmount", obj.RewardAmount);
                Sqldbmanager.AddParameters(2, "@ValidDay", obj.ValidDay);
                Sqldbmanager.AddParameters(3, "@RewardId", obj.RewardId);
                idr = Sqldbmanager.ExecuteReader(CommandType.StoredProcedure, "USP_RewardManagement");
                List<JsonMember.RewardManagementDetails> lstDetails = new List<JsonMember.RewardManagementDetails>();
                while (idr.Read())
                {
                    lstDetails.Add(new JsonMember.RewardManagementDetails()
                    {
                        RewardCode = Convert.ToString(idr["RewardCode"]),
                        ValidFrom = Convert.ToString(idr["ValidFrom"]),
                        ValidTill = Convert.ToString(idr["ValidTill"]),
                        RewardAmount = Convert.ToDecimal(idr["RewardAmount"]),
                    });
                }
                if (idr.NextResult())
                {
                    while (idr.Read())
                    {
                        obj1 = new RewardManagement()
                        {
                            flag = "true",
                            Message = "success",
                            lstRewardManagementDetails = lstDetails
                        };
                    }
                }
               
            }
            catch (Exception Ex)
            {
                DS = LogError("Reward Management Insert Update", Ex.Message.ToString(), "SP Name: USP_RewardManagement");
                obj1 = new RewardManagement()
                {
                    flag = "false",
                    Message = DS.Tables[0].Rows[0]["Meaasge"].ToString(),
                };
            }
            finally
            {
                Sqldbmanager.Close();
            }

            return obj1;
        }

        public object GetRewardManagement(JsonMember.RewardManagement obj)
        {
            RewardManagement obj1 = new RewardManagement();
            try
            {
                Sqldbmanager.Open();
                Sqldbmanager.CreateParameters(0);
                idr = Sqldbmanager.ExecuteReader(CommandType.StoredProcedure, "USP_GetRewardManagement");
                List<JsonMember.RewardManagementDetails> lstDetails = new List<JsonMember.RewardManagementDetails>();
                while (idr.Read())
                {
                    lstDetails.Add(new JsonMember.RewardManagementDetails()
                    {
                        RewardCode = Convert.ToString(idr["RewardCode"]),
                        ValidFrom = Convert.ToString(idr["ValidFrom"]),
                        ValidTill = Convert.ToString(idr["ValidTill"]),
                        RewardAmount = Convert.ToDecimal(idr["RewardAmount"]),
                    });
                }
                if (idr.NextResult())
                {
                    while (idr.Read())
                    {
                        obj1 = new RewardManagement()
                        {
                            flag = "true",
                            Message = "success",
                            lstRewardManagementDetails = lstDetails
                        };
                    }
                }
            }
            catch (Exception Ex)
            {
                DS = LogError("Get Reward Management", Ex.Message.ToString(), "SP Name: USP_GetRewardManagement");
                obj1 = new RewardManagement()
                {
                    flag = "false",
                    Message = DS.Tables[0].Rows[0]["Meaasge"].ToString(),
                };
            }
            finally
            {
                Sqldbmanager.Close();
            }

            return obj1;
        }

        public object ValidateRewardCode(JsonMember.RewardManagement obj)
        {
            RewardManagement obj1 = new RewardManagement();
            try
            {
                Sqldbmanager.Open();
                Sqldbmanager.CreateParameters(2);
                Sqldbmanager.AddParameters(0, "@UserId", obj.UserId);
                Sqldbmanager.AddParameters(1, "@PromoCode", obj.RewardCode);
                idr = Sqldbmanager.ExecuteReader(CommandType.StoredProcedure, "USP_ValidatePromoCode");
                while (idr.Read())
                {
                    obj1 = new RewardManagement()
                    {
                        flag = (Convert.ToBoolean(idr["flag"])).ToString(),
                        Message = Convert.ToString(idr["Message"]),
                        RewardId = Convert.ToInt64(idr["RewardId"]),
                        RewardAmount = Convert.ToDecimal(idr["RewardAmount"]),
                    };
                }

            }
            catch (Exception Ex)
            {
                DS = LogError("Validate Promo Code", Ex.Message.ToString(), "SP Name: USP_ValidatePromoCode");
                obj1 = new RewardManagement()
                {
                    flag = "false",
                    Message = DS.Tables[0].Rows[0]["Meaasge"].ToString(),
                };
            }
            finally
            {
                Sqldbmanager.Close();
            }

            return obj1;
        }

        public object RequestMoney(JsonMember.RequestMoney obj)
        {
            RequestMoney obj1 = new RequestMoney();
            try
            {
                Sqldbmanager.Open();
                Sqldbmanager.CreateParameters(4);
                Sqldbmanager.AddParameters(0, "@RequestId", obj.RequestId);
                Sqldbmanager.AddParameters(1, "@RequesterId", obj.RequesterId);
                Sqldbmanager.AddParameters(2, "@RequestToId", obj.RequestToId);
                Sqldbmanager.AddParameters(3, "@Amount", obj.Amount);
                idr = Sqldbmanager.ExecuteReader(CommandType.StoredProcedure, "USP_RequestMoney");
                while (idr.Read())
                {
                    obj1 = new RequestMoney()
                    {
                        flag = (Convert.ToBoolean(idr["flag"])).ToString(),
                        Message = Convert.ToString(idr["Message"])
                    };
                }

            }
            catch (Exception Ex)
            {
                DS = LogError("Request Money", Ex.Message.ToString(), "SP Name: USP_RequestMoney");
                obj1 = new RequestMoney()
                {
                    flag = "false",
                    Message = DS.Tables[0].Rows[0]["Meaasge"].ToString()
                };
            }
            finally
            {
                Sqldbmanager.Close();
            }

            return obj1;
        }

       
    }
}