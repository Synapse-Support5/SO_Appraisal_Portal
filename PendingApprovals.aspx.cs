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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SO_Appraisal
{
    public partial class PendingApprovals : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["SqlConn"].ToString());
        DataTable dt = new DataTable();
        DataTable resdt = new DataTable();
        DataSet ds = new DataSet();
        bool anyCheckboxSelected = false;

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

        #region GetPendingApprovals
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

                int pendingCount = resdt.Rows.Count;

                var siteMaster = this.Master as SO_Appraisal.SiteMaster;
                if (siteMaster != null)
                {
                    siteMaster.SetPendingCount(pendingCount);
                }

                if (pendingCount > 0)
                {
                    ButtonsDiv.Visible = true;
                    PendingApprovalsGrid.DataSource = resdt;
                    PendingApprovalsGrid.DataBind();
                }
                else
                {
                    GridStatusLabel.Text = "No pendind requests found!";
                    ButtonsDiv.Visible = false;
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
        #endregion

        #region btnViewThisRequest_Click
        protected void btnViewThisRequest_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                string[] CommandArgument = btn.CommandArgument.Split(',');
                int CommandCPRequestId = Convert.ToInt32(CommandArgument[0]);

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_SOApp_PendingApprovals", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@ActionType", "ViewGridLoad");
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@RequestId", CommandCPRequestId);

                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Rows.Clear();
                da.Fill(resdt);

                if (resdt.Rows.Count > 0)
                {
                    DistModal.DataSource = resdt;
                    DistModal.DataBind();
                }
                else
                {
                    DistModal.DataSource = null;
                    DistModal.DataBind();
                }

                con.Close();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "openTransferModal();", true);
            }
            catch (Exception ex)
            {
                LogError("View this request Error", ex);
                showToast("Something went wrong. Please try again later or contact the SYNAPSE team", "toast-danger");
            }
        }
        #endregion

        #region ApproveReject
        public DataSet ApproveReject(int requestId, string approveRejected)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_SOApp_ApproveReject_Newlogic", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@RequestId", requestId);
                cmd1.Parameters.AddWithValue("@ApproveReject", approveRejected);
                cmd1.CommandTimeout = 6000;
                cmd1.ExecuteNonQuery();

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                ds.Clear();
                da.Fill(ds);

                con.Close();
            }
            catch (Exception ex)
            {
                LogError("Approve/Rejected Error", ex);
                showToast("Something went wrong. Please try again later or contact the SYNAPSE team", "toast-danger");
            }

            return ds;
        }
        #endregion

        #region btnRowApprove_Click
        protected void btnRowApprove_Click(object sender, EventArgs e)
        {
            try
            {
                //LinkButton btn = (LinkButton)sender;
                //string[] CommandArgument = btn.CommandArgument.Split(',');
                //int CommandRequestId = Convert.ToInt32(CommandArgument[0]);

                int requestId = Convert.ToInt32(hfApproveRequestId.Value);

                DataSet ds = ApproveReject(requestId, "Approved");

                // If dataset is empty -> SP/DB failure
                if (ds == null || ds.Tables.Count == 0)
                {
                    LogError("ApproveReject returned no resultset", null);
                    showToast("Something went wrong while processing. Please try again later.", "toast-danger");
                    return;
                }

                // First resultset = per-dist details
                DataTable details = ds.Tables[0];

                // Optionally second resultset = summary counts
                DataTable summary = ds.Tables.Count > 1 ? ds.Tables[1] : null;

                // Check for any errors in the detailed results
                var errorRows = details.AsEnumerable()
                                       .Where(r => r.Field<string>("ActionTaken").Equals("Error", StringComparison.OrdinalIgnoreCase))
                                       .ToList();

                if (errorRows.Any())
                {
                    // Build a compact error message: show first N errors + count
                    int showMax = 3;
                    var firstErrors = errorRows.Take(showMax)
                                              .Select(r => $"{r.Field<string>("DistCode")}: {r.Field<string>("ResultMsg")}");
                    string msg = $"Errors for {errorRows.Count} distributor(s): {string.Join("; ", firstErrors)}";
                    if (errorRows.Count > showMax) msg += $" ...(+{errorRows.Count - showMax} more)";

                    showToast(msg, "toast-danger");
                    return;
                }

                // Otherwise success (no errors). Summarize actions
                int inserted = details.AsEnumerable().Count(r => r.Field<string>("ActionTaken").Equals("Inserted", StringComparison.OrdinalIgnoreCase));
                int activated = details.AsEnumerable().Count(r => r.Field<string>("ActionTaken").Equals("Activated", StringComparison.OrdinalIgnoreCase));
                int updated = details.AsEnumerable().Count(r => r.Field<string>("ActionTaken").Equals("UpdatedPending", StringComparison.OrdinalIgnoreCase));
                int noInsert = details.AsEnumerable().Count(r => r.Field<string>("ActionTaken").Equals("NoInsert", StringComparison.OrdinalIgnoreCase));

                // Compose a friendly success message
                var parts = new List<string>();
                if (inserted > 0) parts.Add($"{inserted} inserted");
                if (activated > 0) parts.Add($"{activated} activated");
                if (updated > 0) parts.Add($"{updated} updated");
                if (noInsert > 0) parts.Add($"{noInsert} marked");

                string successMsg = parts.Count > 0 ? $"Success: {string.Join(", ", parts)}." : "Request processed successfully.";
                showToast(successMsg, "toast-success");

                GetPendingApprovals();
            }
            catch (Exception ex)
            {
                LogError("Approve row wise Error", ex);
                showToast("Something went wrong. Please try again later or contact the SYNAPSE team", "toast-danger");
            }
        }
        #endregion

        #region btnRowReject_Click
        protected void btnRowReject_Click(object sender, EventArgs e)
        {
            try
            {
                //LinkButton btn = (LinkButton)sender;
                //string[] CommandArgument = btn.CommandArgument.Split(',');
                //int CommandRequestId = Convert.ToInt32(CommandArgument[0]);

                int requestId = Convert.ToInt32(hfRejectRequestId.Value);

                DataSet ds = ApproveReject(requestId, "Rejected");

                // If dataset is empty -> SP/DB failure
                if (ds == null || ds.Tables.Count == 0)
                {
                    LogError("ApproveReject returned no resultset", null);
                    showToast("Something went wrong while processing. Please try again later.", "toast-danger");
                    return;
                }

                // First resultset = per-dist details
                DataTable details = ds.Tables[0];

                // Optionally second resultset = summary counts
                DataTable summary = ds.Tables.Count > 1 ? ds.Tables[1] : null;

                // Check for any errors in the detailed results
                var errorRows = details.AsEnumerable()
                                       .Where(r => r.Field<string>("ActionTaken").Equals("Error", StringComparison.OrdinalIgnoreCase))
                                       .ToList();

                if (errorRows.Any())
                {
                    // Build a compact error message: show first N errors + count
                    int showMax = 3;
                    var firstErrors = errorRows.Take(showMax)
                                              .Select(r => $"{r.Field<string>("DistCode")}: {r.Field<string>("ResultMsg")}");
                    string msg = $"Errors for {errorRows.Count} distributor(s): {string.Join("; ", firstErrors)}";
                    if (errorRows.Count > showMax) msg += $" ...(+{errorRows.Count - showMax} more)";

                    showToast(msg, "toast-danger");
                    return;
                }

                // Otherwise success (no errors). Summarize actions
                int inserted = details.AsEnumerable().Count(r => r.Field<string>("ActionTaken").Equals("Inserted", StringComparison.OrdinalIgnoreCase));
                int activated = details.AsEnumerable().Count(r => r.Field<string>("ActionTaken").Equals("Activated", StringComparison.OrdinalIgnoreCase));
                int updated = details.AsEnumerable().Count(r => r.Field<string>("ActionTaken").Equals("UpdatedPending", StringComparison.OrdinalIgnoreCase));
                int noInsert = details.AsEnumerable().Count(r => r.Field<string>("ActionTaken").Equals("NoInsert", StringComparison.OrdinalIgnoreCase));

                // Compose a friendly success message
                var parts = new List<string>();
                if (inserted > 0) parts.Add($"{inserted} inserted");
                if (activated > 0) parts.Add($"{activated} activated");
                if (updated > 0) parts.Add($"{updated} updated");
                if (noInsert > 0) parts.Add($"{noInsert} marked");

                string successMsg = parts.Count > 0 ? $"Success: {string.Join(", ", parts)}." : "Request processed successfully.";
                showToast(successMsg, "toast-success");

                GetPendingApprovals();
            }
            catch (Exception ex)
            {
                LogError("Reject row wise Error", ex);
                showToast("Something went wrong. Please try again later or contact the SYNAPSE team", "toast-danger");
            }
        }
        #endregion

        #region ApproveSelectedBtn_Click
        protected void ApproveSelectedBtn_Click(object sender, EventArgs e)
        {
            try
            {
                // Totals across all processed requests
                int totalRequests = 0;
                int totalInserted = 0;
                int totalActivated = 0;
                int totalUpdated = 0;
                int totalNoInsert = 0;
                int totalErrors = 0;

                // Collect error details (limit to avoid massive text)
                var errorDetails = new List<string>();

                foreach (GridViewRow row in PendingApprovalsGrid.Rows)
                {
                    var chkBox = row.FindControl("CheckBox1") as HtmlInputCheckBox;
                    if (chkBox != null && chkBox.Checked)
                    {
                        // Prefer DataKeys for RequestId (safer)
                        int requestId = 0;
                        if (PendingApprovalsGrid.DataKeys != null && PendingApprovalsGrid.DataKeys.Count > row.RowIndex)
                        {
                            requestId = Convert.ToInt32(PendingApprovalsGrid.DataKeys[row.RowIndex].Value);
                            anyCheckboxSelected = true;
                        }
                        else
                        {
                            // Fallback — adjust cell index if RequestId column location changes
                            requestId = Convert.ToInt32(row.Cells[1].Text);
                            anyCheckboxSelected = true;
                        }

                        totalRequests++;

                        // Call SP for this request (returns DataSet with details + summary)
                        DataSet ds = ApproveReject(requestId, "Approved");

                        if (ds == null || ds.Tables.Count == 0)
                        {
                            // SP failed to return anything
                            totalErrors++;
                            errorDetails.Add($"Req {requestId}: no response from server");
                            continue;
                        }

                        DataTable details = ds.Tables[0];

                        // Count action types in this request
                        int inserted = details.AsEnumerable().Count(r => r.Field<string>("ActionTaken").Equals("Inserted", StringComparison.OrdinalIgnoreCase));
                        int activated = details.AsEnumerable().Count(r => r.Field<string>("ActionTaken").Equals("Activated", StringComparison.OrdinalIgnoreCase));
                        int updated = details.AsEnumerable().Count(r => r.Field<string>("ActionTaken").Equals("UpdatedPending", StringComparison.OrdinalIgnoreCase));
                        int noInsert = details.AsEnumerable().Count(r => r.Field<string>("ActionTaken").Equals("NoInsert", StringComparison.OrdinalIgnoreCase));
                        int errors = details.AsEnumerable().Count(r => r.Field<string>("ActionTaken").Equals("Error", StringComparison.OrdinalIgnoreCase));

                        totalInserted += inserted;
                        totalActivated += activated;
                        totalUpdated += updated;
                        totalNoInsert += noInsert;
                        totalErrors += errors;

                        // Collect sample error messages (limit to first 5 overall)
                        if (errors > 0 && errorDetails.Count < 10)
                        {
                            foreach (var rowErr in details.AsEnumerable().Where(r => r.Field<string>("ActionTaken").Equals("Error", StringComparison.OrdinalIgnoreCase)))
                            {
                                string dist = rowErr.Field<string>("DistCode");
                                string msg = rowErr.Field<string>("ResultMsg");
                                errorDetails.Add($"Req {requestId} - {dist}: {msg}");
                                if (errorDetails.Count >= 10) break;
                            }
                        }
                    }
                }

                if (!anyCheckboxSelected)
                {
                    showToast("Please select atleast any one request to approve", "toast-danger");
                    return;
                }

                // Build final toast message
                if (totalErrors > 0)
                {
                    // Error toast: show error count + few samples
                    string sample = errorDetails.Count > 0 ? string.Join("; ", errorDetails.Take(3)) : "";
                    string msg = $"Processed {totalRequests} request(s). Errors: {totalErrors}. {(sample == "" ? "" : "Sample: " + sample)}";
                    showToast(msg, "toast-danger");
                }
                else
                {
                    // Success toast: summarize actions performed
                    var parts = new List<string>();
                    if (totalInserted > 0) parts.Add($"{totalInserted} inserted");
                    if (totalActivated > 0) parts.Add($"{totalActivated} activated");
                    if (totalUpdated > 0) parts.Add($"{totalUpdated} updated");
                    if (totalNoInsert > 0) parts.Add($"{totalNoInsert} marked");

                    string summary = parts.Count > 0 ? string.Join(", ", parts) : "No changes needed";
                    string msg = $"Processed {totalRequests} request(s). Success: {summary}.";
                    showToast(msg, "toast-success");
                }

                GetPendingApprovals();
            }
            catch (Exception ex)
            {
                LogError("Approve Selected Error", ex);
                showToast("Something went wrong. Please try again later or contact the SYNAPSE team", "toast-danger");
            }
        }
        #endregion

        #region RejectSelectedBtn_Click
        protected void RejectSelectedBtn_Click(object sender, EventArgs e)
        {
            try
            {
                // Totals across all processed requests
                int totalRequests = 0;
                int totalInserted = 0;
                int totalActivated = 0;
                int totalUpdated = 0;
                int totalNoInsert = 0;
                int totalErrors = 0;

                // Collect error details (limit to avoid massive text)
                var errorDetails = new List<string>();

                foreach (GridViewRow row in PendingApprovalsGrid.Rows)
                {
                    var chkBox = row.FindControl("CheckBox1") as HtmlInputCheckBox;
                    if (chkBox != null && chkBox.Checked)
                    {
                        // Prefer DataKeys for RequestId (safer)
                        int requestId = 0;
                        if (PendingApprovalsGrid.DataKeys != null && PendingApprovalsGrid.DataKeys.Count > row.RowIndex)
                        {
                            requestId = Convert.ToInt32(PendingApprovalsGrid.DataKeys[row.RowIndex].Value);
                            anyCheckboxSelected = true;
                        }
                        else
                        {
                            // Fallback — adjust cell index if RequestId column location changes
                            requestId = Convert.ToInt32(row.Cells[1].Text);
                            anyCheckboxSelected = true;
                        }

                        totalRequests++;

                        // Call SP for this request (returns DataSet with details + summary)
                        DataSet ds = ApproveReject(requestId, "Rejected");

                        if (ds == null || ds.Tables.Count == 0)
                        {
                            // SP failed to return anything
                            totalErrors++;
                            errorDetails.Add($"Req {requestId}: no response from server");
                            continue;
                        }

                        DataTable details = ds.Tables[0];

                        // Count action types in this request
                        int inserted = details.AsEnumerable().Count(r => r.Field<string>("ActionTaken").Equals("Inserted", StringComparison.OrdinalIgnoreCase));
                        int activated = details.AsEnumerable().Count(r => r.Field<string>("ActionTaken").Equals("Activated", StringComparison.OrdinalIgnoreCase));
                        int updated = details.AsEnumerable().Count(r => r.Field<string>("ActionTaken").Equals("UpdatedPending", StringComparison.OrdinalIgnoreCase));
                        int noInsert = details.AsEnumerable().Count(r => r.Field<string>("ActionTaken").Equals("NoInsert", StringComparison.OrdinalIgnoreCase));
                        int errors = details.AsEnumerable().Count(r => r.Field<string>("ActionTaken").Equals("Error", StringComparison.OrdinalIgnoreCase));

                        totalInserted += inserted;
                        totalActivated += activated;
                        totalUpdated += updated;
                        totalNoInsert += noInsert;
                        totalErrors += errors;

                        // Collect sample error messages (limit to first 5 overall)
                        if (errors > 0 && errorDetails.Count < 10)
                        {
                            foreach (var rowErr in details.AsEnumerable().Where(r => r.Field<string>("ActionTaken").Equals("Error", StringComparison.OrdinalIgnoreCase)))
                            {
                                string dist = rowErr.Field<string>("DistCode");
                                string msg = rowErr.Field<string>("ResultMsg");
                                errorDetails.Add($"Req {requestId} - {dist}: {msg}");
                                if (errorDetails.Count >= 10) break;
                            }
                        }
                    }
                }

                if (!anyCheckboxSelected)
                {
                    showToast("Please select atleast any one request to reject", "toast-danger");
                    return;
                }

                // Build final toast message
                if (totalErrors > 0)
                {
                    // Error toast: show error count + few samples
                    string sample = errorDetails.Count > 0 ? string.Join("; ", errorDetails.Take(3)) : "";
                    string msg = $"Processed {totalRequests} request(s). Errors: {totalErrors}. {(sample == "" ? "" : "Sample: " + sample)}";
                    showToast(msg, "toast-danger");
                }
                else
                {
                    // Success toast: summarize actions performed
                    var parts = new List<string>();
                    if (totalInserted > 0) parts.Add($"{totalInserted} inserted");
                    if (totalActivated > 0) parts.Add($"{totalActivated} activated");
                    if (totalUpdated > 0) parts.Add($"{totalUpdated} updated");
                    if (totalNoInsert > 0) parts.Add($"{totalNoInsert} marked");

                    string summary = parts.Count > 0 ? string.Join(", ", parts) : "No changes needed";
                    string msg = $"Processed {totalRequests} request(s). Success: {summary}.";
                    showToast(msg, "toast-success");
                }

                GetPendingApprovals();
            }
            catch (Exception ex)
            {
                LogError("Rejected Selected Error", ex);
                showToast("Something went wrong. Please try again later or contact the SYNAPSE team", "toast-danger");
            }
        }
        #endregion

        #region PendingApprovalsGrid_RowCommand
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
        }
        #endregion

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
    }
}