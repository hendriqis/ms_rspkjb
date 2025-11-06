using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxEditors;
using System.Text;
using System.IO;
using DevExpress.Web.ASPxCallbackPanel;
using System.Net;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ViewNosokomialEntryCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                string[] paramInfo = param.Split('|');
                SetControlProperties(paramInfo);
            }
        }

        private void SetControlProperties(string[] paramInfo)
        {
            hdnFormType.Value = paramInfo[1];
            hdnHeaderID.Value = paramInfo[2];
            hdnID.Value = paramInfo[3];
            txtObservationDate.Text = paramInfo[4];
            txtObservationTime.Text = paramInfo[5];
            txtParamedicInfo.Text = paramInfo[6];
            divFormContent.InnerHtml = paramInfo[8];
            hdnDivHTML.Value = paramInfo[8];
            hdnFormValues.Value = paramInfo[9];
            txtRemarks.Text = paramInfo[10];  
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
    }
}