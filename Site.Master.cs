using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SO_Appraisal
{
    public partial class SiteMaster : MasterPage
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["SqlConn"].ToString());
        DataTable dt = new DataTable();
        DataTable resdt = new DataTable();
        DataSet ds = new DataSet();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //AccessLoad();

                string token = Session["Token"].ToString();
                string sessionId = Session.SessionID;

                // 1️⃣ Token missing — redirect
                if (string.IsNullOrEmpty(token))
                {
                    showToast("Invalid session. Please login again.", "toast-danger");
                    Response.Redirect("SignIn.aspx", true);
                    return;
                }

                // 2️⃣ Validate token in DB
                string userId = "";
                string businessType = "";
                string role = "";


                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd2 = new SqlCommand("SP_UserSessionTokens", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@ActionType", "Validate");
                cmd2.Parameters.AddWithValue("@UserId", userId);
                cmd2.Parameters.AddWithValue("@PageURL", "ClaimPortalUserCreation");
                cmd2.Parameters.AddWithValue("@Token", token);
                cmd2.Parameters.AddWithValue("@IsUsed", 0);
                cmd2.Parameters.AddWithValue("@SessionId", sessionId);

                SqlDataAdapter da = new SqlDataAdapter(cmd2);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    // Invalid or already used token
                    showToast("Session expired. Please login again.", "toast-danger");
                    Response.Redirect("SignIn.aspx", true);
                    return;
                }

                // Token is valid
                userId = dt.Rows[0]["UserId"].ToString();
                businessType = dt.Rows[0]["BusinessType"].ToString();
                role = dt.Rows[0]["Role"].ToString();

                // ✅ 3️⃣ Mark token as used (one-time)
                SqlCommand cmdUpdate = new SqlCommand("SP_UserSessionTokens", con);
                cmdUpdate.CommandType = CommandType.StoredProcedure;
                cmdUpdate.Parameters.AddWithValue("@ActionType", "Update");
                cmdUpdate.Parameters.AddWithValue("@UserId", userId);
                cmdUpdate.Parameters.AddWithValue("@PageURL", "ClaimPortalUserCreation");
                cmdUpdate.Parameters.AddWithValue("@Token", token);
                cmdUpdate.Parameters.AddWithValue("@IsUsed", 0);
                cmdUpdate.Parameters.AddWithValue("@SessionId", sessionId);
                cmdUpdate.ExecuteNonQuery();


                // ✅ 4️⃣ Set session values
                Session["UserId"] = userId;
                Session["BusinessType"] = businessType;
                Session["Role"] = role;
                Session["name"] = userId;
            }
        }

        public void SetPendingCount(int count)
        {
            if (BadgeLabel != null)
            {
                if (count > 0)
                {
                    BadgeLabel.InnerText = count.ToString();
                    BadgeLabel.Style["display"] = "inline-block";
                }
                else
                {
                    BadgeLabel.InnerText = "0";
                    BadgeLabel.Style["display"] = "none";
                }
            }
        }

        #region ToastNotification
        private void showToast(string message, string styleClass)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showToast", $"showToast('{message}', '{styleClass}');", true);
        }



        #endregion
    }
}