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

namespace QIS.Medinfras.Web.MedicalCheckup.Program
{
    public partial class PhysicianSchedule : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                //case "OP": return Constant.MenuCode.Outpatient.PARAMEDIC_SCHEDULE;
                //case "IS": return Constant.MenuCode.Imaging.PARAMEDIC_SCHEDULE;
                //case "LB": return Constant.MenuCode.Laboratory.PARAMEDIC_SCHEDULE;
                //case "MD": return Constant.MenuCode.MedicalDiagnostic.PARAMEDIC_SCHEDULE;
                default: return Constant.MenuCode.MedicalCheckup.MCU_PARAMEDIC_SCHEDULE;
            }
        }

        protected string GetPhysicianFilter()
        {
            string param = string.Format("ParamedicID IN (SELECT ParamedicID FROM vServiceUnitParamedic WHERE DepartmentID = 'MCU')",
                                            Constant.Facility.MEDICAL_CHECKUP);

            return param;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            Helper.SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, true, true), "mpPhysicianSchedule");
            Helper.SetControlEntrySetting(txtPhysicianName, new ControlEntrySetting(true, true, true), "mpPhysicianSchedule");
            BindGridView(CurrPage, true, ref PageCount);
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        private string GetFilterExpression()
        {
            string filterExpression;
            List<SettingParameterDt> setvar = BusinessLayer.GetSettingParameterDtList(String.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI, Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM));
            string setvarImaging = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).FirstOrDefault().ParameterValue;
            string setvarLaboratory = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).FirstOrDefault().ParameterValue;

            if (hdnPhysicianID.Value != "")
            {
                filterExpression = string.Format("DepartmentID = '{0}' AND ParamedicID = {1}", Constant.Facility.MEDICAL_CHECKUP, hdnPhysicianID.Value);
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