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


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AccessLoad();
                LoadAllTables();
                
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

        public void LoadAllTables()
        {
            SalesLastYearData();
            SalesPlanData();
            SalesAchievementData();
            SalesPerAchievementData();
            SalesGolyData();

            BrandLastYearData();
            BrandPlanData();
            BrandAchievementData();
            BrandPerAchievementData();
            BrandGolyData();
        }

        #region SalesLastYearData
        public void SalesLastYearData()
        {
            DataTable dt = new DataTable();

            // Dynamic columns
            dt.Columns.Add("Sales Value");
            dt.Columns.Add("Q1");
            dt.Columns.Add("Q2");
            dt.Columns.Add("Q3");
            dt.Columns.Add("Q4");
            dt.Columns.Add("Total");

            // Dynamic rows
            dt.Rows.Add("Actual_Value(Rs Lacs)", "581.53", "520.77", "231.323", "324.123", "3245.1234");
            dt.Rows.Add("NStr_Value_Actual(Rs Lacs)", "101.11", "99.28", "233.43", "123.00", "1234.234");
            dt.Rows.Add("Str_Value_Actual(Rs Lacs)", "480.11", "998.213", "54.234", "5342.213", "342.234");
            dt.Rows.Add("Str_Volume_intons(In Tonnes)", "220.32", "12.31", "342.45", "324.21", "234534.23");

            gvSalesLastYear.DataSource = dt;
            gvSalesLastYear.DataBind();
        }
        #endregion

        #region SalesPlanData
        public void SalesPlanData()
        {
            DataTable dt = new DataTable();

            // Dynamic columns
            dt.Columns.Add("Sales Value");
            dt.Columns.Add("Q1");
            dt.Columns.Add("Q2");
            dt.Columns.Add("Q3");
            dt.Columns.Add("Q4");

            // Dynamic rows
            dt.Rows.Add("Actual_Value(Rs Lacs)", "581.53", "520.77", "231.323", "324.123");
            dt.Rows.Add("NStr_Value_Actual(Rs Lacs)", "101.11", "99.28", "233.43", "123.00");
            dt.Rows.Add("Str_Value_Actual(Rs Lacs)", "480.11", "998.213", "54.234", "5342.213");
            dt.Rows.Add("Str_Volume_intons(In Tonnes)", "220.32", "12.31", "342.45", "324.21");

            gvSalesPlan.DataSource = dt;
            gvSalesPlan.DataBind();
        }
        #endregion

        #region SalesAchievementData
        public void SalesAchievementData()
        {
            DataTable dt = new DataTable();

            // Dynamic columns
            dt.Columns.Add("Sales Value");
            dt.Columns.Add("Q1");
            dt.Columns.Add("Q2");
            dt.Columns.Add("Q3");
            dt.Columns.Add("Q4");

            // Dynamic rows
            dt.Rows.Add("Actual_Value(Rs Lacs)", "581.53", "520.77", "231.323", "324.123");
            dt.Rows.Add("NStr_Value_Actual(Rs Lacs)", "101.11", "99.28", "233.43", "123.00");
            dt.Rows.Add("Str_Value_Actual(Rs Lacs)", "480.11", "998.213", "54.234", "5342.213");
            dt.Rows.Add("Str_Volume_intons(In Tonnes)", "220.32", "12.31", "342.45", "324.21");

            gvSalesAchievement.DataSource = dt;
            gvSalesAchievement.DataBind();
        }
        #endregion

        #region SalesAchievementData
        public void SalesPerAchievementData()
        {
            DataTable dt = new DataTable();

            // Dynamic columns
            dt.Columns.Add("Sales Value");
            dt.Columns.Add("Q1");
            dt.Columns.Add("Q2");
            dt.Columns.Add("Q3");
            dt.Columns.Add("Q4");

            // Dynamic rows
            dt.Rows.Add("Actual_Value(Rs Lacs)", "581.53", "520.77", "231.323", "324.123");
            dt.Rows.Add("NStr_Value_Actual(Rs Lacs)", "101.11", "99.28", "233.43", "123.00");
            dt.Rows.Add("Str_Value_Actual(Rs Lacs)", "480.11", "998.213", "54.234", "5342.213");
            dt.Rows.Add("Str_Volume_intons(In Tonnes)", "220.32", "12.31", "342.45", "324.21");

            gvSalesPerAchievement.DataSource = dt;
            gvSalesPerAchievement.DataBind();
        }
        #endregion

        #region SalesGolyData
        public void SalesGolyData()
        {
            DataTable dt = new DataTable();

            // Dynamic columns
            dt.Columns.Add("Sales Value");
            dt.Columns.Add("Q1");
            dt.Columns.Add("Q2");
            dt.Columns.Add("Q3");
            dt.Columns.Add("Q4");

            // Dynamic rows
            dt.Rows.Add("Actual_Value(Rs Lacs)", "581.53", "520.77", "231.323", "324.123");
            dt.Rows.Add("NStr_Value_Actual(Rs Lacs)", "101.11", "99.28", "233.43", "123.00");
            dt.Rows.Add("Str_Value_Actual(Rs Lacs)", "480.11", "998.213", "54.234", "5342.213");
            dt.Rows.Add("Str_Volume_intons(In Tonnes)", "220.32", "12.31", "342.45", "324.21");

            gvSalesGoly.DataSource = dt;
            gvSalesGoly.DataBind();
        }
        #endregion

        #region BrandLastYearData
        public void BrandLastYearData()
        {
            DataTable dt = new DataTable();

            // Dynamic columns
            dt.Columns.Add("Brand Volume");
            dt.Columns.Add("Q1");
            dt.Columns.Add("Q2");
            dt.Columns.Add("Q3");
            dt.Columns.Add("Q4");
            dt.Columns.Add("Total");

            // Dynamic rows
            dt.Rows.Add("Santoor(Tonnes)", "581.53", "520.77", "231.323", "324.123", "3245.1234");
            dt.Rows.Add("Fabric-Softner(Tonnes)", "101.11", "99.28", "233.43", "123.00", "1234.234");
            dt.Rows.Add("Santoor-White(Tonnes)", "480.11", "998.213", "54.234", "5342.213", "342.234");
            dt.Rows.Add("Santoor45(Tonnes)", "220.32", "12.31", "342.45", "324.21", "234534.23");
            dt.Rows.Add("SantoorLime(Tonnes)", "220.32", "12.31", "342.45", "324.21", "234534.23");
            dt.Rows.Add("Maxkleen(Tonnes)", "220.32", "12.31", "342.45", "324.21", "234534.23");
            dt.Rows.Add("HandWash(Tonnes)", "220.32", "12.31", "342.45", "324.21", "234534.23");
            dt.Rows.Add("Safewash-Matic(Tonnes)", "220.32", "12.31", "342.45", "324.21", "234534.23");
            dt.Rows.Add("Glucovita-Bolts(Tonnes)", "220.32", "12.31", "342.45", "324.21", "234534.23");
            dt.Rows.Add("Sweetener(Tonnes)", "220.32", "12.31", "342.45", "324.21", "234534.23");

            gvBrandLastYear.DataSource = dt;
            gvBrandLastYear.DataBind();
        }
        #endregion

        #region BrandPlanData
        public void BrandPlanData()
        {
            DataTable dt = new DataTable();

            // Dynamic columns
            dt.Columns.Add("Brand Volume");
            dt.Columns.Add("Q1");
            dt.Columns.Add("Q2");
            dt.Columns.Add("Q3");
            dt.Columns.Add("Q4");

            // Dynamic rows
            dt.Rows.Add("Santoor(Tonnes)", "581.53", "520.77", "231.323", "324.123");
            dt.Rows.Add("Fabric-Softner(Tonnes)", "101.11", "99.28", "233.43", "123.00");
            dt.Rows.Add("Santoor-White(Tonnes)", "480.11", "998.213", "54.234");
            dt.Rows.Add("Santoor45(Tonnes)", "220.32", "12.31", "342.45", "324.21");
            dt.Rows.Add("SantoorLime(Tonnes)", "220.32", "12.31", "342.45", "324.21");
            dt.Rows.Add("Maxkleen(Tonnes)", "220.32", "12.31", "342.45", "324.21");
            dt.Rows.Add("HandWash(Tonnes)", "220.32", "12.31", "342.45", "324.21");
            dt.Rows.Add("Safewash-Matic(Tonnes)", "220.32", "12.31", "342.45", "324.21");
            dt.Rows.Add("Glucovita-Bolts(Tonnes)", "220.32", "12.31", "342.45", "324.21");
            dt.Rows.Add("Sweetener(Tonnes)", "220.32", "12.31", "342.45", "324.21");

            gvBrandPlan.DataSource = dt;
            gvBrandPlan.DataBind();
        }
        #endregion

        #region BrandAchievementData
        public void BrandAchievementData()
        {
            DataTable dt = new DataTable();

            // Dynamic columns
            dt.Columns.Add("Brand Volume");
            dt.Columns.Add("Q1");
            dt.Columns.Add("Q2");
            dt.Columns.Add("Q3");
            dt.Columns.Add("Q4");

            // Dynamic rows
            dt.Rows.Add("Santoor(Tonnes)", "581.53", "520.77", "231.323", "324.123");
            dt.Rows.Add("Fabric-Softner(Tonnes)", "101.11", "99.28", "233.43", "123.00");
            dt.Rows.Add("Santoor-White(Tonnes)", "480.11", "998.213", "54.234", "987.65");
            dt.Rows.Add("Santoor45(Tonnes)", "220.32", "12.31", "342.45", "324.21");
            dt.Rows.Add("SantoorLime(Tonnes)", "220.32", "12.31", "342.45", "324.21");
            dt.Rows.Add("Maxkleen(Tonnes)", "220.32", "12.31", "342.45", "324.21");
            dt.Rows.Add("HandWash(Tonnes)", "220.32", "12.31", "342.45", "324.21");
            dt.Rows.Add("Safewash-Matic(Tonnes)", "220.32", "12.31", "342.45", "324.21");
            dt.Rows.Add("Glucovita-Bolts(Tonnes)", "220.32", "12.31", "342.45", "324.21");
            dt.Rows.Add("Sweetener(Tonnes)", "220.32", "12.31", "342.45", "324.21");

            gvBrandAchievement.DataSource = dt;
            gvBrandAchievement.DataBind();
        }
        #endregion

        #region BrandPerAchievementData
        public void BrandPerAchievementData()
        {
            DataTable dt = new DataTable();

            // Dynamic columns
            dt.Columns.Add("Brand Volume");
            dt.Columns.Add("Q1");
            dt.Columns.Add("Q2");
            dt.Columns.Add("Q3");
            dt.Columns.Add("Q4");

            // Dynamic rows
            dt.Rows.Add("Santoor(Tonnes)", "581.53", "520.77", "231.323", "324.123");
            dt.Rows.Add("Fabric-Softner(Tonnes)", "101.11", "99.28", "233.43", "123.00");
            dt.Rows.Add("Santoor-White(Tonnes)", "480.11", "998.213", "54.234", "987.65");
            dt.Rows.Add("Santoor45(Tonnes)", "220.32", "12.31", "342.45", "324.21");
            dt.Rows.Add("SantoorLime(Tonnes)", "220.32", "12.31", "342.45", "324.21");
            dt.Rows.Add("Maxkleen(Tonnes)", "220.32", "12.31", "342.45", "324.21");
            dt.Rows.Add("HandWash(Tonnes)", "220.32", "12.31", "342.45", "324.21");
            dt.Rows.Add("Safewash-Matic(Tonnes)", "220.32", "12.31", "342.45", "324.21");
            dt.Rows.Add("Glucovita-Bolts(Tonnes)", "220.32", "12.31", "342.45", "324.21");
            dt.Rows.Add("Sweetener(Tonnes)", "220.32", "12.31", "342.45", "324.21");

            gvBrandPerAchievement.DataSource = dt;
            gvBrandPerAchievement.DataBind();
        }
        #endregion

        #region BrandGolyData
        public void BrandGolyData()
        {
            DataTable dt = new DataTable();

            // Dynamic columns
            dt.Columns.Add("Brand Volume");
            dt.Columns.Add("Q1");
            dt.Columns.Add("Q2");
            dt.Columns.Add("Q3");
            dt.Columns.Add("Q4");

            // Dynamic rows
            dt.Rows.Add("Santoor(Tonnes)", "581.53", "520.77", "231.323", "324.123");
            dt.Rows.Add("Fabric-Softner(Tonnes)", "101.11", "99.28", "233.43", "123.00");
            dt.Rows.Add("Santoor-White(Tonnes)", "480.11", "998.213", "54.234", "987.65");
            dt.Rows.Add("Santoor45(Tonnes)", "220.32", "12.31", "342.45", "324.21");
            dt.Rows.Add("SantoorLime(Tonnes)", "220.32", "12.31", "342.45", "324.21");
            dt.Rows.Add("Maxkleen(Tonnes)", "220.32", "12.31", "342.45", "324.21");
            dt.Rows.Add("HandWash(Tonnes)", "220.32", "12.31", "342.45", "324.21");
            dt.Rows.Add("Safewash-Matic(Tonnes)", "220.32", "12.31", "342.45", "324.21");
            dt.Rows.Add("Glucovita-Bolts(Tonnes)", "220.32", "12.31", "342.45", "324.21");
            dt.Rows.Add("Sweetener(Tonnes)", "220.32", "12.31", "342.45", "324.21");

            gvBrandGoly.DataSource = dt;
            gvBrandGoly.DataBind();
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

    }
}