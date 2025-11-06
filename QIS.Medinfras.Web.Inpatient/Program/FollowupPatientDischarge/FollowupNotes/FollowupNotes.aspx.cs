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

namespace QIS.Medinfras.Web.Inpatient.Program
{
    public partial class FollowupNotes : BasePagePatientPageList
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inpatient.FOLLOWUP_PATIENT_PAGE_NURSING_NOTE;
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
            hdnCurrentSessionID.Value = AppSession.UserLogin.UserID.ToString();
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            vConsultVisit entityLinkedRegistration = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = (SELECT LinkedRegistrationID FROM Registration WHERE RegistrationID = {0})", AppSession.RegisteredPatient.RegistrationID)).FirstOrDefault();
            int cvLinkedID = 0;
            if (entityLinkedRegistration != null)
            {
                cvLinkedID = entityLinkedRegistration.VisitID;
            }

            string filterExpression = string.Format("VisitID IN ({0},{1}) AND GCPatientNoteType = '{2}'", AppSession.RegisteredPatient.VisitID, cvLinkedID, Constant.PatientVisitNotes.FOLLOWUP_NOTES);

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
            url = ResolveUrl("~/Program/FollowupPatientDischarge/FollowupNotes/FollowupNotesCtl.ascx");
            queryString = "";
            popupWidth = 900;
            popupHeight = 500;
            popupHeaderText = "Catatan Followup";
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            if (hdnID.Value != "")
            {
                url = ResolveUrl("~/Program/FollowupPatientDischarge/FollowupNotes/FollowupNotesCtl.ascx");
                queryString = hdnID.Value;
                popupWidth = 900;
                popupHeight = 500;
                popupHeaderText = "Catatan Followup";
                return true;
            }
            return false;
        }

        //protected override bool OnDeleteRecord(ref string errMessage)
        //{
        //    if (hdnID.Value != "")
        //    {
        //        NursingJournal entity = BusinessLayer.GetNursingJournal(Convert.ToInt32(hdnID.Value));
        //        entity.IsDeleted = true;
        //        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
        //        BusinessLayer.UpdateNursingJournal(entity);
        //        return true;
        //    }
        //    return false;
        //}
    }
}