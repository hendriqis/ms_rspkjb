using System;
using System.Collections.Generic;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using System.Text;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class NeedConfirmationParamedicNotesList : BasePagePatientPageList
    {
        protected int PageCount = 1;
        string menuType = string.Empty;
        public override string OnGetMenuCode()
        {
            switch (menuType)
            {
                case Constant.MenuCode.Pharmacy.PHARMACIST_CLINICAL_NOTES_CONFIRMATION:
                    return Constant.MenuCode.Pharmacy.PHARMACIST_CLINICAL_NOTES_CONFIRMATION;
                case Constant.MenuCode.Pharmacy.PHARMACY_NOTES_CONFIRMATION:
                    return Constant.MenuCode.Pharmacy.PHARMACY_NOTES_CONFIRMATION;
                default:
                    return Constant.MenuCode.Pharmacy.PHARMACY_NOTES_CONFIRMATION;
            }
        }

        #region List
        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                string[] param = Page.Request.QueryString["id"].Split('|');
                if (param.Length > 1)
                {
                    hdnDepartmentID.Value = param[0];
                    menuType = param[1];
                }
                else
                {
                    hdnDepartmentID.Value = Page.Request.QueryString["id"];
                }
            }

            if (AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Physician)
            {
                int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                hdnDefaultParamedicID.Value = userLoginParamedic.ToString();
            }
            
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
            string filterExpression = hdnFilterExpression.Value;

            StringBuilder sbLstHealthcareServiceUnitID = new StringBuilder();
            List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, hdnDepartmentID.Value, "");
            foreach (GetServiceUnitUserList serviceUnit in lstServiceUnit)
            {
                if (sbLstHealthcareServiceUnitID.ToString() != "")
                    sbLstHealthcareServiceUnitID.Append(",");
                sbLstHealthcareServiceUnitID.Append(serviceUnit.HealthcareServiceUnitID.ToString());
            }

            string serviceUnitList = sbLstHealthcareServiceUnitID.ToString();

            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID = {0} AND IsDeleted = 0 AND IsNeedNotification = 1 AND NotificationParamedicID IS NULL AND NotificationUnitID IN ({1})", AppSession.RegisteredPatient.VisitID, serviceUnitList);

            //filterExpression += string.Format(" AND GCPatientNoteType IN ('{0}','{1}','{2}')", Constant.PatientVisitNotes.NURSING_NOTES, Constant.PatientVisitNotes.PHARMACY_NOTES,Constant.PatientVisitNotes.DIAGNOSTIC_SUPPORT_NOTES);

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
                        visitNote.NotificationDateTime = DateTime.Now;
                        //Karena yang konfirmasi bukan Apoteker, bisa juga dengan user ID --> supaya tidak membingungkan menggunakan user id saja
                        visitNote.NotificationParamedicID = AppSession.UserLogin.UserID;
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

        protected override void SetControlProperties()
        {
        }        
    }
}