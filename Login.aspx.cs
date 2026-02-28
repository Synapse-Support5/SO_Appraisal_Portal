using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Configuration;
using System.Net.Http;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SO_Appraisal
{
    public partial class Login : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["SqlConn"].ToString());
        DataTable dt = new DataTable();
        DataTable resdt = new DataTable();
        DataSet ds = new DataSet();
        public DataSet resds = new DataSet();

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                showToast("Username and Password are required.", "toast-danger");
                return;
            }

            try
            {
                using (SqlCommand cmd = new SqlCommand("SP_SOApp_Login", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ActionType", "CheckUserExists");
                    cmd.Parameters.AddWithValue("@UserId", username);
                    cmd.Parameters.AddWithValue("@Password", password);

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string status = reader["Status"].ToString();

                            switch (status)
                            {
                                case "SUCCESS":
                                    Session["UserId"] = username;
                                    Session["UserName"] = reader["UserName"].ToString();
                                    Session["Role"] = reader["Role"].ToString();

                                    Response.Redirect("Default.aspx", false);
                                    Context.ApplicationInstance.CompleteRequest();
                                    break;

                                case "USER_NOT_FOUND":
                                    showToast("User does not exist.", "toast-danger");
                                    break;

                                case "INVALID_PASSWORD":
                                    showToast("Incorrect password.", "toast-danger");
                                    break;

                                case "USER_INACTIVE":
                                    showToast("User account is inactive.", "toast-danger");
                                    break;

                                default:
                                    showToast("Unexpected login error.", "toast-danger");
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("Login Error", ex);
                showToast("Something went wrong. Please try again.", "toast-danger");
            }
        }

        #region ToastNotification
        private void showToast(string message, string styleClass)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showToast", $"showToast('{message}', '{styleClass}');", true);
        }

        #endregion

        #region LogError
        private void LogError(string message, Exception ex)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_ErrorLog_SS5", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@Portal", "SOApp");
                cmd1.Parameters.AddWithValue("@Page", "LoginPage");
                cmd1.Parameters.AddWithValue("@Message", message);
                cmd1.Parameters.AddWithValue("@Exception", ex?.ToString() ?? string.Empty);
                cmd1.CommandTimeout = 6000;
                cmd1.ExecuteNonQuery();

                con.Close();
            }
            catch
            {
            }
        }





        #endregion






    }
}