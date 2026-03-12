using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office.Word;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SO_Appraisal
{
    public partial class TrackbyManager : BasePage
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
                //StateLoad();
            }
        }

        #region AccessLoad
        public void AccessLoad()
        {
            try
            {
                string remoteUser = "G116036";
                //string remoteUser = Request.ServerVariables["REMOTE_USER"];
                //remoteUser = Session["UserId"].ToString();

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
                            Session["Username"] = resdt.Rows[0][1].ToString();
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
                LogError("Access load Error", ex);
                showToast("Something went wrong. Please try again later or contact the SYNAPSE team", "toast-danger");
            }
        }
        #endregion

        #region PendingsLoad
        public void PendingsLoad()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                using (SqlCommand cmd1 = new SqlCommand("SP_SOApp_TrackByManager", con))
                {
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                    cmd1.Parameters.AddWithValue("@ActionType", "PendingApprovals");
                    cmd1.Parameters.AddWithValue("@RequestId", "");
                    cmd1.CommandTimeout = 6000;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                    {
                        resdt.Rows.Clear();
                        da.Fill(resdt);

                        if (resdt.Rows.Count > 0)
                        {
                            GridStatusLabel.Text = string.Empty;

                            PendingApprovalsGrid.DataSource = resdt;
                            PendingApprovalsGrid.DataBind();

                            // Check if any row has Status = 1
                            bool hasPending = resdt.AsEnumerable()
                                                   .Any(row => row.Field<int>("Status") == 1);

                            ButtonsDiv.Visible = true;

                            ApproveSelectedBtn.Enabled = hasPending;
                            RejectSelectedBtn.Enabled = hasPending;
                        }
                        else
                        {
                            ButtonsDiv.Visible = false;
                            GridStatusLabel.Text = "No pendind requests found!";
                        }

                    }
                }

                con.Close();

            }
            catch (Exception ex)
            {
                LogError("Pendings load Error", ex);
                showToast("Something went wrong. Please try again later or contact the SYNAPSE team", "toast-danger");
            }
        }
        #endregion

        #region ViewAllLoad
        public void ViewAllLoad()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                using (SqlCommand cmd1 = new SqlCommand("SP_SOApp_TrackByManager", con))
                {
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                    cmd1.Parameters.AddWithValue("@ActionType", "ViewAll");
                    cmd1.Parameters.AddWithValue("@RequestId", "");
                    cmd1.CommandTimeout = 6000;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                    {
                        resdt.Rows.Clear();
                        da.Fill(resdt);

                        if (resdt.Rows.Count > 0)
                        {
                            GridStatusLabelViewAll.Text = string.Empty;

                            ViewAllGrid.DataSource = resdt;
                            ViewAllGrid.DataBind();

                        }
                        else
                        {
                            GridStatusLabelViewAll.Text = "No pendind requests found!";
                        }

                    }
                }

                con.Close();

            }
            catch (Exception ex)
            {
                LogError("ViewAll load Error", ex);
                showToast("Something went wrong. Please try again later or contact the SYNAPSE team", "toast-danger");
            }
        }
        #endregion

        #region PendingApprovalsBtn_Click
        protected void PendingApprovalsBtn_Click(object sender, EventArgs e)
        {
            PendingApprovalsBtn.CssClass = "btn btn-primary form-control";
            ViewAllBtn.CssClass = "btn btn-outline-primary form-control";
            PendApprovalsSec.Visible = true;
            ViewAllSec.Visible = false;
            PendingsLoad();
        }
        #endregion

        #region ViewAllBtn_Click
        protected void ViewAllBtn_Click(object sender, EventArgs e)
        {
            PendingApprovalsBtn.CssClass = "btn btn-outline-primary form-control";
            ViewAllBtn.CssClass = "btn btn-primary form-control";

            PendingApprovalsGrid.DataSource = null;
            PendingApprovalsGrid.DataBind();

            ButtonsDiv.Visible = false;

            PendApprovalsSec.Visible = false;
            ViewAllSec.Visible = true;

            ViewAllLoad();

            txtTraining.ReadOnly = true;
            txtCareer.ReadOnly = true;
            txtSignIn.ReadOnly = true;
            UpdateBtn.Visible = false;
        }
        #endregion

        #region btnDownloadThisRequest_Click
        protected void btnDownloadThisRequest_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;

                string[] args = btn.CommandArgument.Split(',');

                string requestId = (args[0]).ToString();
                string soCode = (args[1]).ToString();
                string pcYear = (args[2]).ToString();
                string quarter = (args[3]).ToString();

                DataSet ds = LoadRequestAchivements(soCode, pcYear, quarter);

                using (ClosedXML.Excel.XLWorkbook wb = new ClosedXML.Excel.XLWorkbook())
                {
                    CreateCustomSheet(wb, ds, 0, 9, "Primary", primaryNames);
                    CreateCustomSheet(wb, ds, 10, 19, "Secondary", secondaryNames);

                    // ================= Distributor Sheet =================
                    if (ds.Tables.Count > 20)
                    {
                        var wsDist = wb.Worksheets.Add("Distributors");
                        var table = wsDist.Cell(1, 1).InsertTable(ds.Tables[20], false);

                        FormatTable(table);
                        wsDist.Columns().AdjustToContents();
                    }

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";

                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition",
                        "attachment;filename=Request_Report_" + soCode + "_" + requestId + ".xlsx");

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        wb.SaveAs(memoryStream);
                        memoryStream.WriteTo(Response.OutputStream);
                    }

                    Response.Flush();
                    Response.SuppressContent = true;
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                LogError("View this request Error", ex);
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

                string[] args = btn.CommandArgument.Split(',');

                int requestId = Convert.ToInt32(args[0]);
                int status = Convert.ToInt32(args[1]);

                // store request id
                hdnRequestId.Value = requestId.ToString();

                LoadRequestDetails(requestId, status);

                ScriptManager.RegisterStartupScript(this, this.GetType(),
                    "showModal", "$('#exampleModalCenter').modal('show');", true);
            }
            catch (Exception ex)
            {
                LogError("View this request Error", ex);
                showToast("Something went wrong. Please try again later or contact the SYNAPSE team", "toast-danger");
            }
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

                PendingsLoad();
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

                PendingsLoad();
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

                PendingsLoad();
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

                PendingsLoad();
            }
            catch (Exception ex)
            {
                LogError("Rejected Selected Error", ex);
                showToast("Something went wrong. Please try again later or contact the SYNAPSE team", "toast-danger");
            }
        }
        #endregion

        #region ApproveReject
        public DataSet ApproveReject(int requestId, string approveRejected)
        {
            try
            {
                showToast("Working in progress. RequestId is : " + requestId, "toast-success");
                //if (con.State == ConnectionState.Closed)
                //{
                //    con.Open();
                //}
                //SqlCommand cmd1 = new SqlCommand("SP_SOApp_ApproveReject_Newlogic", con);
                //cmd1.CommandType = CommandType.StoredProcedure;
                //cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                //cmd1.Parameters.AddWithValue("@RequestId", requestId);
                //cmd1.Parameters.AddWithValue("@ApproveReject", approveRejected);
                //cmd1.CommandTimeout = 6000;
                //cmd1.ExecuteNonQuery();

                //SqlDataAdapter da = new SqlDataAdapter(cmd1);
                //ds.Clear();
                //da.Fill(ds);

                //con.Close();

            }
            catch (Exception ex)
            {
                LogError("Approve/Rejected Error", ex);
                showToast("Something went wrong. Please try again later or contact the SYNAPSE team", "toast-danger");
            }

            return ds;
        }
        #endregion

        #region Helpers
        protected void PendingApprovalsGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string status = DataBinder.Eval(e.Row.DataItem, "Status").ToString();

                if (status != "1")
                {
                    HtmlInputCheckBox chk = (HtmlInputCheckBox)e.Row.FindControl("CheckBox1");

                    if (chk != null)
                    {
                        chk.Visible = false;
                    }

                    // Hide Approve button
                    LinkButton btnApprove = (LinkButton)e.Row.FindControl("btnRowApprove");
                    if (btnApprove != null)
                    {
                        btnApprove.Visible = false;
                    }

                    // Hide Reject button
                    LinkButton btnReject = (LinkButton)e.Row.FindControl("btnRowReject");
                    if (btnReject != null)
                    {
                        btnReject.Visible = false;
                    }
                }
            }
        }

        private void LoadRequestDetails(int requestId, int status)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                using (SqlCommand cmd = new SqlCommand("SP_SOApp_TrackByManager", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                    cmd.Parameters.AddWithValue("@ActionType", "GetRequestDetails");
                    cmd.Parameters.AddWithValue("@RequestId", requestId);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];

                            if (status == 4)
                            {
                                // Show Remarks section
                                RemarksDiv.Visible = true;
                                ObjectivesDiv.Visible = false;
                                UpdateBtn.Visible = false;
                                ForwardBtn.Visible = true;

                                exampleModalLongTitle.InnerText = "Remarks/Feedback";

                                txtRemarks.Text = row["Remarks"].ToString();
                            }
                            else
                            {
                                // Show Objectives section
                                ObjectivesDiv.Visible = true;
                                RemarksDiv.Visible = false;
                                UpdateBtn.Visible = true;
                                ForwardBtn.Visible = false;

                                exampleModalLongTitle.InnerText = "Objectives";

                                txtTraining.Text = row["Training"].ToString();
                                txtCareer.Text = row["Career"].ToString();
                                txtSignIn.Text = row["UserName"].ToString();

                                string rating = row["Rating"].ToString();
                                hdnRating.Value = rating;

                                ScriptManager.RegisterStartupScript(this, this.GetType(),
                                    "setRating", $"setRating({rating});", true);
                            }
                        }
                    }
                }

                con.Close();
            }
            catch (Exception ex)
            {
                LogError("LoadRequestDetails Error", ex);
                showToast("Error loading request details", "toast-danger");
            }
        }

        private DataSet LoadRequestAchivements(string SOCode, string PCYear, string Quarter)
        {
            DataSet ds = new DataSet();   

            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                using (SqlCommand cmd = new SqlCommand("SP_SOApp_SO_DashBoardLoad", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                    cmd.Parameters.AddWithValue("@ActionType", "Fetch");
                    cmd.Parameters.AddWithValue("@SOCode", SOCode);
                    cmd.Parameters.AddWithValue("@PcYear", PCYear);
                    cmd.Parameters.AddWithValue("@Quarter", Quarter);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ds);
                    }
                }

                con.Close();
            }
            catch (Exception ex)
            {
                LogError("LoadRequestDetails Error", ex);
                showToast("Error loading request details", "toast-danger");
            }

            return ds;
        }

        private void CreateCustomSheet(XLWorkbook wb, DataSet ds,
                               int startIndex, int endIndex,
                               string sheetName,
                               string[] tableNames)
        {
            var ws = wb.Worksheets.Add(sheetName);

            int currentRow = 1;
            int currentCol;
            int maxHeightTop = 0;

            // ===== TOP 5 TABLES =====
            currentCol = 1;

            for (int i = startIndex; i < startIndex + 5 && i < ds.Tables.Count; i++)
            {
                DataTable dt = ds.Tables[i];

                ws.Cell(currentRow, currentCol).Value =
                    tableNames[i - startIndex];

                ws.Cell(currentRow, currentCol).Style.Font.Bold = true;
                ws.Cell(currentRow, currentCol).Style.Font.FontSize = 10;

                var table = ws.Cell(currentRow + 1, currentCol)
                              .InsertTable(dt, false);

                FormatTable(table);

                int tableWidth = dt.Columns.Count;
                int tableHeight = dt.Rows.Count + 1;

                maxHeightTop = Math.Max(maxHeightTop, tableHeight);

                currentCol += tableWidth + 1;
            }

            currentRow += maxHeightTop + 3;
            currentCol = 1;

            // ===== NEXT 5 TABLES =====
            for (int i = startIndex + 5; i <= endIndex && i < ds.Tables.Count; i++)
            {
                DataTable dt = ds.Tables[i];

                ws.Cell(currentRow, currentCol).Value =
                    tableNames[i - startIndex];

                ws.Cell(currentRow, currentCol).Style.Font.Bold = true;
                ws.Cell(currentRow, currentCol).Style.Font.FontSize = 10;

                var table = ws.Cell(currentRow + 1, currentCol)
                              .InsertTable(dt, false);

                FormatTable(table);

                int tableWidth = dt.Columns.Count;

                currentCol += tableWidth + 1;
            }

            ws.Columns().AdjustToContents();
        }

        private void FormatTable(IXLTable table)
        {
            table.ShowAutoFilter = false;        // Remove filter
            table.Theme = XLTableTheme.None;     // Remove color theme

            var range = table.AsRange();

            range.Style.Font.FontSize = 10;
            range.Style.Font.Bold = false;

            // Header bold
            range.FirstRow().Style.Font.Bold = true;

            // Borders
            range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        }

        string[] primaryNames = new string[]
        {
            "Last Year",
            "Plan",
            "Achievement(PresentYear)",
            "% Achievement",
            "GOLY",
            "Last Year",
            "Plan",
            "Achievement",
            "% Achievement",
            "GOLY"
        };

        string[] secondaryNames = new string[]
        {
            "Last Year",
            "Plan",
            "Achievement(PresentYear)",
            "% Achievement",
            "GOLY",
            "Last Year",
            "Plan",
            "Achievement",
            "% Achievement",
            "GOLY"
        };
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
                cmd1.Parameters.AddWithValue("@Page", "TrackbyManager");
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

        protected void UpdateBtn_Click(object sender, EventArgs e)
        {
            try
            {
                int requestId = Convert.ToInt32(hdnRequestId.Value);

                string training = txtTraining.Text.Trim();
                string career = txtCareer.Text.Trim();
                string signIn = txtSignIn.Text.Trim();

                decimal rating = 0;

                if (!string.IsNullOrEmpty(hdnRating.Value))
                    rating = Convert.ToDecimal(hdnRating.Value);

                showToast("Request Id is : " + requestId + " rating is : " + rating, "toast-success");

                // Now you can use these values for DB update

            }
            catch (Exception ex)
            {
                LogError("Update Button Error", ex);
                showToast("Something went wrong while updating.", "toast-danger");
            }
        }

        protected void Forward_Click(object sender, EventArgs e)
        {
            try
            {
                int requestId = Convert.ToInt32(hdnRequestId.Value);

                string training = txtTraining.Text.Trim();
                string career = txtCareer.Text.Trim();
                string signIn = txtSignIn.Text.Trim();

                decimal rating = 0;

                if (!string.IsNullOrEmpty(hdnRating.Value))
                    rating = Convert.ToDecimal(hdnRating.Value);

                showToast("Request Id is : " + requestId + " rating is : " + rating, "toast-success");

                // Now you can use these values for DB update

            }
            catch (Exception ex)
            {
                LogError("Forward Button Error", ex);
                showToast("Something went wrong while updating.", "toast-danger");
            }
        }
    }
}