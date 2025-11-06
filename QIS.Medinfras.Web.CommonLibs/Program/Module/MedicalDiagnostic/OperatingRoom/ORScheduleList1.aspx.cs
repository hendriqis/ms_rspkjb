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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ORScheduleList1 : BasePageTrx
    {
        protected int PageCount = 1;

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override string OnGetMenuCode()
        {
            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);
            if (ModuleID == Constant.Module.INPATIENT)
                return Constant.MenuCode.Inpatient.TARIFF_ESTIMATION;
            else if (ModuleID == Constant.Module.OUTPATIENT)
                return Constant.MenuCode.Outpatient.TARIFF_ESTIMATION;
            else if (ModuleID == Constant.Module.EMERGENCY)
                return Constant.MenuCode.EmergencyCare.TARIFF_ESTIMATION;
            else if (ModuleID == Constant.Module.PHARMACY)
                return Constant.MenuCode.Pharmacy.TARIFF_ESTIMATION;
            else if (ModuleID == Constant.Module.LABORATORY)
                return Constant.MenuCode.Laboratory.TARIFF_ESTIMATION;
            else if (ModuleID == Constant.Module.IMAGING)
                return Constant.MenuCode.Imaging.TARIFF_ESTIMATION;
            else if (ModuleID == Constant.Module.MEDICAL_DIAGNOSTIC)
                return Constant.MenuCode.MedicalDiagnostic.OPERATING_ROOM_SCHEDULE;
            else if (ModuleID == Constant.Module.INVENTORY)
                return Constant.MenuCode.Inventory.TARIFF_ESTIMATION;
            else if (ModuleID == Constant.Module.EMR)
                return Constant.MenuCode.EMR.TARIFF_ESTIMATION;
            return Constant.MenuCode.Finance.TARIFF_ESTIMATION;
        }

        protected override void InitializeDataControl()
        {
            SetControlProperties();
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("MRN = {0} AND VisitID != {1}", AppSession.RegisteredPatient.MRN, AppSession.RegisteredPatient.VisitID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPreviousMedicalHistoryRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPreviousMedicalHistory> lstEntity = BusinessLayer.GetvPreviousMedicalHistoryList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "VisitDate DESC");
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