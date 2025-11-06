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
using System.Text.RegularExpressions;
using System.Text;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class AddMedicationAdministrationCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            this.PopupTitle = "Medication Administration";

            string[] paramInfo = param.Split('|');
            hdnPrescriptionOrderDetailID.Value = paramInfo[0];
            hdnPastMedicationID.Value = paramInfo[1];

            SetControlProperties();

            if (paramInfo.Length > 1)
            {
                string filterExpression = "1=0";
                decimal dosage = 0;
                string dosageUnit = string.Empty;
                string medicationAdministration = string.Empty;

                if (!string.IsNullOrEmpty(hdnPrescriptionOrderDetailID.Value) && hdnPrescriptionOrderDetailID.Value != "0")
                {
                    filterExpression = string.Format("PrescriptionOrderDetailID = {0}", hdnPrescriptionOrderDetailID.Value);
                    vPrescriptionOrderDt4 orderDt = BusinessLayer.GetvPrescriptionOrderDt4List(filterExpression).FirstOrDefault();
                    if (orderDt != null)
                    {
                        hdnPrescriptionOrderID.Value = orderDt.PrescriptionOrderID.ToString();
                        hdnItemID.Value = orderDt.ItemID.ToString();
                        txtItemName.Text = orderDt.cfItemName;
                        //dosage = orderDt.NumberOfDosage;
                        //dosageUnit = orderDt.DosingUnit;
                        medicationAdministration = orderDt.MedicationAdministration;
                    }
                }
                else
                {
                    filterExpression = string.Format("ID = {0}", hdnPastMedicationID.Value);
                    vPastMedication medication = BusinessLayer.GetvPastMedicationList(filterExpression).FirstOrDefault();
                    if (medication != null)
                    {
                        hdnPrescriptionOrderID.Value = string.Empty;
                        hdnItemID.Value = string.Empty;
                        txtItemName.Text = medication.DrugName;
                        //dosage = medication.NumberOfDosage;
                        //dosageUnit = medication.DosingUnit;
                        medicationAdministration = medication.Remarks;
                    }
                }

                txtMedicationDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtMedicationTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                txtNumberOfDosage.Text = dosage.ToString();
                txtMedicationAdministration.Text = medicationAdministration;
            }
        }

        private void SetControlProperties()
        {
            string filterExpression = string.Format("ParentID IN ('{0}','{4}') AND StandardCodeID NOT IN ('{1}','{2}','{3}')", Constant.StandardCode.UDD_MEDICATION_STATUS, Constant.MedicationStatus.OPEN, Constant.MedicationStatus.DIPROSES_FARMASI, Constant.MedicationStatus.DISCONTINUE, Constant.StandardCode.ITEM_UNIT);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboMedicationStatus, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.UDD_MEDICATION_STATUS).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDosingUnit, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT && sc.TagProperty == "1").ToList(), "StandardCodeName", "StandardCodeID");

            //Load Physician
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType IN ('{0}') AND ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {1})",
    Constant.ParamedicType.Nurse, AppSession.RegisteredPatient.HealthcareServiceUnitID));
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");
            if (AppSession.UserLogin.ParamedicID != null)
            {
                cboParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
                cboParamedicID.Enabled = false;
            }
            else
            {
                cboParamedicID.Enabled = true;
            }
        }

        protected void cbpPopupProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;

            result = AddMedicationSchedule();

            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = result;
        }

        private string AddMedicationSchedule()
        {
            string result = string.Empty;
            try
            {
                string validationErrMsg = string.Empty;
                if (IsValid(ref validationErrMsg))
                {
                    string dosageInText = txtNumberOfDosage.Text.Replace(',', '.');
                    decimal numberOfDosage = 0;
                    if (dosageInText.Contains('/'))
                    {
                        string[] dosageInTextInfo = dosageInText.Split('/');
                        decimal num1 = Convert.ToDecimal(dosageInTextInfo[0]);
                        decimal num2 = Convert.ToDecimal(dosageInTextInfo[1]);
                        numberOfDosage = Math.Round(num1 / num2, 2);
                    }
                    else
                    {
                        numberOfDosage = Convert.ToDecimal(dosageInText);
                    }

                    MedicationSchedule oSchedule = new MedicationSchedule();
                    oSchedule.MedicationTime = txtMedicationTime.Text;
                    if (cboMedicationStatus.Value != null)
                    {
                        oSchedule.VisitID = AppSession.RegisteredPatient.VisitID;
                        if (!string.IsNullOrEmpty(hdnPrescriptionOrderDetailID.Value) && hdnPrescriptionOrderDetailID.Value != "0")
                        {
                            oSchedule.PrescriptionOrderID = Convert.ToInt32(hdnPrescriptionOrderID.Value);
                            oSchedule.PrescriptionOrderDetailID = Convert.ToInt32(hdnPrescriptionOrderDetailID.Value);
                            oSchedule.PastMedicationID = null;
                            oSchedule.ItemID = Convert.ToInt32(hdnItemID.Value);

                            PrescriptionOrderDt orderdt = BusinessLayer.GetPrescriptionOrderDt(Convert.ToInt32(hdnPrescriptionOrderDetailID.Value));
                            if (orderdt != null)
                            {
                                oSchedule.IsIMM = orderdt.IsIMM;
                            }
                        }
                        else
                        {
                            oSchedule.PrescriptionOrderID = null;
                            oSchedule.PrescriptionOrderDetailID = null;
                            oSchedule.PastMedicationID = Convert.ToInt32(hdnPastMedicationID.Value);
                            oSchedule.ItemID = null;
                        }
                        oSchedule.ItemName = Request.Form[txtItemName.UniqueID];
                        oSchedule.MedicationDate = Helper.GetDatePickerValue(txtMedicationDate);
                        oSchedule.SequenceNo = "0"; //Continously medication
                        oSchedule.MedicationTime = txtMedicationTime.Text;
                        oSchedule.ProceedDate = oSchedule.MedicationDate;
                        oSchedule.ProceedTime = oSchedule.MedicationTime;
                        oSchedule.NumberOfDosage = numberOfDosage;
                        oSchedule.NumberOfDosageInString = dosageInText;
                        oSchedule.MedicationAdministration = Request.Form[txtMedicationAdministration.UniqueID];
                        oSchedule.GCDosingUnit = cboDosingUnit.Value.ToString();
                        oSchedule.GCMedicationStatus = cboMedicationStatus.Value.ToString();
                        oSchedule.OtherMedicationStatus = txtOtherMedicationStatus.Text;
                        oSchedule.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
                        oSchedule.CreatedBy = AppSession.UserLogin.UserID;
                        oSchedule.CreatedDate = DateTime.Now;
                    }
                    BusinessLayer.InsertMedicationSchedule(oSchedule);

                    result = string.Format("process|1|||");
                }
                else
                {
                    string message = string.Format(@"Validation Error for Medication Administration {0} : \n{1}", Request.Form[txtItemName.UniqueID], validationErrMsg);
                    result = string.Format("process|0|{0}||", message);
                }
            }
            catch (Exception ex)
            {
                result = string.Format("process|0|{0}||", ex.Message);
            }
            return result;
        }

        private bool IsValid(ref string errMessage)
        {
            StringBuilder message = new StringBuilder();

            if (string.IsNullOrEmpty(txtNumberOfDosage.Text) || txtNumberOfDosage.Text == "0")
            {
                message.AppendLine(@"Medication Dosage must be entried \n");
            }
            else
            {
                string dosageText = txtNumberOfDosage.Text.Replace(',', '.');
                if (!dosageText.Contains('/'))
                {
                    Decimal output;
                    if (!Decimal.TryParse(dosageText, out output))
                    {
                        message.AppendLine(@"Medication Dosage must be entried in correct format. \n");
                    }
                }
            }

            if (cboDosingUnit.Value == null)
            {
                message.AppendLine(@"Medication Dosage Unit must be entried. \n");
            }

            if (string.IsNullOrEmpty(txtMedicationTime.Text) || txtMedicationTime.Text == "__:__")
            {
                message.AppendLine(@"Medication Time must be entried \n");
            }
            else
            {
                Regex reg = new Regex(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$");
                if (!reg.IsMatch(txtMedicationTime.Text))
                {
                    message.AppendLine(@"Medication time must be entried in correct format (hh:mm) \n");
                }
            }

            if (cboMedicationStatus.Value != null)
            {
                if (cboParamedicID.Value == null)
                {
                    message.AppendLine(@"You must select the paramedic (nurse) for update medication status \n");
                }

                if (string.IsNullOrEmpty(txtMedicationTime.Text) || txtMedicationTime.Text == "__:__")
                {
                    message.AppendLine(@"Medication proceed time must be entried \n");
                }
                else
                {
                    Regex reg = new Regex(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$");
                    if (!reg.IsMatch(txtMedicationTime.Text))
                    {
                        message.AppendLine(@"Medication proceed time must be entried in correct format (hh:mm) \n");
                    }
                }
            }
            errMessage = message.ToString();
            return errMessage == string.Empty;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
        }
    }
}