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

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class PPRAAlertNotificationCtl : BaseEntryPopupCtl4
    {
        public override void InitializeDataControl(string param)
        {
            IsAdd = false;
            string[] paramInfo = param.Split('|');
            hdnPrescriptionOrderID.Value = paramInfo[0];

            rblPPRAMicrobiologyTestStatus.SelectedValue = "0";
            trMicrobiologyTestOrder1.Style.Add("display", "none");
            trMicrobiologyTestOrder2.Style.Add("display", "none");

            SetControlProperties();
        }

        protected override void SetControlProperties()
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

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retVal)
        {
            return true;
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;

            try
            {
                string processResult = "process|0|no data to proceed";
                if (IsValid(ref errMessage))
                {
                    processResult = ProcessFormulirPPRA(hdnPrescriptionOrderID.Value);
                }
                else
                {
                    processResult = string.Format("process|0|{0}", errMessage);
                }

                string[] resultInfo = processResult.Split('|');
                result = resultInfo[1] == "1";
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
            }
            return result;
        }

        protected bool IsValid(ref string errMessage)
        {
            StringBuilder sbMessage = new StringBuilder();

            bool isValid = true;

            if (string.IsNullOrEmpty(rblIsHasPPRAForm.SelectedValue))
            {
                sbMessage.AppendLine("Status pengisian Formulir PPRA tidak boleh kosong atau harus diisi");
            }

            if (string.IsNullOrEmpty(txtPPRASubjectiveSummary.Text))
            {
                sbMessage.AppendLine("Catatan Riwayat Penyakit Pasien harus diisi.");
            }

            if (string.IsNullOrEmpty(txtPPRAObjectiveSummary.Text))
            {
                sbMessage.AppendLine("Catatan Kondisi Pasien harus diisi.");
            }

            if (string.IsNullOrEmpty(txtPPRAAssessmentSummary.Text))
            {
                sbMessage.AppendLine("Catatan Diagnosa Pasien harus diisi.");
            }

            if (string.IsNullOrEmpty(rblPPRAIndication.SelectedValue))
            {
                sbMessage.AppendLine("Indikasi Pemberian Antibiotik tidak boleh kosong atau harus diisi");
            }

            if (string.IsNullOrEmpty(txtPPRAReason.Text))
            {
                sbMessage.AppendLine("Alasan permintaan pemberian harus diisi.");
            }

            if (string.IsNullOrEmpty(txtPPRAPlanningSummary.Text))
            {
                sbMessage.AppendLine("Catatan Pemeriksaan Radiologi/Penunjang Medis berkaitan infeksi harus diisi.");
            }

            if (string.IsNullOrEmpty(rblPPRAMicrobiologyTestStatus.SelectedValue))
            {
                sbMessage.AppendLine("Status Pemeriksaan Kultur tidak boleh kosong atau harus diisi");
            }
            else
            {
                if (rblPPRAMicrobiologyTestStatus.SelectedValue != "0")
                {
                    if (string.IsNullOrEmpty(Request.Form[txtTestOrderNo.UniqueID]))
                    {
                        sbMessage.AppendLine("Nomor Order Pemeriksaan Kultur tidak boleh kosong atau harus diisi");
                    }

                    if (string.IsNullOrEmpty(rblPPRAMicrobiologyTestResultStatus.SelectedValue))
                    {
                        sbMessage.AppendLine("Status Hasil Pemeriksaan Kultur tidak boleh kosong atau harus diisi");
                    }
                }
            }

            errMessage = sbMessage.ToString().Replace(Environment.NewLine, "<br />");

            isValid = string.IsNullOrEmpty(errMessage);

            return isValid;
        }

        private string ProcessFormulirPPRA(string prescriptionOrderID)
        {
            string result = string.Empty;
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderHdInfoDao objDao = new PrescriptionOrderHdInfoDao(ctx);

            try
            {
                PrescriptionOrderHdInfo hdInfo = objDao.Get(Convert.ToInt32(hdnPrescriptionOrderID.Value));
                bool isAdd = false;
                if (hdInfo == null)
                {
                    isAdd = true;
                    hdInfo = new PrescriptionOrderHdInfo();
                }

                hdInfo.PrescriptionOrderID = Convert.ToInt32(hdnPrescriptionOrderID.Value);
                hdInfo.IsHasPPRAItem = true;
                hdInfo.PPRAFormStatus = rblIsHasPPRAForm.SelectedValue;
                if (rblIsHasPPRAForm.SelectedValue != "0")
                {
                    hdInfo.PPRASubjectiveSummary = txtPPRASubjectiveSummary.Text;
                    hdInfo.PPRAObjectiveSummary = txtPPRAObjectiveSummary.Text;
                    hdInfo.PPRAAssessmentSummary = txtPPRAAssessmentSummary.Text;
                    hdInfo.PPRAPlanningSummary = txtPPRAPlanningSummary.Text;
                    hdInfo.PPRAIndication = rblPPRAIndication.SelectedValue;
                    hdInfo.PPRAReason = txtPPRAReason.Text;
                    hdInfo.IsHasMicrobiologyTestOrder = rblPPRAMicrobiologyTestStatus.SelectedValue == "1" ? true : false;
                    hdInfo.IsHasMicrobiologyTestResult = rblPPRAMicrobiologyTestResultStatus.SelectedValue == "1" ? true : false;
                    if (hdInfo.IsHasMicrobiologyTestOrder)
                    {
                        hdInfo.TestOrderID = Convert.ToInt32(hdnTestOrderID.Value);
                    }
                }

                if (isAdd)
                {
                    hdInfo.PPRAFormDateTime = DateTime.Now;
                    objDao.Insert(hdInfo);
                }
                else
                {
                    objDao.Update(hdInfo);
                }

                ctx.CommitTransaction();
                result = string.Format("process|1|");
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                result = string.Format("process|0|{0}", ex.Message);
            }
            return result;
        }

        private void PopulateFormContent()
        {
            string filePath = HttpContext.Current.Server.MapPath("~/Libs/App_Data");

            string fileName = string.Format(@"{0}\medicalForm\PPRA\ppra.html", filePath);
            IEnumerable<string> lstText = File.ReadAllLines(fileName);
            StringBuilder innerHtml = new StringBuilder();
            foreach (string text in lstText)
            {
                innerHtml.AppendLine(text);
            }

            hdnFormLayout.Value = innerHtml.ToString();
            divFormContent.InnerHtml = innerHtml.ToString();
        }
    }
}