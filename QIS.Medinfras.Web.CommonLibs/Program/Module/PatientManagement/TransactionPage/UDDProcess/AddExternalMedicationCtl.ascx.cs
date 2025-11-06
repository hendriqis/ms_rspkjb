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
using Newtonsoft.Json;
using System.Text;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class AddExternalMedicationCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            this.PopupTitle = "Patient Medication";

            string[] paramInfo = param.Split('|');
            hdnPrescriptionOrderDetailID.Value = paramInfo[0];

            SetControlProperties();
        }

        private void SetControlProperties()
        {
            String filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.DOSING_FREQUENCY, Constant.StandardCode.MEDICATION_ROUTE, Constant.StandardCode.COENAM_RULE,Constant.StandardCode.ITEM_UNIT);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            txtStartTime1.Attributes.Add("validationgroup", "mpAEM");
            txtStartTime2.Attributes.Add("validationgroup", "mpAEM");
            txtStartTime3.Attributes.Add("validationgroup", "mpAEM");
            txtStartTime4.Attributes.Add("validationgroup", "mpAEM");
            txtStartTime5.Attributes.Add("validationgroup", "mpAEM");
            txtStartTime6.Attributes.Add("validationgroup", "mpAEM");
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboFrequencyTimeline, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DOSING_FREQUENCY).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDosingUnit, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT && sc.TagProperty == "1").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboMedicationRoute, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.MEDICATION_ROUTE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboCoenamRule, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.COENAM_RULE).ToList(), "StandardCodeName", "StandardCodeID");

            cboFrequencyTimeline.SelectedIndex = 1;
            cboMedicationRoute.SelectedIndex = 0;

            txtStartDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        }

        protected void cbpPopupProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";

            string recordID = hdnSelectedID.Value;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;

            result = CreateMedicationSchedule(recordID);

            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = result;
        }

        private string CreateMedicationSchedule(string scheduleID)
        {
            string result = string.Empty;
            IDbContext ctx = DbFactory.Configure(true);
            PastMedicationDao pastMedicationDao = new PastMedicationDao(ctx);
            MedicationScheduleDao scheduleDao = new MedicationScheduleDao(ctx);
            try
            {
                string validationErrMsg = string.Empty;
                if (IsValid(ref validationErrMsg))
                {
                    int frequency = Convert.ToInt16(txtFrequencyNumber.Text);

                    DateTime startDate = Helper.GetDatePickerValue(txtStartDate.Text);
                    int duration = Convert.ToInt16(txtDosingDuration.Text);

                    decimal dose = Convert.ToDecimal(txtDosingDose.Text);
                    bool isPRN = chkIsAsRequired.Checked;
                    bool isMorning = chkIsMorning.Checked;
                    bool isNoon = chkIsNoon.Checked;
                    bool isEvening = chkIsEvening.Checked;
                    bool isNight = chkIsNight.Checked;

                    #region PastMedication
                    PastMedication oMedication = new PastMedication();
                    oMedication.VisitID = AppSession.RegisteredPatient.VisitID;
                    oMedication.MRN = AppSession.RegisteredPatient.MRN;
                    oMedication.ItemID = null;
                    oMedication.GenericName = string.Empty;
                    oMedication.GCDrugForm = "X122^999";
                    oMedication.DrugName = txtItemName.Text;
                    oMedication.Frequency = Convert.ToInt16(frequency);
                    oMedication.GCDosingFrequency = cboFrequencyTimeline.Value.ToString();
                    oMedication.NumberOfDosage = dose;
                    oMedication.GCDosingUnit = cboDosingUnit.Value.ToString();
                    oMedication.DosingDuration = duration;
                    oMedication.GCRoute = cboMedicationRoute.Value.ToString();
                    oMedication.StartDate = startDate;
                    oMedication.EndDate = startDate.AddDays(duration);
                    oMedication.IsAsRequired = chkIsAsRequired.Checked;
                    oMedication.IsExternalMedication = chkIsExternalMedication.Checked;
                    oMedication.MedicationPurpose = txtMedicationPurpose.Text;
                    oMedication.MedicationAdministration = txtMedicationAdministration.Text;
                    
                    oMedication.CreatedBy = AppSession.UserLogin.UserID;
                    oMedication.CreatedDate = DateTime.Now;
                    int pastMedicationID = pastMedicationDao.InsertReturnPrimaryKeyID(oMedication);
                    #endregion

                    DateTime date = startDate;

                    for (int i = 0; i < duration; i++)
                    {
                        DateTime medicationDate = startDate.AddDays(i);
                        for (int j = 1; j <= frequency; j++)
                        {
                            MedicationSchedule oSchedule = new MedicationSchedule();
                            oSchedule.VisitID = AppSession.RegisteredPatient.VisitID;
                            oSchedule.PastMedicationID = pastMedicationID;
                            oSchedule.ItemName = txtItemName.Text;
                            oSchedule.MedicationDate = medicationDate;
                            oSchedule.SequenceNo = j.ToString();
                            switch (j)
                            {
                                case 1:
                                    oSchedule.MedicationTime = txtStartTime1.Text;
                                    break;
                                case 2:
                                    oSchedule.MedicationTime = txtStartTime2.Text;
                                    break;
                                case 3:
                                    oSchedule.MedicationTime = txtStartTime3.Text;
                                    break;
                                case 4:
                                    oSchedule.MedicationTime = txtStartTime4.Text;
                                    break;
                                case 5:
                                    oSchedule.MedicationTime = txtStartTime5.Text;
                                    break;
                                case 6:
                                    oSchedule.MedicationTime = txtStartTime6.Text;
                                    break;
                                default:
                                    oSchedule.MedicationTime = "00:00";
                                    break;
                            }

                            oSchedule.NumberOfDosage = dose;
                            oSchedule.NumberOfDosageInString = dose.ToString("G29");
                            oSchedule.GCDosingUnit = cboDosingUnit.Value.ToString();
                            oSchedule.ConversionFactor = 1;
                            oSchedule.ResultQuantity = 0;
                            oSchedule.ChargeQuantity = 0;
                            oSchedule.IsAsRequired = isPRN;
                            if (j == 0) oSchedule.IsMorning = isMorning;
                            if (j == 1) oSchedule.IsNoon = isNoon;
                            if (j == 2) oSchedule.IsEvening = isEvening;
                            if (j == 3) oSchedule.IsNight = isNight;
                            oSchedule.GCRoute = cboMedicationRoute.Value.ToString();
                            if (cboCoenamRule.Value != null)
                                oSchedule.GCCoenamRule = cboCoenamRule.Value.ToString();                                
                            oSchedule.MedicationAdministration = txtMedicationAdministration.Text;
                            oSchedule.GCMedicationStatus = Constant.MedicationStatus.DIPROSES_FARMASI;
                            oSchedule.IsInternalMedication = false;
                            oSchedule.CreatedBy = AppSession.UserLogin.UserID;
                            scheduleDao.Insert(oSchedule);
                        }                    
                    }

                    result = string.Format("process|1|Medication schedule was created successfully for <b>{0}</b>||", txtItemName.Text);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = string.Format("process|0|Validation Error for Creating Medication Schedule for {0} : {1}||", txtItemName.Text, validationErrMsg);
                    ctx.RollBackTransaction();
                }
            }
            catch (Exception ex)
            {
                result = string.Format("process|0|{0}||", ex.Message);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private bool IsValid(ref string errMessage)
        {
            StringBuilder message = new StringBuilder();

            string itemName = txtItemName.Text;

            if (string.IsNullOrEmpty(itemName))
                message.AppendLine("Drug name should be filled");            

            if (string.IsNullOrEmpty(cboDosingUnit.Value.ToString()))
                message.AppendLine("Dosing unit should be filled");

            if (string.IsNullOrEmpty(cboDosingUnit.Value.ToString()))
                message.AppendLine("Dosing unit should be filled");

            int frequencyNo = Convert.ToInt16(txtFrequencyNumber.Text);
            if (frequencyNo <= 0)
                message.AppendLine("Frequency number should be greater than 0");

            decimal numberOfDosage = Convert.ToDecimal(txtDosingDose.Text);
            if (numberOfDosage <= 0)
                message.AppendLine("Frequency number should be greater than 0");

            int duration = Convert.ToInt16(txtDosingDuration.Text);
            if (numberOfDosage <= 0)
                message.AppendLine("Duration should be greater than 0");

            errMessage = message.ToString();
            return string.IsNullOrEmpty(errMessage);
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
        }
    }
}