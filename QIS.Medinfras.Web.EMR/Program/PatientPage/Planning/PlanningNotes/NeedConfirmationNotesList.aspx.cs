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

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class NeedConfirmationNotesList : BasePagePatientPageListEntry
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.NEED_CONFIRMATION_NOTES;
        }

        #region List
        protected override void InitializeDataControl()
        {
            if (AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Physician)
            {
                int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                hdnDefaultParamedicID.Value = userLoginParamedic.ToString();
            }

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;

            string code = ddlViewType.SelectedValue;

            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID = {0} AND IsDeleted = 0 AND IsNeedConfirmation = 1 AND IsConfirmed = 0 AND ConfirmationPhysicianID = {1}", AppSession.RegisteredPatient.VisitID, AppSession.UserLogin.ParamedicID);

            if (code == "0")
                filterExpression += string.Format(" AND GCPatientNoteType IN ('{0}','{1}','{2}','{3}')", Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES, Constant.PatientVisitNotes.NURSING_NOTES, Constant.PatientVisitNotes.PHARMACY_NOTES, Constant.PatientVisitNotes.DIAGNOSTIC_SUPPORT_NOTES);
            else if (code == "1")
                filterExpression += string.Format(" AND GCPatientNoteType = '{0}'", Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES);
            else
                filterExpression += string.Format(" AND GCPatientNoteType IN ('{0}','{1}','{2}')", Constant.PatientVisitNotes.NURSING_NOTES, Constant.PatientVisitNotes.PHARMACY_NOTES,Constant.PatientVisitNotes.DIAGNOSTIC_SUPPORT_NOTES);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientVisitNoteRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientVisitNote> lstEntity = BusinessLayer.GetvPatientVisitNoteList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "NoteDate DESC,NoteTime DESC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpCustomProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";

            string lstRecordID = hdnSelectedID.Value;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;

            result = ConfirmPlanningNote(param[0],lstRecordID);

            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = result;
        }

        private string ConfirmPlanningNote(string type,string lstRecordID)
        {
            string result = string.Empty;
            IDbContext ctx = DbFactory.Configure(true);
            PatientVisitNoteDao visitNoteDao = new PatientVisitNoteDao(ctx);
            string filterExpression =  string.Format("ID IN ({0})",lstRecordID);

            try
            {
                if (type == "confirm")
                {
                    //Confirm
                    List<PatientVisitNote> lstVisitNote = BusinessLayer.GetPatientVisitNoteList(filterExpression, ctx);
                    foreach (PatientVisitNote visitNote in lstVisitNote)
                    {
                        visitNote.IsConfirmed = true;
                        visitNote.ConfirmationDateTime = DateTime.Now;
                        visitNoteDao.Update(visitNote);
                    }
                }
                ctx.CommitTransaction();
                result = string.Format("process|1|{0}", string.Empty);
            }
            catch (Exception ex)
            {
                result = string.Format("process|0|{0}", ex.Message);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
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

        #endregion
        #region Entry
        protected override void SetControlProperties()
        {
        }

        protected override void OnControlEntrySetting()
        {
        }

        private void ControlToEntity(PatientVisitNote entity)
        {
        }
        #endregion
    }
}