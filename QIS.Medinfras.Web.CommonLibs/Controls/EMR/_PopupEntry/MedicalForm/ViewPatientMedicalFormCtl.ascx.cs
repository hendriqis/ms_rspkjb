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
    public partial class ViewPatientMedicalFormCtl : BaseViewPopupCtl
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

            txtObservationDate.Text = paramInfo[2];
            txtObservationTime.Text = paramInfo[3];
            txtParamedicInfo.Text = paramInfo[4];

            txtObservationDate.Text = paramInfo[5];
            txtObservationTime.Text = paramInfo[6];
            txtParamedicInfo.Text = paramInfo[7];
            divFormContent.InnerHtml = paramInfo[8];
            hdnDivHTML.Value = paramInfo[8];
            hdnFormValues.Value = paramInfo[9];
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
    }
}