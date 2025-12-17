using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SO_Appraisal
{
    public partial class Transfer : System.Web.UI.Page
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
                StateLoad();
            }
        }

        #region AccessLoad
        public void AccessLoad()
        {
            try
            {
                string remoteUser = "G112377";
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
                LogError("Access load Error", ex);
                showToast("Something went wrong. Please try again later or contact the SYNAPSE team", "toast-danger");
            }
        }
        #endregion

        #region StateLoad
        public void StateLoad()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_SOApp_Transfer_Dropdowns", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@ActionType", "StateLoad");
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@StateId", "");
                cmd1.Parameters.AddWithValue("@Area", "");
                cmd1.Parameters.AddWithValue("@FromZoneName", "");
                cmd1.Parameters.AddWithValue("@DistCode", "");
                cmd1.Parameters.AddWithValue("@ToZoneName", "");
                cmd1.Parameters.AddWithValue("@FromSOCode", "");
                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Rows.Clear();
                da.Fill(resdt);
                StateDrp.DataSource = resdt;
                StateDrp.DataTextField = resdt.Columns["StateName"].ToString();
                StateDrp.DataValueField = resdt.Columns["StateId"].ToString();
                StateDrp.DataBind();
                StateDrp.Items.Insert(0, new ListItem("State", ""));
                con.Close();
            }
            catch (Exception ex)
            {
                LogError("State Load Error", ex);
                showToast("Something went wrong. Please try again later or contact the SYNAPSE team", "toast-danger");
            }
        }
        #endregion

        #region AreaLoad
        public void AreaLoad()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_SOApp_Transfer_Dropdowns", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@ActionType", "AreaLoad");
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@StateId", StateDrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@Area", "");
                cmd1.Parameters.AddWithValue("@FromZoneName", "");
                cmd1.Parameters.AddWithValue("@DistCode", "");
                cmd1.Parameters.AddWithValue("@ToZoneName", "");
                cmd1.Parameters.AddWithValue("@FromSOCode", "");
                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Rows.Clear();
                da.Fill(resdt);
                AreaDrp.DataSource = resdt;
                AreaDrp.DataTextField = resdt.Columns["AreaName"].ToString();
                AreaDrp.DataValueField = resdt.Columns["AreaCode"].ToString();
                AreaDrp.DataBind();
                AreaDrp.Items.Insert(0, new ListItem("Area", ""));
                con.Close();
            }
            catch (Exception ex)
            {
                LogError("Area Load Error", ex);
                showToast("Something went wrong. Please try again later or contact the SYNAPSE team", "toast-danger");
            }
        }
        #endregion

        #region FromZoneLoad
        public void ZoneLoad()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_SOApp_Transfer_Dropdowns", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@ActionType", "FromZoneLoad");
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@StateId", StateDrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@Area", AreaDrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@FromZoneName", "");
                cmd1.Parameters.AddWithValue("@DistCode", "");
                cmd1.Parameters.AddWithValue("@ToZoneName", "");
                cmd1.Parameters.AddWithValue("@FromSOCode", "");
                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Rows.Clear();
                da.Fill(resdt);
                ZoneDrp.DataSource = resdt;
                ZoneDrp.DataTextField = resdt.Columns["ZoneName"].ToString();
                ZoneDrp.DataValueField = resdt.Columns["ZoneCode"].ToString();
                ZoneDrp.DataBind();
                ZoneDrp.Items.Insert(0, new ListItem("From Zone", ""));
                con.Close();
            }
            catch (Exception ex)
            {
                LogError("From Zone Load Error", ex);
                showToast("Something went wrong. Please try again later or contact the SYNAPSE team", "toast-danger");
            }
        }
        #endregion

        #region FromSOLoad
        public void FromSOLoad()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_SOApp_Transfer_Dropdowns", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@ActionType", "FromSoLoad");
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@StateId", StateDrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@Area", AreaDrp.SelectedItem.ToString());
                cmd1.Parameters.AddWithValue("@FromZoneName", ZoneDrp.SelectedItem.ToString());
                cmd1.Parameters.AddWithValue("@DistCode", "");
                cmd1.Parameters.AddWithValue("@ToZoneName", "");
                cmd1.Parameters.AddWithValue("@FromSOCode", "");
                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Rows.Clear();
                da.Fill(resdt);
                FromSODrp.DataSource = resdt;
                FromSODrp.DataTextField = resdt.Columns["SOName"].ToString();
                FromSODrp.DataValueField = resdt.Columns["SOCode"].ToString();
                FromSODrp.DataBind();
                FromSODrp.Items.Insert(0, new ListItem("From SO", ""));
                con.Close();
            }
            catch (Exception ex)
            {
                LogError("From SOLoad Error", ex);
                showToast("Something went wrong. Please try again later or contact the SYNAPSE team", "toast-danger");
            }
        }
        #endregion

        #region DistModalLoad
        public void DistModalLoad()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_SOApp_Transfer_Dropdowns", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@ActionType", "DistModal");
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@StateId", StateDrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@Area", AreaDrp.SelectedItem.ToString());
                cmd1.Parameters.AddWithValue("@FromZoneName", ZoneDrp.SelectedItem.ToString());
                cmd1.Parameters.AddWithValue("@DistCode", "");
                cmd1.Parameters.AddWithValue("@ToZoneName", "");
                cmd1.Parameters.AddWithValue("@FromSOCode", FromSODrp.SelectedValue);
                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Rows.Clear();
                da.Fill(resdt);
                DistModal.DataSource = resdt;
                DistModal.DataBind();
                con.Close();

                if (resdt.Rows.Count == 1)
                {
                    showToast("At least one Distributor will remain in Transfer case", "toast-danger");
                }
            }
            catch (Exception ex)
            {
                LogError("Distributor Load Error", ex);
                showToast("Something went wrong. Please try again later or contact the SYNAPSE team", "toast-danger");
            }
        }
        #endregion

        #region SelectBtn_Click
        protected void SelectBtn_Click(object sender, EventArgs e)
        {
            ToZoneLoad();
        }
        #endregion

        #region ToZoneLoad
        public void ToZoneLoad()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_SOApp_Transfer_Dropdowns", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@ActionType", "ToZoneLoad");
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@StateId", StateDrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@Area", AreaDrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@FromZoneName", ZoneDrp.SelectedItem.ToString());
                cmd1.Parameters.AddWithValue("@DistCode", "");
                cmd1.Parameters.AddWithValue("@ToZoneName", "");
                cmd1.Parameters.AddWithValue("@FromSOCode", "");
                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Rows.Clear();
                da.Fill(resdt);
                ToZoneLoadDrp.DataSource = resdt;
                ToZoneLoadDrp.DataTextField = resdt.Columns["ZoneName"].ToString();
                ToZoneLoadDrp.DataValueField = resdt.Columns["ZoneCode"].ToString();
                ToZoneLoadDrp.DataBind();
                ToZoneLoadDrp.Items.Insert(0, new ListItem("To Zone", ""));
                con.Close();
            }
            catch (Exception ex)
            {
                LogError("To ZoneLoad Error", ex);
                showToast("Something went wrong. Please try again later or contact the SYNAPSE team", "toast-danger");
            }
        }
        #endregion

        #region ToSOLoad
        public void ToSOLoad()
        {
            try
            {
                List<string> checkedDists = new List<string>();

                foreach (GridViewRow row in DistModal.Rows)
                {
                    HtmlInputCheckBox chkBox = (HtmlInputCheckBox)row.FindControl("CheckBox1");
                    if (chkBox != null && chkBox.Checked)
                    {
                        string distCode = DistModal.DataKeys[row.RowIndex].Value.ToString();
                        checkedDists.Add(distCode);
                    }
                }

                string dists = string.Join(",", checkedDists);
                ViewState["dists"] = dists;

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_SOApp_Transfer_Dropdowns", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@ActionType", "ToSoLoad");
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@StateId", StateDrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@Area", AreaDrp.SelectedItem.ToString());
                cmd1.Parameters.AddWithValue("@FromZoneName", "");
                cmd1.Parameters.AddWithValue("@DistCode", dists);
                cmd1.Parameters.AddWithValue("@ToZoneName", ToZoneLoadDrp.SelectedItem.ToString());
                cmd1.Parameters.AddWithValue("@FromSOCode", FromSODrp.SelectedValue);
                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Rows.Clear();
                da.Fill(resdt);
                ToSODrp.DataSource = resdt;
                ToSODrp.DataTextField = resdt.Columns["SOName"].ToString();
                ToSODrp.DataValueField = resdt.Columns["SOCode"].ToString();
                ToSODrp.DataBind();
                ToSODrp.Items.Insert(0, new ListItem("To SO", ""));
                con.Close();
            }
            catch (Exception ex)
            {
                LogError("To SOLoad Error", ex);
                showToast("Something went wrong. Please try again later or contact the SYNAPSE team", "toast-danger");
            }
        }
        #endregion

        #region UploFileBtn_Click
        protected void SaveModalUploFileBtn_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "openTransferModal();", true);

                if (!FileUpload_Id.HasFile)
                {
                    showToast("Please select a file to upload.", "toast-danger");
                    return;
                }

                string ext = System.IO.Path.GetExtension(FileUpload_Id.FileName).ToLowerInvariant();
                string[] allowed = { ".msg", ".mht", ".eml", ".jpg", ".jpeg", ".png", ".pdf" };

                if (!allowed.Contains(ext))
                {
                    showToast("Invalid file type. Allowed: .msg, .mht, .eml, .jpg, .jpeg, .png, .pdf", "toast-danger");
                    return;
                }

                // choose your upload path
                string folder = Server.MapPath("~/Uploads/TransferProof/");
                if (!System.IO.Directory.Exists(folder))
                {
                    System.IO.Directory.CreateDirectory(folder);
                }

                string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + FileUpload_Id.FileName;
                string fullPath = System.IO.Path.Combine(folder, fileName);

                FileUpload_Id.SaveAs(fullPath);

                // store relative path for use later (e.g., in db)
                string relativePath = "~/Uploads/TransferProof/" + fileName;
                string virtualPath = "/Uploads/TransferProof/" + fileName;
                ViewState["attachmentPath"] = virtualPath;

                DataTable dt = FilesTable;
                DataRow row = dt.NewRow();
                row["FileName"] = FileUpload_Id.FileName;
                row["FilePath"] = relativePath;
                dt.Rows.Add(row);
                FilesTable = dt;
                BindFilesGrid();

                showToast("File added successfully.", "toast-success");
            }
            catch (Exception ex)
            {
                LogError("File Upload Error", ex);
                showToast("Something went wrong. Please try again later or contact the SYNAPSE team", "toast-danger");
            }
        }
        #endregion

        #region gvFiles_RowCommand
        protected void gvFiles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteFile")
            {
                string filePath = e.CommandArgument.ToString();

                DataTable dt = FilesTable;
                DataRow[] rows = dt.Select($"FilePath = '{filePath.Replace("'", "''")}'");
                foreach (DataRow r in rows)
                {
                    dt.Rows.Remove(r);
                }

                // delete physical file if needed
                string physical = Server.MapPath(filePath);
                if (System.IO.File.Exists(physical))
                {
                    System.IO.File.Delete(physical);
                }

                BindFilesGrid();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "openTransferModal();", true);
                showToast("File removed successfully.", "toast-success");
            }
        }

        private DataTable FilesTable
        {
            get
            {
                if (Session["UploadedFiles"] == null)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("FileName", typeof(string));
                    dt.Columns.Add("FilePath", typeof(string)); // physical or virtual path
                    Session["UploadedFiles"] = dt;
                }
                return (DataTable)Session["UploadedFiles"];
            }
            set
            {
                ViewState["FilesTable"] = value;
            }
        }

        public void BindFilesGrid()
        {
            try
            {
                if (FilesTable.Rows.Count > 0)
                {
                    FileUploadDiv.Visible = false;
                }
                else
                {
                    FileUploadDiv.Visible = true;
                }

                gvFiles.DataSource = FilesTable;
                gvFiles.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Transfer_Submit_Click
        protected void Transfer_Submit_Click(object sender, EventArgs e)
        {
            try
            {
                string dists = ViewState["dists"] as string ?? string.Empty;
                string virtualPath = ViewState["attachmentPath"] as string ?? string.Empty;

                if (StateSearch.Value == "" || StateSearch.Value == null || StateDrp.SelectedValue == "" || StateDrp.SelectedValue == null)
                {
                    showToast("Please select State", "toast-danger");
                    return;
                }
                else if (AreaSearch.Value == "" || AreaSearch.Value == null || AreaDrp.SelectedValue == "" || AreaDrp.SelectedValue == null)
                {
                    showToast("Please select Area", "toast-danger");
                    return;
                }
                else if (ZoneDrpSearch.Value == "" || ZoneDrpSearch.Value == null || ZoneDrp.SelectedValue == "" || ZoneDrp.SelectedValue == null)
                {
                    showToast("Please select Zone", "toast-danger");
                    return;
                }
                else if (FromSOSearch.Value == "" || FromSOSearch.Value == null || FromSODrp.SelectedValue == "" || FromSODrp.SelectedValue == null)
                {
                    showToast("Please select From SO", "toast-danger");
                    return;
                }
                else if (dists == "" || dists == null)
                {
                    showToast("Please select atleast one distributor", "toast-danger");
                    return;
                }
                else if (ToZoneLoadDrpSearch.Value == "" || ToZoneLoadDrpSearch.Value == null || ToZoneLoadDrp.SelectedValue == "" || ToZoneLoadDrp.SelectedValue == null)
                {
                    showToast("Please select To Zone", "toast-danger");
                    return;
                }
                else if (ToSODrpSearch.Value == "" || ToSODrpSearch.Value == null || ToSODrp.SelectedValue == "" || ToSODrp.SelectedValue == null)
                {
                    showToast("Please select To SO", "toast-danger");
                    return;
                }
                else if (virtualPath == "" || virtualPath == null)
                {
                    showToast("Please upload any file", "toast-danger");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "openTransferModal();", true);
                    return;
                }
                else if (txtRemarks.Text == "" || txtRemarks.Text == null)
                {
                    showToast("Reason should not be blank", "toast-danger");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "openTransferModal();", true);
                    return;
                }

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_SOApp_Transfer_Newlogic", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@State", StateDrp.SelectedItem.ToString());
                cmd1.Parameters.AddWithValue("@Area", AreaDrp.SelectedItem.ToString());
                cmd1.Parameters.AddWithValue("@DistCode", dists);
                cmd1.Parameters.AddWithValue("@FromSOCode", FromSODrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@ToSOCode", ToSODrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@FromZone", ZoneDrp.SelectedItem.ToString());
                cmd1.Parameters.AddWithValue("@ToZone", ToZoneLoadDrp.SelectedItem.ToString());
                cmd1.Parameters.AddWithValue("@Reason", txtRemarks.Text);
                cmd1.Parameters.AddWithValue("@Attachment", virtualPath);
                cmd1.Parameters.AddWithValue("@Status", "Pending for Approval");
                cmd1.Parameters.AddWithValue("@LogFor", "Transfer");
                cmd1.CommandTimeout = 6000;
                cmd1.ExecuteNonQuery();

                con.Close();

                showToast("Distributor(s) Transferred Successfully!", "toast-success");

                ClearForm();
            }
            catch (Exception ex)
            {
                LogError("Transfer Submit Error", ex);
                showToast("Something went wrong. Please try again later or contact the SYNAPSE team", "toast-danger");
            }
        }
        #endregion

        #region SelectedIndexChanged
        protected void StateDrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            AreaLoad();
        }

        protected void AreaDrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            ZoneLoad();
        }

        protected void ZoneDrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            FromSOLoad();
        }

        protected void FromSODrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            DistModalLoad();
        }

        protected void ToZoneLoadDrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToSOLoad();
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
                cmd1.Parameters.AddWithValue("@Page", "Transfer");
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

        #region ClearForm
        public void ClearForm()
        {
            // Clear Search Boxes
            StateSearch.Value = string.Empty;
            AreaSearch.Value = string.Empty;
            ZoneDrpSearch.Value = string.Empty;
            FromSOSearch.Value = string.Empty;
            ToZoneLoadDrpSearch.Value = string.Empty;
            ToSODrpSearch.Value = string.Empty;

            // Reset DropDownLists
            StateDrp.ClearSelection();
            AreaDrp.ClearSelection();
            ZoneDrp.ClearSelection();
            FromSODrp.ClearSelection();
            ToZoneLoadDrp.ClearSelection();
            ToSODrp.ClearSelection();

            StateDrp.SelectedValue = string.Empty;
            AreaDrp.SelectedValue = string.Empty;
            ZoneDrp.SelectedValue = string.Empty;
            FromSODrp.SelectedValue = string.Empty;
            ToZoneLoadDrp.SelectedValue = string.Empty;
            ToSODrp.SelectedValue = string.Empty;

            // Clear Distributors checkboxes inside DistModal Grid
            foreach (GridViewRow row in DistModal.Rows)
            {
                HtmlInputCheckBox chk = (HtmlInputCheckBox)row.FindControl("CheckBox1");
                if (chk != null)
                {
                    chk.Checked = false;
                    chk.Disabled = false;
                    chk.Style["border-color"] = "";  // reset red color if applied
                }
            }

            // Clear Distributor Modal grid
            DistModal.DataSource = null;
            DistModal.DataBind();

            // Clear Uploaded Files Grid & ViewState table
            ViewState["FilesTable"] = null;
            gvFiles.DataSource = null;
            gvFiles.DataBind();

            ViewState["dists"] = null;
            ViewState["attachmentPath"] = null;
            Session["UploadedFiles"] = null;

            // Clear Remarks
            txtRemarks.Text = string.Empty;

            // Clear FileUpload control by forcing re-render
            FileUpload_Id.Attributes.Clear(); // keeps HTML clean

            // Clear stored dynamic values
            ViewState["dists"] = null;
            ViewState["attachmentPath"] = null;

            FileUploadDiv.Visible = true;

            // Reset the files table to a fresh empty table
            FilesTable = CreateEmptyFilesTable();
            BindFilesGrid();
        }
        #endregion

        #region Helpers
        // helper used by both property-init and ClearForm
        private DataTable CreateEmptyFilesTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("FileName", typeof(string));
            dt.Columns.Add("FilePath", typeof(string));
            return dt;
        }

        protected void TestBtn_Click(object sender, EventArgs e)
        {
            showToast("Toast is working fine", "toast-success");
        }
        #endregion



    }
}