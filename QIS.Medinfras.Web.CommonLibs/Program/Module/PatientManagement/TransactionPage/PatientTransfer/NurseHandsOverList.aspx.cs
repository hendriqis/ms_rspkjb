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
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class NurseHandsOverList : BasePagePatientPageList
    {
        protected int PageCount = 1;
        private string pageTitle = string.Empty;
        string menuType = string.Empty;
        string deptType = string.Empty;

        public override string OnGetMenuCode()
        {
            if (menuType == "fo")
            {
                #region Follow-up Pasien Pulang
                switch (deptType)
                {
                    case Constant.Facility.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.FOLLOWUP_NURSE_HANDS_OVER;
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.FOLLOWUP_NURSE_HANDS_OVER;
                    case Constant.Facility.INPATIENT:
                        return Constant.MenuCode.Inpatient.FOLLOWUP_NURSE_HANDS_OVER;
                    case Constant.Facility.DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.FOLLOWUP_NURSE_HANDS_OVER;
                    default:
                        return Constant.MenuCode.Outpatient.FOLLOWUP_NURSE_HANDS_OVER;
                }
                #endregion
            }
            else
            {
                #region Pasien Dalam Perawatan
                switch (deptType)
                {
                    case Constant.Facility.INPATIENT:
                        return Constant.MenuCode.Inpatient.PATIENT_PAGE_NURSE_HANDS_OVER;
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.PATIENT_PAGE_NURSE_HANDS_OVER;
                    case Constant.Facility.DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.PATIENT_PAGE_NURSE_HANDS_OVER;
                    case Constant.Facility.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.PATIENT_PAGE_NURSE_HANDS_OVER;
                    case Constant.Facility.IMAGING:
                        return Constant.MenuCode.Imaging.PATIENT_PAGE_NURSE_HANDS_OVER;
                    case Constant.Facility.LABORATORY:
                        return Constant.MenuCode.Laboratory.PATIENT_PAGE_NURSE_HANDS_OVER;
                    case Constant.Module.RADIOTHERAPHY:
                        return Constant.MenuCode.Radiotheraphy.PATIENT_PAGE_RT_PATIENT_HANDOVER;
                    default:
                        return Constant.MenuCode.Inpatient.PATIENT_PAGE_NURSE_HANDS_OVER;
                }
                #endregion
            }
        }

        protected string GetPageTitle()
        {
            return pageTitle;
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
                    deptType = param[0];
                }
            }
            else
            {
                hdnDepartmentID.Value = string.Empty;
            }

            pageTitle = BusinessLayer.GetMenuMasterList(string.Format("MenuCode= '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            List<Registration> dataRegID = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0} OR LinkedToRegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID));
            string lstReg = "";
            if (dataRegID != null)
            {
                foreach (Registration reg in dataRegID)
                {
                    if (lstReg != "")
                    {
                        lstReg += ",";
                    }
                    lstReg += reg.RegistrationID;
                }
            }

            string filterExpression = string.Format("RegistrationID IN ({0}) AND IsDeleted = 0", lstReg);

            //filterExpression += string.Format("RegistrationID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientNurseTransferRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientNurseTransfer> lstView = BusinessLayer.GetvPatientNurseTransferList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
            grdView.DataSource = lstView;
            grdView.DataBind();
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = true;
            IsAllowEdit = true;
            IsAllowDelete = true;
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
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

	//protected override bool OnBeforeEditRecord(ref string errMessage)
        //{
        //    bool result = true;
        //    errMessage = string.Empty;

        //    if (hdnIsEditable.Value != "True")
        //    {
        //        errMessage = "Proses perubahan data tidak boleh dilakukan karena data sudah di-propose atau dikonfirmasi!";
        //        result = false;
        //    }
        //    else
        //    {
        //        int fromNurseID = Convert.ToInt32(hdnFromNurseID.Value);
        //        if (AppSession.UserLogin.ParamedicID != fromNurseID)
        //        {
        //            errMessage = "Harus Perawat yang melakukan pengentrian awal serah terima yang bisa melakukan Perubahan Data";
        //            result = false;
        //        }
        //    }
        //    return result;
        //}
         protected override bool OnBeforeEditRecord(ref string errMessage)
        {
            bool result = true;
            errMessage = string.Empty;
            int fromNurseID = Convert.ToInt32(hdnFromNurseID.Value);

            if (AppSession.UserLogin.ParamedicID == fromNurseID && hdnIsConfirmed.Value == "True")
            {
                {
                    errMessage = "Proses perubahan data tidak boleh dilakukan karena data sudah di-propose atau dikonfirmasi!";
                    result = false;
                }
            }
            else
              
                if (AppSession.UserLogin.ParamedicID != fromNurseID)
                {
                    errMessage = "Harus Perawat yang melakukan pengentrian awal serah terima yang bisa melakukan Perubahan Data";
                    result = false;
                }
            return result;
        }

        protected override bool OnBeforeDeleteRecord(ref string errMessage)
        {
            bool result = true;
            errMessage = string.Empty;

            if (hdnIsEditable.Value != "True")
            {
                errMessage = "Proses hapus data tidak boleh dilakukan karena data sudah di-propose atau dikonfirmasi!";
                result = false;
            }
            else
            {
                int fromNurseID = Convert.ToInt32(hdnFromNurseID.Value);
                if (AppSession.UserLogin.ParamedicID != fromNurseID)
                {
                    errMessage = "Harus Perawat yang melakukan pengentrian awal serah terima yang bisa melakukan Penghapusan Data";
                    result = false;
                }
            }
            return result;
        }

        protected override bool OnAddRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/PatientTransfer/NurseHandsOverEntryCtl.ascx");
            queryString = "";
            popupWidth = 800;
            popupHeight = 450;
            pageTitle = "Serah Terima Pasien";
            popupHeaderText = string.Format("Serah Terima Pasien : {0} (MRN = {1}, REG = {2})", AppSession.RegisteredPatient.PatientName, AppSession.RegisteredPatient.MedicalNo, AppSession.RegisteredPatient.RegistrationNo);
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/PatientTransfer/NurseHandsOverEntryCtl.ascx");
            queryString = hdnID.Value + "|" + hdnFromHealthcareServiceUnitID.Value + "|" + hdnToHealthcareServiceUnitID.Value;
            popupWidth = 800;
            popupHeight = 450;
            pageTitle = "Serah Terima Pasien";
            popupHeaderText = string.Format("Serah Terima Pasien : {0} (MRN = {1}, REG = {2})", AppSession.RegisteredPatient.PatientName, AppSession.RegisteredPatient.MedicalNo, AppSession.RegisteredPatient.RegistrationNo);
            return true;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            errMessage = string.Empty;

            IDbContext ctx = DbFactory.Configure(true);

            PatientNurseTransferDao transferDao = new PatientNurseTransferDao(ctx);
            NursingJournalDao journalDao = new NursingJournalDao(ctx);

            try
            {
                if (hdnID.Value != "")
                {
                    int id = Convert.ToInt32(hdnID.Value); 
                    PatientNurseTransfer entity = transferDao.Get(id);
                    entity.IsDeleted = true;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    transferDao.Update(entity);

                    NursingJournal journal = journalDao.Get(Convert.ToInt32(entity.NursingJournalID));
                    journal.IsDeleted = true;
                    journal.LastUpdatedBy = AppSession.UserLogin.UserID;
                    journalDao.Update(journal);
                }

                ctx.CommitTransaction();

                result = true;
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        protected void cbpPropose_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int id = Convert.ToInt32(e.Parameter);
            result = "success" + "|" + string.Empty;
            try
            {
                PatientNurseTransfer transfer = BusinessLayer.GetPatientNurseTransfer(id);
                if (transfer != null)
                {
                    if (IsValidToPropose(transfer, ref errMessage))
                    {
                        transfer.GCPatientNurseTransferStatus = Constant.NursePatientTransferStatus.PROPOSED;
                        transfer.LastUpdatedBy = AppSession.UserLogin.UserID;
                        BusinessLayer.UpdatePatientNurseTransfer(transfer);
                        result = string.Format("success|{0}", errMessage);
                    }
                    else
                    {
                        result = string.Format("fail|{0}", errMessage);
                    }
                }
                else
                {
                    errMessage = "Record not found";
                    result = string.Format("fail|{0}", errMessage);
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = id;
        }

        private bool IsValidToPropose(PatientNurseTransfer transfer, ref string errMessage)
        {
            bool result = true;
            if (transfer.FromNurseID != AppSession.UserLogin.ParamedicID)
            {
                errMessage = "Harus Perawat yang melakukan pengentrian awal serah terima yang bisa melakukan Propose";
                result =  false;
            }
            return result;
        }
    }
}