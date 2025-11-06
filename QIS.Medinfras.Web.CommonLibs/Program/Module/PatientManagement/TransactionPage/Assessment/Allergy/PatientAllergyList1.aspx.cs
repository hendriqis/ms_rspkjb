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
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientAllergyList1 : BasePagePatientPageList
    {
        string menuType = string.Empty;
        string deptType = string.Empty;
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            string MenuCode = "";

            if (menuType == "fo")
            {
                switch (hdnDepartmentID.Value)
                {
                    case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.FOLLOWUP_PATIENT_PAGE_ALLERGY;
                    case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Outpatient.FOLLOWUP_PATIENT_PAGE_ALLERGY;
                    case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.PHARMACIST_CLINICAL_ALLERGY;
                    case Constant.Facility.DIAGNOSTIC: return Constant.MenuCode.MedicalDiagnostic.FOLLOWUP_PATIENT_PAGE_ALLERGY;
                    default: return Constant.MenuCode.Outpatient.FOLLOWUP_PATIENT_PAGE_ALLERGY;
                }
            }
            else
            {
                if (MenuCode == Constant.MenuCode.Nutrition.NUTRITION_ALLERGY)
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
                    String[] paramInfo = Request.QueryString["id"].Split('|');
                    string deptID = paramInfo[0];
                    string menuID = String.Empty;
                    if (paramInfo.Length > 1)
                    {
                        menuID = paramInfo[1];
                    }

                    switch (deptID)
                    {
                        case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.PATIENT_PAGE_ALLERGY;
                        case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Outpatient.PATIENT_PAGE_ALLERGY;
                        case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.ALLERGY_INPATIENT;
                        case Constant.Facility.DIAGNOSTIC:
                            if (menuID == "nt")
                            {
                                return Constant.MenuCode.Nutrition.NUTRITION_ALLERGY;
                            }
                            else
                            {
                                return Constant.MenuCode.MedicalDiagnostic.PATIENT_PAGE_ALLERGY; 
                            }
                        case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.PHARMACIST_CLINICAL_ALLERGY;
                        case Constant.Module.RADIOTHERAPHY:
                            return Constant.MenuCode.Radiotheraphy.PATIENT_PAGE_RT_PATIENT_ALLERGY;
                        default: return Constant.MenuCode.Nutrition.NUTRITION_ALLERGY;
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
                    hdnDepartmentID.Value = param[0];
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
            filterExpression += string.Format("MRN = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.MRN);

            if (hdnSubMenuType.Value == "nt")
                filterExpression += string.Format("AND GCAllergenType = '{0}'", Constant.AllergenType.FOOD);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientAllergyRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientAllergy> lstEntity = BusinessLayer.GetvPatientAllergyList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
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
            url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/Assessment/Allergy/PatientAllergyEntryCtl1.ascx");
            queryString = hdnSubMenuType.Value != "nt" ? "" : ""+ "|" + hdnSubMenuType.Value;
            popupWidth = 500;
            popupHeight = 400;
            popupHeaderText = "Alergi Pasien";
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            if (hdnID.Value != "")
            {
                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/Assessment/Allergy/PatientAllergyEntryCtl1.ascx");
                queryString = hdnSubMenuType.Value != "nt" ? hdnID.Value : hdnID.Value + "|" + hdnSubMenuType.Value;
                queryString = hdnID.Value;
                popupWidth = 500;
                popupHeight = 400;
                popupHeaderText = "Alergi Pasien";
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value != "")
            {
                PatientAllergy entity = BusinessLayer.GetPatientAllergy(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedDate = DateTime.Now;
                BusinessLayer.UpdatePatientAllergy(entity);

                if (AppSession.SA0137 == "1")
                {
                    if (AppSession.SA0133 == Constant.CenterBackConsumerAPI.MEDINFRAS_EMR_V1)
                    {
                        BridgingToMedinfrasV1(2);
                    }
                }
                return true;
            }
            return false;
        }

        private void BridgingToMedinfrasV1(int ProcessType)
        {
            APIMessageLog apiLog = new APIMessageLog();
            apiLog.MessageDateTime = DateTime.Now;
            apiLog.Sender = Constant.BridgingVendor.HIS;
            apiLog.Recipient = Constant.BridgingVendor.MEDINFRAS_API;

            MedinfrasV1Service oService = new MedinfrasV1Service();
            string serviceResult = string.Empty;
            serviceResult = oService.OnSendPatientAllergiesInformation(ProcessType, AppSession.RegisteredPatient.RegistrationNo);
            if (!string.IsNullOrEmpty(serviceResult))
            {
                string[] serviceResultInfo = serviceResult.Split('|');
                if (serviceResultInfo[0] == "1")
                {
                    apiLog.IsSuccess = true;
                    apiLog.MessageText = serviceResultInfo[1];
                    apiLog.Response = serviceResultInfo[2];
                }
                else
                {
                    apiLog.IsSuccess = false;
                    apiLog.MessageText = serviceResultInfo[1];
                    apiLog.Response = serviceResultInfo[2];
                    apiLog.ErrorMessage = serviceResultInfo[2];
                }
                BusinessLayer.InsertAPIMessageLog(apiLog);
            }
        }
    }
}