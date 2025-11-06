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
    public partial class ObstetricHistoryList : BasePagePatientPageList
    {
        string menuType = string.Empty;
        string deptType = string.Empty;
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            string MenuCode = "";

            if (menuType == "fo")
            {
                #region Follow-up Pasien Pulang
                switch (deptType)
                {
                    case "EMERGENCY": MenuCode = Constant.MenuCode.EmergencyCare.FOLLOWUP_PATIENT_PAGE_OBSTETRIC_HISTORY; break;
                    case "OUTPATIENT": MenuCode = Constant.MenuCode.Outpatient.FOLLOWUP_PATIENT_PAGE_OBSTETRIC_HISTORY; break;
                    case "INPATIENT": MenuCode = Constant.MenuCode.Inpatient.FOLLOWUP_PATIENT_PAGE_OBSTETRIC_HISTORY; break;
                    case "DIAGNOSTIC": MenuCode = Constant.MenuCode.MedicalDiagnostic.FOLLOWUP_PATIENT_PAGE_OBSTETRIC_HISTORY; break;
                    default: MenuCode = Constant.MenuCode.Outpatient.FOLLOWUP_PATIENT_PAGE_OBSTETRIC_HISTORY; break;
                }
                #endregion
            }
            else
            {
                #region Pasien Dalam Perawatan
                switch (deptType)
                {
                    case "EMERGENCY": MenuCode = Constant.MenuCode.EmergencyCare.PATIENT_PAGE_OBSTETRIC_HISTORY; break;
                    case "OUTPATIENT": MenuCode = Constant.MenuCode.Outpatient.PATIENT_PAGE_OBSTETRIC_HISTORY; break;
                    case "INPATIENT": MenuCode = Constant.MenuCode.Inpatient.PATIENT_PAGE_OBSTETRIC_HISTORY; break;
                    case "DIAGNOSTIC": MenuCode = Constant.MenuCode.MedicalDiagnostic.PATIENT_PAGE_OBSTETRIC_HISTORY; break;
                    case Constant.Module.RADIOTHERAPHY: MenuCode = Constant.MenuCode.Radiotheraphy.PATIENT_PAGE_RT_OBSGYN_HISTORY; break;
                    default: MenuCode = Constant.MenuCode.Outpatient.PATIENT_PAGE_OBSTETRIC_HISTORY; break;
                }
                #endregion
            }

            
            return MenuCode;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                string[] param = Page.Request.QueryString["id"].Split('|');
                if (param.Count() > 1)
                {
                    deptType = param[0];
                    menuType = param[1];
                }
                else
                {
                    hdnDepartmentID.Value = Page.Request.QueryString["id"];
                    hdnSubMenuType.Value = string.Empty;
                    deptType = param[0];
                }
            }
            else
            {
                hdnDepartmentID.Value = string.Empty;
                hdnSubMenuType.Value = string.Empty;
            }

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            BindGridView(1, true, ref PageCount);
           
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = ""; //hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("MRN = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.MRN);

            if (hdnSubMenuType.Value == "nt")
                filterExpression += string.Format("AND GCAllergenType = '{0}'", Constant.AllergenType.FOOD);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvObstetricHistoryRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vObstetricHistory> lstEntity = BusinessLayer.GetvObstetricHistoryList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
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
            url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/Assessment/Obsgyn/ObstetricHistoryEntryCtl.ascx");
            queryString = "";
            popupWidth = 680;
            popupHeight = 500;
            popupHeaderText = "Riwayat Persalinan";
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            if (hdnID.Value != "")
            {
                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/Assessment/Obsgyn/ObstetricHistoryEntryCtl.ascx");
                queryString = hdnID.Value;
                popupWidth = 680;
                popupHeight = 500;
                popupHeaderText = "Riwayat Persalinan";
                return true;
            }
            errMessage = "Pilih Riwayat Persalinan Terlebih Dahulu!";
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value != "")
            {
                ObstetricHistory entity = BusinessLayer.GetObstetricHistory(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedDate = DateTime.Now;
                BusinessLayer.UpdateObstetricHistory(entity);
                return true;
            }
            errMessage = "Pilih Riwayat Persalinan Terlebih Dahulu!";
            return false;
        }
    }
}