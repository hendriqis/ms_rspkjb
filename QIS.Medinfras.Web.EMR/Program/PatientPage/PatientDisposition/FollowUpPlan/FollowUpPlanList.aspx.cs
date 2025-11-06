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
using QIS.Data.Core.Dal;
using System.Data;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class FollowUpPlanList : BasePagePatientPageListEntry
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.FOLLOW_UP_VISIT;
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
            filterExpression += string.Format("FromVisitID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}')", AppSession.RegisteredPatient.VisitID, Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvAppointmentRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vAppointment> lstEntity = BusinessLayer.GetvAppointmentList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
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
                Appointment entity = BusinessLayer.GetAppointment(Convert.ToInt32(hdnID.Value));
                entity.GCAppointmentStatus = Constant.AppointmentStatus.DELETED;
                entity.LastUpdatedDate = DateTime.Now;
                BusinessLayer.UpdateAppointment(entity);
                return true;
            }
            return false;
        }
        #endregion

        #region Entry
        protected override void SetControlProperties()
        {
            txtVisitDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtVisitTime.Text = DateTime.Now.ToString("HH:mm");

            ParamedicMaster oParamedic = BusinessLayer.GetParamedicMaster(AppSession.RegisteredPatient.ParamedicID);
            if (oParamedic != null)
            {
                hdnPhysicianID.Value = oParamedic.ParamedicID.ToString();
                hdnPhysicianCode.Value = oParamedic.ParamedicCode.ToString();
                hdnPhysicianName.Value = oParamedic.FullName;               
            }

            hdnHealthcareServiceUnitID.Value = AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString();

            List<vHealthcareServiceUnit> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND DepartmentID = 'OUTPATIENT' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID));
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboClinic, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
            //cboClinic.Value = hdnHealthcareServiceUnitID.Value;
            cboClinic.SelectedIndex = 0;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(cboClinic, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtPhysicianName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtVisitTypeCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtVisitTypeName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtVisitDate, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtVisitTime, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
        }

        private void ControlToEntity(Appointment entity)
        {
            entity.HealthcareServiceUnitID = Convert.ToInt32(cboClinic.Value);
            entity.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
            entity.VisitTypeID = Convert.ToInt32(hdnVisitTypeID.Value);
            entity.StartDate = Helper.GetDatePickerValue(txtVisitDate);
            entity.StartTime = txtVisitTime.Text;
            entity.Notes = txtRemarks.Text;
            entity.VisitDuration = BusinessLayer.GetHealthcareServiceUnit(entity.HealthcareServiceUnitID).ServiceInterval;

            int minute = Convert.ToInt32(entity.StartTime.Substring(3));
            int hour = Convert.ToInt32(entity.StartTime.Substring(0, 2));
            DateTime dtStartTime = new DateTime(2000, 1, 1, hour, minute, 0);
            entity.EndTime = dtStartTime.AddMinutes(entity.VisitDuration).ToString("HH:mm");
            entity.EndDate = entity.StartDate;
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            AppointmentDao entityDao = new AppointmentDao(ctx);
            try
            {
                Appointment entity = new Appointment();
                ControlToEntity(entity);
                entity.MRN = AppSession.RegisteredPatient.MRN;
                entity.FromVisitID = AppSession.RegisteredPatient.VisitID;
                entity.MRN = AppSession.RegisteredPatient.MRN;
                entity.TransactionCode = Constant.TransactionCode.OP_APPOINTMENT;
                entity.GCAppointmentStatus = Constant.AppointmentStatus.STARTED;
                entity.AppointmentNo = BusinessLayer.GenerateTransactionNo(entity.TransactionCode, entity.StartDate, ctx);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entityDao.Insert(entity);
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                ctx.RollBackTransaction();
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
            try
            {
                Appointment entity = BusinessLayer.GetAppointment(Convert.ToInt32(hdnEntryID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateAppointment(entity);
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