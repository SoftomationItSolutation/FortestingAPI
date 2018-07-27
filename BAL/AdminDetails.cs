using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using DAL;
using System.Configuration;
using static BAL.JsonMember;

namespace BAL
{
    public class AdminDetails
    {
        DBManager Sqldbmanager = new DBManager(DataProvider.SqlServer, ConfigurationManager.ConnectionStrings["E_wallet"].ConnectionString);
        DataSet DS = new DataSet();
        DataTable dt = new DataTable();

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

        public Object AdminLoginMaster(JsonMember.UserDetails obj)
        {
            LoginReturn Lobj = new LoginReturn();
            DataTable Logindt = new DataTable();
            try
            {
                Sqldbmanager.Open();
                Sqldbmanager.CreateParameters(1);
                Sqldbmanager.AddParameters(0, "@LoginId", obj.LoginId);
                DS = Sqldbmanager.ExecuteDataSet(CommandType.StoredProcedure, "USP_AdminLogin");
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
                            UserId = DS.Tables[1].Rows[0]["UserId"].ToString(),
                            FirstName = DS.Tables[1].Rows[0]["FirstName"].ToString(),
                            LastName = DS.Tables[1].Rows[0]["LastName"].ToString(),
                            ProfilePicPath = DS.Tables[1].Rows[0]["ProfilePicPath"].ToString()
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
                            UserId = "",
                            FirstName = "",
                            LastName = "",
                            ProfilePicPath = "",
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
                        UserId = "",
                        FirstName = "",
                        LastName = "",
                        ProfilePicPath = "",
                    };

                }
            }
            catch (Exception)
            {
                Lobj = new LoginReturn()
                {
                    flag = "false",
                    Message = "Invalid Email/Mobile",
                    EmailId = "",
                    MobileNo = "",
                    Name = "",
                    UserId = "",
                    FirstName = "",
                    LastName = "",
                    ProfilePicPath = "",
                };

            }
            finally
            {
                Sqldbmanager.Close();
            }
            return Lobj;
        }
    }
}