using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using System.Globalization;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class PatientInstructionList : BasePagePatientPageListEntry
    {
        protected int PageCount = 1;
        protected int linkedVisitID = 0;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.PATIENT_INSTRUCTION;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        #region List
        protected override void InitializeDataControl()
        {
            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT)
            {
                ConsultVisit obj = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", AppSession.RegisteredPatient.LinkedRegistrationID)).FirstOrDefault();
                if (obj != null)
                {
                    linkedVisitID = obj.VisitID;
                }
            }
            else
            {
                linkedVisitID = 0;
            }

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID IN ({0},{1}) AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, linkedVisitID);
            string code = ddlInstructionTypeFilter.SelectedValue;
            if (code != "0")
                filterExpression += string.Format(" AND GCInstructionGroup = '{0}^{1}'", Constant.StandardCode.PATIENT_INSTRUCTION_GROUP, code);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientInstructionRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientInstruction> lstEntity = BusinessLayer.GetvPatientInstructionList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "InstructionDate DESC, InstructionTime DESC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value != "")
            {
                PatientInstruction entity = BusinessLayer.GetPatientInstruction(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedDate = DateTime.Now;
                BusinessLayer.UpdatePatientInstruction(entity);
                return true;
            }
            return false;
        }
        #endregion
        #region Entry
        protected override void SetControlProperties()
        {
            txtDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType = '{0}'", Constant.ParamedicType.Physician));
            Methods.SetComboBoxField<vParamedicMaster>(cboPhysician, lstParamedic, "ParamedicName", "ParamedicID");
            hdnDefaultParamedicID.Value = AppSession.RegisteredPatient.ParamedicID.ToString();

            if (AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Physician)
            {
                int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                cboPhysician.ClientEnabled = false;
                hdnDefaultParamedicID.Value = userLoginParamedic.ToString();
            }

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.PATIENT_INSTRUCTION_GROUP));
            Methods.SetComboBoxField(cboInstructionType, lstSc, "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtDate, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtTime, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboPhysician, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboInstructionType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtInstruction, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtAdditionalText, new ControlEntrySetting(true, true, false));
        }

        private void ControlToEntity(PatientInstruction entity)
        {
            entity.InstructionDate = Helper.GetDatePickerValue(txtDate);
            entity.InstructionTime = txtTime.Text;

            entity.PhysicianID = Convert.ToInt32(cboPhysician.Value); 
            entity.GCInstructionGroup = cboInstructionType.Value.ToString();
            entity.Description = txtInstruction.Text;
            entity.AdditionalText = txtAdditionalText.Text;
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                PatientInstruction entity = new PatientInstruction();
                ControlToEntity(entity);
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertPatientInstruction(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                PatientInstruction entity = BusinessLayer.GetPatientInstruction(Convert.ToInt32(hdnEntryID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePatientInstruction(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
        #endregion
    }
}