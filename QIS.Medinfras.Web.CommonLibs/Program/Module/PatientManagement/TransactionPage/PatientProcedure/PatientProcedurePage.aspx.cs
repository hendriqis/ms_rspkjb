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
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientProcedurePage : BasePagePatientPageList
    {
        protected int PageCount = 1;
        string menuType = string.Empty;

        public override string OnGetMenuCode()
        {
            string menuCode = string.Empty;
            if (string.IsNullOrEmpty(menuType))
            {
                switch (hdnDepartmentID.Value)
                {
                    case Constant.Facility.EMERGENCY: menuCode = Constant.MenuCode.EmergencyCare.TRANSACTION_PAGE_PATIENT_PROCEDURES; break;
                    case Constant.Facility.INPATIENT: menuCode = Constant.MenuCode.Inpatient.PATIENT_TRANSACTION_PAGE_PROCEDURES; break;
                    default: menuCode = Constant.MenuCode.Outpatient.TRANSACTION_PAGE_PATIENT_PROCEDURES; break;
                }
            }
            else
            {
                switch (menuType)
                {
                    case "md": menuCode = Constant.MenuCode.MedicalDiagnostic.PATIENT_TRANSACTION_PAGE_PROCEDURES; break;
                    case "er": menuCode = Constant.MenuCode.EmergencyCare.PATIENT_TRANSACTION_PAGE_PROCEDURES; break;
                    default:
                        break;
                }
            }
            return menuCode;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                menuType = Page.Request.QueryString["id"];
            }

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            ConsultVisit entity = BusinessLayer.GetConsultVisit(AppSession.RegisteredPatient.VisitID);
            vHealthcareServiceUnit entityHSU = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", entity.HealthcareServiceUnitID)).FirstOrDefault();
            hdnDepartmentID.Value = entityHSU.DepartmentID;
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

        protected override bool OnAddRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientAssessment/TreatmentProcedure/TreatmentProcedureCtl.ascx");
            if (Page.Request.QueryString["id"] != null)
            {
                queryString = "|" + Page.Request.QueryString["id"];
            }
            else
            {
                queryString = "";
            }
            popupWidth = 900;
            popupHeight = 500;
            popupHeaderText = "Prosedur/Tindakan Pasien";
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            if (hdnID.Value != "")
            {
                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientAssessment/TreatmentProcedure/TreatmentProcedureCtl.ascx");
                if (Page.Request.QueryString["id"] != null)
                {
                    queryString = hdnID.Value + "|" + Page.Request.QueryString["id"];
                }
                else
                {
                    queryString = hdnID.Value;
                }
                queryString = hdnID.Value + "|" + Page.Request.QueryString["id"];
                popupWidth = 900;
                popupHeight = 500;
                popupHeaderText = "Prosedur/Tindakan Pasien";
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value != "")
            {
                PatientProcedure entity = BusinessLayer.GetPatientProcedure(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePatientProcedure(entity);
                return true;
            }
            return false;
        }
    }
}