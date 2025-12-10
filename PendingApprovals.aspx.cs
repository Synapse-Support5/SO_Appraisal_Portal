using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SO_Appraisal
{
    public partial class PendingApprovals : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["SqlConn"].ToString());
        DataTable dt = new DataTable();
        DataTable resdt = new DataTable();
        DataSet ds = new DataSet();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AccessLoad();
                GetPendingApprovals();
            }
        }

        #region AccessLoad
        public void AccessLoad()
        {
            try
            {
                string remoteUser = "G116036";
                //string remoteUser = Request.ServerVariables["REMOTE_USER"];

                if (!string.IsNullOrEmpty(remoteUser))
                {
                    if (remoteUser == Request.ServerVariables["REMOTE_USER"])
                    {
                        Session["name"] = remoteUser.Substring(6);
                    }
                    else
                    {
                        Session["name"] = remoteUser;
                    }

                    var name = Session["name"]?.ToString().Trim().ToUpper();
                    if (name == "G116036" || name == "G112377" || name == "SY12108G" || name == "G115193")
                    {
                        //isDev = true;
                        Session["isDev"] = true;
                    }
                    else
                    {
                        Session["isDev"] = false;
                    }

                    if (!string.IsNullOrEmpty(Session["name"]?.ToString()))
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        SqlCommand cmd1 = new SqlCommand("SP_SOApp_AccessLoad", con);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());

                        cmd1.CommandTimeout = 6000;
                        SqlDataAdapter da = new SqlDataAdapter(cmd1);
                        da.Fill(resdt);

                        if (resdt.Rows.Count > 0)
                        {
                            //lblUserName.Text = "User Name > " + resdt.Rows[0][0].ToString() + ": User ID > " + Session["name"].ToString();
                            lblUserName.Text = "Welcome, " + resdt.Rows[0][1].ToString();
                            Session["Username"] = resdt.Rows[0][0].ToString();
                            hdnBusinessType.Value = resdt.Rows[0][2].ToString();
                            hdnRole.Value = resdt.Rows[0][3].ToString();
                        }
                        else
                        {
                            Response.Redirect("AccessDeniedPage.aspx");
                        }
                        con.Close();
                    }
                    else
                    {
                        Response.Redirect("AccessDeniedPage.aspx");
                    }
                }
                else
                {
                    Response.Redirect("AccessDeniedPage.aspx");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public void GetPendingApprovals()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_SOApp_PendingApprovals", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@ActionType", "MainGridLoad");
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@RequestId", "");

                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Rows.Clear();
                da.Fill(resdt);

                if (resdt.Rows.Count > 0)
                {
                    PendingApprovalsGrid.DataSource = resdt;
                    PendingApprovalsGrid.DataBind();
                }
                else
                {
                    PendingApprovalsGrid.DataSource = null;
                    PendingApprovalsGrid.DataBind();
                }

                con.Close();
            }
            catch (Exception ex)
            {
                LogError("Get Pending Approvals Error", ex);
                showToast("Something went wrong. Please try again later or contact the SYNAPSE team", "toast-danger");
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
                cmd1.Parameters.AddWithValue("@Page", "PendingApprovals");
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

        protected void btnViewThisRequest_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                string[] CommandArgument = btn.CommandArgument.Split(',');
                int CommandCPRequestId = Convert.ToInt32(CommandArgument[0]);

                testTabel.Text = CommandCPRequestId.ToString();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "openTransferModal();", true);
            }
            catch (Exception ex)
            {
                LogError("View this request Error", ex);
                showToast("Something went wrong. Please try again later or contact the SYNAPSE team", "toast-danger");
            }
        }

        protected void PendingApprovalsGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DownloadAttachment")
            {
                string virtualPath = e.CommandArgument as string;

                if (string.IsNullOrWhiteSpace(virtualPath))
                {
                    showToast("Attachment not available.", "toast-danger");
                    return;
                }

                // Normalize the virtual path (database has something like '/Uploads/TransferProof/...')
                virtualPath = virtualPath.Trim();

                if (!virtualPath.StartsWith("~"))
                {
                    if (!virtualPath.StartsWith("/"))
                        virtualPath = "~/" + virtualPath;          // 'Uploads/...' → '~/Uploads/...'
                    else
                        virtualPath = "~" + virtualPath;           // '/Uploads/...' → '~/Uploads/...'
                }

                string physicalPath = Server.MapPath(virtualPath);

                if (!File.Exists(physicalPath))
                {
                    showToast("Attachment file not found on server.", "toast-danger");
                    return;
                }

                string fileName = Path.GetFileName(physicalPath);
                string contentType = MimeMapping.GetMimeMapping(fileName);  // .NET 4.5+

                Response.Clear();
                Response.ContentType = contentType;
                Response.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
                Response.TransmitFile(physicalPath);
                Response.End();
            }
            else if (e.CommandName == "ApproveRow")
            {
                string requestId = e.CommandArgument as string;
                showToast("CPRequestId is " + requestId + "toast is working fine for approve", "toast-success");
            }
            else if (e.CommandName == "RejectRow")
            {
                string requestId = e.CommandArgument as string;
                showToast("CPRequestId is " + requestId + "toast is working fine for reject", "toast-success");
            }
        }


    }
}