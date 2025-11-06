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
    public partial class PatientChargesList : BasePagePatientPageList
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.PATIENT_CHARGES;
        }

        public override bool IsEntryUsePopup()
        {
            return true;
        }

        protected override void InitializeDataControl()
        {
            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.OUTPATIENT)
            {
                List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.OP_VALIDATE_DX_ON_TRANSACTION));
                hdnIsValidateBeforeEntry.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.OP_VALIDATE_DX_ON_TRANSACTION).ParameterValue;
            }
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

            if (code == "1")
                filterExpression += string.Format("VisitID = {0} AND GCTransactionStatus != '{1}' AND TransactionID IN (SELECT TransactionID FROM vPatientChargesDt WHERE ParamedicID = {2} AND GCItemType NOT IN ('{4}','{5}','{6}')) AND DepartmentID NOT IN ('{3}') AND IsEntryByPhysician = 1",
                    AppSession.RegisteredPatient.VisitID, Constant.TransactionStatus.VOID, AppSession.UserLogin.ParamedicID,Constant.Facility.PHARMACY,Constant.ItemType.OBAT_OBATAN,Constant.ItemType.BARANG_MEDIS,Constant.ItemType.BARANG_UMUM);
            else if (code == "2")
                filterExpression += string.Format("VisitID = {0} AND GCTransactionStatus != '{1}' AND TransactionID IN (SELECT TransactionID FROM vPatientChargesDt WHERE ParamedicID = {2} AND GCItemType NOT IN ('{4}','{5}','{6}')) AND DepartmentID NOT IN ('{3}') AND IsEntryByPhysician = 0",
                    AppSession.RegisteredPatient.VisitID, Constant.TransactionStatus.VOID, AppSession.UserLogin.ParamedicID, Constant.Facility.PHARMACY, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM);
            else
                filterExpression += string.Format("VisitID = {0} AND GCTransactionStatus != '{1}' AND TransactionID IN (SELECT TransactionID FROM vPatientChargesDt WHERE ParamedicID = {2} AND GCItemType NOT IN ('{4}','{5}','{6}')) AND DepartmentID NOT IN ('{3}')",
                    AppSession.RegisteredPatient.VisitID, Constant.TransactionStatus.VOID, AppSession.UserLogin.ParamedicID, Constant.Facility.PHARMACY, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientChargesHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientChargesHd> lstEntity = BusinessLayer.GetvPatientChargesHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "TransactionDate DESC");
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
            if (hdnID.Value != "")
            {
                filterExpression = string.Format("TransactionID = {0} AND GCItemType NOT IN ('{1}','{2}','{3}') AND IsDeleted = 0", hdnID.Value, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM);

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

        protected override bool OnBeforeAddRecord(ref string errMessage)
        {
            bool result = true;
            if (hdnIsValidateBeforeEntry.Value == "1")
            {
                try
                {
                    if (!IsValidToEntry())
                    {
                        errMessage = "You should entry Patient Chief Complaint and Diagnosis before entry transaction to this patient";
                        result = false;
                    }
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    Helper.InsertErrorLog(ex);
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
                    if (oRegistration.IsLockDown != true)
                    {
                        PatientChargesHd entity = BusinessLayer.GetPatientChargesHd(Convert.ToInt32(hdnID.Value));
                        if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
                        {
                            errMessage = "Cannot delete transaction because has already been approved.";
                            result = false;
                        }
                    }
                    else
                    {
                        errMessage = "Transaction is currently being locked for Patient Billing Process.";
                        result = false;
                    }
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
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
                    if (oRegistration.IsLockDown != true)
                    {
                        PatientChargesHd entity = BusinessLayer.GetPatientChargesHd(Convert.ToInt32(hdnID.Value));
                        if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
                        {
                            errMessage = "Cannot edit transaction because has already been approved.";
                            result = false;
                        }
                    }
                    else
                    {
                        errMessage = "Transaction is currently being locked for Patient Billing Process.";
                        result = false;
                    }
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
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
                    url = ResolveUrl("~/Program/PatientPage/PatientTransaction/PatientCharges/ChargesEntryQuickPicksCtl.ascx");
                    queryString = string.Format("{0}|{1}|{2}|{3}|{4}|{5}",
                    "0", AppSession.RegisteredPatient.HealthcareServiceUnitID, AppSession.UserLogin.ParamedicID, AppSession.RegisteredPatient.RegistrationID, AppSession.RegisteredPatient.VisitID, AppSession.RegisteredPatient.ChargeClassID);
                    popupWidth = 1000;
                    popupHeight = 600;
                    popupHeaderText = "Patient Transaction - Charges";
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
            //        "0", AppSession.RegisteredPatient.HealthcareServiceUnitID, AppSession.UserLogin.ParamedicID, AppSession.RegisteredPatient.RegistrationID, AppSession.RegisteredPatient.VisitID, AppSession.RegisteredPatient.ChargeClassID,hdnID.Value);
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
            if (hdnID.Value != "")
            {
                bool result = true;
                int id = Convert.ToInt32(hdnID.Value);
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
                        entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityHdDao.Update(entityHd);
                    }

                    ctx.CommitTransaction();
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
            if (type == "Void")
            {
                if (hdnID.Value != "")
                {
                    IDbContext ctx = DbFactory.Configure(true);
                    PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
                    PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
                    try
                    {
                        PatientChargesHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnID.Value));
                        entityHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                        entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityHdDao.Update(entityHd);

                        List<PatientChargesDt> lstEntityDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = {0} AND IsDeleted = 0", entityHd.TransactionID), ctx);
                        foreach (PatientChargesDt entityDt in lstEntityDt)
                        {
                            entityDt.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                            entityDt.IsDeleted = true;
                            entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entityDt);
                        }

                        ctx.CommitTransaction();
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
                return result;
            }
            return false;
        }

        private bool IsValidToEntry()
        {
            vChiefComplaint oChiefComplaint = BusinessLayer.GetvChiefComplaintList(string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            bool isChiefComplaintExist = oChiefComplaint != null;

            vPatientDiagnosis oDiagnosis = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            bool isDiagnosisExist = oDiagnosis != null;

            return isChiefComplaintExist && isDiagnosisExist;
        }
    }
}