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


namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class MRPatientNotes : BasePagePatientPageList
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.PATIENT_SOAP;
        }

        protected override void InitializeDataControl()
        {
            ctlToolbar.SetSelectedMenu(8);
            hdnCurrentSessionID.Value = AppSession.UserLogin.UserID.ToString();
            BindGridView(1, true, ref PageCount);
            
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowDelete = false;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            vConsultVisit entityLinkedRegistration = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = (SELECT LinkedRegistrationID FROM Registration WHERE RegistrationID = {0})", AppSession.RegisteredPatient.RegistrationID)).FirstOrDefault();
            int cvLinkedID = 0;
            if (entityLinkedRegistration != null)
            {
                cvLinkedID = entityLinkedRegistration.VisitID;
            }

            string filterExpression = string.Format("VisitID IN ({0},{1})", AppSession.RegisteredPatient.VisitID, cvLinkedID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientVisitNoteRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientVisitNote> lstEntity = BusinessLayer.GetvPatientVisitNoteList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "NoteDate DESC, NoteTime DESC");
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
            url = ResolveUrl("~/Program/PatientMedicalRecord/MRPatientSOAP/MRPatientNotes/MRPatientNotesCtl.ascx");
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
                url = ResolveUrl("~/Program/PatientMedicalRecord/MRPatientSOAP/MRPatientNotes/MRPatientNotesCtl.ascx");
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