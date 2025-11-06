using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientUseDetailDrugMSReturnCtl : BaseUserControlCtl
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

        public void SetControlProperties()
        {
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
            IsEditable = (GCTransactionStatus == "" || GCTransactionStatus == Constant.TransactionStatus.OPEN);
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
           
            if (param[0] == "approve")
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
                    PatientChargesHd entityHd = entityHdDao.Get(transactionID);
                    if (DetailPage.IsPatientBillSummaryPage() && AppSession.IsAdminCanCancelAllTransaction)
                    {
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || entityHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                        {
                            PatientChargesDt entity = entityDtDao.Get(ID);
                            if (!entity.IsDeleted && !entity.IsApproved)
                            {
                                entity.IsApproved = true;
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(entity);
                            }
                        }
                        else
                        {
                            errMessage = "Transaksi Sudah Diproses. Tidak Bisa Diubah";
                            result = false;
                        }
                    }
                    else
                    {
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            PatientChargesDt entity = entityDtDao.Get(ID);
                            if (!entity.IsDeleted && !entity.IsApproved)
                            {
                                entity.IsApproved = true;
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(entity);
                            }
                        }
                        else
                        {
                            errMessage = "Transaksi Sudah Diproses. Tidak Bisa Diubah";
                            result = false;
                        }
                    }
                }
                ctx.CommitTransaction();
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
                    if (DetailPage.IsPatientBillSummaryPage() && AppSession.IsAdminCanCancelAllTransaction)
                    {
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || entityHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                        {
                            PatientChargesDt entity = entityDtDao.Get(ID);
                            if (!entity.IsDeleted && entity.IsApproved)
                            {
                                entity.IsApproved = false;
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(entity);
                            }
                        }
                        else
                        {
                            errMessage = "Transaksi Sudah Diproses. Tidak Bisa Diubah";
                            result = false;
                        }
                    }
                    else
                    {
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            PatientChargesDt entity = entityDtDao.Get(ID);
                            if (!entity.IsDeleted && entity.IsApproved)
                            {
                                entity.IsApproved = false;
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(entity);
                            }
                        }
                        else
                        {
                            errMessage = "Transaksi Sudah Diproses. Tidak Bisa Diubah";
                            result = false;
                        }
                    }
                }
                ctx.CommitTransaction();
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

        private bool OnDeleteChargesDt(ref string errMessage,string param)
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
                    PatientChargesHd entityHd = entityHdDao.Get(transactionID);
                    if (DetailPage.IsPatientBillSummaryPage() && AppSession.IsAdminCanCancelAllTransaction)
                    {
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || entityHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                        {
                            PatientChargesDt entity = entityDtDao.Get(ID);
                            if (!entity.IsDeleted && !entity.IsApproved)
                            {
                                entity.IsDeleted = true;
                                entity.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                                entity.GCDeleteReason = gcDeleteReason;
                                entity.DeleteReason = reason;
                                entity.DeleteDate = DateTime.Now;
                                entity.DeleteBy = AppSession.UserLogin.UserID;
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(entity);
                            }
                        }
                        else
                        {
                            errMessage = "Transaksi Sudah Diproses. Tidak Bisa Diubah";
                            result = false;
                        }
                    }
                    else
                    {
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            PatientChargesDt entity = entityDtDao.Get(ID);
                            if (!entity.IsDeleted && !entity.IsApproved)
                            {
                                entity.IsDeleted = true;
                                entity.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                                entity.GCDeleteReason = gcDeleteReason;
                                entity.DeleteReason = reason;
                                entity.DeleteDate = DateTime.Now;
                                entity.DeleteBy = AppSession.UserLogin.UserID;
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(entity);
                            }
                        }
                        else
                        {
                            errMessage = "Transaksi Sudah Diproses. Tidak Bisa Diubah";
                            result = false;
                        }
                    }
                }
                ctx.CommitTransaction();
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

        public void OnVoidAllChargesDt(IDbContext ctx, int transactionHdID)
        {
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            PatientChargesHd entityHd = entityHdDao.Get(transactionHdID);
            if (DetailPage.IsPatientBillSummaryPage() && AppSession.IsAdminCanCancelAllTransaction)
            {
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || entityHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                {
                    List<vPatientChargesDt> lstPatientChargesDt = BusinessLayer.GetvPatientChargesDtList(string.Format("TransactionID = {0} AND GCItemType = '{1}' AND IsDeleted = 0", transactionHdID, Constant.ItemGroupMaster.DRUGS), ctx);
                    foreach (vPatientChargesDt patientChargesDt in lstPatientChargesDt)
                    {
                        PatientChargesDt entity = entityDtDao.Get(patientChargesDt.ID);
                        if (entity.IsApproved)
                        {
                            entity.IsApproved = false;
                            entity.IsDeleted = true;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entity.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                            entityDtDao.Update(entity);
                        }
                    }
                }
            }
            else
            {
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    List<vPatientChargesDt> lstPatientChargesDt = BusinessLayer.GetvPatientChargesDtList(string.Format("TransactionID = {0} AND GCItemType = '{1}' AND IsDeleted = 0", transactionHdID, Constant.ItemGroupMaster.DRUGS), ctx);
                    foreach (vPatientChargesDt patientChargesDt in lstPatientChargesDt)
                    {
                        PatientChargesDt entity = entityDtDao.Get(patientChargesDt.ID);
                        if (entity.IsApproved)
                        {
                            entity.IsApproved = false;
                            entity.IsDeleted = true;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entity.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                            entityDtDao.Update(entity);
                        }
                    }
                }
            }
        }
        #endregion
    }
}