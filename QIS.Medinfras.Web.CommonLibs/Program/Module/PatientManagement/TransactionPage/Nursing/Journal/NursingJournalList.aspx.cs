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
    public partial class NursingJournalList : BasePagePatientPageList
    {
        string menuType = string.Empty;
        string deptType = string.Empty;
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            if (menuType == "fo")
            {
                switch (deptType)
                {
                    case Constant.Facility.INPATIENT:
                        return Constant.MenuCode.Inpatient.FOLLOWUP_NURSING_JOURNAL;
                    case Constant.Facility.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.FOLLOWUP_PATIENT_PAGE_NURSING_JOURNAL;
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.FOLLOWUP_PATIENT_PAGE_NURSING_JOURNAL;
                    case Constant.Facility.DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.FOLLOWUP_PATIENT_NURSING_JOURNAL;
                    default:
                        return Constant.MenuCode.Inpatient.FOLLOWUP_NURSING_JOURNAL;
                }
            }
            else if (menuType == "dp")
            {
                switch (deptType)
                {
                    case Constant.Facility.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.DATA_PATIENT_PATIENT_NURSING_JOURNAL;
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.DATA_PATIENT_PATIENT_NURSING_JOURNAL;
                    default:
                        return Constant.MenuCode.Outpatient.DATA_PATIENT_PATIENT_NURSING_JOURNAL;
                }
            }
            else
            {
                switch (deptType)
                {
                    case Constant.Facility.INPATIENT:
                        return Constant.MenuCode.Inpatient.PATIENT_PAGE_NURSING_JOURNAL;
                    case Constant.Facility.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.PATIENT_PAGE_NURSING_JOURNAL;
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.PATIENT_PAGE_NURSING_JOURNAL;
                    case Constant.Facility.DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.PATIENT_NURSING_JOURNAL;
                    case Constant.Facility.LABORATORY:
                        return Constant.MenuCode.Laboratory.PATIENT_NURSING_JOURNAL;
                    case Constant.Module.RADIOTHERAPHY:
                        return Constant.MenuCode.Radiotheraphy.PATIENT_PAGE_RT_NURSING_NOTES_CONFIRMATION;
                    default:
                        return Constant.MenuCode.Inpatient.PATIENT_PAGE_NURSING_JOURNAL;
                }
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowDelete = false;
        }

        protected override void InitializeDataControl()
        {
            string[] param = Page.Request.QueryString["id"].Split('|');
            if (param.Count() > 1)
            {
                deptType = param[0];
                menuType = param[1];
            }
            else
            {
                deptType = param[0];
            }

            hdnCurrentSessionID.Value = AppSession.UserLogin.UserID.ToString();
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();

            List<SettingParameterDt> lstSetvarDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IS_VISIBLE_PATIENT_HANDOVER_NOTES_IN_NURSING_JOURNAL));
            hdnIsVisiblePatientHandoverInNursingJournal.Value = lstSetvarDt.Where(w => w.ParameterCode == Constant.SettingParameter.IS_VISIBLE_PATIENT_HANDOVER_NOTES_IN_NURSING_JOURNAL).FirstOrDefault().ParameterValue;

            SetComboBox();
            BindGridView(1, true, ref PageCount);
        }

        private void SetComboBox()
        {
            List<Variable> lstDisplay = new List<Variable>() { new Variable() { Code = "All Notes", Value = "0" }, new Variable() { Code = "My Notes Only", Value = "1" }};
            Methods.SetComboBoxField(cboDisplay, lstDisplay, "Code", "Value");
            cboDisplay.Value = "0";
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            vConsultVisit4 entityLinkedRegistration = BusinessLayer.GetvConsultVisit4List(string.Format("RegistrationID = (SELECT LinkedRegistrationID FROM Registration WHERE RegistrationID = {0})", AppSession.RegisteredPatient.RegistrationID)).FirstOrDefault();
            int cvLinkedID = 0;
            if (entityLinkedRegistration != null)
            {
                cvLinkedID = entityLinkedRegistration.VisitID;
            }

            string filterExpression = string.Format("VisitID IN ({0},{1})", AppSession.RegisteredPatient.VisitID, cvLinkedID);

            if (cboDisplay.Value.ToString() != "1")
            {
                filterExpression = string.Format("VisitID IN ({0},{1})", AppSession.RegisteredPatient.VisitID, cvLinkedID);
            }
            else
            {
                filterExpression = string.Format("VisitID IN ({0},{1}) AND ParamedicID = {2}", AppSession.RegisteredPatient.VisitID, cvLinkedID,  AppSession.UserLogin.ParamedicID);
            }

            if (hdnIsVisiblePatientHandoverInNursingJournal.Value == "0")
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND IsDeleted = 0 AND IsPatientHandover = 0";
                }
                else
                {
                    filterExpression = "IsDeleted = 0 AND IsPatientHandover = 0";
                }
            }
            else
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND IsDeleted = 0";
                }
                else
                {
                    filterExpression = "IsDeleted = 0";
                }
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvNursingJournalRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vNursingJournal> lstEntity = BusinessLayer.GetvNursingJournalList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "JournalDate DESC, JournalTime DESC");
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
            url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/Nursing/Journal/NursingJournalCtl.ascx");
            queryString = "";
            popupWidth = 900;
            popupHeight = 500;
            popupHeaderText = "Catatan Perawat";
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            if (hdnID.Value != "")
            {
                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/Nursing/Journal/NursingJournalCtl.ascx");
                queryString = hdnID.Value;
                popupWidth = 900;
                popupHeight = 500;
                popupHeaderText = "Catatan Perawat";
                return true;
            }
            return false;
        }
    }
}