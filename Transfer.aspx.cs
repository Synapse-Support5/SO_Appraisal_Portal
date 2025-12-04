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
                showToast("An error occurred: " + ex.Message, "toast-danger");
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
                cmd1.Parameters.AddWithValue("@SOCode", "");
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
                showToast("An error occurred: " + ex.Message, "toast-danger");
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
                cmd1.Parameters.AddWithValue("@SOCode", "");
                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Rows.Clear();
                da.Fill(resdt);
                AreaDrp.DataSource = resdt;
                AreaDrp.DataTextField = resdt.Columns["AreaName"].ToString();
                AreaDrp.DataValueField = resdt.Columns["AreaId"].ToString();
                AreaDrp.DataBind();
                AreaDrp.Items.Insert(0, new ListItem("Area", ""));
                con.Close();
            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
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
                cmd1.Parameters.AddWithValue("@SOCode", "");
                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Rows.Clear();
                da.Fill(resdt);
                FromSODrp.DataSource = resdt;
                FromSODrp.DataTextField = resdt.Columns["SOName"].ToString();
                FromSODrp.DataValueField = resdt.Columns["SOCode"].ToString();
                FromSODrp.DataBind();
                AreaDrp.Items.Insert(0, new ListItem("From SO", ""));
                con.Close();
            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
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
                cmd1.Parameters.AddWithValue("@SOCode", FromSODrp.SelectedValue);
                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Rows.Clear();
                da.Fill(resdt);
                DistModal.DataSource = resdt;
                DistModal.DataBind();
                con.Close();
            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }
        }
        #endregion

        protected void SelectBtn_Click(object sender, EventArgs e)
        {
            showToast("Toast is working fine", "toast-success");
        }

        #region ToastNotification
        private void showToast(string message, string styleClass)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showToast", $"showToast('{message}', '{styleClass}');", true);
        }

        #endregion

        #region SelectedIndexChanged
        protected void StateDrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            AreaLoad();
        }

        protected void AreaDrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            FromSOLoad();
        }

        protected void FromSODrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            DistModalLoad();
        }



        #endregion

        protected void FromSODrp_SelectedIndexChanged1(object sender, EventArgs e)
        {
            DistModalLoad();
        }
    }
}