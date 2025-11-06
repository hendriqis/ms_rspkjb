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
    public partial class EditMedicationSignaCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            this.PopupTitle = "Edit Medication Signa";

            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            string[] paramInfo = param.Split('|');
            hdnPrescriptionOrderDetailID.Value = paramInfo[0];
            if (!string.IsNullOrEmpty(paramInfo[1]))
                hdnDispensaryServiceUnitID.Value = Convert.ToString(paramInfo[1]);
            else
                hdnDispensaryServiceUnitID.Value = string.Empty;

            SetControlProperties();
            
            if (hdnPrescriptionOrderDetailID.Value != null && hdnPrescriptionOrderDetailID.Value != "" && hdnPrescriptionOrderDetailID.Value != "0")
            {
                EntityToControl();

                int id = Convert.ToInt32(hdnPrescriptionOrderDetailID.Value);
                string filterExp = string.Format("PrescriptionOrderDetailID = {0}", id);
                List<vPrescriptionOrderDt1> oItemList = BusinessLayer.GetvPrescriptionOrderDt1List(filterExp);
                if (oItemList.Count() > 0)
                {
                    vPrescriptionOrderDt1 oItem = oItemList.FirstOrDefault();

                    //Serialize old object 
                    hdnSelectedItem.Value = JsonConvert.SerializeObject(oItem);

                    txtItemName.Text = oItem.DrugName;
                    hdnOldFrequency.Value = oItem.Frequency.ToString();
                    txtFrequencyNumber.Text = oItem.Frequency.ToString();
                    cboFrequencyTimeline.Value = oItem.GCDosingFrequency;
                    txtDosingDose.Text = oItem.NumberOfDosage.ToString();
                    txtDosingUnit.Text = oItem.DosingUnit;
                    chkIsAsRequired.Checked = oItem.IsAsRequired;
                    chkIsMorning.Checked = oItem.IsMorning;
                    chkIsNoon.Checked = oItem.IsNoon;
                    chkIsEvening.Checked = oItem.IsEvening;
                    chkIsNight.Checked = oItem.IsNight;
                    txtStartTime1.Text = oItem.Sequence1Time;
                    txtStartTime2.Text = oItem.Sequence2Time;
                    txtStartTime3.Text = oItem.Sequence3Time;
                    txtStartTime4.Text = oItem.Sequence4Time;
                    txtStartTime5.Text = oItem.Sequence5Time;
                    txtStartTime6.Text = oItem.Sequence6Time;
                    txtMedicationAdministration.Text = oItem.MedicationAdministration;
                    cboMedicationRoute.Value = oItem.GCRoute;
                }
            }
        }
        
        private void SetControlProperties()
        {
            //Load Physician
            string filterExpression = string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID);
            List<vParamedicTeam> lstPhysician = BusinessLayer.GetvParamedicTeamList(filterExpression);
            Methods.SetComboBoxField(cboPhysician, lstPhysician, "ParamedicName", "ParamedicID");
            cboPhysician.Value = AppSession.RegisteredPatient.ParamedicID.ToString();
            txtStartTime1.Attributes.Add("validationgroup", "mpvPrescriptionOrderDt1");
            txtStartTime2.Attributes.Add("validationgroup", "mpvPrescriptionOrderDt1");
            txtStartTime3.Attributes.Add("validationgroup", "mpvPrescriptionOrderDt1");
            txtStartTime4.Attributes.Add("validationgroup", "mpvPrescriptionOrderDt1");
            txtStartTime5.Attributes.Add("validationgroup", "mpvPrescriptionOrderDt1");
            txtStartTime6.Attributes.Add("validationgroup", "mpvPrescriptionOrderDt1");
            txtStartTime1.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            txtStartTime2.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            txtStartTime3.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            txtStartTime4.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            txtStartTime5.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            txtStartTime6.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.DOSING_FREQUENCY, Constant.StandardCode.MEDICATION_ROUTE, Constant.StandardCode.COENAM_RULE, Constant.StandardCode.PHYSICIAN_INSTRUCTION_SOURCE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboFrequencyTimeline, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DOSING_FREQUENCY).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboMedicationRoute, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.MEDICATION_ROUTE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboCoenamRule, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.COENAM_RULE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboPhysicianInstructionSource, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.PHYSICIAN_INSTRUCTION_SOURCE).ToList(), "StandardCodeName", "StandardCodeID");

            cboFrequencyTimeline.SelectedIndex = 1;
            cboMedicationRoute.SelectedIndex = 0;

            txtStartDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            if (AppSession.UserLogin.ParamedicID != null)
            {
                chkIsGenerateCPPT.Enabled = true;
            }
            else
            {
                chkIsGenerateCPPT.Checked = false;
                chkIsGenerateCPPT.Enabled = false;
            }
        }

        private void EntityToControl()
        {
            int id = Convert.ToInt32(hdnPrescriptionOrderDetailID.Value);
            string filterExp = string.Format("PrescriptionOrderDetailID = {0}", id);
            vPrescriptionOrderDt1 entity = BusinessLayer.GetvPrescriptionOrderDt1List(filterExp).FirstOrDefault();
            txtStartTime1.Text = entity.Sequence1Time;
            txtStartTime2.Text = entity.Sequence2Time;
            txtStartTime3.Text = entity.Sequence3Time;
            txtStartTime4.Text = entity.Sequence4Time;
            txtStartTime5.Text = entity.Sequence5Time;
            txtStartTime6.Text = entity.Sequence6Time;
        }

        protected void cbpPopupProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";

            string recordID = hdnSelectedID.Value;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;

            result = UpdateMedicationSchedule(recordID);

            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = result;
        }

        private string UpdateMedicationSchedule(string scheduleID)
        {
            string result = string.Empty;
            IDbContext ctx = DbFactory.Configure(true);
            MedicationScheduleDao scheduleDao = new MedicationScheduleDao(ctx);
            PatientVisitNoteDao visitNoteDao = new PatientVisitNoteDao(ctx);
            PrescriptionOrderDtDao orderDtDao = new PrescriptionOrderDtDao(ctx);
            PrescriptionOrderDtSignaDao signaDao = new PrescriptionOrderDtSignaDao(ctx);
            PrescriptionOrderDtLogDao orderLogDao = new PrescriptionOrderDtLogDao(ctx);
            PrescriptionOrderChangesLogDao changesLogDao = new PrescriptionOrderChangesLogDao(ctx);
            PharmacyJournalDao journalDao = new PharmacyJournalDao(ctx);
            try
            {
                string validationErrMsg = string.Empty;
                if (IsValid(ref validationErrMsg))
                {
                    vPrescriptionOrderDt1 oOldItem = JsonConvert.DeserializeObject<vPrescriptionOrderDt1>(hdnSelectedItem.Value);
                    decimal dose = oOldItem.NumberOfDosage;
                    decimal newDose = Convert.ToDecimal(txtDosingDose.Text);
                    int frequency = Convert.ToInt16(txtFrequencyNumber.Text);
                    int addition = frequency - oOldItem.Frequency;
                    bool isAdding = addition > 0;
                    bool isDoseChanged = dose != newDose;

                    DateTime startDate = Helper.GetDatePickerValue(txtStartDate.Text);
                    DateTime endDate = oOldItem.StartDate.AddDays(Convert.ToDouble(txtDosingDuration.Text));
                    int duration = endDate.Subtract(startDate).Days;

                    bool isPRN = chkIsAsRequired.Checked;
                    bool isMorning = chkIsMorning.Checked;
                    bool isNoon = chkIsNoon.Checked;
                    bool isEvening = chkIsEvening.Checked;
                    bool isNight = chkIsNight.Checked;

                    DateTime date = startDate;

                    while (date <= oOldItem.cfEndDate1)
                    {
                        if (isAdding)
                        {
                            for (int j = 1; j <= frequency; j++)
                            {
                                string filterExpression = string.Format("PrescriptionOrderDetailID = {0} AND ItemID = {1} AND MedicationDate = '{2}' AND SequenceNo = '{3}'", oOldItem.PrescriptionOrderDetailID, oOldItem.ItemID, date.ToString(Constant.FormatString.DATE_FORMAT_112), j);
                                MedicationSchedule oSchedule = BusinessLayer.GetMedicationScheduleList(filterExpression, ctx).FirstOrDefault();

                                if (oSchedule == null)
                                {
                                    oSchedule = new MedicationSchedule();
                                    oSchedule.VisitID = AppSession.RegisteredPatient.VisitID;
                                    oSchedule.PrescriptionOrderID = oOldItem.PrescriptionOrderID;
                                    oSchedule.PrescriptionOrderDetailID = oOldItem.PrescriptionOrderDetailID;
                                    oSchedule.ItemID = oOldItem.ItemID;
                                    oSchedule.ItemName = oOldItem.DrugName;
                                    oSchedule.MedicationDate = date;
                                    oSchedule.SequenceNo = j.ToString();
                                    oSchedule.MedicationTime = GetSequenceMedicationTime(j);
                                    oSchedule.NumberOfDosage = newDose;
                                    oSchedule.NumberOfDosageInString = newDose.ToString("G29");
                                    oSchedule.GCDosingUnit = oOldItem.GCDosingUnit;
                                    oSchedule.ConversionFactor = oOldItem.ConversionFactor;
                                    oSchedule.ResultQuantity = 0;
                                    oSchedule.ChargeQuantity = 0;
                                    oSchedule.IsAsRequired = isPRN;
                                    oSchedule.IsUsingUDD = oOldItem.IsUsingUDD;
                                    if (j == 0) oSchedule.IsMorning = isMorning;
                                    if (j == 1) oSchedule.IsNoon = isNoon;
                                    if (j == 2) oSchedule.IsEvening = isEvening;
                                    if (j == 3) oSchedule.IsNight = isNight;
                                    oSchedule.GCRoute = oOldItem.GCRoute;
                                    oSchedule.GCCoenamRule = oOldItem.GCCoenamRule;
                                    oSchedule.MedicationAdministration = txtMedicationAdministration.Text;
                                    oSchedule.GCMedicationStatus = Constant.MedicationStatus.OPEN;
                                    oSchedule.IsInternalMedication = true;
                                    oSchedule.CreatedBy = AppSession.UserLogin.UserID;
                                    scheduleDao.Insert(oSchedule);
                                }
                                else
                                {
                                    if (oSchedule.GCMedicationStatus == Constant.MedicationStatus.OPEN || oSchedule.GCMedicationStatus == Constant.MedicationStatus.DISCONTINUE)
                                    {
                                        oSchedule.MedicationTime = GetSequenceMedicationTime(j);
                                        oSchedule.NumberOfDosage = Convert.ToDecimal(txtDosingDose.Text);
                                        oSchedule.NumberOfDosageInString = txtDosingDose.Text;
                                        oSchedule.GCMedicationStatus = Constant.MedicationStatus.OPEN;
                                        oSchedule.IsInternalMedication = true;
                                        oSchedule.IsAsRequired = chkIsAsRequired.Checked;
                                        if (cboCoenamRule.Value != null)
                                            oSchedule.GCCoenamRule = cboCoenamRule.Value.ToString();
                                        oSchedule.MedicationAdministration = txtMedicationAdministration.Text;
                                        oSchedule.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        scheduleDao.Update(oSchedule);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (addition == 0)
                            {
                                //same frequency - difference dosage
                                int itemID = oOldItem.ItemID;
                                string filterExpression = string.Format("PrescriptionOrderDetailID = {0} AND ItemID = {1} AND MedicationDate >= '{2}'", oOldItem.PrescriptionOrderDetailID, itemID.ToString(), Helper.GetDatePickerValue(txtStartDate).ToString(Constant.FormatString.DATE_FORMAT_112), txtFrequencyNumber.Text);
                                List<MedicationSchedule> lstSchedule = BusinessLayer.GetMedicationScheduleList(filterExpression, ctx);
                                foreach (MedicationSchedule oSchedule in lstSchedule)
                                {
                                    if (oSchedule.GCMedicationStatus == Constant.MedicationStatus.OPEN)
                                    {
                                        oSchedule.MedicationTime = GetSequenceMedicationTime(Convert.ToInt16(oSchedule.SequenceNo));
                                        oSchedule.NumberOfDosage = Convert.ToDecimal(txtDosingDose.Text);
                                        oSchedule.NumberOfDosageInString = txtDosingDose.Text;
                                        oSchedule.IsAsRequired = chkIsAsRequired.Checked;
                                        if (cboCoenamRule.Value != null)
                                            oSchedule.GCCoenamRule = cboCoenamRule.Value.ToString();
                                        oSchedule.MedicationAdministration = txtMedicationAdministration.Text;
                                        oSchedule.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        scheduleDao.Update(oSchedule);
                                    }
                                }
                            }
                            else
                            {
                                //change number of dosage
                                int itemID = oOldItem.ItemID;
                                string filterExpression = string.Empty;
                                List<MedicationSchedule> lstSchedule = new List<MedicationSchedule>();
                                //if (dose != newDose) // ditutup oleh RN per 20211113 (patch 202111-02) karna saat change signa hanya mengubah presOrderDt tidak medication schedule nya
                                //{
                                    filterExpression = string.Format("PrescriptionOrderDetailID = {0} AND ItemID = {1} AND MedicationDate >= '{2}' AND SequenceNo <= '{3}'", oOldItem.PrescriptionOrderDetailID, itemID.ToString(), Helper.GetDatePickerValue(txtStartDate).ToString(Constant.FormatString.DATE_FORMAT_112), txtFrequencyNumber.Text);
                                    lstSchedule = BusinessLayer.GetMedicationScheduleList(filterExpression, ctx);
                                    foreach (MedicationSchedule oSchedule in lstSchedule)
                                    {
                                        if (oSchedule.GCMedicationStatus == Constant.MedicationStatus.OPEN)
                                        {
                                            oSchedule.MedicationTime = GetSequenceMedicationTime(Convert.ToInt16(oSchedule.SequenceNo));
                                            oSchedule.NumberOfDosage = Convert.ToDecimal(txtDosingDose.Text);
                                            oSchedule.NumberOfDosageInString = txtDosingDose.Text;
                                            oSchedule.IsAsRequired = chkIsAsRequired.Checked;
                                            if (cboCoenamRule.Value != null)
                                                oSchedule.GCCoenamRule = cboCoenamRule.Value.ToString();
                                            oSchedule.MedicationAdministration = txtMedicationAdministration.Text;
                                            oSchedule.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            scheduleDao.Update(oSchedule);
                                        }
                                    } 
                                //}

                                //reduce frequency
                                filterExpression = string.Format("PrescriptionOrderDetailID = {0} AND ItemID = {1} AND MedicationDate >= '{2}' AND SequenceNo > '{3}'", oOldItem.PrescriptionOrderDetailID, itemID.ToString(), Helper.GetDatePickerValue(txtStartDate).ToString(Constant.FormatString.DATE_FORMAT_112), txtFrequencyNumber.Text);
                                lstSchedule = BusinessLayer.GetMedicationScheduleList(filterExpression, ctx);
                                foreach (MedicationSchedule oSchedule in lstSchedule)
                                {
                                    oSchedule.MedicationTime = "00:00";
                                    oSchedule.GCMedicationStatus = Constant.MedicationStatus.DISCONTINUE;
                                    oSchedule.GCDiscontinueReason = Constant.DiscontinueMedicationReason.OTHER;
                                    oSchedule.DiscontinueDate = Helper.GetDatePickerValue(txtStartDate);
                                    oSchedule.DiscontinueBy = AppSession.UserLogin.UserID;
                                    oSchedule.DiscontinueReason = "Perubahan aturan pakai";
                                    oSchedule.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    scheduleDao.Update(oSchedule);
                                }
                            }
                        }
                        date = date.AddDays(1);
                    }

                    #region Update Prescription Order Detail
                    PrescriptionOrderDt orderDt = BusinessLayer.GetPrescriptionOrderDt(oOldItem.PrescriptionOrderDetailID);
                    if (orderDt != null)
                    {
                        orderDt.StartDate = Helper.GetDatePickerValue(txtStartDate);
                        orderDt.StartTime = txtStartTime1.Text;
                        orderDt.Frequency = Convert.ToInt16(txtFrequencyNumber.Text);
                        orderDt.NumberOfDosage = Convert.ToDecimal(txtDosingDose.Text);
                        orderDt.IsAsRequired = chkIsAsRequired.Checked;
                        if (cboCoenamRule.Value != null)
                            orderDt.GCCoenamRule = cboCoenamRule.Value.ToString();
                        orderDt.Sequence1Time = txtStartTime1.Text.Replace('.', ':');
                        orderDt.Sequence2Time = txtStartTime2.Text.Replace('.', ':');
                        orderDt.Sequence3Time = txtStartTime3.Text.Replace('.', ':');
                        orderDt.Sequence4Time = txtStartTime4.Text.Replace('.', ':');
                        orderDt.Sequence5Time = txtStartTime5.Text.Replace('.', ':');
                        orderDt.Sequence6Time = txtStartTime6.Text.Replace('.', ':');
                        orderDt.MedicationAdministration = txtMedicationAdministration.Text;
                        orderDtDao.Update(orderDt);
                    }
                    #endregion

                    #region Log Prescription Order Changes
                    PrescriptionOrderDtSigna orderDtSigna = new PrescriptionOrderDtSigna();
                    orderDtSigna.PrescriptionOrderDetailID = oOldItem.PrescriptionOrderDetailID;
                    orderDtSigna.Frequency = oOldItem.Frequency;
                    orderDtSigna.GCDosingFrequency = oOldItem.GCDosingFrequency;
                    orderDtSigna.NumberOfDosage = oOldItem.NumberOfDosage;
                    orderDtSigna.NumberOfDosageInString = oOldItem.NumberOfDosageInString;
                    orderDtSigna.GCCompoundUnit = oOldItem.GCCompoundUnit;
                    orderDtSigna.GCDosingUnit = oOldItem.GCDosingUnit;
                    orderDtSigna.StartDate = oOldItem.StartDate;
                    orderDtSigna.StartTime = oOldItem.StartTime;
                    orderDtSigna.DosingDuration = oOldItem.DosingDuration;
                    orderDtSigna.GCRoute = oOldItem.GCRoute;
                    orderDtSigna.GCCoenamRule = oOldItem.GCCoenamRule;
                    orderDtSigna.MedicationAdministration = oOldItem.MedicationAdministration;
                    orderDtSigna.MedicationPurpose = oOldItem.MedicationPurpose;
                    orderDtSigna.IsAsRequired = oOldItem.IsAsRequired;
                    orderDtSigna.IsMorning = oOldItem.IsMorning;
                    orderDtSigna.IsNoon = oOldItem.IsNoon;
                    orderDtSigna.IsEvening = oOldItem.IsEvening;
                    orderDtSigna.IsNight = oOldItem.IsNight;
                    if (!string.IsNullOrEmpty(hdnPlanningNoteID.Value) && hdnPlanningNoteID.Value != "0")
                    {
                        orderDtSigna.PatientVisitNoteID = Convert.ToInt32(hdnPlanningNoteID.Value);
                    }
                    orderDtSigna.CreatedBy = AppSession.UserLogin.UserID;
                    orderDtSigna.CreatedDate = DateTime.Now;
                    signaDao.Insert(orderDtSigna);

                    PrescriptionOrderDtLog orderDtLog = new PrescriptionOrderDtLog();
                    orderDtLog.LogDate = DateTime.Now;
                    orderDtLog.PrescriptionOrderDetailID = oOldItem.PrescriptionOrderDetailID;
                    orderDtLog.OldValues = hdnSelectedItem.Value;
                    orderDtLog.NewValues = JsonConvert.SerializeObject(orderDt);
                    orderDtLog.UserID = AppSession.UserLogin.UserID;
                    orderLogDao.Insert(orderDtLog);
                    #endregion

                    int linkedNoteID = 0;
                    if (hdnPlanningNoteID.Value != "" && hdnPlanningNoteID.Value != "0")
                        linkedNoteID = Convert.ToInt32(hdnPlanningNoteID.Value);

                    StringBuilder pharmacistNoteText = new StringBuilder();

                    if (!string.IsNullOrEmpty(txtPharmacyNoteText.Text))
                        pharmacistNoteText.AppendLine(txtPharmacyNoteText.Text);
                    else
                        pharmacistNoteText.AppendLine("Change Medication Signatura :");
                    string signa = string.Format("S {0} x {1} {2}", txtFrequencyNumber.Text, txtDosingDose.Text, Request.Form[txtDosingUnit.UniqueID]);
                    string noteDetail = string.Format("- {0} {1}, Start Date : {2}", orderDt.DrugName, signa, txtStartDate.Text);
                    pharmacistNoteText.AppendLine(noteDetail);

                    #region Pharmacist Notes Detail
                    if (chkIsGenerateCPPT.Checked)
                    {
                        if (cboPhysicianInstructionSource.Value != null)
                        {
                            pharmacistNoteText.AppendLine(string.Format("Cara Pemberian Instruksi : {0}", cboPhysicianInstructionSource.Text));
                        }

                        PatientVisitNote note = new PatientVisitNote();
                        note.VisitID = Convert.ToInt32(hdnVisitID.Value);
                        note.GCPatientNoteType = Constant.PatientVisitNotes.PHARMACY_NOTES;
                        note.NoteDate = DateTime.Now.Date;
                        note.NoteTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                        if (!string.IsNullOrEmpty(hdnDispensaryServiceUnitID.Value))
                        {
                            note.HealthcareServiceUnitID = Convert.ToInt32(hdnDispensaryServiceUnitID.Value);
                        }
                        note.ParamedicID = AppSession.UserLogin.ParamedicID;
                        note.NoteText = pharmacistNoteText.ToString();
                        if (linkedNoteID != 0)
                            note.LinkedNoteID = linkedNoteID;
                        if (cboPhysicianInstructionSource.Value != null)
                            note.GCPhysicianInstructionSource = cboPhysicianInstructionSource.Value.ToString();
                        if (chkIsNeedConfirmation.Checked)
                        {
                            note.IsNeedConfirmation = chkIsNeedConfirmation.Checked; if (hdnParamedicID.Value != "" && hdnParamedicID.Value != "0")
                                note.ConfirmationPhysicianID = Convert.ToInt32(hdnParamedicID.Value);
                        }
                        note.CreatedBy = AppSession.UserLogin.UserID;
                        visitNoteDao.Insert(note);

                        //Confirm Notification Note
                        if (linkedNoteID != 0)
                        {
                            PatientVisitNote oNote = BusinessLayer.GetPatientVisitNote(linkedNoteID);
                            if (oNote.NotificationParamedicID == null)
                            {
                                oNote.NotificationDateTime = DateTime.Now;
                                oNote.NotificationParamedicID = AppSession.UserLogin.ParamedicID;
                                oNote.LastUpdatedBy = AppSession.UserLogin.UserID;
                                visitNoteDao.Update(oNote);
                            }
                        } 
                    }
                    #endregion


                    #region Pharmacy Journal
                    if (chkIsGenerateJournal.Checked)
                    {
                        PharmacyJournal oJournal = new PharmacyJournal();
                        oJournal.JournalDate = DateTime.Now.Date;
                        oJournal.JournalTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                        oJournal.VisitID = AppSession.RegisteredPatient.VisitID;
                        oJournal.HealthcareServiceUnitID = Convert.ToInt32(hdnDispensaryServiceUnitID.Value);
                        oJournal.JournalText = pharmacistNoteText.ToString();
                        if (linkedNoteID != 0)
                        {
                            oJournal.PatientVisitNoteID = linkedNoteID;
                        }
                        oJournal.CreatedBy = AppSession.UserLogin.UserID;
                        journalDao.Insert(oJournal);
                    }
                    #endregion

                    ctx.CommitTransaction();

                    string message = string.Format("Medication Signature has been changed successfully for item <b>{0}</b>", Request.Form[txtItemName.UniqueID]);
                    result = string.Format("process|1|{0}||", message);
                }
                else
                {
                    string message = string.Format("Validation Error for Medication Signa Change {0} : {1}", Request.Form[txtItemName.UniqueID], validationErrMsg);
                    result = string.Format("process|0|{0}||", message);
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

        private string GetSequenceMedicationTime(int sequence)
        {
            string medicationTime = "";
            switch (sequence)
            {
                case 1:
                    medicationTime = txtStartTime1.Text;
                    break;
                case 2:
                    medicationTime = txtStartTime2.Text;
                    break;
                case 3:
                    medicationTime = txtStartTime3.Text;
                    break;
                case 4:
                    medicationTime = txtStartTime4.Text;
                    break;
                case 5:
                    medicationTime = txtStartTime5.Text;
                    break;
                case 6:
                    medicationTime = txtStartTime6.Text;
                    break;
                default:
                    medicationTime = "00:00";
                    break;
            }
            return medicationTime;
        }

        private bool IsValid(ref string errMessage)
        {
            vPrescriptionOrderDt1 oOldItem = JsonConvert.DeserializeObject<vPrescriptionOrderDt1>(hdnSelectedItem.Value);
            if (oOldItem != null)
            {
                int frequencyNo = Convert.ToInt16(txtFrequencyNumber.Text);
                if (frequencyNo <= 0)
                {
                    errMessage = "Frequency number should be greater than 0";
                    return false;
                }

                //if (frequencyNo.Equals(oOldItem.Frequency))
                //{
                //    errMessage = "Frequency number should be different with old value (" + frequencyNo.ToString() + ")";
                //    return false;
                //}
                return true;
            }
            return true;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
        }
    }
}