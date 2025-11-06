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
    public partial class PatientVisitType : BasePagePatientPageList
    {
        string menuType = string.Empty;
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            string MenuCode = "";
            if (menuType == "fo")
            {
                return Constant.MenuCode.Outpatient.FOLLOWUP_PATIENT_VISIT_TYPE;
            }
            else
            {
                if (MenuCode == Constant.MenuCode.Outpatient.PATIENT_VISIT_TYPE)
                {
                    if (hdnDepartmentID.Value != "")
                    {
                        switch (hdnDepartmentID.Value)
                        {
                            case Constant.Facility.PHARMACY:
                                if (hdnSubMenuType.Value == "cp")
                                    MenuCode = Constant.MenuCode.Pharmacy.PHARMACIST_CLINICAL_ALLERGY;
                                break;
                            case Constant.Facility.DIAGNOSTIC:
                                if (hdnSubMenuType.Value == "nt")
                                    MenuCode = Constant.MenuCode.Nutrition.NUTRITION_ALLERGY;
                                break;
                            default:
                                break;
                        }
                    }
                    return MenuCode;
                }
                else
                {
                    String MenuID = Request.QueryString["id"];
                    switch (MenuID)
                    {
                        default: return Constant.MenuCode.Outpatient.PATIENT_VISIT_TYPE;
                    }
                }
            }   
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                string MenuCode = "";
                if (MenuCode == Constant.MenuCode.EmergencyCare.PATIENT_ALLERGY)
                {
                    string[] param = Page.Request.QueryString["id"].Split('|');
                    hdnDepartmentID.Value = param[0];
                    hdnSubMenuType.Value = param[1];
                    if (param.Length > 1)
                    {
                        hdnSubMenuType.Value = param[1];
                        menuType = param[1];
                    }
                }
                else
                {
                    hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
                    string[] param = Page.Request.QueryString["id"].Split('|');
                    if (param.Length > 1)
                    {
                        menuType = param[1];
                    }
                }
            }
            else
            {
                hdnDepartmentID.Value = string.Empty;
                hdnSubMenuType.Value = string.Empty;
            }

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = ""; //hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvConsultVisitTypeRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vConsultVisitType> lstEntity = BusinessLayer.GetvConsultVisitTypeList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
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
            url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/Assessment/VisitType/PatientVisitTypeEntryCtl.ascx");
            queryString = hdnSubMenuType.Value != "nt" ? "" : ""+ "|" + hdnSubMenuType.Value;
            popupWidth = 500;
            popupHeight = 150;
            popupHeaderText = "Jenis Kunjungan";
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            if (hdnID.Value != "")
            {
                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/Assessment/VisitType/PatientVisitTypeEntryCtl.ascx");
                queryString = hdnSubMenuType.Value != "nt" ? hdnID.Value : hdnID.Value + "|" + hdnSubMenuType.Value;
                queryString = hdnID.Value;
                popupWidth = 500;
                popupHeight = 150;
                popupHeaderText = "Jenis Kunjungan";
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value != "")
            {
                ConsultVisitType entity = BusinessLayer.GetConsultVisitType(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedDate = DateTime.Now;
                BusinessLayer.UpdateConsultVisitType(entity);
                return true;
            }
            return false;
        }
    }
}