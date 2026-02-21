using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SO_Appraisal
{
    public partial class SODashBoard : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["SqlConn"].ToString());
        DataTable dt = new DataTable();
        DataTable resdt = new DataTable();
        DataSet ds = new DataSet();
        public DataSet resds = new DataSet();
        string SOCode = "4076L2";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
                LogError("Access load Error", ex);
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
                SqlCommand cmd1 = new SqlCommand("SP_SOApp_SO_DashBoardLoad", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@ActionType", "Landing");
                cmd1.Parameters.AddWithValue("@SOCode", SOCode);
                cmd1.Parameters.AddWithValue("@PcYearText", "");
                cmd1.Parameters.AddWithValue("@PcYearVal", "");
                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                ds.Clear();
                da.Fill(ds);

                // ✅ First Result Set → Geo
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    lblGeo.Text = "Geo : " + ds.Tables[0].Rows[0]["Geo"].ToString();
                }

                // ✅ Second Result Set → FY
                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    DataTable dtFY = ds.Tables[1];

                    FYDrp.DataSource = dtFY;
                    FYDrp.DataTextField = "PcYear";   // Column name
                    FYDrp.DataValueField = "Value";   // Column name
                    FYDrp.DataBind();

                    FYDrp.Items.Insert(0, new ListItem("Select FY", ""));

                    // ✅ Auto select where IsCurrent = 1
                    DataRow[] currentRow = dtFY.Select("IsCurrent = 1");
                    if (currentRow.Length > 0)
                    {
                        FYDrp.SelectedValue = currentRow[0]["Value"].ToString();

                        FetchAllData();

                        if (resds.Tables.Count > 0 && resds.Tables[0].Rows.Count > 0)
                        {
                            DistCountBtn.Text = "Dist. Count : " +
                                resds.Tables[0].Rows[0]["DistCount"].ToString();
                        }
                        else
                        {
                            DistCountBtn.Text = "Dist. Count : 0";
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

        #region FetchAllData
        public void FetchAllData()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_SOApp_SO_DashBoardLoad", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@ActionType", "Fetch");
                cmd1.Parameters.AddWithValue("@SOCode", SOCode);
                cmd1.Parameters.AddWithValue("@PcYearText", FYDrp.SelectedItem.Text.ToString());
                cmd1.Parameters.AddWithValue("@PcYearVal", FYDrp.SelectedValue);
                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resds.Clear();
                da.Fill(resds);

                Session["DashData"] = resds;

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
            FetchAllData();

            if (resds.Tables.Count > 0 && resds.Tables[0].Rows.Count > 0)
            {
                DistCountBtn.Text = "Dist. Count : " +
                    resds.Tables[0].Rows[0]["DistCount"].ToString();
            }
            else
            {
                DistCountBtn.Text = "Dist. Count : 0";
            }
        }

        protected void TypeDrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TypeDrp.SelectedValue == "Primary")
            {
                PrimaryLoad();
                statusBtnDiv.Visible = true;
            }
            else if (TypeDrp.SelectedValue == "Secondary")
            {
                SecondaryLoad();
                statusBtnDiv.Visible = true;
            }
            else if (TypeDrp.SelectedValue == "Distributors")
            {
                DistributorLoad();
                statusBtnDiv.Visible = true;
            }
            else
            {
                PriSecDiv.Visible = false;
                statusBtnDiv.Visible = false;
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
                    gvSalesLastYear.DataSource = resds.Tables[1];
                    gvSalesLastYear.DataBind();

                    gvSalesPlan.DataSource = resds.Tables[2];
                    gvSalesPlan.DataBind();

                    gvSalesAchievement.DataSource = resds.Tables[3];
                    gvSalesAchievement.DataBind();

                    gvSalesPerAchievement.DataSource = resds.Tables[4];
                    gvSalesPerAchievement.DataBind();

                    gvSalesGoly.DataSource = resds.Tables[5];
                    gvSalesGoly.DataBind();
                    //------------------------

                    //Brand Volume -----------
                    gvBrandLastYear.DataSource = resds.Tables[6];
                    gvBrandLastYear.DataBind();

                    gvBrandPlan.DataSource = resds.Tables[7];
                    gvBrandPlan.DataBind();

                    gvBrandAchievement.DataSource = resds.Tables[8];
                    gvBrandAchievement.DataBind();

                    gvBrandPerAchievement.DataSource = resds.Tables[9];
                    gvBrandPerAchievement.DataBind();

                    gvBrandGoly.DataSource = resds.Tables[10];
                    gvBrandGoly.DataBind();
                    //------------------------
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
                    gvSalesLastYear.DataSource = resds.Tables[11];
                    gvSalesLastYear.DataBind();

                    gvSalesPlan.DataSource = resds.Tables[12];
                    gvSalesPlan.DataBind();

                    gvSalesAchievement.DataSource = resds.Tables[13];
                    gvSalesAchievement.DataBind();

                    gvSalesPerAchievement.DataSource = resds.Tables[14];
                    gvSalesPerAchievement.DataBind();

                    gvSalesGoly.DataSource = resds.Tables[15];
                    gvSalesGoly.DataBind();
                    //------------------------

                    //Brand Volume -----------
                    gvBrandLastYear.DataSource = resds.Tables[16];
                    gvBrandLastYear.DataBind();

                    gvBrandPlan.DataSource = resds.Tables[17];
                    gvBrandPlan.DataBind();

                    gvBrandAchievement.DataSource = resds.Tables[18];
                    gvBrandAchievement.DataBind();

                    gvBrandPerAchievement.DataSource = resds.Tables[19];
                    gvBrandPerAchievement.DataBind();

                    gvBrandGoly.DataSource = resds.Tables[20];
                    gvBrandGoly.DataBind();
                    //------------------------
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
                    if (resds.Tables[21] != null)
                    {
                        gvDistributors.DataSource = resds.Tables[21];
                        gvDistributors.DataBind();

                        statusBtnDiv.Visible = true;
                    }
                    else
                    {
                        statusBtnDiv.Visible = false;
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
        }
        #endregion

        #region ExportBtn_Click
        protected void ExportBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["DashData"] == null) return;

                DataSet ds = (DataSet)Session["DashData"];

                using (ClosedXML.Excel.XLWorkbook wb = new ClosedXML.Excel.XLWorkbook())
                {
                    CreateCustomSheet(wb, ds, 1, 10, "Primary", primaryNames);
                    CreateCustomSheet(wb, ds, 11, 20, "Secondary", secondaryNames);

                    // ================= Distributor Sheet =================
                    if (ds.Tables.Count > 21)
                    {
                        var wsDist = wb.Worksheets.Add("Distributors");
                        var table = wsDist.Cell(1, 1).InsertTable(ds.Tables[21], false);

                        FormatTable(table);
                        wsDist.Columns().AdjustToContents();
                    }

                    Response.Clear();
                    Response.ContentType =
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition",
                        "attachment;filename=Dashboard_Report_" + SOCode + ".xlsx");

                    using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                    {
                        wb.SaveAs(memoryStream);
                        memoryStream.WriteTo(Response.OutputStream);
                        Response.End();
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("Export Error", ex);
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
            "Achievement",
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
            "Achievement",
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

        protected void Proceed_Submit_Click(object sender, EventArgs e)
        {
            try
            {
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
                cmd1.Parameters.AddWithValue("@SOCode", SOCode);
                cmd1.Parameters.AddWithValue("@Remarks", txtRemarks.Text);
                cmd1.Parameters.AddWithValue("@Checked", chkConfirm.Checked);
                cmd1.CommandTimeout = 6000;
                cmd1.ExecuteNonQuery();

                con.Close();

                showToast("Data Submitted!", "toast-success");
            }
            catch (Exception ex)
            {
                LogError("Proceed Submit Error", ex);
                showToast("Something went wrong. Please try again later or contact the SYNAPSE team", "toast-danger");
            }
        }



    }
}