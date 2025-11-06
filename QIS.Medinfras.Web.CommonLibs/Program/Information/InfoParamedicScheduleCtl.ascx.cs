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
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class InfoParamedicScheduleCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        public override void InitializeDataControl(string param)
        {
            int serviceUnitUserCount = 0;
            string filterExpression = "";
            List<SettingParameterDt> setvar = BusinessLayer.GetSettingParameterDtList(String.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI, Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM));
            string setvarImaging = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).FirstOrDefault().ParameterValue;
            string setvarLaboratory = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).FirstOrDefault().ParameterValue;

            if (param == "OP")
            {
                serviceUnitUserCount = BusinessLayer.GetvServiceUnitUserRowCount(string.Format("DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2}", Constant.Facility.OUTPATIENT, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID));
            }
            else if (param == "IS")
            {
                serviceUnitUserCount = BusinessLayer.GetvServiceUnitUserRowCount(string.Format("HealthcareServiceUnitID = '{0}' AND HealthcareID = '{1}' AND UserID = {2}", setvarImaging, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID));            
            }
            else if (param == "LB")
            {
                serviceUnitUserCount = BusinessLayer.GetvServiceUnitUserRowCount(string.Format("HealthcareServiceUnitID = '{0}' AND HealthcareID = '{1}' AND UserID = {2}", setvarLaboratory, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID));
            }
            else if (param == "MD")
            {
                serviceUnitUserCount = BusinessLayer.GetvServiceUnitUserRowCount(string.Format("DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2} AND HealthcareServiceUnitID NOT IN ('{3}','{4}')", Constant.Facility.DIAGNOSTIC, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, setvarImaging, setvarLaboratory));
            }

            if (param == "OP")
            {
                filterExpression = string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}'", AppSession.UserLogin.HealthcareID, Constant.Facility.OUTPATIENT);
                if (serviceUnitUserCount > 0)
                    filterExpression += string.Format(" AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vServiceUnitUser WHERE DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2})", Constant.Facility.OUTPATIENT, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID);
            }
            else if (param == "IS")
            {
                filterExpression = string.Format("HealthcareID = '{0}' AND HealthcareServiceUnitID = '{1}'", AppSession.UserLogin.HealthcareID, setvarImaging);
                if (serviceUnitUserCount > 0)
                    filterExpression += string.Format(" AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vServiceUnitUser WHERE HealthcareServiceUnitID = '{0}' AND HealthcareID = '{1}' AND UserID = {2})", setvarImaging, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID);            
            }
            else if (param == "LB")
            {
                filterExpression = string.Format("HealthcareID = '{0}' AND HealthcareServiceUnitID = '{1}'", AppSession.UserLogin.HealthcareID, setvarLaboratory);
                if (serviceUnitUserCount > 0)
                    filterExpression += string.Format(" AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vServiceUnitUser WHERE HealthcareServiceUnitID = '{0}' AND HealthcareID = '{1}' AND UserID = {2})", setvarLaboratory, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID);            
            }
            else if (param == "MD")
            {
                filterExpression = string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND HealthcareServiceUnitID NOT IN ('{2}','{3}')", AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC, setvarImaging, setvarLaboratory);
                if (serviceUnitUserCount > 0)
                    filterExpression += string.Format(" AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vServiceUnitUser WHERE DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2} AND HealthcareServiceUnitID NOT IN ('{3}','{4}'))", Constant.Facility.DIAGNOSTIC, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, setvarImaging, setvarLaboratory);            
            }

            List<vHealthcareServiceUnit> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboInfoParamedicScheduleServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
            cboInfoParamedicScheduleServiceUnit.SelectedIndex = 0;

            BindGridView(1, true, ref PageCount);
            BindGridTodaySchedule();
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
            if (lstEntity.Count > 0)
                filterExpression = string.Format("ParamedicID IN ({0}) AND HealthcareServiceUnitID = {1}", string.Join(",", lstEntity.Select(p => p.ParamedicID)), cboInfoParamedicScheduleServiceUnit.Value);
            else
                filterExpression = "1 = 0";
            lstParamedicSchedule = BusinessLayer.GetvParamedicScheduleList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
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

        protected void cbpViewTodaySchedule_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridTodaySchedule();
        }

        private void BindGridTodaySchedule()
        {
            List<vParamedicScheduleInformationToday> lstTodaySchedule = BusinessLayer.GetvParamedicScheduleInformationTodayList(string.Format(string.Format("HealthcareServiceUnitID = {0} ORDER BY ParamedicID", cboInfoParamedicScheduleServiceUnit.Value)));
            grdView.DataSource = lstTodaySchedule;
            grdView.DataBind();
        }
    }
}