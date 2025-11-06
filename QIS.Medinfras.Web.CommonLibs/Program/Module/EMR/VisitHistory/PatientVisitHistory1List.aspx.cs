using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientVisitHistory1List : BasePagePatientPageList
    {
        public override string OnGetMenuCode()
        {
            string menuCode = Constant.MenuCode.MedicalDiagnostic.MEDICAL_HISTORY;
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.DIAGNOSTIC:
                    menuCode = Constant.MenuCode.MedicalDiagnostic.MEDICAL_HISTORY;
                    break;
                default:
                    menuCode = Constant.MenuCode.MedicalDiagnostic.MEDICAL_HISTORY;
                    break;
            }
            return menuCode;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected int PageCount = 1;
        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                string[] param = Page.Request.QueryString["id"].Split('|');
                hdnDepartmentID.Value = param[0];
            }

            DateTime.Now.AddMonths(1).AddDays(-DateTime.Now.Day);
            BindGridView(1, true, ref PageCount);
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = false;
            IsAllowEdit = false;
            IsAllowDelete = false;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("MRN = {0}", AppSession.RegisteredPatient.MRN);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvConsultVisitRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vConsultVisitCustom> lstEntity = BusinessLayer.GetvConsultVisitCustomList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "VisitID DESC");
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