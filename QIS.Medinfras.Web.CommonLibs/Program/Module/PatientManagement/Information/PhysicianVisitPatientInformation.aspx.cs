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
using QIS.Medinfras.Web.CommonLibs.Service;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PhysicianVisitPatientInformation : BasePageTrx
    {
        protected int PageCount = 1;
        protected int CurrPageDate = 1;
        protected bool emr = false;
        List<vParamedicSchedule> lstParamedicSchedule = null;

        public override string OnGetMenuCode()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                string type = Page.Request.QueryString["id"];
                switch (type)
                {
                    case "EM":
                        emr = true;
                        return Constant.MenuCode.EMR.PATIENT_INFORMATION_PHYSICIAN_VISIT;
                    case "IP":
                        return Constant.MenuCode.Inpatient.PATIENT_INFORMATION_PHYSICIAN_VISIT;
                    default:
                        return Constant.MenuCode.Inpatient.PATIENT_INFORMATION_PHYSICIAN_VISIT;
                }
            }
            else
            {
                return Constant.MenuCode.Inpatient.PATIENT_INFORMATION_PHYSICIAN_VISIT;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            List<Department> lstKln = BusinessLayer.GetDepartmentList(string.Format("DepartmentID = '{0}' AND IsActive = 1 AND IsHasRegistration = 1", Constant.Facility.INPATIENT));
            Methods.SetComboBoxField<Department>(cboInfoParamedicScheduleServiceUnit, lstKln, "DepartmentName", "DepartmentID");
            cboInfoParamedicScheduleServiceUnit.SelectedIndex = 0;


            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        private void BindGridViewDate(int pageIndexDate, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Empty;

            List<vHealthcareServiceUnit> lstEntity = new List<vHealthcareServiceUnit>();
            filterExpression += string.Format("DepartmentID IN ('{0}')", cboInfoParamedicScheduleServiceUnit.Value.ToString());
            lstEntity = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vAppointmentRegistration WHERE DepartmentID = '{0}' AND DischargeDate IS NULL AND GCVisitStatus NOT IN ('{1}', '{2}', '{3}')) ORDER BY ServiceUnitName ASC", cboInfoParamedicScheduleServiceUnit.Value.ToString(), Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED));

            if (txtPhysicianCode.Text != "" && txtPhysicianCode.Text != null)
            {
                lstEntity = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vAppointmentRegistration WHERE ParamedicTeamPhysicianID = {0} AND DepartmentID = '{1}' AND DischargeDate IS NULL AND GCVisitStatus NOT IN ('{2}', '{3}', '{4}')) ORDER BY ServiceUnitName ASC", hdnPhysicianID.Value, cboInfoParamedicScheduleServiceUnit.Value.ToString(), Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED));
            }
            else
            {
                lstEntity = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vAppointmentRegistration WHERE ParamedicTeamPhysicianID = 0 AND DepartmentID = '{0}' AND DischargeDate IS NULL AND GCVisitStatus NOT IN ('{1}', '{2}', '{3}')) ORDER BY ServiceUnitName ASC", cboInfoParamedicScheduleServiceUnit.Value.ToString(), Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED));
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvAppointmentRegistrationRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                //vParamedicMaster obj = (vParamedicMaster)e.Item.DataItem;
                //List<vParamedicSchedule> lstSchedule = lstParamedicSchedule.Where(p => p.ParamedicID == obj.ParamedicID).ToList();
                //for (int i = 1; i < 8; ++i)
                //{
                //    HtmlTableCell tdCol = (HtmlTableCell)e.Item.FindControl("tdCol" + i);
                //    vParamedicSchedule schedule = lstSchedule.FirstOrDefault(p => p.DayNumber == i);
                //    if (schedule != null)
                //        tdCol.InnerHtml = schedule.DisplayOperationalTimeInParamedicSchedule;
                //    else
                //        tdCol.Attributes.Add("class", "tdEmptySchedule");
                //}
            }
        }

        protected void cbpInfoParamedicScheduleDateView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDate(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDate(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string filterExpression = string.Empty;
            if (txtPhysicianCode.Text != "" && txtPhysicianCode.Text != null)
            {
                filterExpression = String.Format("HealthcareServiceUnitID = '{0}' AND DischargeDate IS NULL AND GCVisitStatus NOT IN ('{1}', '{2}', '{3}') AND DepartmentID = '{4}' AND ParamedicTeamPhysicianID = {5} ORDER BY BedCode", hdnID.Value, Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, cboInfoParamedicScheduleServiceUnit.Value.ToString(), hdnPhysicianID.Value);
            }
            else
            {
                filterExpression = String.Format("HealthcareServiceUnitID = '{0}' AND DischargeDate IS NULL AND GCVisitStatus NOT IN ('{1}', '{2}', '{3}') AND DepartmentID = '{4}' AND ParamedicTeamPhysicianID = 0 ORDER BY BedCode", hdnID.Value, Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, cboInfoParamedicScheduleServiceUnit.Value.ToString());
            }

            List<vAppointmentRegistration> lstEntityDate = null;
            lstEntityDate = BusinessLayer.GetvAppointmentRegistrationList(filterExpression).GroupBy(g => g.RegistrationID).Select(s => s.FirstOrDefault()).ToList();
            lvwView.DataSource = lstEntityDate;
            lvwView.DataBind();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}