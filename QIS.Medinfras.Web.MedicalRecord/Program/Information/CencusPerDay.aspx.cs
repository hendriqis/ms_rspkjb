using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxPivotGrid;
using QIS.Medinfras.Web.Common;
using DevExpress.Utils;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxEditors;
using System.Web.UI.HtmlControls;


namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class CencusPerDay : BasePageList
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.CENCUS_PER_DAY;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            txtLogDate.Text = DateTime.Now.AddDays(-1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PATIENT_ATD_STATUS));
            Methods.SetComboBoxField(cboGCATDStatus,lstStandardCode,"StandardCodeName","StandardCodeID");
            cboGCATDStatus.SelectedIndex = 0;

            List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.INPATIENT, "");
            Methods.SetComboBoxField<GetServiceUnitUserList>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
            cboServiceUnit.SelectedIndex = 0;

            BindGridView(1, true, ref PageCount);
            BindGridView2();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            //int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref PageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView(1, true, ref PageCount);
                    result = "refresh|" + PageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            
            if (txtLogDate.Text != "") filterExpression = String.Format("LogDate = '{0}'", Helper.GetDatePickerValue(txtLogDate.Text));
            if (cboGCATDStatus.Value != null && cboGCATDStatus.Value.ToString() != "") filterExpression += String.Format(" AND GCATDStatus = '{0}'", cboGCATDStatus.Value);
            if (cboServiceUnit.Value != null && cboServiceUnit.Value.ToString() != "") filterExpression += String.Format(" AND HealthcareServiceUnitID = {0} ", cboServiceUnit.Value);
            
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientATDLogRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientATDLog> lstEntity = BusinessLayer.GetvPatientATDLogList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "RegistrationNo");
            grdView.DataSource = lstEntity;
            grdView.DataBind();

        }

        protected void cbpView2_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView2();
        }

        private void BindGridView2()
        {
            List<GetPatientATDLogSummary> lstEntity = BusinessLayer.GetPatientATDLogSummary(Helper.GetDatePickerValue(txtLogDate.Text), Convert.ToInt32(cboServiceUnit.Value));
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();

            if (lstEntity.Count > 0)
            {
                decimal totalPatientBegin = lstEntity.Sum(p => p.PatientBegin);
                decimal totalTransferIN = lstEntity.Sum(p => p.TransferIN);
                decimal totalAdmission = lstEntity.Sum(p => p.Admission);
                decimal totalTransferOUT = lstEntity.Sum(p => p.TransferOUT);
                decimal totalDischargeVVIP = lstEntity.Sum(p => p.DischargeVVIP);
                decimal totalDischargeVIP = lstEntity.Sum(p => p.DischargeVIP);
                decimal totalDischargeI = lstEntity.Sum(p => p.DischargeI);
                decimal totalDischargeII = lstEntity.Sum(p => p.DischargeII);
                decimal totalDischargeIII = lstEntity.Sum(p => p.DischargeIII);
                decimal totalDischargeNonKelas = lstEntity.Sum(p => p.DischargeNonKelas);
                decimal totalDied = lstEntity.Sum(p => p.Died);
                decimal totalStay = lstEntity.Sum(p => p.Stay);

                ((HtmlTableCell)lvwView.FindControl("tdTotalPatientBegin")).InnerHtml = totalPatientBegin > 0 ? totalPatientBegin.ToString() : "-";
                ((HtmlTableCell)lvwView.FindControl("tdTotalTransferIN")).InnerHtml = totalTransferIN > 0 ? totalTransferIN.ToString() : "-";
                ((HtmlTableCell)lvwView.FindControl("tdTotalAdmission")).InnerHtml = totalAdmission > 0 ? totalAdmission.ToString() : "-";
                ((HtmlTableCell)lvwView.FindControl("tdTotalTransferOUT")).InnerHtml = totalTransferOUT > 0 ? totalTransferOUT.ToString() : "-";
                ((HtmlTableCell)lvwView.FindControl("tdTotalDischargeVVIP")).InnerHtml = totalDischargeVVIP > 0 ? totalDischargeVVIP.ToString() : "-";
                ((HtmlTableCell)lvwView.FindControl("tdTotalDischargeVIP")).InnerHtml = totalDischargeVIP > 0 ? totalDischargeVIP.ToString() : "-";
                ((HtmlTableCell)lvwView.FindControl("tdTotalDischargeI")).InnerHtml = totalDischargeI > 0 ? totalDischargeI.ToString() : "-";
                ((HtmlTableCell)lvwView.FindControl("tdTotalDischargeII")).InnerHtml = totalDischargeII > 0 ? totalDischargeII.ToString() : "-";
                ((HtmlTableCell)lvwView.FindControl("tdTotalDischargeIII")).InnerHtml = totalDischargeIII > 0 ? totalDischargeIII.ToString() : "-";
                ((HtmlTableCell)lvwView.FindControl("tdTotalDischargeNonKelas")).InnerHtml = totalDischargeNonKelas > 0 ? totalDischargeNonKelas.ToString() : "-";
                ((HtmlTableCell)lvwView.FindControl("tdTotalDied")).InnerHtml = totalDied > 0 ? totalDied.ToString() : "-";
                ((HtmlTableCell)lvwView.FindControl("tdTotalStay")).InnerHtml = totalStay > 0 ? totalStay.ToString() : "-";
            }
        }
    }
}