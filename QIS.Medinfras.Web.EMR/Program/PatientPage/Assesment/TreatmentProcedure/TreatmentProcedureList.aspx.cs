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
    public partial class TreatmentProcedureList : BasePagePatientPageListEntry
    {
        protected int PageCount = 1;
        protected int ParamedicID = 0;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.TREATMENT_PROCEDURE;
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
            filterExpression += string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientProcedureRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientProcedure> lstEntity = BusinessLayer.GetvPatientProcedureList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
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
                PatientProcedure entity = BusinessLayer.GetPatientProcedure(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedDate = DateTime.Now;
                BusinessLayer.UpdatePatientProcedure(entity);
                return true;
            }
            return false;
        }
        #endregion

        #region Entry
        protected override void SetControlProperties()
        {
            ParamedicID = AppSession.RegisteredPatient.ParamedicID;

            ledProcedure.FilterExpression = string.Format("SpecialtyID = '{0}'", AppSession.RegisteredPatient.SpecialtyID);

            txtObservationDate.Text = AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtObservationTime.Text = AppSession.RegisteredPatient.VisitTime;

            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType = '{0}' AND ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {1})", Constant.ParamedicType.Physician, AppSession.RegisteredPatient.HealthcareServiceUnitID));
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");
            cboParamedicID.Value = ParamedicID.ToString();

            if (AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Physician)
            {
                int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                cboParamedicID.ClientEnabled = false;
                cboParamedicID.Value = userLoginParamedic.ToString();
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtObservationDate, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtObservationTime, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(true, true, true));
            //SetControlEntrySetting(ddlYear, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
        }

        private void ControlToEntity(PatientProcedure entity)
        {
            entity.ProcedureDate = Helper.GetDatePickerValue(txtObservationDate);
            entity.ProcedureTime = txtObservationTime.Text;

            entity.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
            entity.ProcedureID = hdnProcedureID.Value;
            entity.Remarks = txtRemarks.Text;
            entity.IsCreatedBySystem = false;
            entity.ReferenceID = null;
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                PatientProcedure entity = new PatientProcedure();
                ControlToEntity(entity);
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertPatientProcedure(entity);
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
                PatientProcedure entity = BusinessLayer.GetPatientProcedure(Convert.ToInt32(hdnEntryID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePatientProcedure(entity);
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