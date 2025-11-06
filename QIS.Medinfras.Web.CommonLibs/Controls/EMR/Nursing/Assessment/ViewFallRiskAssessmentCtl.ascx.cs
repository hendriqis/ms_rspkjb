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
    public partial class ViewFallRiskAssessmentCtl : BaseViewPopupCtl
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
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}'", Constant.StandardCode.FALL_RISK_INTERPRETATION));
            Methods.SetComboBoxField<StandardCode>(cboFallRiskScoreType, lstStandardCode, "StandardCodeName", "StandardCodeID");

            hdnFormType.Value = paramInfo[0];
            hdnID.Value = paramInfo[1];
            txtObservationDate.Text = paramInfo[2];
            txtObservationTime.Text = paramInfo[3];
            txtParamedicInfo.Text = paramInfo[4];
            chkIsInitialAssessment.Checked = paramInfo[5] == "True" ? true : false;
            hdnFormLayout.Value = paramInfo[6];
            hdnFormValues.Value = paramInfo[7];
            divFormContent.InnerHtml = hdnFormLayout.Value;
            chkIsFallRisk.Checked = paramInfo[8] == "1" ? true : false;
            cboFallRiskScoreType.Value = paramInfo[9];

            if (paramInfo.Length >= 15)
            {
                txtMedicalNo.Text = paramInfo[11];
                txtPatientName.Text = paramInfo[12];
                txtDateOfBirth.Text = paramInfo[13];
                txtRegistrationNo.Text = paramInfo[14];
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
    }
}