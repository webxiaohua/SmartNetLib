using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Smart.NETLib;
namespace WebTest
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write(ConfigHelper.GetConfigInt("key1"));
            Response.Write(DirFileHelper.GetMapPath(""));
        }
    }
}