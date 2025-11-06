using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.MCU.Program
{
    public partial class MCUResultFormViewDetailCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                string[] paramInfo = param.Split('|');
                SetControlProperties(paramInfo);
            }
        }

        private void PopulateFormContent()
        {
            //string filePath = HttpContext.Current.Server.MapPath("~/Libs/App_Data");

            string path = AppConfigManager.QISPhysicalDirectory;
            path += string.Format("{0}\\", AppConfigManager.QISMCUResultForm.Replace('/', '\\'));

            string fileName = string.Format(@"{0}\{1}.html", path, hdnGCResultTypeCtl.Value.Replace('^', '_'));
            IEnumerable<string> lstText = File.ReadAllLines(fileName);
            StringBuilder innerHtml = new StringBuilder();
            foreach (string text in lstText)
            {
                innerHtml.AppendLine(text);
            }

            divFormContent.InnerHtml = innerHtml.ToString();
            hdnDivHTMLCtl.Value = innerHtml.ToString();
        }

        private void SetControlProperties(string[] paramInfo)
        {
            hdnGCResultTypeCtl.Value = paramInfo[0];
            lblTitle.InnerText = paramInfo[1];
            hdnIDCtl.Value = paramInfo[2];
            txtRemarks.Text = paramInfo[3];

            MCUResultForm obj1 = BusinessLayer.GetMCUResultForm(Convert.ToInt32(hdnIDCtl.Value));
            if (obj1 != null)
            {
                hdnFormLayoutCtl.Value = obj1.FormLayout;
                hdnFormValueCtl.Value = obj1.FormValue;

                divFormContent.InnerHtml = obj1.FormLayout;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
    }
}