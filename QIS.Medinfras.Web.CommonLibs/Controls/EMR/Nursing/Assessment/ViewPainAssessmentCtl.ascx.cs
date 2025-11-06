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
    public partial class ViewPainAssessmentCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                //Tidak pakai |, karena di form elektronik pengkajian nyeri ada yang menggunakan |
                string[] paramInfo = param.Split('~');
                SetControlProperties(paramInfo);
             }
        }

        private void SetControlProperties(string[] paramInfo)
        {
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}'", Constant.StandardCode.PAIN_SCALE_INTERPRETATION));
            Methods.SetComboBoxField<StandardCode>(cboPainScoreType, lstStandardCode, "StandardCodeName", "StandardCodeID");

            hdnFormType.Value = paramInfo[0];
            hdnID.Value = paramInfo[1];
            txtObservationDate.Text = paramInfo[2];
            txtObservationTime.Text = paramInfo[3];
            txtParamedicInfo.Text = paramInfo[4];
            chkIsInitialAssessment.Checked = paramInfo[5] == "True" ? true : false;
            hdnFormLayout.Value = paramInfo[6].Replace("Ѽ", "~");
            hdnFormValues.Value = paramInfo[7];
            divFormContent.InnerHtml = paramInfo[6];
            chkIsPain.Checked = paramInfo[8] == "1" ? true : false;
            cboPainScoreType.Value = paramInfo[9];
 
            txtProvocation.Text = paramInfo[18].Replace("&nbsp;", "");
            txtQuality.Text = paramInfo[19].Replace("&nbsp;", "");
            txtRegio.Text = paramInfo[20].Replace("&nbsp;", "");
            txtTime.Text = paramInfo[21].Replace("&nbsp;", "");

            if (paramInfo.Length >= 23)
            {
                string[] patientInfo = paramInfo.LastOrDefault().Split('|');

                txtMedicalNo.Text = patientInfo[0];
                txtPatientName.Text = patientInfo[1];
                txtDateOfBirth.Text = patientInfo[2];
                txtRegistrationNo.Text = patientInfo[3];
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
    }
}