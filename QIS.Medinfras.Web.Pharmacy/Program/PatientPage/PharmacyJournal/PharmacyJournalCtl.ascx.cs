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

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class PharmacyJournalCtl : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnUserID.Value = AppSession.UserLogin.UserID.ToString();
            string[] paramInfo = param.Split('|');

            if (paramInfo[0] != "0")
            {
                IsAdd = false;
                hdnID.Value = paramInfo[0];
                OnControlEntrySettingLocal();
                ReInitControl();
                vPharmacyJournal entity = BusinessLayer.GetvPharmacyJournalList(string.Format("ID = {0}",Convert.ToInt32(hdnID.Value))).FirstOrDefault();
                EntityToControl(entity);
            }
            else
            {
                hdnInstructionID.Value = paramInfo[1];
                hdnInstructionText.Value = paramInfo[2];
                OnControlEntrySettingLocal();
                ReInitControl();
                hdnID.Value = "";
                IsAdd = true;
                txtNoteText.Text = hdnInstructionText.Value;
            }
        }

        private void SetControlProperties()
        {
            //int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            //List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format(
            //                                        "GCParamedicMasterType IN ('{0}','{1}','{2}') AND ParamedicID = {3}",
            //                                        Constant.ParamedicType.Nurse, Constant.ParamedicType.Bidan, Constant.ParamedicType.Nutritionist, paramedicID));
            //Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");
            //cboParamedicID.SelectedIndex = 0;
        }

        private void OnControlEntrySettingLocal()
        {
            SetControlEntrySetting(txtNoteDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtNoteTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlProperties();

            //if (AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Nurse
            //        || AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Bidan
            //        || AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Nutritionist)
            //{
            //    int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
            //    SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(false, false, true, userLoginParamedic.ToString()));
            //}
            //else
            //{
            //    SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(true, true, true));
            //}
        }

        private void EntityToControl(vPharmacyJournal entity)
        {
            txtNoteDate.Text = entity.JournalDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtNoteTime.Text = entity.JournalTime;
            txtNoteText.Text = entity.JournalText;
            hdnTransactionID.Value = entity.TransactionID.ToString();
            txtTransactionNo.Text = entity.ChargeTransactionNo;
            hdnPhysicianInstructionID.Value = entity.PatientInstructionID.ToString();
        }

        private void ControlToEntity(PharmacyJournal entity)
        {
            entity.JournalDate = Helper.GetDatePickerValue(txtNoteDate);
            entity.JournalTime = txtNoteTime.Text;
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

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PharmacyJournalDao journalDao = new PharmacyJournalDao(ctx);
            bool isError = false;
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
            finally
            {
                ctx.Close();
            }
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
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