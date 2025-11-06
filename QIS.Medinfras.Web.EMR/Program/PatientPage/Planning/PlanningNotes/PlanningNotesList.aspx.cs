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
    public partial class PlanningNotesList : BasePagePatientPageListEntry
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.PLANNING_NOTES;
        }

        #region List
        protected override void InitializeDataControl()
        {
            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID = {0}  AND ParamedicID = {1} AND GCPatientNoteType = '{2}' AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, AppSession.UserLogin.ParamedicID,  Constant.PatientVisitNotes.PLANNING_NOTES);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientVisitNoteRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientVisitNote> lstEntity = BusinessLayer.GetvPatientVisitNoteList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "NoteDate,NoteTime DESC");
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
            txtNoteDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtNoteTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType = '{0}'", Constant.ParamedicType.Physician));
            Methods.SetComboBoxField<vParamedicMaster>(cboPhysician, lstParamedic, "ParamedicName", "ParamedicID");
            hdnDefaultParamedicID.Value = AppSession.RegisteredPatient.ParamedicID.ToString();

            if (AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Physician)
            {
                int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                cboPhysician.ClientEnabled = false;
                hdnDefaultParamedicID.Value = userLoginParamedic.ToString();
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtNoteDate, new ControlEntrySetting(true, true, true, DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtNoteTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(cboPhysician, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtNoteText, new ControlEntrySetting(true, true, false));
        }

        private void ControlToEntity(PatientVisitNote entity)
        {
            entity.GCPatientNoteType = Constant.PatientVisitNotes.PLANNING_NOTES;
            entity.NoteDate = Helper.GetDatePickerValue(txtNoteDate);
            entity.NoteTime = txtNoteTime.Text;
            entity.ParamedicID = Convert.ToInt32(cboPhysician.Value);
            entity.NoteText = txtNoteText.Text;
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                PatientVisitNote entity = new PatientVisitNote();
                ControlToEntity(entity);
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertPatientVisitNote(entity);
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
                PatientVisitNote entity = BusinessLayer.GetPatientVisitNote(Convert.ToInt32(hdnEntryID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePatientVisitNote(entity);
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