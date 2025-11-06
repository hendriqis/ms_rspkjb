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
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Outpatient.Program
{
    public partial class InfoParamedicSchedule : BasePageContent
    {
        protected int PageCount = 1;
        protected int CurrPageDate = 1;
        protected int PageCountDate = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Outpatient.PARAMEDIC_SCHEDULE;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                int serviceUnitUserCount = BusinessLayer.GetvServiceUnitUserRowCount(string.Format("DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2}", Constant.Facility.OUTPATIENT, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID));
                string filterExpression = string.Format("IsDeleted = 0 AND IsUsingRegistration = 1 AND HealthcareID = '{0}' AND DepartmentID = '{1}'", AppSession.UserLogin.HealthcareID, Constant.Facility.OUTPATIENT);
                if (serviceUnitUserCount > 0)
                    filterExpression += string.Format(" AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vServiceUnitUser WHERE DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2})", Constant.Facility.OUTPATIENT, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID);

                List<vHealthcareServiceUnit> lstKln = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboInfoParamedicScheduleServiceUnit, lstKln, "ServiceUnitName", "HealthcareServiceUnitID");
                cboInfoParamedicScheduleServiceUnit.SelectedIndex = 0;

                txtPSSchduleDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                BindGridView(1, true, ref PageCount);
                BindGridViewDate(1, true, ref PageCountDate);
            }
        }

        List<vParamedicSchedule> lstParamedicSchedule = null;
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {0}) AND ParamedicID IN (SELECT ParamedicID FROM vParamedicSchedule WHERE HealthcareServiceUnitID = {0}) AND IsDeleted = 0", cboInfoParamedicScheduleServiceUnit.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvParamedicMasterRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vParamedicMaster> lstEntity = BusinessLayer.GetvParamedicMasterList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
            string filter = string.Join(",", lstEntity.Select(p => p.ParamedicID));
            if(filter != "")
                lstParamedicSchedule = BusinessLayer.GetvParamedicScheduleList(string.Format("ParamedicID IN ({0}) AND HealthcareServiceUnitID = {1}", filter, cboInfoParamedicScheduleServiceUnit.Value));

            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        private void BindGridViewDate(int pageIndexDate, bool isCountPageCount, ref int pageCountDate)
        {
            DateTime ScheduleDate = Helper.GetDatePickerValue(txtPSSchduleDate.Text);
            int daynumber = (int)ScheduleDate.DayOfWeek;

            if (daynumber == 0)
            {
                daynumber = 7;
            }

            string filterExpression = String.Format("IsUsingRegistration = 1 AND IsDeleted = 0 AND DepartmentID = '{0}' AND ServiceUnitID IN (SELECT ServiceUnitID FROM vHealthcareServiceUnit WHERE HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM ParamedicSchedule WHERE DayNumber = '{1}'))", Constant.Facility.OUTPATIENT, daynumber);
            string filterExpressionDate = String.Format("IsUsingRegistration = 1 AND IsDeleted = 0 AND DepartmentID = '{0}' AND ServiceUnitID IN (SELECT ServiceUnitID FROM vHealthcareServiceUnit WHERE HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM ParamedicScheduleDate WHERE ScheduleDate = '{1}'))", Constant.Facility.OUTPATIENT, ScheduleDate);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetServiceUnitMasterRowCount(filterExpression) + BusinessLayer.GetServiceUnitMasterRowCount(filterExpressionDate);
                pageCountDate = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            filterExpression += String.Format(" ORDER BY ServiceUnitName ASC");
            filterExpressionDate += String.Format( "ORDER BY ServiceUnitName ASC");

            List<ServiceUnitMaster> lstEntity = BusinessLayer.GetServiceUnitMasterList(filterExpression);
            List<ServiceUnitMaster> lstEntityDate = BusinessLayer.GetServiceUnitMasterList(filterExpressionDate);
            lstEntity.AddRange(lstEntityDate);

            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vParamedicMaster obj = (vParamedicMaster)e.Item.DataItem;
                List<vParamedicSchedule> lstSchedule = lstParamedicSchedule.Where(p => p.ParamedicID == obj.ParamedicID).ToList();
                for (int i = 1; i < 8; ++i)
                {
                    HtmlTableCell tdCol = (HtmlTableCell)e.Item.FindControl("tdCol" + i);
                    vParamedicSchedule schedule = lstSchedule.FirstOrDefault(p => p.DayNumber == i);
                    if (schedule != null)
                        tdCol.InnerHtml = schedule.DisplayOperationalTimeInParamedicSchedule;
                    else
                        tdCol.Attributes.Add("class", "tdEmptySchedule");
                }
            }
        }

        protected void cbpInfoParamedicScheduleView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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

        protected void cbpInfoParamedicScheduleDateView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCountDate = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDate(Convert.ToInt32(param[1]), false, ref pageCountDate);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDate(1, true, ref pageCountDate);
                    result = "refresh|" + pageCountDate;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            DateTime ScheduleDate = Helper.GetDatePickerValue(txtPSSchduleDate.Text);
            int daynumber = (int)ScheduleDate.DayOfWeek;
            int healthcareServiceUnitID = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("ServiceUnitID = {0}", Convert.ToInt32(hdnID.Value))).FirstOrDefault().HealthcareServiceUnitID;
            if (daynumber == 0)
            {
                daynumber = 7;
            }

            String FilterScheduleDate = Helper.GetDatePickerValue(txtPSSchduleDate).ToString(Constant.FormatString.DATE_PICKER_FORMAT2);
            string filterExpressionDate = string.Format("TempDate IN ('{0}','{1}') AND HealthcareServiceUnitID = {2}", daynumber, FilterScheduleDate, hdnID.Value);

            List<GetParamedicScheduleDateInfo> lstEntityDate = BusinessLayer.GetParamedicScheduleDateInfoList(daynumber, Helper.GetDatePickerValue(txtPSSchduleDate).ToString(Constant.FormatString.DATE_FORMAT_112), healthcareServiceUnitID);
            grdDetail.DataSource = lstEntityDate;
            grdDetail.DataBind();
        }
    }
}