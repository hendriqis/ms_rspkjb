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

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class PatientChargesList1 : BasePagePatientPageList
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.PATIENT_CHARGES_ENTRY;
        }

        public override bool IsEntryUsePopup()
        {
            return true;
        }

        public String IsAllowPreviewTariff()
        {
            return hdnIsAllowPreviewTariff.Value;
        }

        protected override void InitializeDataControl()
        {
            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                                "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                                                                                AppSession.UserLogin.HealthcareID, //0
                                                                                Constant.SettingParameter.EM0017, //1
                                                                                Constant.SettingParameter.SA0168, //2
                                                                                Constant.SettingParameter.EM_IS_DOCTOR_FEE_ALLOW_PREVIEW_TARIFF //3
                                                                            ));
            hdnIsValidateBeforeEntry.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.EM0017).ParameterValue;
            hdnIsShowItemNotificationWhenProposed.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA0168).ParameterValue;
            hdnIsAllowPreviewTariff.Value = lstSettingParameterDt.Where(a => a.ParameterCode == Constant.SettingParameter.EM_IS_DOCTOR_FEE_ALLOW_PREVIEW_TARIFF).FirstOrDefault().ParameterValue;

            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            
            if (AppSession.EM0051 == "1")
                ddlViewType.SelectedValue = "0";
            else
                ddlViewType.SelectedValue = "1";

            BindGridView(1, true, ref PageCount);
            BindGridViewDt(1, true, ref PageCount);            
        }

        #region Patient Charges Hd
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            
            string code = ddlViewType.SelectedValue;

            if (code == "0")
                filterExpression += string.Format("VisitID = {0} AND ParamedicID = {1}",
                  AppSession.RegisteredPatient.VisitID, AppSession.UserLogin.ParamedicID);
            if (code == "1")
                filterExpression += string.Format("VisitID = {0} AND ParamedicID = {1} AND IsEntryByPhysician = 1",
                    AppSession.RegisteredPatient.VisitID, AppSession.UserLogin.ParamedicID);
            else if (code == "2")
                filterExpression += string.Format("VisitID = {0} AND ParamedicID = {1} AND IsEntryByPhysician = 0",
                    AppSession.RegisteredPatient.VisitID, AppSession.UserLogin.ParamedicID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientChargesHd8RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientChargesHd8> lstEntity = BusinessLayer.GetvPatientChargesHd8List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "TransactionID DESC");
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
        #endregion

        #region Patient Charges Dt
        private void BindGridViewDt(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnTransactionID.Value != "")
            {
                filterExpression = string.Format("TransactionID = {0} AND GCItemType NOT IN ('{1}','{2}','{3}') AND IsDeleted = 0", hdnTransactionID.Value, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvPatientChargesDtRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }
            }
            List<vPatientChargesDt> lstEntity = BusinessLayer.GetvPatientChargesDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID ASC");
            grdViewDt.DataSource = lstEntity;
            grdViewDt.DataBind();
        }
        protected void cbpViewDt_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowEdit = false;
            IsAllowDelete = false;
        }
        protected override bool OnBeforeAddRecord(ref string errMessage)
        {
            bool result = true;
            if (hdnIsValidateBeforeEntry.Value == "1")
            {
                try
                {
                    if (!IsValidToEntry())
                    {
                        errMessage = "You should entry Patient Chief Complaint, Diagnosis or SOAP Note before entry transaction to this patient";
                        result = false;
                    }
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    result = false;
                } 
            }
            return result;
        }

        protected override bool OnBeforeDeleteRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                Registration oRegistration = BusinessLayer.GetRegistration(AppSession.RegisteredPatient.RegistrationID);
                if (oRegistration != null)
                {
                    PatientChargesHd entity = BusinessLayer.GetPatientChargesHd(Convert.ToInt32(hdnTransactionID.Value));
                    if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
                    {
                        errMessage = "Cannot delete transaction because has already been approved.";
                        result = false;
                    }
                    else
                    {
                        if (oRegistration.IsLockDown)
                        {
                            errMessage = "Transaction is currently being locked for Patient Billing Process.";
                            result = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }
            return result;
        }

        protected override bool OnBeforeEditRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                Registration oRegistration = BusinessLayer.GetRegistration(AppSession.RegisteredPatient.RegistrationID);
                if (oRegistration != null)
                {
                    PatientChargesHd entity = BusinessLayer.GetPatientChargesHd(Convert.ToInt32(hdnTransactionID.Value));
                    if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
                    {
                        errMessage = "Cannot edit transaction because has already been approved.";
                        result = false;
                    }
                    else
                    {
                        if (oRegistration.IsLockDown)
                        {
                            errMessage = "Transaction is currently being locked for Patient Billing Process.";
                            result = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }
            return result;
        }

        protected override bool OnAddRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            bool result = true;
            Registration oRegistration = BusinessLayer.GetRegistration(AppSession.RegisteredPatient.RegistrationID);
            if (oRegistration != null)
            {
                if (oRegistration.GCRegistrationStatus == Constant.VisitStatus.CLOSED)
                {
                    errMessage = "Registration has already been closed.";
                    result = false;
                }

                if (oRegistration.IsLockDown != true)
                {
                    url = ResolveUrl("~/Program/PatientPage/PatientTransaction/ChargesEntry/ChargesEntryQuickPicksCtl1.ascx");
                    queryString = string.Format("{0}|{1}|{2}|{3}|{4}|{5}",
                    "0", AppSession.RegisteredPatient.HealthcareServiceUnitID, AppSession.UserLogin.ParamedicID, AppSession.RegisteredPatient.RegistrationID, AppSession.RegisteredPatient.VisitID, AppSession.RegisteredPatient.ChargeClassID);
                    popupWidth = 1200;
                    popupHeight = 600;
                    popupHeaderText = "Doctor Fee";
                    result = true;
                }
                else
                {
                    errMessage = "Transaction is currently being locked for Patient Billing Process.";
                    result = false;
                }
            }
            return result;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            bool result = true;
            errMessage = "This function currently is not available yet.";
            result = false;
            //Registration oRegistration = BusinessLayer.GetRegistration(AppSession.RegisteredPatient.RegistrationID);
            //if (oRegistration != null)
            //{
            //    if (oRegistration.IsLockDown != true)
            //    {
            //        url = ResolveUrl("~/Program/PatientPage/PatientTransaction/PatientCharges/ChargesEditCtl.ascx");
            //        queryString = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}",
            //        "0", AppSession.RegisteredPatient.HealthcareServiceUnitID, AppSession.UserLogin.ParamedicID, AppSession.RegisteredPatient.RegistrationID, AppSession.RegisteredPatient.VisitID, AppSession.RegisteredPatient.ChargeClassID,hdnTransactionID.Value);
            //        popupWidth = 600;
            //        popupHeight = 400;
            //        popupHeaderText = "Patient Transaction - Charges";
            //        result = true;
            //    }
            //    else
            //    {
            //        errMessage = "Transaction is currently being locked for Patient Billing Process.";
            //        result = false;
            //    }
            //}
            return result;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnTransactionID.Value != "")
            {
                bool result = true;
                int id = Convert.ToInt32(hdnTransactionID.Value);
                IDbContext ctx = DbFactory.Configure(true);
                PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
                PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
                try
                {
                    PatientChargesHd entityHd = BusinessLayer.GetPatientChargesHdList(string.Format("TransactionID = {0}", id), ctx).FirstOrDefault();
                    if (entityHd != null)
                    {
                        List<PatientChargesDt> lstChargesDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = {0} AND IsDeleted=0", id), ctx);
                        foreach (PatientChargesDt entityDt in lstChargesDt)
                        {
                            if (entityDt != null)
                            {
                                entityDt.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                                entityDt.IsApproved = false;
                                entityDt.IsDeleted = true;
                                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(entityDt);
                            }
                        }

                        entityHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                        entityHd.VoidBy = AppSession.UserLogin.UserID;
                        entityHd.VoidDate = DateTime.Now;
                        entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityHdDao.Update(entityHd);

                        ctx.CommitTransaction();
                    }
                }
                catch (Exception ex)
                {
                    result = false;
                    errMessage = ex.Message;
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }
                return result;
            }
            return false;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            string[] param = type.Split(';');
            string gcDeleteReason = param[1];
            string reason = param[2];

            if (hdnTransactionID.Value != "")
            {
                if (param[0] == "addDetail")
                {
                    string url = ResolveUrl("~/Program/PatientPage/PatientTransaction/ChargesEntry/ChargesEntryQuickPicksCtl1.ascx");
                    string queryString = string.Format("{0}|{1}|{2}|{3}|{4}|{5}",
                    hdnTransactionID.Value, AppSession.RegisteredPatient.HealthcareServiceUnitID, AppSession.UserLogin.ParamedicID, AppSession.RegisteredPatient.RegistrationID, AppSession.RegisteredPatient.VisitID, AppSession.RegisteredPatient.ChargeClassID);
                    int popupWidth = 1200;
                    int popupHeight = 600;
                    string popupHeaderText = "Doctor Fee";
                    string message = string.Empty;
                    OnAddRecord(ref url, ref message, ref queryString, ref popupWidth, ref popupHeight, ref popupHeaderText);
                }
                else
                {
                    IDbContext ctx = DbFactory.Configure(true);
                    PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
                    PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
                    try
                    {
                        PatientChargesHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            if (param[0] == "void")
                            {
                                entityHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                entityHd.GCVoidReason = gcDeleteReason;
                                if (gcDeleteReason == Constant.DeleteReason.OTHER)
                                {
                                    entityHd.VoidReason = reason;
                                }
                                entityHd.VoidBy = AppSession.UserLogin.UserID;
                                entityHd.VoidDate = DateTime.Now;
                                entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityHdDao.Update(entityHd);

                                List<PatientChargesDt> lstEntityDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = {0} AND IsDeleted = 0", entityHd.TransactionID), ctx);
                                foreach (PatientChargesDt entityDt in lstEntityDt)
                                {
                                    entityDt.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                                    entityDt.IsApproved = false;
                                    entityDt.IsDeleted = true;
                                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityDtDao.Update(entityDt);
                                }
                            }
                            else if (param[0] == "propose")
                            {
                                entityHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                entityHd.ProposedBy = AppSession.UserLogin.UserID;
                                entityHd.ProposedDate = DateTime.Now;
                                entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityHdDao.Update(entityHd);

                                List<PatientChargesDt> lstEntityDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = {0} AND IsDeleted = 0", entityHd.TransactionID), ctx);
                                foreach (PatientChargesDt entityDt in lstEntityDt)
                                {
                                    entityDt.GCTransactionDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityDtDao.Update(entityDt);
                                }
                            }
                            else if (param[0] == "deleteDetail")
                            {
                                PatientChargesDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnDetailID.Value));

                                if (entityDt != null)
                                {
                                    entityDt.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                                    entityDt.IsDeleted = true;
                                    entityDt.DeleteBy = AppSession.UserLogin.UserID;
                                    entityDt.DeleteDate = DateTime.Now;
                                    entityDt.GCDeleteReason = gcDeleteReason;
                                    if (gcDeleteReason == Constant.DeleteReason.OTHER)
                                    {
                                        entityDt.DeleteReason = reason;
                                    }
                                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityDtDao.Update(entityDt);

                                    //Void Header if no detail items remaining
                                    List<PatientChargesDt> lstEntityDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = {0} AND IsDeleted = 0", entityHd.TransactionID), ctx);
                                    if (lstEntityDt.Count == 0)
                                    {
                                        entityHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                        entityHd.GCVoidReason = gcDeleteReason;
                                        if (gcDeleteReason == Constant.DeleteReason.OTHER)
                                        {
                                            entityHd.VoidReason = reason;
                                        }
                                        entityHd.VoidBy = AppSession.UserLogin.UserID;
                                        entityHd.VoidDate = DateTime.Now;
                                        entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        entityHdDao.Update(entityHd);
                                    }
                                }
                            }

                            ctx.CommitTransaction();
                        }
                        else
                        {
                            result = false;
                            errMessage = "Cannot edit transaction because has already been approved.";
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                    }
                    catch (Exception ex)
                    {
                        result = false;
                        errMessage = ex.Message;
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                    finally
                    {
                        ctx.Close();
                    }
                    return result;
                }
            }
            return result;
        }

        private bool IsValidToEntry()
        {
            bool isValid = true;
            if (AppSession.RegisteredPatient.DepartmentID != Constant.Facility.INPATIENT)
            {
                vChiefComplaint oChiefComplaint = BusinessLayer.GetvChiefComplaintList(string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
                bool isChiefComplaintExist = oChiefComplaint != null;

                vPatientDiagnosis oDiagnosis = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
                bool isDiagnosisExist = oDiagnosis != null;

                isValid = isChiefComplaintExist && isDiagnosisExist;
            }
            else
            {
                string filterExp = string.Format("VisitID = {0} AND ParamedicID = {1} AND NoteDate = '{2}'", AppSession.RegisteredPatient.VisitID, AppSession.UserLogin.ParamedicID,DateTime.Now.Date.ToString(Constant.FormatString.DATE_FORMAT_112));
                int rowCount = BusinessLayer.GetvPatientVisitNoteRowCount(filterExp);

                isValid = rowCount > 0;
            }

            return isValid;
        }

        protected string GetQuickPicksParam()
        {
            string queryString = string.Format("{0}|{1}|{2}|{3}|{4}",
                AppSession.RegisteredPatient.HealthcareServiceUnitID, AppSession.UserLogin.ParamedicID, AppSession.RegisteredPatient.RegistrationID, AppSession.RegisteredPatient.VisitID, AppSession.RegisteredPatient.ChargeClassID);

            return queryString;
        }
    }
}