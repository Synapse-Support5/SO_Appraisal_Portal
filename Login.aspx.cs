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

        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void SendOTPBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string email = EmailTxt.Text.Trim();

                if (con.State == ConnectionState.Closed)
                    con.Open();

                string username = "";
                string role = "";

                // 1️⃣ Check User Exists
                using (SqlCommand cmd = new SqlCommand("SP_SOApp_LoginViaOTP", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ActionType", "CheckUserExists");
                    cmd.Parameters.AddWithValue("@Email", email);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (!dr.Read())
                        {
                            showToast("Email not found", "toast-danger");
                            return;
                        }

                        username = dr["UserName"].ToString();
                        role = dr["Role"].ToString();
                    }
                }

                // 2️⃣ Generate OTP
                string otp = GenerateOTP();

                // ✅ NOW SEND OTP
                bool mailSent = SendOtpEmail(email, username, otp);

                if (!mailSent)
                {
                    showToast("Failed to send OTP email", "toast-danger");
                    return;
                }

                // 3️⃣ Insert OTP
                using (SqlCommand cmd = new SqlCommand("SP_SOApp_LoginViaOTP", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ActionType", "InsertOTP");
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@OTP", Convert.ToInt32(otp));
                    cmd.Parameters.AddWithValue("@Expiry", DateTime.Now.AddMinutes(5));

                    cmd.ExecuteNonQuery();
                }

                // 4️⃣ Store user details in Session (NOT OTP)
                Session["UserEmail"] = email;
                Session["UserName"] = username;
                Session["UserRole"] = role;

                Session["OTP_SENT"] = true;

                emailSection.Visible = false;
                otpSection.Visible = true;

                showToast("OTP Sent Successfully", "toast-success");
            }
            catch (Exception ex)
            {
                LogError("Send OTP Error", ex);
                showToast("Something went wrong", "toast-danger");
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        public bool SendOtpEmail(string emailTo, string userName, string otp)
        {
            try
            {
                var template = GetEmailTemplate();

                string subject = template.Subject;
                string body = template.Body;

                // Replace placeholders
                body = body.Replace("{UserName}", userName)
                           .Replace("{OTP}", otp);

                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = "https://apps.wcclg.com/wccmail/api/sendmail.php";

                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("token", "688faf8b9f3334c111a1fea39c5926aa");

                    var requestData = new
                    {
                        credentialID = "4",
                        emailTo = emailTo,
                        emailCC = "",
                        emailBCC = "",
                        subject = subject,
                        mailbody = body,
                        attchmentFileData = "",
                        attchFileName = ""
                    };

                    string jsonBody = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);

                    var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    // 🔥 Synchronous call
                    HttpResponseMessage response = client.PostAsync(apiUrl, content).Result;

                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                LogError("SendOtpEmail Error", ex);
                return false;
            }
        }

        protected void VerifyLoginBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string enteredOtp = EnterOtpTxt.Text.Trim();
                string email = Session["UserEmail"]?.ToString();

                if (string.IsNullOrEmpty(email))
                {
                    showToast("Session expired. Please login again.", "toast-danger");
                    return;
                }

                if (con.State == ConnectionState.Closed)
                    con.Open();

                int otpId = 0;

                // 1️⃣ Check OTP
                using (SqlCommand cmd = new SqlCommand("SP_SOApp_LoginViaOTP", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ActionType", "FindOTP");
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@OTP", Convert.ToInt32(enteredOtp));

                    object result = cmd.ExecuteScalar();

                    if (result == null)
                    {
                        showToast("Invalid or Expired OTP", "toast-danger");
                        return;
                    }

                    otpId = Convert.ToInt32(result);
                }

                // 2️⃣ Mark OTP as Used
                using (SqlCommand cmd = new SqlCommand("SP_SOApp_LoginViaOTP", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ActionType", "UpdateOTP");
                    cmd.Parameters.AddWithValue("@Id", otpId);

                    cmd.ExecuteNonQuery();
                }

                // 3️⃣ Login
                FormsAuthentication.SetAuthCookie(email, false);

                // Optional: Clear OTP session flag
                //Session.Remove("OTP_SENT");

                // Redirect to Home (/)
                Response.Redirect("~/");

                //string role = Session["UserRole"].ToString();

                //if (role == "SO")
                //    Response.Redirect("SODashboard.aspx");
                //else if (role == "HR")
                //    Response.Redirect("HRDashboard.aspx");
                //else
                //    Response.Redirect("DefaultDashboard.aspx");
            }
            catch (Exception ex)
            {
                LogError("Verify OTP Error", ex);
                showToast("Something went wrong", "toast-danger");
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        private string GenerateOTP()
        {
            byte[] bytes = new byte[4];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }

            int value = BitConverter.ToInt32(bytes, 0);
            value = Math.Abs(value % 900000) + 100000;

            return value.ToString();
        }

        private (string Subject, string Body) GetEmailTemplate()
        {
            string subject = "";
            string body = "";

            using (SqlCommand cmd = new SqlCommand("SP_SOApp_LoginViaOTP", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionType", "GetEmailTemplate");
                cmd.Parameters.AddWithValue("@Email", DBNull.Value);
                cmd.Parameters.AddWithValue("@OTP", DBNull.Value);
                cmd.Parameters.AddWithValue("@Expiry", DBNull.Value);
                cmd.Parameters.AddWithValue("@Id", DBNull.Value);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        subject = dt.Rows[0]["Subject"].ToString();
                        body = dt.Rows[0]["Body"].ToString();
                    }
                }
            }

            return (subject, body);
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