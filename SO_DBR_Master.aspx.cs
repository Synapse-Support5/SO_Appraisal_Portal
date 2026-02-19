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
    public partial class SO_DBR_Master : System.Web.UI.Page
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

        #region StateLoad
        public void StateLoad()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_SOApp_Master_Dropdowns", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@ActionType", "StateLoad");
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@StateId", "");
                cmd1.Parameters.AddWithValue("@Area", "");
                cmd1.Parameters.AddWithValue("@ZoneName", "");
                cmd1.Parameters.AddWithValue("@SOCode", "");
                cmd1.Parameters.AddWithValue("@DistCode", "");
                
                
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
                SqlCommand cmd1 = new SqlCommand("SP_SOApp_Master_Dropdowns", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@ActionType", "AreaLoad");
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@StateId", StateDrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@Area", "");
                cmd1.Parameters.AddWithValue("@ZoneName", "");
                cmd1.Parameters.AddWithValue("@SOCode", "");
                cmd1.Parameters.AddWithValue("@DistCode", "");
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
                SqlCommand cmd1 = new SqlCommand("SP_SOApp_Master_Dropdowns", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@ActionType", "ZoneLoad");
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@StateId", StateDrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@Area", AreaDrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@ZoneName", "");
                cmd1.Parameters.AddWithValue("@SOCode", "");
                cmd1.Parameters.AddWithValue("@DistCode", "");
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
                SqlCommand cmd1 = new SqlCommand("SP_SOApp_Master_Dropdowns", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@ActionType", "SoLoad");
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@StateId", StateDrp.SelectedValue);
                //cmd1.Parameters.AddWithValue("@Area", AreaDrp.SelectedItem.ToString());
                cmd1.Parameters.AddWithValue("@Area", AreaDrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@ZoneName", ZoneDrp.SelectedItem.ToString());
                cmd1.Parameters.AddWithValue("@SOCode", "");
                cmd1.Parameters.AddWithValue("@DistCode", "");
                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Rows.Clear();
                da.Fill(resdt);
                SODrp.DataSource = resdt;
                SODrp.DataTextField = resdt.Columns["SOName"].ToString();
                SODrp.DataValueField = resdt.Columns["SOCode"].ToString();
                SODrp.DataBind();
                SODrp.Items.Insert(0, new ListItem("From SO", ""));
                con.Close();
            }
            catch (Exception ex)
            {
                LogError("From SOLoad Error", ex);
                showToast("Something went wrong. Please try again later or contact the SYNAPSE team", "toast-danger");
            }
        }
        #endregion


        // 🔍 SEARCH BUTTON CLICK
        protected void SearchBtn_Click(object sender, EventArgs e)
        {
            //BindGridUsingSP();
            string distCode = MainSearch.Text.Trim();
            BindGridUsingSP(distCode);
        }

        // 📥 FETCH BUTTON CLICK
        protected void Fetch_Click(object sender, EventArgs e)
        {
            //BindGridUsingSP();
            string distCode = "";  
            BindGridUsingSP(distCode);
        }


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
            //Fetch_Click();
        }

        



        #endregion


        // NEW: Download button click handler (exports current grid data as HTML)
        protected void DownloadBtn_Click(object sender, EventArgs e)
        {
            DataTable dt = Session["ResultGridData"] as DataTable;

            // If session doesn't have data, try re-binding with the default fetch SP
            if (dt == null)
            {
                //BindGridUsingSP();
                dt = Session["ResultGridData"] as DataTable;
            }

            if (dt == null || dt.Rows.Count == 0)
            {
                // nothing to export
                Response.Write("<script>alert('No data available to download.');</script>");
                return;
            }

            // Use a temporary GridView to render HTML exactly like ASP.NET GridView output
            GridView exportGrid = new GridView();
            exportGrid.AllowPaging = false;
            exportGrid.AutoGenerateColumns = true;
            exportGrid.DataSource = dt;
            exportGrid.DataBind();

            // Render control to string
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            exportGrid.RenderControl(hw);

            // Send the rendered HTML to the client as a downloadable file
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=ResultGrid.html");
            Response.Charset = "";
            Response.ContentType = "text/html";
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }

        // Main binder — stores DataTable into Session for reuse (e.g., export)
        private void BindGridUsingSP(string distCode)
        {
            DataTable dt = new DataTable();

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                if (distCode == "")
                {
                    SqlCommand cmd1 = new SqlCommand("SP_SOApp_Master_Dropdowns", con);
                    cmd1.CommandType = CommandType.StoredProcedure;

                    // Your fixed SP parameters
                    cmd1.Parameters.AddWithValue("@ActionType", "FetchData");
                    cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                    cmd1.Parameters.AddWithValue("@StateId", StateDrp.SelectedValue);
                    //cmd1.Parameters.AddWithValue("@Area", AreaDrp.SelectedItem.ToString());
                    cmd1.Parameters.AddWithValue("@Area", AreaDrp.SelectedValue);
                    cmd1.Parameters.AddWithValue("@ZoneName", ZoneDrp.SelectedItem.ToString());
                    cmd1.Parameters.AddWithValue("@SOCode", SODrp.SelectedItem.ToString());
                    cmd1.Parameters.AddWithValue("@DistCode", "");

                    cmd1.CommandTimeout = 6000;

                    SqlDataAdapter da = new SqlDataAdapter(cmd1);

                    dt.Rows.Clear();
                    da.Fill(dt);

                    // Save for download
                    Session["ResultGridData"] = dt;

                    // Bind Grid
                    ResultGrid.DataSource = dt;
                    ResultGrid.DataBind();
                }

                else
                {
                    SqlCommand cmd1 = new SqlCommand("SP_SOApp_Master_Dropdowns", con);
                    cmd1.CommandType = CommandType.StoredProcedure;

                    // Your fixed SP parameters
                    cmd1.Parameters.AddWithValue("@ActionType", "SearchData");
                    cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                    cmd1.Parameters.AddWithValue("@StateId", "");
                    cmd1.Parameters.AddWithValue("@Area", "");
                    cmd1.Parameters.AddWithValue("@ZoneName", "");
                    cmd1.Parameters.AddWithValue("@SOCode", "");
                    cmd1.Parameters.AddWithValue("@DistCode", distCode);

                    cmd1.CommandTimeout = 6000;

                    SqlDataAdapter da = new SqlDataAdapter(cmd1);

                    dt.Rows.Clear();
                    da.Fill(dt);

                    // Save for download
                    Session["ResultGridData"] = dt;

                    // Bind Grid
                    ResultGrid.DataSource = dt;
                    ResultGrid.DataBind();

                }

            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message.Replace("'", "\\'") + "');</script>");
            }
        }


        // Required when calling RenderControl on a server control
        public override void VerifyRenderingInServerForm(Control control)
        {
            // This method must be present and empty to allow rendering controls to string
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

    }
}