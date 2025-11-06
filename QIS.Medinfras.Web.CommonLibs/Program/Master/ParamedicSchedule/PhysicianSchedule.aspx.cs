using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PhysicianSchedule : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        private int jknColumn;

        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case "OP": return Constant.MenuCode.Outpatient.PARAMEDIC_SCHEDULE;
                case "IS": return Constant.MenuCode.Imaging.PARAMEDIC_SCHEDULE;
                case "LB": return Constant.MenuCode.Laboratory.PARAMEDIC_SCHEDULE;
                case "MD": return Constant.MenuCode.MedicalDiagnostic.PARAMEDIC_SCHEDULE;
                case "MC": return Constant.MenuCode.MedicalCheckup.MCU_PARAMEDIC_SCHEDULE;
                case "RT": return Constant.MenuCode.Radiotheraphy.PARAMEDIC_SCHEDULE;
                default: return Constant.MenuCode.Outpatient.PARAMEDIC_SCHEDULE;
            }
        }

        protected string GetPhysicianFilter()
        {
            string url = Page.Request.QueryString["id"];
            string param = "IsDeleted = 0";
            string LabServiceUnitID = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).ParameterValue;
            string RadServiceUnitID = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID).ParameterValue;

            if (url == "OP")
            {
                param += string.Format(" AND ParamedicID IN (SELECT ParamedicID FROM vServiceUnitParamedic WHERE DepartmentID = '{0}')", Constant.Facility.OUTPATIENT);
            }
            else if (url == "IS")
            {
                param += string.Format(" AND ParamedicID IN (SELECT ParamedicID FROM vServiceUnitParamedic WHERE DepartmentID = '{0}' AND ServiceUnitID = {1})",
                                            Constant.Facility.DIAGNOSTIC, RadServiceUnitID);
            }
            else if (url == "LB")
            {
                param += string.Format(" AND ParamedicID IN (SELECT ParamedicID FROM vServiceUnitParamedic WHERE DepartmentID = '{0}' AND IsLaboratoryUnit = 1)", Constant.Facility.DIAGNOSTIC);
            }
            else if (url == "MD")
            {
                param += string.Format(" AND ParamedicID IN (SELECT ParamedicID FROM vServiceUnitParamedic WHERE DepartmentID = '{0}' AND ServiceUnitID NOT IN ({1},{2}) AND IsLaboratoryUnit = 0)",
                                            Constant.Facility.DIAGNOSTIC, RadServiceUnitID, LabServiceUnitID);
            }
            else if (url == "RT")
            {
                param += string.Format(" AND ParamedicID IN (SELECT ParamedicID FROM vServiceUnitParamedic WHERE DepartmentID = '{0}' AND HealthcareServiceUnitID = {1})",
                                            Constant.Facility.DIAGNOSTIC, AppSession.RT0001);
            }
            else if (url == "MC")
            {
                param += string.Format(" AND ParamedicID IN (SELECT ParamedicID FROM vServiceUnitParamedic WHERE DepartmentID = '{0}')", Constant.Facility.MEDICAL_CHECKUP);
            }

            return param;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            if (Page.Request.QueryString["id"] != null)
            {
                hdnDepartmentID.Value = Page.Request.QueryString["id"];
                Helper.SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, true, true), "mpPhysicianSchedule");
                Helper.SetControlEntrySetting(txtPhysicianName, new ControlEntrySetting(true, true, true), "mpPhysicianSchedule");
                BindGridView(CurrPage, true, ref PageCount);
            }
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        private string GetFilterExpression()
        {
            string filterExpression;
            List<SettingParameterDt> setvar = BusinessLayer.GetSettingParameterDtList(String.Format("ParameterCode IN ('{0}','{1}','{2}')", Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI, Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM, Constant.SettingParameter.RT0001));
            string setvarImaging = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).FirstOrDefault().ParameterValue;
            string setvarLaboratory = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).FirstOrDefault().ParameterValue;
            string radiotheraphyUnitID = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.RT0001).FirstOrDefault().ParameterValue;

            if (hdnPhysicianID.Value != "")
            {
                if (hdnDepartmentID.Value == "OP")
                {
                    filterExpression = string.Format("DepartmentID = '{0}' AND ParamedicID = {1}", Constant.Facility.OUTPATIENT, hdnPhysicianID.Value);
                }
                else if (hdnDepartmentID.Value == "IS")
                {
                    filterExpression = string.Format("HealthcareServiceUnitID = '{0}' AND ParamedicID = {1}", setvarImaging, hdnPhysicianID.Value);
                }
                else if (hdnDepartmentID.Value == "LB")
                {
                    filterExpression = string.Format("IsLaboratoryUnit = 1 AND IsUsingRegistration = 1 AND ParamedicID = {0}", hdnPhysicianID.Value);
                }
                else if (hdnDepartmentID.Value == "MD")
                {
                    filterExpression = string.Format("IsLaboratoryUnit = 0 AND DepartmentID = '{0}' AND ParamedicID = {1} AND HealthcareServiceUnitID NOT IN ('{2}','{3}')", Constant.Facility.DIAGNOSTIC, hdnPhysicianID.Value, setvarImaging, setvarLaboratory);
                }
                else if (hdnDepartmentID.Value == "MC")
                {
                    filterExpression = string.Format("DepartmentID = '{0}' AND ParamedicID = {1}", Constant.Facility.MEDICAL_CHECKUP, hdnPhysicianID.Value);
                }
                else if (hdnDepartmentID.Value == "RT")
                {
                    filterExpression = string.Format("HealthcareServiceUnitID = '{0}' AND ParamedicID = {1}", radiotheraphyUnitID, hdnPhysicianID.Value);
                }
                else
                {
                    filterExpression = "1 = 0";
                }
            }
            else
            {
                filterExpression = "1 = 0";
            }

            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvServiceUnitParamedicRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vServiceUnitParamedic> lstEntity = BusinessLayer.GetvServiceUnitParamedicList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
            grdView.DataSource = lstEntity;
            grdView.DataBind();

            List<SettingParameterDt> lstParameter = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                    "HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                                                                    AppSession.UserLogin.HealthcareID, //0
                                                                    Constant.SettingParameter.SA0138 //1
                                                                ));

            string isBridgingToMobileJKN = lstParameter.Where(w => w.ParameterCode == Constant.SettingParameter.SA0138).FirstOrDefault().ParameterValue;

            if (isBridgingToMobileJKN == "1")
            {
                ((DataControlField)grdView.Columns
               .Cast<DataControlField>()
               .Where(fld => (fld.HeaderText == "JKN"))
               .SingleOrDefault()).Visible = true;
            }
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
    }
}