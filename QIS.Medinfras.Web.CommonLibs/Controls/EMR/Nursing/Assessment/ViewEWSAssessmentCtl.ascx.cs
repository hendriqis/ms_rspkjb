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
    public partial class ViewEWSAssessmentCtl : BaseViewPopupCtl
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
            hdnFormType.Value = paramInfo[0];
            hdnID.Value = paramInfo[1];

            vEWSAssessment obj = BusinessLayer.GetvEWSAssessmentList(string.Format("AssessmentID = {0}", hdnID.Value)).FirstOrDefault();
            if (obj != null)
            {
                txtObservationDate.Text = obj.cfAssessmentDate;
                txtObservationTime.Text = obj.AssessmentTime;
                txtParamedicInfo.Text = obj.ParamedicName;
                chkIsInitialAssessment.Checked = obj.IsInitialAssessment;
                hdnFormLayout.Value = obj.AssessmentFormLayout;
                hdnFormValues.Value = obj.AssessmentFormValue;
                divFormContent.InnerHtml = hdnFormLayout.Value;
                chkIsEWSAlert.Checked = obj.IsEWSAlert;
                txtTotalScore.Text = obj.EWSScore.ToString();
                txtTotalScoreType.Text = obj.EWSScoreType;
                txtRemarks.Text = obj.Remarks;
            }

            if (paramInfo.Length >= 6)
            {
                txtMedicalNo.Text = paramInfo[2];
                txtPatientName.Text = paramInfo[3];
                txtDateOfBirth.Text = paramInfo[4];
                txtRegistrationNo.Text = paramInfo[5];
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
    }
}