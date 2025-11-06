using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using QIS.Medinfras.Web.Common;
using System.Web.UI.HtmlControls;
using QIS.Medinfras.Data.Service;
using System.Net;

namespace QIS.Medinfras.Web.CommonLibs.MasterPage
{
    public partial class MPBaseDashboard : System.Web.UI.MasterPage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!Page.IsPostBack)
            {
                this.Page.Title = string.Format("{0} - {1}", GetServerIPAddress(), "MEDINFRAS");
                XDocument xdoc = Helper.LoadXMLFile(this, "config.xml");
                var config = (from pg in xdoc.Descendants("page")
                              select new
                              {
                                  Themes = pg.Attribute("themes").Value
                              }).FirstOrDefault();
                //string themes = AppConfigManager.QISThemes;
                AddLink(string.Format("../Styles/{0}/medinfras.css", config.Themes));
                AddLink(string.Format("../Styles/{0}/jquery/jquery.ui.theme.css", config.Themes));
            }
        }

        private void AddLink(string href)
        {
            HtmlHead head = (HtmlHead)Page.Header;
            HtmlLink link = new HtmlLink();
            link.Attributes.Add("href", href);
            link.Attributes.Add("type", "text/css");
            link.Attributes.Add("rel", "stylesheet");
            head.Controls.Add(link);
        }

        protected string GetServerIPAddress()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = null;
            if (ipHostInfo.AddressList.Length <= 2)
                ipAddress = ipHostInfo.AddressList[1];
            else
                ipAddress = ipHostInfo.AddressList[2];

            return ipAddress.ToString();
        }
    }
}