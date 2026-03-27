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
            if (Session["UserId"] == null || Session["Role"] == null)
            {
                // First time visit (no referrer OR direct open)
                if (Request.UrlReferrer == null)
                {
                    Response.Redirect("~/Login.aspx");
                }
                else
                {
                    // Session expired
                    Response.Redirect("~/Login.aspx?sessionExpired=true");
                }

                return;
            }

            base.OnLoad(e);
        }
    }



}