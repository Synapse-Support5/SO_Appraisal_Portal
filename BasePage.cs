using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SO_Appraisal
{
    public class BasePage : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            base.OnLoad(e);
        }
    }

}