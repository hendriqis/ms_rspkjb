using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class PharmacyJournalNotesCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                string[] localParam = param.Split('|');
                string registrationID = localParam[0];
                hdnTransactionID.Value = localParam[1];
                hdnUserID.Value = AppSession.UserLogin.UserID.ToString();
                vPatientChargesHd entityCharges = BusinessLayer.GetvPatientChargesHdList(string.Format("TransactionID = {0}", hdnTransactionID.Value)).FirstOrDefault();
                vPrescriptionOrderHd entity = BusinessLayer.GetvPrescriptionOrderHdList(string.Format("PrescriptionOrderID = {0}", entityCharges.PrescriptionOrderID))[0];

                if (entityCharges != null)
                {
                    txtMRN.ReadOnly = true;
                    txtPatientName.ReadOnly = true;
                    if (entityCharges.MedicalNo == null || entityCharges.MedicalNo == "")
                    {
                        txtMRN.Text = entityCharges.GuestNo;
                    }
                    else
                    {
                        txtMRN.Text = entityCharges.MedicalNo;
                    }
                    txtPatientName.Text = entityCharges.PatientName;
                    hdnVisitID.Value = entityCharges.VisitID.ToString();
                    txtTransactionDate.Text = entityCharges.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtTransactionNo.Text = entityCharges.TransactionNo;
                    hdnTransactionID.Value = entityCharges.TransactionID.ToString();
                    txtTransactionTime.Text = entityCharges.TransactionTime;
                    hdnPrescriptionOrderPhysicianID.Value = entity.ParamedicID.ToString();
                    txtParamedic.Text = entity.ParamedicName;
                    BindGridView();
                    SetControlProperties();
                }
            }
        }

        private void SetControlProperties()
        {
            txtLogDate.Attributes.Add("validationgroup", "mpLogNotes");
            txtLogTime.Attributes.Add("validationgroup", "mpLogNotes");
            txtNoteText.Attributes.Add("validationgroup", "mpLogNotes");
            txtLogDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtLogTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
        }

        private void BindGridView()
        {
            grdVisitNotes.DataSource = BusinessLayer.GetvPharmacyJournalList(string.Format("TransactionID = {0} AND IsDeleted = 0", hdnTransactionID.Value));
            grdVisitNotes.DataBind();
        }

        protected void cbpLogNotes_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string result = "";
            string errMessage = "";
            if (e.Parameter == "save")
            {
                result = "save|";
                if (hdnID.Value.ToString() != "")
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else
            {
                result = "delete|";
                if (OnDeleteRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(PharmacyJournal entity)
        {
            entity.JournalDate = Helper.GetDatePickerValue(txtLogDate);
            entity.JournalTime = txtLogTime.Text;
            entity.VisitID = AppSession.RegisteredPatient.VisitID;
            entity.HealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
            entity.JournalText = txtNoteText.Text;
            if (!string.IsNullOrEmpty(hdnTransactionID.Value))
            {
                entity.TransactionID = Convert.ToInt32(hdnTransactionID.Value);
            }
            if (!string.IsNullOrEmpty(hdnPhysicianInstructionID.Value))
            {
                entity.PatientInstructionID = Convert.ToInt32(hdnPhysicianInstructionID.Value);
            }
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            bool isError = false;
            IDbContext ctx = DbFactory.Configure(true);
            PharmacyJournalDao journalDao = new PharmacyJournalDao(ctx);
            try
            {
                PharmacyJournal entity = new PharmacyJournal();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                journalDao.Insert(entity);

                if (!string.IsNullOrEmpty(hdnInstructionID.Value))
                {
                    string processResult = UpdateInstructionToCompleted(hdnInstructionID.Value, ctx);
                    string[] resultInfo = processResult.Split('|');
                    isError = resultInfo[1] == "0";
                    errMessage = resultInfo[2];
                }

                if (!isError)
                {
                    ctx.CommitTransaction();
                }
                else
                {
                    ctx.RollBackTransaction();
                }
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = false;
            }
            ctx.CommitTransaction();
            return result;
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                PharmacyJournal entityUpdate = BusinessLayer.GetPharmacyJournal(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entityUpdate);
                entityUpdate.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePharmacyJournal(entityUpdate);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }
            return result;
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                PharmacyJournal entityUpdate = BusinessLayer.GetPharmacyJournal(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entityUpdate);
                entityUpdate.IsDeleted = true;
                entityUpdate.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePharmacyJournal(entityUpdate);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }
            return result;
        }

        private string UpdateInstructionToCompleted(string lstRecordID, IDbContext ctx)
        {
            string result = string.Format("process|0|{0}", string.Empty); ;
            PatientInstructionDao entityDao = new PatientInstructionDao(ctx);
            string filterExpression = string.Format("PatientInstructionID IN ({0})", lstRecordID);

            //Update Instruction to complete
            List<PatientInstruction> oList = BusinessLayer.GetPatientInstructionList(filterExpression, ctx);
            foreach (PatientInstruction instruction in oList)
            {
                if (!instruction.IsCompleted)
                {
                    instruction.IsCompleted = true;
                    instruction.ExecutedDateTime = DateTime.Now;
                    instruction.ExecutedBy = AppSession.UserLogin.ParamedicID;
                    instruction.ExecutedByUserID = AppSession.UserLogin.UserID;
                    entityDao.Update(instruction);
                    result = string.Format("process|1|{0}", string.Empty);
                }
            }

            return result;
        }
    }
}