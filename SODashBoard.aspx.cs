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
                SqlCommand cmd1 = new SqlCommand("SP_SOApp_SO_HR_DashBoardLoad", con);
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
                SqlCommand cmd1 = new SqlCommand("SP_SOApp_SO_HR_DashBoardLoad", con);
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
            }
            else if (TypeDrp.SelectedValue == "Secondary")
            {
                SecondaryLoad();
            }
            else if (TypeDrp.SelectedValue == "Distributors")
            {
                DistributorLoad();
            }
            else
            {
                PriSecDiv.Visible = false;
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
                    gvDistributors.DataSource = resds.Tables[21];
                    gvDistributors.DataBind();
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

        protected void ExportBtn_Click(object sender, EventArgs e)
        {
            if (Session["DashData"] != null)
            {
                DataSet ds = (DataSet)Session["DashData"];

                using (ClosedXML.Excel.XLWorkbook wb = new ClosedXML.Excel.XLWorkbook())
                {
                    // ================= PRIMARY SHEET =================
                    var wsPrimary = wb.Worksheets.Add("Primary");
                    int primaryRow = 1;

                    for (int i = 1; i <= 10 && i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];

                        wsPrimary.Cell(primaryRow, 1).InsertTable(dt, true);
                        primaryRow += dt.Rows.Count + 3; // space between tables
                    }

                    // ================= SECONDARY SHEET =================
                    var wsSecondary = wb.Worksheets.Add("Secondary");
                    int secondaryRow = 1;

                    for (int i = 11; i <= 20 && i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];

                        wsSecondary.Cell(secondaryRow, 1).InsertTable(dt, true);
                        secondaryRow += dt.Rows.Count + 3;
                    }

                    // ================= DISTRIBUTORS SHEET =================
                    if (ds.Tables.Count > 21)
                    {
                        var wsDist = wb.Worksheets.Add("Distributors");
                        wsDist.Cell(1, 1).InsertTable(ds.Tables[21], true);
                    }

                    // ================= DOWNLOAD =================
                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType =
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition",
                        "attachment;filename=Dashboard_Report.xlsx");

                    using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                    {
                        wb.SaveAs(memoryStream);
                        memoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
        }




    }
}