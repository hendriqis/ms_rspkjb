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
using System.Globalization;
using Newtonsoft.Json;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class NurseEditMedicationSigna2Ctl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnHealthcareServiceUnitID.Value = paramInfo[0];
            txtDefaultStartDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            SetControlProperties();
        }

        private bool IsValidated(string lstDuration, string lstStartDate, string lstTime1, string lstTime2, string lstTime3, string lstTime4, string lstTime5, string lstTime6, ref string result)
        {
            string message = string.Empty;

            if (string.IsNullOrEmpty(message))
            {
                #region Validate Duration
                string[] selectedDuration = lstDuration.Split(',');
                foreach (string duration in selectedDuration)
                {
                    if (string.IsNullOrEmpty(duration))
                    {
                        message = "Ada obat dengan jumlah hari extend yang belum diisi";
                        break;
                    }
                    else
                    {
                        Decimal value;
                        if (!Decimal.TryParse(duration, out value))
                        {
                            message = string.Format("Ada obat dengan jumlah hari extend yang tidak sesuai ({0})", duration);
                            break;
                        }
                        else
                        {
                            if (value == 0)
                            {
                                message = string.Format("Ada obat dengan jumlah hari extend yang tidak sesuai ({0})", duration);
                                break;
                            }
                        }
                    }
                }
                #endregion

                #region Validate Start Date
                string[] selectedDate = lstStartDate.Split(',');
                foreach (string date in selectedDate)
                {
                    if (string.IsNullOrEmpty(date))
                    {
                        message = "Ada obat dengan tanggal mulai perubahan yang belum diisi";
                        break;
                    }
                    else
                    {
                        DateTime startDate;
                        string format = Constant.FormatString.DATE_PICKER_FORMAT;
                        try
                        {
                            startDate = DateTime.ParseExact(date, format, CultureInfo.InvariantCulture);
                        }
                        catch (FormatException)
                        {
                            message = string.Format("Ada obat dengan tanggal mulai perubahan {0} yang tidak benar", date);
                            break;
                        }
                        DateTime sdate = Helper.GetDatePickerValue(date);
                        if (DateTime.Compare(sdate, DateTime.Now.Date) < 0)
                        {
                            message = "Tanggal mulai perubahan harus lebih besar atau sama dengan tanggal hari ini.";
                            break;
                        }
                    }
                }
                #endregion

                #region Validate Medication Time
                string[] lstMedicationTime = lstTime1.Split(',');
                foreach (string time in lstMedicationTime)
                {
                    if (time != "-")
                    {
                        if (!string.IsNullOrEmpty(time) || time == "__:__")
                        {
                            Regex reg = new Regex(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$");
                            if (!reg.IsMatch(time))
                            {
                                message = "Waktu pemberian obat sequence #1 harus sesuai dengan format jam (00:00 s/d 23:59)";
                                return false;
                            }
                        }
                    }
                }

                lstMedicationTime = lstTime2.Split(',');
                foreach (string time in lstMedicationTime)
                {
                    if (time != "-")
                    {
                        if (!string.IsNullOrEmpty(time) || time == "__:__")
                        {
                            Regex reg = new Regex(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$");
                            if (!reg.IsMatch(time))
                            {
                                message = "Waktu pemberian obat sequence #2 harus sesuai dengan format jam (00:00 s/d 23:59)";
                                return false;
                            }
                        }
                    }
                }

                lstMedicationTime = lstTime3.Split(',');
                foreach (string time in lstMedicationTime)
                {
                    if (time != "-")
                    {
                        if (!string.IsNullOrEmpty(time) || time == "__:__")
                        {
                            Regex reg = new Regex(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$");
                            if (!reg.IsMatch(time))
                            {
                                message = "Waktu pemberian obat sequence #3 harus sesuai dengan format jam (00:00 s/d 23:59)";
                                return false;
                            }
                        }
                    }
                }

                lstMedicationTime = lstTime4.Split(',');
                foreach (string time in lstMedicationTime)
                {
                    if (time != "-")
                    {
                        if (!string.IsNullOrEmpty(time) || time == "__:__")
                        {
                            Regex reg = new Regex(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$");
                            if (!reg.IsMatch(time))
                            {
                                message = "Waktu pemberian obat sequence #4 harus sesuai dengan format jam (00:00 s/d 23:59)";
                                return false;
                            }
                        }
                    }
                }

                lstMedicationTime = lstTime5.Split(',');
                foreach (string time in lstMedicationTime)
                {
                    if (time != "-")
                    {
                        if (!string.IsNullOrEmpty(time) || time == "__:__")
                        {
                            Regex reg = new Regex(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$");
                            if (!reg.IsMatch(time))
                            {
                                message = "Waktu pemberian obat sequence #5 harus sesuai dengan format jam (00:00 s/d 23:59)";
                                return false;
                            }
                        }
                    }
                }

                lstMedicationTime = lstTime6.Split(',');
                foreach (string time in lstMedicationTime)
                {
                    if (time != "-")
                    {
                        if (!string.IsNullOrEmpty(time) || time == "__:__")
                        {
                            Regex reg = new Regex(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$");
                            if (!reg.IsMatch(time))
                            {
                                message = "Waktu pemberian obat sequence #6 harus sesuai dengan format jam (00:00 s/d 23:59)";
                                return false;
                            }
                        }
                    }
                } 
                #endregion
            }

            if (!string.IsNullOrEmpty(message))
            {
                result = message;
            }
            return message == string.Empty;
        }

        protected void cbpPopupProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";

            string lstRecordID = hdnSelectedID.Value;
            string lstFrequency = hdnSelectedFrequency.Value;
            string lstQuantity = hdnSelectedQuantity.Value;
            string lstStartDate = hdnSelectedStartDate.Value;
            string lstDuration = hdnSelectedDuration.Value;
            string lstSelectedTime1 = hdnSelectedTime1.Value;
            string lstSelectedTime2 = hdnSelectedTime2.Value;
            string lstSelectedTime3 = hdnSelectedTime3.Value;
            string lstSelectedTime4 = hdnSelectedTime4.Value;
            string lstSelectedTime5 = hdnSelectedTime5.Value;
            string lstSelectedTime6 = hdnSelectedTime6.Value;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;

            string errorMessage = string.Empty;
            bool isError = false;
            if (!IsValidated(lstDuration,lstStartDate, lstSelectedTime1, lstSelectedTime2, lstSelectedTime3, lstSelectedTime4, lstSelectedTime5, lstSelectedTime6, ref errorMessage))
            {
                isError = true;
                result = string.Format("process|0|{0}", errorMessage);
            }

            if (!isError)
            {
                result = ProcessSelectedItems(lstRecordID,lstFrequency, lstQuantity, lstStartDate, lstDuration, lstSelectedTime1, lstSelectedTime2, lstSelectedTime3, lstSelectedTime4, lstSelectedTime5, lstSelectedTime6); 
            }

            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = result;
        }

        private string ProcessSelectedItems(string lstRecordID, string lstFrequency, string lstQuantity, string lstStartDate, string lstDuration, string lstSelectedTime1, string lstSelectedTime2,
            string lstSelectedTime3, string lstSelectedTime4, string lstSelectedTime5, string lstSelectedTime6)
        {
            string result = string.Empty;
            String[] selectedID = lstRecordID.Split(',');
            String[] selectedStartDate = lstStartDate.Split(',');
            String[] selectedDuration = lstDuration.Split(',');
            String[] selectedFrequency = lstFrequency.Split(',');
            String[] selectedQuantity = lstQuantity.Split(',');
            String[] selectedTime1 = lstSelectedTime1.Split(',');
            String[] selectedTime2 = lstSelectedTime2.Split(',');
            String[] selectedTime3 = lstSelectedTime3.Split(',');
            String[] selectedTime4 = lstSelectedTime4.Split(',');
            String[] selectedTime5 = lstSelectedTime5.Split(',');
            String[] selectedTime6 = lstSelectedTime6.Split(',');

            IDbContext ctx = DbFactory.Configure(true);
            PatientVisitNoteDao visitNoteDao = new PatientVisitNoteDao(ctx);
            MedicationScheduleDao scheduleDao = new MedicationScheduleDao(ctx);
            PrescriptionOrderDtDao orderDtDao = new PrescriptionOrderDtDao(ctx);
            PrescriptionOrderChangesLogDao changesLogDao = new PrescriptionOrderChangesLogDao(ctx);
            NursingJournalDao journalDao = new NursingJournalDao(ctx);

            try
            {
                StringBuilder nursingNoteText = new StringBuilder();

                if (!string.IsNullOrEmpty(txtPharmacyNoteText.Text))
                    nursingNoteText.AppendLine(txtPharmacyNoteText.Text);
                else
                    nursingNoteText.AppendLine("Extend Medication Duration :");

                for (int ct = 0; ct < selectedID.Length; ct++)
                {
                    PrescriptionOrderDt orderDt = BusinessLayer.GetPrescriptionOrderDtList(string.Format("PrescriptionOrderDetailID = {0}", selectedID[ct]),ctx).FirstOrDefault();
                    DateTime startDate = Helper.GetDatePickerValue(selectedStartDate[ct]);
                    int duration = Convert.ToInt16(selectedDuration[ct]);

                    #region Nursing Notes Detail
                    string signa = string.Format("S {0} x {1} {2}", selectedFrequency[ct], selectedQuantity[ct], orderDt.GCDosingUnit.Substring(5));
                    string noteDetail = string.Format("- {0} {1}, Start Date : {2}, Duration : {3}", orderDt.DrugName, signa, startDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT), duration.ToString());
                    nursingNoteText.AppendLine(noteDetail);
                    #endregion

                    #region Log Prescription Order Changes
                    StringBuilder logText = new StringBuilder();

                    if (!string.IsNullOrEmpty(txtPharmacyNoteText.Text))
                        logText.AppendLine(txtPharmacyNoteText.Text);

                    logText.AppendLine("Extend Medication Duration :");
                    logText.AppendLine(noteDetail);

                    PrescriptionOrderChangesLog changesLog = new PrescriptionOrderChangesLog();
                    changesLog.VisitID = Convert.ToInt32(hdnVisitID.Value);
                    changesLog.LogDate = DateTime.Now.Date;
                    changesLog.LogTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    changesLog.GCPrescriptionOrderChangeType = Constant.PrescriptionOrderChangesType.QUANTITY;
                    changesLog.PrescriptionOrderID = orderDt.PrescriptionOrderID;
                    changesLog.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                    changesLog.NoteText = logText.ToString();
                    changesLog.IsAutoGeneratedBySystem = true;
                    changesLog.CreatedBy = AppSession.UserLogin.UserID;
                    changesLogDao.Insert(changesLog);
                    #endregion

                    UpdateMedicationSchedule(orderDt, selectedStartDate[ct], selectedFrequency[ct], selectedQuantity[ct], selectedDuration[ct], selectedTime1[ct], selectedTime2[ct],selectedTime3[ct], selectedTime4[ct], selectedTime5[ct], selectedTime6[ct], ctx);
                }

                #region Nursing Notes


                int linkedNoteID = 0;
                if (hdnPlanningNoteID.Value != "" && hdnPlanningNoteID.Value != "0")
                    linkedNoteID = Convert.ToInt32(hdnPlanningNoteID.Value);

                if (chkIsGenerateCPPT.Checked)
                {
                    PatientVisitNote note = new PatientVisitNote();
                    note.VisitID = Convert.ToInt32(hdnVisitID.Value);
                    note.GCPatientNoteType = Constant.PatientVisitNotes.NURSING_NOTES;
                    note.NoteDate = DateTime.Now.Date;
                    note.NoteTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    note.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                    note.ParamedicID = AppSession.UserLogin.ParamedicID;
                    note.NoteText = nursingNoteText.ToString();

                    if (cboPhysicianInstructionSource.Value != null)
                        note.GCPhysicianInstructionSource = cboPhysicianInstructionSource.Value.ToString();
                    if (linkedNoteID != 0)
                        note.LinkedNoteID = linkedNoteID;

                    if (chkIsNeedConfirmation.Checked)
                    {
                        note.IsNeedConfirmation = chkIsNeedConfirmation.Checked;
                        if (hdnParamedicID.Value != "" && hdnParamedicID.Value != "0")
                            note.ConfirmationPhysicianID = Convert.ToInt32(hdnParamedicID.Value);
                    }
                    visitNoteDao.Insert(note); 
                }

                //Confirm Notification Note
                if (linkedNoteID != 0)
                {
                    PatientVisitNote oNote = BusinessLayer.GetPatientVisitNote(linkedNoteID);
                    if (oNote.NotificationParamedicID == null)
                    {
                        oNote.NotificationDateTime = DateTime.Now;
                        oNote.NotificationParamedicID = AppSession.UserLogin.ParamedicID;
                        visitNoteDao.Update(oNote);
                    }
                }
                #endregion

                #region Nursing Journal
                if (chkIsGenerateJournal.Checked)
                {
                    NursingJournal oJournal = new NursingJournal();
                    oJournal.JournalDate = DateTime.Now.Date;
                    oJournal.JournalTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    oJournal.VisitID = AppSession.RegisteredPatient.VisitID;
                    oJournal.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                    oJournal.NursingJournalText = nursingNoteText.ToString();
                    oJournal.Remarks = nursingNoteText.ToString();
                    oJournal.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                    if (linkedNoteID != 0)
                    {
                        oJournal.PatientVisitNoteID = linkedNoteID;
                    }
                    journalDao.Insert(oJournal);
                }
                #endregion

                ctx.CommitTransaction();

                string message = string.Format("Medication schedule was created successfully");
                result = string.Format("process|1|{0}", message);
            }
            catch (Exception ex)
            {
                result = string.Format("process|0|{0}", ex.Message);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private string UpdateMedicationSchedule(PrescriptionOrderDt item, string startFromDate, string frequencyNumber, string dosingDose, string duration, string time1, string time2, string time3, string time4, string time5, string time6, IDbContext ctx)
        {
            string result = string.Empty;
            MedicationScheduleDao scheduleDao = new MedicationScheduleDao(ctx);
            PatientVisitNoteDao visitNoteDao = new PatientVisitNoteDao(ctx);
            PrescriptionOrderDtDao orderDtDao = new PrescriptionOrderDtDao(ctx);
            PrescriptionOrderDtSignaDao signaDao = new PrescriptionOrderDtSignaDao(ctx);
            PrescriptionOrderDtLogDao orderLogDao = new PrescriptionOrderDtLogDao(ctx);
            PrescriptionOrderChangesLogDao changesLogDao = new PrescriptionOrderChangesLogDao(ctx);

            PrescriptionOrderDt oOldItem = item;
            DateTime oldEndDate = oOldItem.StartDate.AddDays(Convert.ToDouble(oOldItem.DosingDuration));
            decimal dose = oOldItem.NumberOfDosage;
            decimal newDose = Convert.ToDecimal(dosingDose);
            int frequency = Convert.ToInt16(frequencyNumber);
            int addition = frequency - oOldItem.Frequency;
            bool isAdding = addition > 0;
            bool isDoseChanged = dose != newDose;

            DateTime startDate = Helper.GetDatePickerValue(startFromDate);
            int noOfDays = Convert.ToInt32(duration);
            DateTime endDate = oOldItem.StartDate.AddDays(Convert.ToDouble(txtDefaultDuration.Text));

            bool isPRN = item.IsAsRequired;

            DateTime date = startDate;
            DateTime cfEndDate1 = startDate.Date.AddDays(Convert.ToDouble(oOldItem.DosingDuration - 1));

            while (date <= cfEndDate1)
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
                            oSchedule.MedicationTime = GetSequenceMedicationTime(j, time1, time2, time3, time4, time5, time6);
                            oSchedule.NumberOfDosage = newDose;
                            oSchedule.NumberOfDosageInString = newDose.ToString("G29");
                            oSchedule.GCDosingUnit = oOldItem.GCDosingUnit;
                            oSchedule.ConversionFactor = oOldItem.ConversionFactor;
                            oSchedule.ResultQuantity = 0;
                            oSchedule.ChargeQuantity = 0;
                            oSchedule.IsAsRequired = isPRN;
                            oSchedule.GCRoute = oOldItem.GCRoute;
                            oSchedule.GCCoenamRule = oOldItem.GCCoenamRule;
                            oSchedule.MedicationAdministration = oOldItem.MedicationAdministration;
                            oSchedule.GCMedicationStatus = Constant.MedicationStatus.OPEN;
                            oSchedule.IsInternalMedication = true;
                            oSchedule.IsUsingUDD = oOldItem.IsUsingUDD;
                            oSchedule.CreatedBy = AppSession.UserLogin.UserID;
                            scheduleDao.Insert(oSchedule);
                        }
                        else
                        {
                            if (oSchedule.GCMedicationStatus == Constant.MedicationStatus.OPEN || oSchedule.GCMedicationStatus == Constant.MedicationStatus.DISCONTINUE)
                            {
                                oSchedule.MedicationTime = GetSequenceMedicationTime(j, time1, time2, time3, time4, time5, time6);
                                oSchedule.NumberOfDosage = Convert.ToDecimal(dosingDose);
                                oSchedule.NumberOfDosageInString = dosingDose;
                                oSchedule.GCMedicationStatus = Constant.MedicationStatus.OPEN;
                                oSchedule.IsInternalMedication = true;
                                oSchedule.IsAsRequired = oOldItem.IsAsRequired;
                                oSchedule.GCCoenamRule = oOldItem.GCCoenamRule;
                                oSchedule.MedicationAdministration = oOldItem.MedicationAdministration;
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
                        int itemID = Convert.ToInt32(oOldItem.ItemID);
                        string filterExpression = string.Format("PrescriptionOrderDetailID = {0} AND ItemID = {1} AND MedicationDate >= '{2}'", oOldItem.PrescriptionOrderDetailID, itemID.ToString(), Helper.GetDatePickerValue(startFromDate).ToString(Constant.FormatString.DATE_FORMAT_112), frequencyNumber);
                        List<MedicationSchedule> lstSchedule = BusinessLayer.GetMedicationScheduleList(filterExpression, ctx);
                        foreach (MedicationSchedule oSchedule in lstSchedule)
                        {
                            if (oSchedule.GCMedicationStatus == Constant.MedicationStatus.OPEN)
                            {
                                oSchedule.MedicationTime = GetSequenceMedicationTime(Convert.ToInt16(oSchedule.SequenceNo), time1, time2, time3, time4, time5, time6);
                                oSchedule.NumberOfDosage = Convert.ToDecimal(dosingDose);
                                oSchedule.NumberOfDosageInString = dosingDose;
                                oSchedule.IsAsRequired = oOldItem.IsAsRequired;
                                oSchedule.GCCoenamRule = oOldItem.GCCoenamRule;
                                oSchedule.MedicationAdministration = oOldItem.MedicationAdministration;
                                oSchedule.LastUpdatedBy = AppSession.UserLogin.UserID;
                                scheduleDao.Update(oSchedule);
                            }
                        }
                    }
                    else
                    {
                        //change number of dosage
                        int itemID = Convert.ToInt32(oOldItem.ItemID);
                        string filterExpression = string.Empty;
                        List<MedicationSchedule> lstSchedule = new List<MedicationSchedule>();
                        //if (dose != newDose)
                        //{
                            filterExpression = string.Format("PrescriptionOrderDetailID = {0} AND ItemID = {1} AND MedicationDate >= '{2}' AND SequenceNo <= '{3}'", oOldItem.PrescriptionOrderDetailID, itemID.ToString(), Helper.GetDatePickerValue(startFromDate).ToString(Constant.FormatString.DATE_FORMAT_112), frequencyNumber);
                            lstSchedule = BusinessLayer.GetMedicationScheduleList(filterExpression, ctx);
                            foreach (MedicationSchedule oSchedule in lstSchedule)
                            {
                                if (oSchedule.GCMedicationStatus == Constant.MedicationStatus.OPEN)
                                {
                                    oSchedule.MedicationTime = GetSequenceMedicationTime(Convert.ToInt16(oSchedule.SequenceNo), time1, time2, time3, time4, time5, time6);
                                    oSchedule.NumberOfDosage = Convert.ToDecimal(dosingDose);
                                    oSchedule.NumberOfDosageInString = dosingDose;
                                    oSchedule.IsAsRequired = oOldItem.IsAsRequired;
                                    oSchedule.GCCoenamRule = oOldItem.GCCoenamRule;
                                    oSchedule.MedicationAdministration = oOldItem.MedicationAdministration;
                                    oSchedule.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    scheduleDao.Update(oSchedule);
                                }
                            }
                        //}

                        //reduce frequency
                        filterExpression = string.Format("PrescriptionOrderDetailID = {0} AND ItemID = {1} AND MedicationDate >= '{2}' AND SequenceNo > '{3}'", oOldItem.PrescriptionOrderDetailID, itemID.ToString(), Helper.GetDatePickerValue(startFromDate).ToString(Constant.FormatString.DATE_FORMAT_112), frequencyNumber);
                        lstSchedule = BusinessLayer.GetMedicationScheduleList(filterExpression, ctx);
                        foreach (MedicationSchedule oSchedule in lstSchedule)
                        {
                            if (oSchedule.GCMedicationStatus == Constant.MedicationStatus.OPEN)
                            {
                                oSchedule.MedicationTime = "00:00";
                                oSchedule.GCMedicationStatus = Constant.MedicationStatus.DISCONTINUE;
                                oSchedule.GCDiscontinueReason = Constant.DiscontinueMedicationReason.OTHER;
                                oSchedule.DiscontinueDate = Helper.GetDatePickerValue(startFromDate);
                                oSchedule.DiscontinueBy = AppSession.UserLogin.UserID;
                                oSchedule.DiscontinueReason = "Perubahan aturan pakai";
                                oSchedule.LastUpdatedBy = AppSession.UserLogin.UserID;
                                scheduleDao.Update(oSchedule);
                            }
                        }
                    }
                }
                date = date.AddDays(1);
            }

            #region Update Prescription Order Detail
            PrescriptionOrderDt orderDt = BusinessLayer.GetPrescriptionOrderDt(oOldItem.PrescriptionOrderDetailID);
            if (orderDt != null)
            {
                orderDt.StartDate = Helper.GetDatePickerValue(startFromDate);
                orderDt.StartTime = "00:00"; ;
                orderDt.Frequency = Convert.ToInt16(frequencyNumber);
                orderDt.NumberOfDosage = Convert.ToDecimal(dosingDose);
                orderDt.IsAsRequired = oOldItem.IsAsRequired;
                orderDt.GCCoenamRule = oOldItem.GCCoenamRule;
                orderDt.Sequence1Time = time1;
                orderDt.Sequence2Time = time2;
                orderDt.Sequence3Time = time3;
                orderDt.Sequence4Time = time4;
                orderDt.Sequence5Time = time5;
                orderDt.Sequence6Time = time6;
                orderDt.MedicationAdministration = orderDt.MedicationAdministration;
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
            orderDtLog.OldValues = JsonConvert.SerializeObject(oOldItem);
            orderDtLog.NewValues = JsonConvert.SerializeObject(orderDt);
            orderDtLog.UserID = AppSession.UserLogin.UserID;
            orderLogDao.Insert(orderDtLog);
            #endregion

            return result;
        }

        private string GetSequenceMedicationTime(int sequence, string time1, string time2, string time3, string time4, string time5, string time6)
        {
            string medicationTime = "";
            switch (sequence)
            {
                case 1:
                    medicationTime = time1;
                    break;
                case 2:
                    medicationTime = time2;
                    break;
                case 3:
                    medicationTime = time3;
                    break;
                case 4:
                    medicationTime = time4;
                    break;
                case 5:
                    medicationTime = time5;
                    break;
                case 6:
                    medicationTime = time6;
                    break;
                default:
                    medicationTime = "00:00";
                    break;
            }
            if (string.IsNullOrEmpty(medicationTime))
            {
                medicationTime = "00:00";
            }
            return medicationTime;
        }

        private void SetControlProperties()
        {
            //Load Physician
            string filterExpression = string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID);
            List<vParamedicTeam> lstPhysician = BusinessLayer.GetvParamedicTeamList(filterExpression);
            Methods.SetComboBoxField(cboPhysician, lstPhysician, "ParamedicName", "ParamedicID");
            cboPhysician.Value = AppSession.RegisteredPatient.ParamedicID.ToString();

            filterExpression = string.Format("ParentID = '{0}'", Constant.StandardCode.PHYSICIAN_INSTRUCTION_SOURCE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField(cboPhysicianInstructionSource, lstStandardCode, "StandardCodeName", "StandardCodeID");

            if (AppSession.UserLogin.ParamedicID != null)
            {
                chkIsGenerateCPPT.Enabled = true;
            }
            else
            {
                chkIsGenerateCPPT.Checked = false;
                chkIsGenerateCPPT.Enabled = false;
            }

            BindGridView();
        }

        protected void lvwExtendView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vMedicationChartItem item = (vMedicationChartItem)e.Item.DataItem;
                TextBox txtStartDate = e.Item.FindControl("txtStartDate") as TextBox;
                TextBox txtDuration = e.Item.FindControl("txtDuration") as TextBox;
                TextBox txtMedicationTime1 = e.Item.FindControl("txtMedicationTime1") as TextBox;
                TextBox txtMedicationTime2 = e.Item.FindControl("txtMedicationTime2") as TextBox;
                TextBox txtMedicationTime3 = e.Item.FindControl("txtMedicationTime3") as TextBox;
                TextBox txtMedicationTime4 = e.Item.FindControl("txtMedicationTime4") as TextBox;
                TextBox txtMedicationTime5 = e.Item.FindControl("txtMedicationTime5") as TextBox;
                TextBox txtMedicationTime6 = e.Item.FindControl("txtMedicationTime6") as TextBox;

                int duration = 1;
                if (!string.IsNullOrEmpty(txtDefaultDuration.Text))
                {
                    duration = Convert.ToInt16(txtDefaultDuration.Text);
                }
                txtStartDate.Text = Helper.GetDatePickerValue(txtDefaultStartDate.Text).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtDuration.Text = txtDefaultDuration.Text;
                string[] medicationTimeInfo = Methods.GetMedicationSequenceTime(item.Frequency).Split('|');
                txtMedicationTime1.Text = medicationTimeInfo[0];
                txtMedicationTime2.Text = medicationTimeInfo[1];
                txtMedicationTime3.Text = medicationTimeInfo[2];
                txtMedicationTime4.Text = medicationTimeInfo[3];
                txtMedicationTime5.Text = medicationTimeInfo[4];
                txtMedicationTime6.Text = medicationTimeInfo[5];
            }
        }

        private void BindGridView()
        {
            //string filterExpression = string.Format("VisitID = {0} AND StartDate <= '{1}' AND EndDate", AppSession.RegisteredPatient.VisitID, Helper.GetDatePickerValue(txtDefaultStartDate.Text).ToString(Constant.FormatString.DATE_FORMAT_112));
            //List<vMedicationScheduleForExtend> lstEntity = BusinessLayer.GetvMedicationScheduleForExtendList(filterExpression);

            string filterExpression = string.Format("VisitID = {0} AND (PrescriptionOrderDetailID IN (SELECT PrescriptionOrderDetailID FROM MedicationSchedule WITH (NOLOCK) WHERE MedicationDate >= '{1}' AND GCMedicationStatus IN ('{2}')) OR PastMedicationID IN (SELECT PastMedicationID FROM MedicationSchedule WITH (NOLOCK) WHERE MedicationDate >= '{1}' AND GCMedicationStatus IN ('{3}')))", AppSession.RegisteredPatient.VisitID, Helper.GetDatePickerValue(txtDefaultStartDate).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.MedicationStatus.OPEN, Constant.MedicationStatus.DIPROSES_FARMASI);
            List<vMedicationChartItem> lstEntity = BusinessLayer.GetvMedicationChartItemList(filterExpression);            
            lvwExtendView.DataSource = lstEntity;
            lvwExtendView.DataBind();
        }

        protected void cbpExtendView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }
    }
}