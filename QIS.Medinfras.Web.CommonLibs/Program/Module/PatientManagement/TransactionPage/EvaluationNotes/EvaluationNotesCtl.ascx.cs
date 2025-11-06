using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class EvaluationNotesCtl : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                IsAdd = false;
                hdnID.Value = param;
                SetControlProperties();
                NursingJournal entity = BusinessLayer.GetNursingJournal(Convert.ToInt32(hdnID.Value));
                EntityToControl(entity);
            }
            else
            {
                hdnID.Value = "";
                IsAdd = true;
                SetControlProperties();
            }
        }

        private void SetControlProperties()
        {
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType = '{0}' AND ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {1})",
                Constant.ParamedicType.Nurse, AppSession.RegisteredPatient.HealthcareServiceUnitID));
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");
            cboParamedicID.SelectedIndex = 0;

            if (AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Nurse)
            {
                int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                cboParamedicID.ClientEnabled = false;
                cboParamedicID.Value = userLoginParamedic.ToString();
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtJournalDate, new ControlEntrySetting(true, false, true, AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtJournalTime, new ControlEntrySetting(true, false, true, AppSession.RegisteredPatient.VisitTime));
            SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(true, false, true));
        }

        private void EntityToControl(NursingJournal entity)
        {
            txtJournalDate.Text = entity.JournalDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtJournalTime.Text = entity.JournalTime;
            cboParamedicID.Value = entity.ParamedicID.ToString();
            txtSituation.Text = entity.Situation;
            txtAssessment.Text = entity.Assessment;
            txtBackground.Text = entity.Background;
            txtEvaluationRemarks.Text = entity.Remarks;
            txtRecommendation.Text = entity.Recommendation;
        }

        private void ControlToEntity(NursingJournal entity)
        {
            entity.JournalDate = Helper.GetDatePickerValue(txtJournalDate);
            entity.JournalTime = txtJournalTime.Text;
            entity.VisitID = AppSession.RegisteredPatient.VisitID;
            entity.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
            entity.HealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
            entity.Situation = txtSituation.Text;
            entity.Assessment = txtAssessment.Text;
            entity.Background = txtBackground.Text;
            entity.Remarks = txtEvaluationRemarks.Text;
            entity.Recommendation = txtRecommendation.Text;
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                NursingJournal entity = new NursingJournal();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertNursingJournal(entity);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                NursingJournal entityUpdate = BusinessLayer.GetNursingJournal(Convert.ToInt32(hdnID.Value));
                //ControlToEntity(entity);
                entityUpdate.IsDeleted = true;
                entityUpdate.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateNursingJournal(entityUpdate);

                NursingJournal entity = new NursingJournal();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertNursingJournal(entity);

            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }
            return result;
        }
    }
}