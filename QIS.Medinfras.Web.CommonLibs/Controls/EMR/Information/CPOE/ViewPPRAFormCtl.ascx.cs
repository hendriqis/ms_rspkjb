using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxCallbackPanel;
using System.Data;
using System.Text;
using System.IO;
using System.Web.Services;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ViewPPRAFormCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnPrescriptionOrderID.Value = paramInfo[0];

            rblPPRAMicrobiologyTestStatus.SelectedValue = "0";
            trMicrobiologyTestOrder1.Style.Add("display", "none");
            trMicrobiologyTestOrder2.Style.Add("display", "none");

            SetControlProperties();
        }

        protected void SetControlProperties()
        {
            List<PrescriptionOrderPPRAInfo> lstItem = BusinessLayer.GetPrescriptionOrderPPRAInfo(Convert.ToInt32(hdnPrescriptionOrderID.Value));
            if (lstItem.Count > 0)
            {
                PrescriptionOrderPPRAInfo oHeader = lstItem.FirstOrDefault();
                txtPrescriptionOrderNo.Text = oHeader.PrescriptionOrderNo;
                txtRegistrationNo.Text = oHeader.RegistrationNo;
                txtMedicalNo.Text = oHeader.MedicalNo;
                txtDateOfBirth.Text = oHeader.DateOfBirth.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtPatientName.Text = oHeader.PatientName;
                txtParamedicName.Text = oHeader.ParamedicName;
                hdnPopupVisitID.Value = oHeader.VisitID.ToString();
                hdnPopupParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();

                StringBuilder itemList = new StringBuilder();
                foreach (PrescriptionOrderPPRAInfo item in lstItem)
                {
                    itemList.AppendLine(string.Format("{0}, {1}", item.DrugName, item.cfConsumeMethod));
                }
                txtItemList.Text = itemList.ToString();

                rblIsHasPPRAForm.SelectedValue = oHeader.PPRAFormStatus;
                if (rblIsHasPPRAForm.SelectedValue != "0")
                {
                    hdnTestOrderID.Value = oHeader.TestOrderID.ToString();
                    txtTestOrderNo.Text = oHeader.TestOrderNo;
                    txtPPRASubjectiveSummary.Text = oHeader.PPRASubjectiveSummary;
                    txtPPRAObjectiveSummary.Text = oHeader.PPRAObjectiveSummary;
                    txtPPRAAssessmentSummary.Text = oHeader.PPRAAssessmentSummary;
                    rblPPRAIndication.SelectedValue = oHeader.PPRAIndication;
                    txtPPRAReason.Text = oHeader.PPRAReason;
                    rblPPRAMicrobiologyTestStatus.SelectedValue = oHeader.IsHasMicrobiologyTestOrder ? "1" : "0";
                    if (rblPPRAMicrobiologyTestStatus.SelectedValue == "1")
                    {
                        trMicrobiologyTestOrder1.Style.Add("display", "table-row");
                        trMicrobiologyTestOrder2.Style.Add("display", "table-row");
                    }
                    else
                    {
                        trMicrobiologyTestOrder1.Style.Add("display", "none");
                        trMicrobiologyTestOrder2.Style.Add("display", "none");
                    }
                    rblPPRAMicrobiologyTestResultStatus.SelectedValue = oHeader.IsHasMicrobiologyTestResult ? "1" : "0";
                    txtPPRAPlanningSummary.Text = oHeader.PPRAPlanningSummary;
                }
                else
                {
                    hdnTestOrderID.Value = "0";
                    txtTestOrderNo.Text = string.Empty;
                    txtPPRASubjectiveSummary.Text = string.Empty;
                    txtPPRAObjectiveSummary.Text = string.Empty;
                    txtPPRAAssessmentSummary.Text = string.Empty;
                    rblPPRAIndication.SelectedValue = string.Empty;
                    txtPPRAReason.Text = string.Empty;
                    rblPPRAMicrobiologyTestStatus.SelectedValue = string.Empty;
                    trMicrobiologyTestOrder1.Style.Add("display", "none");
                    trMicrobiologyTestOrder2.Style.Add("display", "none");
                    rblPPRAMicrobiologyTestResultStatus.SelectedValue = string.Empty;
                    txtPPRAPlanningSummary.Text = string.Empty;
                }
                txtPPRARemarks.Text = oHeader.PPRARemarks;
            }
        }
    }
}