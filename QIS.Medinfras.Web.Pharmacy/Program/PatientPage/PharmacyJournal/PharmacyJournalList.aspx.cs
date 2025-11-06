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

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class PharmacyJournalList : BasePagePatientPageList
    {
        string menuType = string.Empty;
        string deptType = string.Empty;
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            string menuCode = Constant.MenuCode.Inpatient.PATIENT_PAGE_NURSING_NOTE;
            if (hdnSubMenuType.Value == Constant.MenuCode.Pharmacy.PHARMACY_NOTES_JOURNAL)
                menuCode = Constant.MenuCode.Pharmacy.PHARMACY_NOTES_JOURNAL;
            else
                menuCode = Constant.MenuCode.Pharmacy.PHARMACIST_CLINICAL_JOURNAL;

            return menuCode;
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
                hdnSubMenuType.Value = param[1];
            }
            else
            {
                deptType = param[0];
            }

            hdnCurrentUserID.Value = AppSession.UserLogin.UserID.ToString();
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            SetComboBox();

            txtFromDate.Text = DateTime.Today.AddDays(-1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtToDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

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
                filterExpression = string.Format("VisitID IN ({0},{1}) AND CreatedBy = {2}", AppSession.RegisteredPatient.VisitID, cvLinkedID,  AppSession.UserLogin.UserID);
            }

            if (filterExpression != "")
            {
                filterExpression += " AND IsDeleted = 0";
            }
            else
            {
                filterExpression = "IsDeleted = 0";
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPharmacyJournalRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPharmacyJournal> lstEntity = BusinessLayer.GetvPharmacyJournalList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "JournalDate DESC, JournalTime DESC");
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
            url = ResolveUrl("~/Program/PatientPage/PharmacyJournal/PharmacyJournalCtl.ascx");
            queryString = "0"+"|"+"|";
            popupWidth = 900;
            popupHeight = 500;
            popupHeaderText = "Catatan Jurnal Farmasi";
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            if (hdnID.Value != "")
            {
                url = ResolveUrl("~/Program/PatientPage/PharmacyJournal/PharmacyJournalCtl.ascx");
                queryString = hdnID.Value + "|" + "|";
                popupWidth = 900;
                popupHeight = 500;
                popupHeaderText = "Catatan Jurnal Farmasi";
                return true;
            }
            return false;
        }
    }
}