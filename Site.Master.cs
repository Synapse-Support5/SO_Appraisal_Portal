using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SO_Appraisal
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void SetPendingCount(int count)
        {
            if (BadgeLabel != null)
            {
                if (count > 0)
                {
                    BadgeLabel.InnerText = count.ToString();
                    BadgeLabel.Style["display"] = "inline-block";
                }
                else
                {
                    BadgeLabel.InnerText = "0";
                    BadgeLabel.Style["display"] = "none";
                }
            }
        }
    }
}