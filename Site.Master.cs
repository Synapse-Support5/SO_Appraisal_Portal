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
                string role = Session["Role"]?.ToString();

                if (string.IsNullOrEmpty(role))
                {
                    Response.Redirect("Login.aspx");
                    return;
                }

                // Hide all first
                defaultLanding.Visible = false;
                hrLanding.Visible = false;
                transfer.Visible = false;
                transferpendingapprovals.Visible = false;   
                sodbrdetails.Visible = false;
                sodashboard.Visible = false;
                trachbymanager.Visible = false;
                trackbyhr.Visible = false;
                btnLogout.Visible = false;

                switch (role)
                {
                    case "ADMIN":
                        defaultLanding.Visible = true;
                        transfer.Visible = true;
                        transferpendingapprovals.Visible = true;
                        sodbrdetails.Visible = true;
                        sodashboard.Visible = true;
                        trachbymanager.Visible = true;
                        btnLogout.Visible = true;
                        break;

                    case "SO":
                        defaultLanding.Visible = true;
                        sodashboard.Visible = true;
                        break;

                    case "HR":
                        hrLanding.Visible = true;
                        transfer.Visible = true;
                        transferpendingapprovals.Visible = true;
                        sodbrdetails.Visible = true;
                        sodashboard.Visible = true;
                        trackbyhr.Visible = true;
                        btnLogout.Visible = true;
                        break;

                }
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Clear all session data
            Session.Clear();
            Session.Abandon();

            // Optional: Clear authentication cookie
            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-1);
            }

            // Redirect to login
            Response.Redirect("~/Login.aspx");
        }



    }
}