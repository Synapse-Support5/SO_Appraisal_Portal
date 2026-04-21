using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SO_Appraisal
{
    public partial class SODashBoard : BasePage
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["SqlConn"].ToString());
        DataTable dt = new DataTable();
        DataTable resdt = new DataTable();
        DataSet ds = new DataSet();
        public DataSet resds = new DataSet();
        string Button, remoteUser, SOCode;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string role = Session["Role"]?.ToString();

                if (role != "ADMIN" && role != "HR" && role != "SO")
                {
                    Response.Redirect("AccessDeniedPage.aspx");
                }

                AccessLoad();
                BindFYDropdown();
                //LoadAllTables();

            }
        }

        #region AccessLoad
        public void AccessLoad()
        {
            try
            {
                //string remoteUser = "G116036";
                //string remoteUser = Request.ServerVariables["REMOTE_USER"];
                remoteUser = Session["UserId"].ToString();
                //remoteUser = "8474RU";

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
                            lblUserName.Text = "Welcome, " + resdt.Rows[0][1].ToString() + "_" + resdt.Rows[0][0].ToString();
                            Session["Username"] = resdt.Rows[0][1].ToString();
                            hdnBusinessType.Value = resdt.Rows[0][2].ToString();
                            hdnRole.Value = resdt.Rows[0][3].ToString();

                            Session["Role"] = hdnRole.Value;

                            if (hdnRole.Value == "HR" || hdnRole.Value == "ADMIN")
                            {
                                SODrpDiv.Visible = true;
                                SOLoad();

                                BtnDiv.Visible = false;
                            }
                            else
                            {
                                SODrpDiv.Visible = false;
                            }

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

        #region SOLoad
        public void SOLoad()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                using (SqlCommand cmd1 = new SqlCommand("SP_SOApp_SO_DashBoardLoad", con))
                {
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                    cmd1.Parameters.AddWithValue("@ActionType", "SOLoad");
                    cmd1.Parameters.AddWithValue("@SOCode", "");
                    cmd1.Parameters.AddWithValue("@PcYear", "");
                    cmd1.Parameters.AddWithValue("@Quarter", "");
                    cmd1.CommandTimeout = 6000;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                    {
                        resdt.Rows.Clear();
                        da.Fill(resdt);

                        SODrp.DataSource = resdt;
                        SODrp.DataTextField = resdt.Columns["SOName"].ToString();
                        SODrp.DataValueField = resdt.Columns["SOCode"].ToString();
                        SODrp.DataBind();
                        SODrp.Items.Insert(0, new ListItem("Select SO", ""));

                    }
                }

                con.Close();

            }
            catch (Exception ex)
            {
                LogError("Quarter load Error", ex);
                showToast("Something went wrong. Please try again later or contact the SYNAPSE team", "toast-danger");
            }
        }
        #endregion

        #region BindFYDropdown
        public void BindFYDropdown()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                using (SqlCommand cmd1 = new SqlCommand("SP_SOApp_SO_DashBoardLoad", con))
                {
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                    cmd1.Parameters.AddWithValue("@ActionType", "Landing");
                    cmd1.Parameters.AddWithValue("@SOCode", Session["name"].ToString());
                    cmd1.Parameters.AddWithValue("@PcYear", "");
                    cmd1.Parameters.AddWithValue("@Quarter", "");

                    cmd1.CommandTimeout = 6000;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                    {
                        ds.Clear();
                        da.Fill(ds);

                        // ✅ First Result Set → Geo
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            if (ds.Tables[0].Rows[0]["Geo"].ToString() != "")
                            {
                                lblGeo.Text = "Geo : " + ds.Tables[0].Rows[0]["Geo"].ToString();
                            }
                            else
                            {
                                lblGeo.Text = "";
                            }

                        }

                        // ✅ Second Result Set → FY
                        if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                        {
                            DataTable dtFY = ds.Tables[1];

                            FYDrp.DataSource = dtFY;
                            FYDrp.DataTextField = "PcYear";   // Column name
                            FYDrp.DataValueField = "PcYear";   // Column name
                            FYDrp.DataBind();

                            FYDrp.Items.Insert(0, new ListItem("Select FY", ""));

                            // ✅ Auto select where IsCurrent = 1
                            DataRow[] currentRow = dtFY.Select("IsCurrent = 1");
                            if (currentRow.Length > 0)
                            {
                                FYDrp.SelectedValue = currentRow[0]["PcYear"].ToString();

                                QuarterLoad();

                                //FetchAllData();

                                //if (resds.Tables.Count > 0 && resds.Tables[0].Rows.Count > 0)
                                //{
                                //    var count = resds.Tables[0].Rows[0]["DistCount"].ToString();
                                //    DstCountLbl.InnerText = $" ({count})";
                                //}
                                //else
                                //{
                                //    DstCountLbl.InnerText = "";
                                //}
                            }

                        }

                        // ✅ Third Result Set → Button status
                        if (ds.Tables.Count > 1 && ds.Tables[2].Rows.Count > 0)
                        {
                            Button = ds.Tables[2].Rows[0]["Status"].ToString();
                            Session["Button"] = Button;
                        }
                    }
                }

                con.Close();
            }
            catch (Exception ex)
            {
                LogError("FY and Geo Load Error", ex);
                showToast("Something went wrong. Please try again later or contact the SYNAPSE team", "toast-danger");
            }
        }
        #endregion

        #region QuarterLoad
        public void QuarterLoad()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                using (SqlCommand cmd1 = new SqlCommand("SP_SOApp_SO_DashBoardLoad", con))
                {
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                    cmd1.Parameters.AddWithValue("@ActionType", "QuarterLoad");
                    cmd1.Parameters.AddWithValue("@SOCode", Session["name"].ToString());
                    cmd1.Parameters.AddWithValue("@PcYear", FYDrp.SelectedValue);
                    cmd1.Parameters.AddWithValue("@Quarter", "");

                    cmd1.CommandTimeout = 6000;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                    {
                        resdt.Rows.Clear();
                        da.Fill(resdt);
                        QtrDrp.DataSource = resdt;
                        QtrDrp.DataTextField = resdt.Columns["QuarterText"].ToString();
                        QtrDrp.DataValueField = resdt.Columns["QuarterText"].ToString();
                        QtrDrp.DataBind();
                        QtrDrp.Items.Insert(0, new ListItem("Qtr From", ""));
                    }
                }
                con.Close();

            }
            catch (Exception ex)
            {
                LogError("Quarter load Error", ex);
                showToast("Something went wrong. Please try again later or contact the SYNAPSE team", "toast-danger");
            }
        }
        #endregion

        #region FetchAllData
        public void FetchAllData()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                using (SqlCommand cmd1 = new SqlCommand("SP_SOApp_SO_DashBoardLoad", con))
                {
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                    cmd1.Parameters.AddWithValue("@ActionType", "Fetch");
                    cmd1.Parameters.AddWithValue("@SOCode", SOCode);
                    cmd1.Parameters.AddWithValue("@PcYear", FYDrp.SelectedValue);
                    cmd1.Parameters.AddWithValue("@Quarter", QtrDrp.SelectedValue);

                    cmd1.CommandTimeout = 6000;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                    {
                        resds.Clear();
                        da.Fill(resds);

                        Session["DashData"] = resds;
                    }
                }

                con.Close();
            }
            catch (Exception ex)
            {
                LogError("Fetch All Data Error", ex);
                showToast("Something went wrong. Please try again later or contact the SYNAPSE team", "toast-danger");
            }
        }
        #endregion

        #region SelectedIndexChanged
        protected void FYDrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            QuarterLoad();
        }

        protected void QtrDrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            string role = Convert.ToString(Session["Role"]).Trim().ToUpper();

            if (role == "SO")
            {
                SOCode = Session["name"].ToString();
                Session["SO_Code"] = SOCode;
            }
            else
            {
                SOCode = SODrp.SelectedValue;
                Session["SO_Code"] = SOCode;
            }

            FetchAllData();
        }

        protected void TypeDrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TypeDrp.SelectedValue == "Primary")
            {
                PrimaryLoad();
                ButtonVisibilityHelper();

            }
            else if (TypeDrp.SelectedValue == "Secondary")
            {
                SecondaryLoad();
                ButtonVisibilityHelper();
            }
            else if (TypeDrp.SelectedValue == "Distributors")
            {
                DistributorLoad();
                ButtonVisibilityHelper();
            }
            else
            {
                PriSecDiv.Visible = false;
                btn_Proceed.Visible = false;
                btn_Common.Visible = false;
            }

        }
        #endregion

        #region PrimaryLoad
        public void PrimaryLoad()
        {
            try
            {
                distDiv.Visible = false;

                if (Session["DashData"] != null)
                {
                    PriSecDiv.Visible = true;

                    resds = (DataSet)Session["DashData"];

                    //Sales Value ------------
                    gvSalesLastYear.DataSource = resds.Tables[0];
                    gvSalesLastYear.DataBind();

                    gvSalesPlan.DataSource = resds.Tables[1];
                    gvSalesPlan.DataBind();

                    gvSalesAchievement.DataSource = resds.Tables[2];
                    gvSalesAchievement.DataBind();

                    gvSalesPerAchievement.DataSource = resds.Tables[3];
                    gvSalesPerAchievement.DataBind();

                    gvSalesGoly.DataSource = resds.Tables[4];
                    gvSalesGoly.DataBind();
                    //------------------------

                    //Brand Volume -----------
                    gvBrandLastYear.DataSource = resds.Tables[5];
                    gvBrandLastYear.DataBind();

                    gvBrandPlan.DataSource = resds.Tables[6];
                    gvBrandPlan.DataBind();

                    gvBrandAchievement.DataSource = resds.Tables[7];
                    gvBrandAchievement.DataBind();

                    gvBrandPerAchievement.DataSource = resds.Tables[8];
                    gvBrandPerAchievement.DataBind();

                    gvBrandGoly.DataSource = resds.Tables[9];
                    gvBrandGoly.DataBind();
                    //------------------------

                    smdiv.Visible = false;
                }
                else
                {
                    PriSecDiv.Visible = false;
                    showToast("Something went wrong. Please try again later or contact the SYNAPSE team", "toast-danger");
                }

            }
            catch (Exception ex)
            {
                LogError("Primary Load Error", ex);
                showToast("Something went wrong. Please try again later or contact the SYNAPSE team", "toast-danger");
            }
        }
        #endregion

        #region SecondaryLoad
        public void SecondaryLoad()
        {
            try
            {
                distDiv.Visible = false;

                if (Session["DashData"] != null)
                {
                    PriSecDiv.Visible = true;

                    resds = (DataSet)Session["DashData"];

                    //Sales Value ------------
                    gvSalesLastYear.DataSource = resds.Tables[10];
                    gvSalesLastYear.DataBind();

                    gvSalesPlan.DataSource = resds.Tables[11];
                    gvSalesPlan.DataBind();

                    gvSalesAchievement.DataSource = resds.Tables[12];
                    gvSalesAchievement.DataBind();

                    gvSalesPerAchievement.DataSource = resds.Tables[13];
                    gvSalesPerAchievement.DataBind();

                    gvSalesGoly.DataSource = resds.Tables[14];
                    gvSalesGoly.DataBind();
                    //------------------------

                    //Brand Volume -----------
                    gvBrandLastYear.DataSource = resds.Tables[15];
                    gvBrandLastYear.DataBind();

                    gvBrandAchievement.DataSource = resds.Tables[16];
                    gvBrandAchievement.DataBind();

                    gvBrandPlan.DataSource = resds.Tables[17];      //ECO
                    gvBrandPlan.DataBind();                         //ECO 

                    gvBrandPerAchievement.DataSource = resds.Tables[18];
                    gvBrandPerAchievement.DataBind();

                    gvBrandGoly.DataSource = resds.Tables[19];
                    gvBrandGoly.DataBind();
                    //------------------------

                    //ECO -----------
                    gvSMLastYear.DataSource = resds.Tables[20];
                    gvSMLastYear.DataBind();

                    gvSMAchievement.DataSource = resds.Tables[21];
                    gvSMAchievement.DataBind();

                    gvSMEco.DataSource = resds.Tables[22];
                    gvSMEco.DataBind();

                    //------------------------
                    smdiv.Visible = true;

                }
                else
                {
                    PriSecDiv.Visible = false;
                    showToast("Something went wrong. Please try again later or contact the SYNAPSE team", "toast-danger");
                }

            }
            catch (Exception ex)
            {
                LogError("Secondary Load Error", ex);
                showToast("Something went wrong. Please try again later or contact the SYNAPSE team", "toast-danger");
            }
        }
        #endregion

        #region DistributorLoad
        public void DistributorLoad()
        {
            try
            {
                PriSecDiv.Visible = false;
                if (Session["DashData"] != null)
                {
                    distDiv.Visible = true;

                    resds = (DataSet)Session["DashData"];

                    //Sales Value ------------
                    if (resds.Tables[20] != null)
                    {
                        gvDistributors.DataSource = resds.Tables[23];
                        gvDistributors.DataBind();

                        var count = resds.Tables[23].Rows.Count;
                        DstCountLbl.InnerText = $" ({count})";

                        //statusBtnDiv.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("Distributor Load Error", ex);
                showToast("Something went wrong. Please try again later or contact the SYNAPSE team", "toast-danger");
            }
        }
        #endregion

        #region DistCountBtn_Click
        protected void DistCountBtn_Click(object sender, EventArgs e)
        {
            TypeDrp.SelectedValue = "Distributors";
            DistributorLoad();

            ButtonVisibilityHelper();
        }
        #endregion

        #region ExportBtn_Click
        protected void ExportBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["DashData"] == null)
                {
                    showToast("No data available to export.", "toast-danger");
                    return;
                }

                DataSet ds = (DataSet)Session["DashData"];

                // 🔴 Check if any table is empty
                bool hasBlankTable = ds.Tables.Count == 0 ||
                                     ds.Tables.Cast<DataTable>().Any(t => t.Rows.Count == 0);

                if (hasBlankTable)
                {
                    showToast("Export not allowed. Some dashboard tables are empty.", "toast-danger");
                    return;
                }

                using (ClosedXML.Excel.XLWorkbook wb = new ClosedXML.Excel.XLWorkbook())
                {
                    CreateCustomSheet(wb, ds, 0, 9, "Primary", primaryNames);
                    CreateCustomSheet(wb, ds, 10, 22, "Secondary", secondaryNames);

                    // ================= Distributor Sheet =================
                    if (ds.Tables.Count > 23)
                    {
                        var wsDist = wb.Worksheets.Add("Distributors");
                        var table = wsDist.Cell(1, 1).InsertTable(ds.Tables[23], false);

                        FormatTable(table);
                        wsDist.Columns().AdjustToContents();
                    }

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";

                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition",
                        "attachment;filename=Dashboard_Report_" + Session["name"].ToString() + ".xlsx");

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
                LogError("Export Error", ex);
                showToast("Something went wrong. Please try again later or contact the SYNAPSE team", "toast-danger");
            }
        }
        #endregion

        #region Proceed_Submit_Click
        protected void Proceed_Submit_Click(object sender, EventArgs e)
        {
            try
            {
                string significantAchievement = txtSigAchi.Text;
                string personalDevelopment = txtPerDev.Text;
                string txtCareerDevelopmentAmbitions = txtCarDevAmb.Text;
                decimal wiproValues = string.IsNullOrEmpty(hdnWiproValues.Value) ? 0 : Convert.ToDecimal(hdnWiproValues.Value);
                decimal leadingPeople = string.IsNullOrEmpty(hdnLeadingPeople.Value) ? 0 : Convert.ToDecimal(hdnLeadingPeople.Value);
                decimal execution = string.IsNullOrEmpty(hdnExecution.Value) ? 0 : Convert.ToDecimal(hdnExecution.Value);
                decimal passion = string.IsNullOrEmpty(hdnPassion.Value) ? 0 : Convert.ToDecimal(hdnPassion.Value);
                decimal collab = string.IsNullOrEmpty(hdnCollab.Value) ? 0 : Convert.ToDecimal(hdnCollab.Value);
                decimal customer = string.IsNullOrEmpty(hdnCustomer.Value) ? 0 : Convert.ToDecimal(hdnCustomer.Value);

                //showToast("Rating : " + rating, "toast-success");

                if (txtRemarks.Text == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "toast",
                        "showToast('Please provide remarks before submitting the request', 'toast-danger');" +
                        "$('#proceedModalCenter').modal('show');", true);
                    return;
                }

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_SOApp_SO_DashBoardLoad_NewLogic", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@ActionType", "CreateRequest");
                cmd1.Parameters.AddWithValue("@SOCode", Session["SO_Code"]);
                cmd1.Parameters.AddWithValue("@PCYear", FYDrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@Quarter", QtrDrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@Remarks", txtRemarks.Text);
                cmd1.Parameters.AddWithValue("@Checked", chkConfirm.Checked);
                cmd1.Parameters.AddWithValue("@SignificantAchievement", significantAchievement);
                cmd1.Parameters.AddWithValue("@JOB_PersonalDevelopment", personalDevelopment);
                cmd1.Parameters.AddWithValue("@CareerDevelopment_Ambitions", txtCareerDevelopmentAmbitions);
                cmd1.Parameters.AddWithValue("@WiproValues", wiproValues);
                cmd1.Parameters.AddWithValue("@LeadingPeople", leadingPeople);
                cmd1.Parameters.AddWithValue("@ExecutionExcellence", execution);
                cmd1.Parameters.AddWithValue("@PassionforResult", passion);
                cmd1.Parameters.AddWithValue("@CollaborativeWorking", collab);
                cmd1.Parameters.AddWithValue("@CustomerOrientation", customer);
                cmd1.CommandTimeout = 6000;
                cmd1.ExecuteNonQuery();

                con.Close();

                ClearForm();

                showToast("Data Submitted!", "toast-success");
            }
            catch (Exception ex)
            {
                LogError("Proceed Submit Error", ex);
                showToast("Something went wrong. Please try again later or contact the SYNAPSE team", "toast-danger");
            }
        }
        #endregion

        #region Helpers
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
            "ECO",
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
            "Achievement",
            "ECO",
            "% Achievement",
            "GOLY",
            "Last Year",
            "Achievement",
            "ECO"
        };

        public void ButtonVisibilityHelper()
        {

            btn_Proceed.Visible = false;
            btn_Common.Visible = true;
            btn_Common.Text = Session["Button"].ToString();

            if (Session["Button"].ToString() == "NewRequest")
            {
                btn_Proceed.Visible = true;
                btn_Common.Visible = false;
            }
            else if (Session["Button"].ToString() == "Pending approval")
            {
                btn_Common.CssClass = "btn btn-outline-primary form-control";
            }
            else if (Session["Button"].ToString() == "Approved")
            {
                btn_Common.CssClass = "btn btn-outline-success form-control";
            }
            else if (Session["Button"].ToString() == "Rejected")
            {
                btn_Common.CssClass = "btn btn-outline-danger form-control";
            }

            DataSet ds = Session["DashData"] as DataSet;

            //bool hideButtons = ds == null ||
            //                   ds.Tables.Cast<DataTable>().Any(t => t.Rows.Count == 0);

            //btn_Proceed.Visible = !hideButtons;
            //btn_Common.Visible = !hideButtons;

        }
        #endregion

        #region ToastNotification
        private void showToast(string message, string styleClass)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showToast", $"showToast('{message}', '{styleClass}');", true);
        }

        protected void UpdateBtn_Click(object sender, EventArgs e)
        {

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
                cmd1.Parameters.AddWithValue("@Page", "SODashBoard");
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
            PriSecDiv.Visible = false;
            distDiv.Visible = false;
            btn_Common.Visible = false;

            SODrp.ClearSelection();
            FYDrp.ClearSelection();
            QtrDrp.ClearSelection();
            TypeDrp.ClearSelection();

            txtSigAchi.Text = string.Empty;
            txtPerDev.Text = string.Empty;
            txtCarDevAmb.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            chkConfirm.Checked = false;
        }

        #endregion



    }
}