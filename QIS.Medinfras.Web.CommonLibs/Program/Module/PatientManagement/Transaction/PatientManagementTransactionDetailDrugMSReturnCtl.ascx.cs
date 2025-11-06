using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientManagementTransactionDetailDrugMSReturnCtl : BaseUserControlCtl
    {
        protected bool IsShowSwitchIcon = false;
        private BasePageTrxPatientManagement DetailPage
        {
            get { return (BasePageTrxPatientManagement)Page; }
        }
        public void InitializeTransactionControl(bool flagHaveCharges)
        {
            if (flagHaveCharges)
            {
                BindGridDrugMSReturn();
            }
        }

        public void OnAddRecord()
        {
            IsEditable = true;
            BindGridDrugMSReturn();
            IsEditable = true;
            hdnIsEditable.Value = "1";
        }

        protected bool IsEditable = true;
        private void BindGridDrugMSReturn()
        {
            string GCTransactionStatus = DetailPage.GetGCTransactionStatus();
            Registration entity = BusinessLayer.GetRegistration(Convert.ToInt32(AppSession.RegisteredPatient.RegistrationID));
            if (DetailPage.IsPatientBillSummaryPage() && AppSession.IsAdminCanCancelAllTransaction)
            {
                IsEditable = (GCTransactionStatus == "" || GCTransactionStatus == Constant.TransactionStatus.OPEN || GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL);
            }
            else
            {
                IsEditable = (GCTransactionStatus == "" || GCTransactionStatus == Constant.TransactionStatus.OPEN);
            }
            //IsEditable = (GCTransactionStatus == "" || GCTransactionStatus == Constant.TransactionStatus.OPEN);
            hdnIsEditable.Value = IsEditable && !entity.IsLockDown ? "1" : "0";
            IsEditable = entity.IsLockDown ? false : IsEditable;

            string filterExpression = "1 = 0";
            string transactionID = DetailPage.GetTransactionHdID();
            if (transactionID != "")
                filterExpression = string.Format("TransactionID = {0} AND UsedQuantity < 0 AND GCItemType IN ('{1}','{2}') AND IsDeleted = 0", transactionID, Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.SUPPLIES);
            List<vPatientChargesDt1> lst = BusinessLayer.GetvPatientChargesDt1List(filterExpression);
            IsShowSwitchIcon = DetailPage.GetGCCustomerType() != Constant.CustomerType.PERSONAL;
            lvwDrugMSReturn.DataSource = lst;
            lvwDrugMSReturn.DataBind();

            decimal totalPatientAmount = lst.Sum(p => p.PatientAmount);
            decimal totalPayerAmount = lst.Sum(p => p.PayerAmount);
            decimal totalLineAmount = lst.Sum(p => p.LineAmount);
            if (lst.Count > 0)
            {
                ((HtmlTableCell)lvwDrugMSReturn.FindControl("tdDrugMSReturnTotalPayer")).InnerHtml = totalPayerAmount.ToString("N");
                ((HtmlTableCell)lvwDrugMSReturn.FindControl("tdDrugMSReturnTotalPatient")).InnerHtml = totalPatientAmount.ToString("N");
                ((HtmlTableCell)lvwDrugMSReturn.FindControl("tdDrugMSReturnTotal")).InnerHtml = totalLineAmount.ToString("N");
            }
            hdnDrugMSReturnAllTotalPatient.Value = totalPatientAmount.ToString();
            hdnDrugMSReturnAllTotalPayer.Value = totalPayerAmount.ToString();

            Helper.SetControlEntrySetting(txtDrugMSReturnPatient, new ControlEntrySetting(true, true, true), "mpTrxDrugMSReturn");
            Helper.SetControlEntrySetting(txtDrugMSReturnPayer, new ControlEntrySetting(true, true, true), "mpTrxDrugMSReturn");

            PatientChargesHd entityHd = BusinessLayer.GetPatientChargesHdList(string.Format("TransactionID = {0}", Convert.ToInt32(DetailPage.GetTransactionHdID()))).FirstOrDefault();
            if (entityHd != null)
            {
                if (entityHd.IsAIOTransaction)
                {
                    hdnIsAIOTransactionDrugReturnCtl.Value = "1";
                }
                else
                {
                    hdnIsAIOTransactionDrugReturnCtl.Value = "0";
                }

                if (entityHd.ConsultVisitItemPackageID != null && entityHd.ConsultVisitItemPackageID != 0)
                {
                    hdnIsChargesGenerateMCUDrugReturnCtl.Value = "1";
                }
                else
                {
                    hdnIsChargesGenerateMCUDrugReturnCtl.Value = "0";
                }
            }
            else
            {
                hdnIsAIOTransactionDrugReturnCtl.Value = "0";
                hdnIsChargesGenerateMCUDrugReturnCtl.Value = "0";
            }

        }

        #region Drug MS
        protected void cbpDrugMSReturn_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string result = "";
            string errMessage = "";
            int transactionID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (hdnDrugMSReturnTransactionDtID.Value.ToString() != "")
                {
                    transactionID = Convert.ToInt32(DetailPage.GetTransactionHdID());
                    if (OnSaveEditRecordDrugMSReturn(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "approve")
            {
                int ID = Convert.ToInt32(param[1]);
                if (OnApproveChargesDt(ref errMessage, ID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "void")
            {
                int ID = Convert.ToInt32(param[1]);
                if (OnVoidChargesDt(ref errMessage, ID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "delete")
            {
                if (OnDeleteChargesDt(ref errMessage, param[1]))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "switch")
            {
                int ID = Convert.ToInt32(param[1]);
                if (OnSwitchChargesDt(ref errMessage, ID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            BindGridDrugMSReturn();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = transactionID.ToString();
        }

        private bool OnApproveChargesDt(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            try
            {
                int transactionID = Convert.ToInt32(DetailPage.GetTransactionHdID());
                if (transactionID > 0)
                {
                    bool isAllowSaveDt = false;
                    PatientChargesHd entityHd = entityHdDao.Get(transactionID);
                    if (DetailPage.IsPatientBillSummaryPage() && AppSession.IsAdminCanCancelAllTransaction)
                    {
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || entityHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                        {
                            isAllowSaveDt = true;
                        }
                        else
                        {
                            isAllowSaveDt = false;
                            errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat diubah lagi.", entityHd.TransactionNo);
                            result = false;
                        }
                    }
                    else
                    {
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            isAllowSaveDt = true;
                        }
                        else
                        {
                            isAllowSaveDt = false;
                            errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat diubah lagi.", entityHd.TransactionNo);
                            result = false;
                        }
                    }

                    if (isAllowSaveDt)
                    {
                        PatientChargesDt entity = entityDtDao.Get(ID);
                        if (!entity.IsDeleted && !entity.IsApproved)
                        {
                            entity.IsApproved = true;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entity);
                        }
                    }
                }

                if (result)
                {
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
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

        private bool OnSwitchChargesDt(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            try
            {
                int transactionID = Convert.ToInt32(DetailPage.GetTransactionHdID());
                if (transactionID > 0)
                {
                    bool isAllowSaveDt = false;
                    PatientChargesHd entityHd = entityHdDao.Get(transactionID);
                    if (DetailPage.IsPatientBillSummaryPage() && AppSession.IsAdminCanCancelAllTransaction)
                    {
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || entityHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                        {
                            isAllowSaveDt = true;
                        }
                        else
                        {
                            isAllowSaveDt = false;
                            errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat diubah lagi.", entityHd.TransactionNo);
                            result = false;
                        }
                    }
                    else
                    {
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            isAllowSaveDt = true;
                        }
                        else
                        {
                            isAllowSaveDt = false;
                            errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat diubah lagi.", entityHd.TransactionNo);
                            result = false;
                        }
                    }

                    if (isAllowSaveDt)
                    {
                        PatientChargesDt entity = entityDtDao.Get(ID);
                        if (!entity.IsDeleted && entity.IsApproved)
                        {
                            entity.IsApproved = false;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entity);
                        }
                    }
                }

                if (result)
                {
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
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

        private bool OnVoidChargesDt(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            try
            {
                int transactionID = Convert.ToInt32(DetailPage.GetTransactionHdID());
                if (transactionID > 0)
                {
                    PatientChargesHd entityHd = entityHdDao.Get(transactionID);
                    bool isAllowSaveDt = false;
                    if (DetailPage.IsPatientBillSummaryPage() && AppSession.IsAdminCanCancelAllTransaction)
                    {
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || entityHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                        {
                            isAllowSaveDt = true;
                        }
                        else
                        {
                            isAllowSaveDt = false;
                            errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat diubah lagi.", entityHd.TransactionNo);
                            result = false;
                        }
                    }
                    else
                    {
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            isAllowSaveDt = true;
                        }
                        else
                        {
                            isAllowSaveDt = false;
                            errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat diubah lagi.", entityHd.TransactionNo);
                            result = false;
                        }
                    }

                    if (isAllowSaveDt)
                    {
                        PatientChargesDt entity = entityDtDao.Get(ID);
                        if (!entity.IsDeleted && entity.IsApproved)
                        {
                            entity.IsApproved = false;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entity);
                        }
                    }
                }

                if (result)
                {
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
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

        private bool OnDeleteChargesDt(ref string errMessage, string param)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            string[] paramDelete = param.Split(';');
            int ID = Convert.ToInt32(paramDelete[0]);
            string gcDeleteReason = paramDelete[1];
            string reason = paramDelete[2];
            try
            {
                int transactionID = Convert.ToInt32(DetailPage.GetTransactionHdID());
                if (transactionID > 0)
                {
                    bool isAllowSaveDt = false;
                    PatientChargesHd entityHd = entityHdDao.Get(transactionID);
                    if (DetailPage.IsPatientBillSummaryPage() && AppSession.IsAdminCanCancelAllTransaction)
                    {
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || entityHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                        {
                            isAllowSaveDt = true;
                        }
                        else
                        {
                            isAllowSaveDt = false;
                            errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat dihapus lagi.", entityHd.TransactionNo);
                            result = false;
                        }
                    }
                    else
                    {
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            isAllowSaveDt = true;
                        }
                        else
                        {
                            isAllowSaveDt = false;
                            errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat dihapus lagi.", entityHd.TransactionNo);
                            result = false;
                        }
                    }

                    if (isAllowSaveDt)
                    {
                        PatientChargesDt entity = entityDtDao.Get(ID);
                        if (!entity.IsDeleted && !entity.IsApproved)
                        {
                            entity.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                            entity.IsDeleted = true;
                            entity.GCDeleteReason = gcDeleteReason;
                            entity.DeleteReason = reason;
                            entity.DeleteDate = DateTime.Now;
                            entity.DeleteBy = AppSession.UserLogin.UserID;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entity);
                        }
                    }
                }

                if (result)
                {
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
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

        public void OnVoidAllChargesDt(IDbContext ctx, int transactionHdID)
        {
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            PatientChargesHd entityHd = entityHdDao.Get(transactionHdID);
            bool isAllowSaveDt = false;
            if (DetailPage.IsPatientBillSummaryPage() && AppSession.IsAdminCanCancelAllTransaction)
            {
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || entityHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                {
                    isAllowSaveDt = true;
                }
            }
            else
            {
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    isAllowSaveDt = false;
                }
            }

            if (isAllowSaveDt)
            {
                List<vPatientChargesDt1> lstPatientChargesDt = BusinessLayer.GetvPatientChargesDt1List(string.Format("TransactionID = {0} AND GCItemType IN ('{1}','{2}') AND IsDeleted = 0", transactionHdID, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS), ctx);
                foreach (vPatientChargesDt1 patientChargesDt in lstPatientChargesDt)
                {
                    PatientChargesDt entity = entityDtDao.Get(patientChargesDt.ID);
                    if (!entity.IsDeleted)
                    {
                        entity.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                        entity.IsApproved = false;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityDtDao.Update(entity);
                    }
                }
            }
        }

        private bool OnSaveEditRecordDrugMSReturn(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            try
            {
                int transactionID = Convert.ToInt32(DetailPage.GetTransactionHdID());
                if (transactionID > 0)
                {
                    PatientChargesHd entityHd = entityHdDao.Get(transactionID);
                    bool isAllowSaveDt = false;
                    if (DetailPage.IsPatientBillSummaryPage() && AppSession.IsAdminCanCancelAllTransaction)
                    {
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || entityHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                        {
                            isAllowSaveDt = true;
                        }
                        else
                        {
                            isAllowSaveDt = false;
                            errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat diubah lagi.", entityHd.TransactionNo);
                            result = false;
                        }
                    }
                    else
                    {
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            isAllowSaveDt = true;
                        }
                        else
                        {
                            isAllowSaveDt = false;
                            errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat diubah lagi.", entityHd.TransactionNo);
                            result = false;
                        }
                    }

                    if (isAllowSaveDt)
                    {
                        PatientChargesDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnDrugMSReturnTransactionDtID.Value));
                        if (!entityDt.IsDeleted && !entityDt.IsApproved)
                        {
                            entityDt.PatientAmount = Convert.ToDecimal(Request.Form[txtDrugMSReturnPatient.UniqueID]);
                            entityDt.PayerAmount = Convert.ToDecimal(Request.Form[txtDrugMSReturnPayer.UniqueID]);
                            entityDt.IsApproved = true;
                            entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entityDt);
                        }
                    }
                }

                if (result)
                {
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
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
        #endregion
    }
}