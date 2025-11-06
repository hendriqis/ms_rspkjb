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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class DiscontinueMedicationCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnDispensaryServiceUnitID.Value = paramInfo[0];
            txtDefaultDiscontinueDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            SetControlProperties();
        }

        protected void cbpPopupProcessDiscontinue_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";

            string lstRecordID = hdnSelectedID.Value;
            string lstRecordPmID = hdnSelectedPmID.Value;
            string lstDate = hdnSelectedDate.Value;
            string lstReasonType = hdnSelectedReasonType.Value;
            string lstReasonRemark = hdnSelectedReasonRemark.Value;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;

            if (IsValidated(lstDate, ref result))
                result = DiscontinueMedication(lstRecordID, lstRecordPmID, lstDate, lstReasonType, lstReasonRemark);

            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = result;
        }

        private bool IsValidated(string lstDate, ref string result)
        {
            string message = string.Empty;

            if (string.IsNullOrEmpty(cboPhysician.Value.ToString()))
                message = "Physician field should not leave blanks";

            if (string.IsNullOrEmpty(message))
            {
                #region Validate Discontinue Date
                string[] selectedDate = lstDate.Split(',');
                foreach (string date in selectedDate)
                {
                    if (string.IsNullOrEmpty(date))
                    {
                        message = "There is medication with empty discontinue date.";
                        break;
                    }
                }
                #endregion
            }

            if (!string.IsNullOrEmpty(message))
            {
                result = string.Format("process|0|{0}", message);
            }
            return message == string.Empty;
        }

        private string DiscontinueMedication(string lstRecordID, string lstRecordPmID, string lstDate, string lstReasonType, string lstReasonRemark)
        {
            string result = string.Empty;
            String[] selectedID = lstRecordID.Split(',');
            String[] selectedPmID = lstRecordPmID.Split(',');
            String[] selectedDate = lstDate.Split(',');
            String[] selectedReasonType = lstReasonType.Split(',');
            String[] selectedReasonRemark = lstReasonRemark.Split(',');

            IDbContext ctx = DbFactory.Configure(true);
            PatientVisitNoteDao visitNoteDao = new PatientVisitNoteDao(ctx);
            MedicationScheduleDao scheduleDao = new MedicationScheduleDao(ctx);
            PrescriptionOrderDtDao orderDtDao = new PrescriptionOrderDtDao(ctx);
            PastMedicationDao pastMedicationDao = new PastMedicationDao(ctx);
            PrescriptionOrderChangesLogDao changesLogDao = new PrescriptionOrderChangesLogDao(ctx);
            PharmacyJournalDao journalDao = new PharmacyJournalDao(ctx);

            try
            {
                bool isAllowDiscontinue = true;
               
                StringBuilder pharmacistNoteText = new StringBuilder();
                string drugName = string.Empty;

                if (!string.IsNullOrEmpty(txtPharmacyNoteText.Text))
                    pharmacistNoteText.AppendLine(txtPharmacyNoteText.Text);
                else
                    pharmacistNoteText.AppendLine("Discontinue Medication :");

                for (int ct = 0; ct < selectedID.Length; ct++)
                {
                    bool isFromOrder = true;
                    string recordID = string.Empty;

                    if (selectedID[ct] != "" && selectedID[ct] != "0")
                    {
                        isFromOrder = true;
                    }
                    else
                    {
                        isFromOrder = false;
                    }

                    DateTime date = Helper.GetDatePickerValue(selectedDate[ct]);

                    string filterExp = string.Format("PrescriptionOrderDetailID = {0} AND MedicationDate >= '{1}' AND GCMedicationStatus = '{2}'",
                        selectedID[ct], date.ToString(Constant.FormatString.DATE_FORMAT_112), Constant.MedicationStatus.OPEN);

                    if (!isFromOrder)
                        filterExp = string.Format("PastMedicationID = {0} AND MedicationDate >= '{1}' AND GCMedicationStatus = '{2}'",
                           selectedPmID[ct], date.ToString(Constant.FormatString.DATE_FORMAT_112), Constant.MedicationStatus.DIPROSES_FARMASI);

                    List<MedicationSchedule> lstSchedule = BusinessLayer.GetMedicationScheduleList(filterExp, ctx);

                    if (lstSchedule.Count > 0)
                    {
                        foreach (MedicationSchedule oSchedule in lstSchedule)
                        {
                            if (oSchedule.TransactionID != null && oSchedule.TransactionID != 0)
                            {
                                isAllowDiscontinue = false;
                                break;
                            }
                            else
                            {
                                oSchedule.GCMedicationStatus = Constant.MedicationStatus.DISCONTINUE;
                                oSchedule.DiscontinueDate = date;
                                oSchedule.GCDiscontinueReason = selectedReasonType[ct];
                                oSchedule.DiscontinueReason = selectedReasonRemark[ct];
                                oSchedule.DiscontinueParamedicID = Convert.ToInt16(cboPhysician.Value);
                                oSchedule.DiscontinueBy = AppSession.UserLogin.UserID;
                                scheduleDao.Update(oSchedule);
                            }
                        }

                        if (isAllowDiscontinue)
                        {
                            #region PrescriptionOrderDt or Past Medication
                            if (isFromOrder)
                            {
                                PrescriptionOrderDt orderDt = BusinessLayer.GetPrescriptionOrderDtList(string.Format("PrescriptionOrderDetailID = {0}", selectedID[ct]), ctx).FirstOrDefault();
                                drugName = orderDt.DrugName;
                                filterExp = string.Format("PrescriptionOrderDetailID = {0} AND GCMedicationStatus IN ('{1}','{2}')",
                                selectedID[ct], Constant.MedicationStatus.OPEN, Constant.MedicationStatus.DIPROSES_FARMASI);

                                #region Pharmacist Notes Detail
                                string noteDetail = string.Format("- {0}, Discontinue Date : {1}", drugName, date.ToString(Constant.FormatString.DATE_PICKER_FORMAT));
                                pharmacistNoteText.AppendLine(noteDetail);
                                #endregion

                                List<vMedicationSchedule> lstOpenSchedule = BusinessLayer.GetvMedicationScheduleList(filterExp, ctx);
                                if (lstOpenSchedule.Count == 0)
                                {
                                    orderDt.GCPrescriptionOrderStatus = Constant.OrderStatus.CLOSED;
                                    orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    orderDtDao.Update(orderDt);
                                }

                                #region Log Prescription Order Changes
                                StringBuilder logText = new StringBuilder();

                                if (!string.IsNullOrEmpty(txtPharmacyNoteText.Text))
                                    logText.AppendLine(txtPharmacyNoteText.Text);

                                logText.AppendLine("Discontinue Medication :");
                                logText.AppendLine(noteDetail);

                                PrescriptionOrderChangesLog changesLog = new PrescriptionOrderChangesLog();
                                changesLog.VisitID = Convert.ToInt32(hdnVisitID.Value);
                                changesLog.LogDate = DateTime.Now.Date;
                                changesLog.LogTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                                changesLog.GCPrescriptionOrderChangeType = Constant.PrescriptionOrderChangesType.DURATION;
                                changesLog.PrescriptionOrderID = orderDt.PrescriptionOrderID;
                                changesLog.HealthcareServiceUnitID = Convert.ToInt32(hdnDispensaryServiceUnitID.Value);
                                changesLog.NoteText = logText.ToString();
                                changesLog.IsAutoGeneratedBySystem = true;
                                changesLog.CreatedBy = AppSession.UserLogin.UserID;
                                changesLogDao.Insert(changesLog);
                                #endregion
                            }
                            else
                            {
                                PastMedication pastMedication = BusinessLayer.GetPastMedicationList(string.Format("ID = {0}", selectedPmID[ct]), ctx).FirstOrDefault();
                                drugName = pastMedication.DrugName;
                                filterExp = string.Format("PastMedicationID = {0} AND GCMedicationStatus IN ('{1}','{2}')",
                                selectedPmID[ct], Constant.MedicationStatus.OPEN, Constant.MedicationStatus.DIPROSES_FARMASI);

                                List<vMedicationSchedule> lstOpenSchedule = BusinessLayer.GetvMedicationScheduleList(filterExp, ctx);
                                if (lstOpenSchedule.Count == 0)
                                {
                                    pastMedication.DiscontinueDate = date;
                                    pastMedication.GCDiscontinueReason = selectedReasonType[ct];
                                    pastMedication.DiscontinueReason = selectedReasonRemark[ct];
                                    pastMedication.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    pastMedicationDao.Update(pastMedication);
                                }
                            }
                            #endregion
                        }
                    }
                }

                if (isAllowDiscontinue)
                {
                    int linkedNoteID = 0;
                    if (hdnPlanningNoteID.Value != "" && hdnPlanningNoteID.Value != "0")
                        linkedNoteID = Convert.ToInt32(hdnPlanningNoteID.Value);

                    #region Pharmacist Notes
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
                        note.HealthcareServiceUnitID = Convert.ToInt32(hdnDispensaryServiceUnitID.Value);
                        note.ParamedicID = AppSession.UserLogin.ParamedicID;
                        note.NoteText = pharmacistNoteText.ToString();
                        if (linkedNoteID != 0)
                            note.LinkedNoteID = linkedNoteID;
                        if (chkIsNeedConfirmation.Checked)
                        {
                            note.IsNeedConfirmation = chkIsNeedConfirmation.Checked;
                            if (cboPhysicianInstructionSource.Value != null)
                                note.GCPhysicianInstructionSource = cboPhysicianInstructionSource.Value.ToString();
                            if (hdnParamedicID.Value != "" && hdnParamedicID.Value != "0")
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

                    string message = string.Format("Discontinue Medication was processed successfully");
                    result = string.Format("process|1|{0}", message);
                }
                else
                {
                    result = string.Format("process|0|{0}", "Discontinue Medication failed because has a dispense");
                    ctx.RollBackTransaction();
                }
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

        private void SetControlProperties()
        {
            //Load Physician
            string filterExpression = string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID);
            List<vParamedicTeam> lstPhysician = BusinessLayer.GetvParamedicTeamList(filterExpression);
            Methods.SetComboBoxField(cboPhysician, lstPhysician, "ParamedicName", "ParamedicID");
            cboPhysician.Value = AppSession.RegisteredPatient.ParamedicID.ToString();

            filterExpression = string.Format("ParentID IN ('{0}','{1}')", Constant.StandardCode.DISCONTINUE_MEDICATION_REASON, Constant.StandardCode.PHYSICIAN_INSTRUCTION_SOURCE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            List<StandardCode> list1 = lstStandardCode.Where(lst => lst.ParentID == Constant.StandardCode.DISCONTINUE_MEDICATION_REASON).ToList();
            List<StandardCode> list2 = lstStandardCode.Where(lst => lst.ParentID == Constant.StandardCode.PHYSICIAN_INSTRUCTION_SOURCE).ToList();

            Methods.SetComboBoxField(cboDefaultGCDiscontinueReason, list1, "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboPhysicianInstructionSource, list2, "StandardCodeName", "StandardCodeID");

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

        protected void lvwDiscontinueView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vMedicationChartItem item = (vMedicationChartItem)e.Item.DataItem;
                List<StandardCode> lstStatusNotes = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' ORDER BY StandardCodeID", Constant.StandardCode.DISCONTINUE_MEDICATION_REASON));
                DropDownList cboDiscontinueReason = e.Item.FindControl("cboDiscontinueReason") as DropDownList;
                cboDiscontinueReason.DataValueField = "StandardCodeID";
                cboDiscontinueReason.DataTextField = "StandardCodeName";
                cboDiscontinueReason.DataSource = lstStatusNotes;
                cboDiscontinueReason.DataBind();
            }
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("VisitID = {0} AND (PrescriptionOrderDetailID IN (SELECT PrescriptionOrderDetailID FROM MedicationSchedule WITH (NOLOCK) WHERE VisitID = {0} AND MedicationDate >= '{1}' AND GCMedicationStatus IN ('{2}')) OR PastMedicationID IN (SELECT PastMedicationID FROM MedicationSchedule WITH (NOLOCK) WHERE VisitID = {0} AND MedicationDate >= '{1}' AND GCMedicationStatus IN ('{3}')))", AppSession.RegisteredPatient.VisitID, DateTime.Now.Date.ToString(Constant.FormatString.DATE_FORMAT_112), Constant.MedicationStatus.OPEN, Constant.MedicationStatus.DIPROSES_FARMASI);
            List<vMedicationChartItem> lstEntity = BusinessLayer.GetvMedicationChartItemList(filterExpression);
            lvwDiscontinueView.DataSource = lstEntity;
            lvwDiscontinueView.DataBind();
        }

        protected void cbpDiscontinueView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }
    }
}