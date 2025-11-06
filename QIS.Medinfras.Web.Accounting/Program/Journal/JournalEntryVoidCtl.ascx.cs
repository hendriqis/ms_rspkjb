using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class JournalEntryVoidCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;
            List<StandardCode> lstVoidReason = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.DELETE_REASON));
            Methods.SetComboBoxField<StandardCode>(cboVoidReason, lstVoidReason, "StandardCodeName", "StandardCodeID");
            cboVoidReason.SelectedIndex = 0;

            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                "HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                                                                AppSession.UserLogin.HealthcareID,
                                                                Constant.SettingParameter.ARRECEIVING_FROM_TREASURY_CAN_DELETE_OR_VOID //1
                                                            )
                                                        );
            hdnIsARReceivingCanDeleteOrVoid.Value = lstSettingParameterDt.Find(a => a.ParameterCode == Constant.SettingParameter.ARRECEIVING_FROM_TREASURY_CAN_DELETE_OR_VOID).ParameterValue;

        }

        protected void cbpJournalVoid_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            if (OnVoidRecord(ref errMessage))
                result = "success";
            else
                result = "fail|" + errMessage;

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnVoidRecord(ref string errMessage)
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            GLTransactionHdDao glTransactionHdDao = new GLTransactionHdDao(ctx);
            GLTransactionDtDao glTransactionDtDao = new GLTransactionDtDao(ctx);
            SupplierPaymentHdDao supPaymentHdDao = new SupplierPaymentHdDao(ctx);
            SupplierPaymentDtDao supPaymentDtDao = new SupplierPaymentDtDao(ctx);
            ARReceivingHdDao arReceivingHdDao = new ARReceivingHdDao(ctx);
            ARInvoiceReceivingDao entityARRcvDao = new ARInvoiceReceivingDao(ctx);
            ARInvoiceHdDao eInvoiceHd = new ARInvoiceHdDao(ctx);
            ARInvoiceDtDao eInvoiceDt = new ARInvoiceDtDao(ctx);
            TransRevenueSharingPaymentHdDao rspaymentHdDao = new TransRevenueSharingPaymentHdDao(ctx);
            TransRevenueSharingPaymentDtDao rspaymentDtDao = new TransRevenueSharingPaymentDtDao(ctx);
            DirectPurchaseHdDao directPurchaseHdDao = new DirectPurchaseHdDao(ctx);

            try
            {
                GLTransactionHd entityHD = glTransactionHdDao.Get(Convert.ToInt32(hdnParam.Value));
                if (entityHD.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    if (entityHD.GCTreasuryGroup == Constant.TreasuryGroup.AR_RECEIVING && hdnIsARReceivingCanDeleteOrVoid.Value == "1")
                    {
                        errMessage = "GAGAL VOID : Voucher " + entityHD.JournalNo + " dari penerimaan piutang tidak diperbolehkan void / delete (Setting Parameter : AC0010)";
                        result = false;
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                    else
                    {
                        entityHD.GCVoidReason = cboVoidReason.Value.ToString();
                        if (cboVoidReason.Value.ToString() == Constant.DeleteReason.OTHER)
                        {
                            entityHD.VoidReason = txtReason.Text;
                        }
                        entityHD.VoidBy = AppSession.UserLogin.UserID;
                        entityHD.VoidDate = DateTime.Now;
                        entityHD.GCTransactionStatus = Constant.TransactionStatus.VOID;
                        entityHD.LastUpdatedBy = AppSession.UserLogin.UserID;

                        List<GLTransactionDt> lstEntityDt = BusinessLayer.GetGLTransactionDtList(String.Format(
                                "GLTransactionID = {0} AND GCItemDetailStatus = '{1}' AND IsDeleted = 0", entityHD.GLTransactionID, Constant.TransactionStatus.OPEN), ctx);
                        foreach (GLTransactionDt entityDt in lstEntityDt)
                        {
                            entityDt.GCItemDetailStatus = Constant.TransactionStatus.VOID;
                            entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            glTransactionDtDao.Update(entityDt);

                            if (entityHD.GCTreasuryGroup == Constant.TreasuryGroup.SUPPLIER_PAYMENT && entityDt.DisplayOrder != 1)
                            {
                                #region Supplier Payment

                                string filterSPHD = string.Format("SupplierPaymentNo = '{0}'", entityDt.ReferenceNo);
                                SupplierPaymentHd supPaymentHd = BusinessLayer.GetSupplierPaymentHdList(filterSPHD, ctx).FirstOrDefault();
                                supPaymentHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                supPaymentHd.GLTransactionID = null;
                                supPaymentHd.ApprovedBy = null;
                                supPaymentHd.ApprovedDate = DateTime.Parse("1900-01-01");
                                supPaymentHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                supPaymentHdDao.Update(supPaymentHd);

                                string filterSPDT = string.Format("SupplierPaymentID = '{0}'", supPaymentHd.SupplierPaymentID);
                                List<SupplierPaymentDt> lstSupPaymentDt = BusinessLayer.GetSupplierPaymentDtList(filterSPDT, ctx);
                                foreach (SupplierPaymentDt supPaymentDt in lstSupPaymentDt)
                                {
                                    supPaymentDt.GLTransactionDtID = null;
                                    supPaymentDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    supPaymentDtDao.Update(supPaymentDt);
                                }

                                #endregion
                            }
                            else if (entityHD.GCTreasuryGroup == Constant.TreasuryGroup.AR_RECEIVING && entityDt.DisplayOrder != 1)
                            {
                                #region AR Receiving

                                // VOID ARReceiving
                                string filterAR = string.Format("ARReceivingNo = '{0}'", entityDt.ReferenceNo);
                                ARReceivingHd rcvHD = BusinessLayer.GetARReceivingHdList(filterAR, ctx).FirstOrDefault();
                                rcvHD.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                rcvHD.GCVoidReason = Constant.DeleteReason.OTHER;
                                rcvHD.VoidReason = "Delete from TreasuryDT";
                                rcvHD.VoidBy = AppSession.UserLogin.UserID;
                                rcvHD.VoidDate = DateTime.Now;
                                rcvHD.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                arReceivingHdDao.Update(rcvHD);

                                // DELETE ARInvoiceReceiving
                                string filterInvRcvList = string.Format("ARReceivingID = {0} AND IsDeleted = 0", rcvHD.ARReceivingID);
                                List<ARInvoiceReceiving> lstInvoiceReceiving = BusinessLayer.GetARInvoiceReceivingList(filterInvRcvList, ctx);
                                foreach (ARInvoiceReceiving invRcv in lstInvoiceReceiving)
                                {
                                    invRcv.IsDeleted = true;
                                    invRcv.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    entityARRcvDao.Update(invRcv);

                                    // mengurangi nilai saat sudah alokasi
                                    ARInvoiceHd oInvoiceHd = eInvoiceHd.Get(invRcv.ARInvoiceID);
                                    oInvoiceHd.TotalPaymentAmount -= invRcv.ReceivingAmount;
                                    if (oInvoiceHd.TotalPaymentAmount == 0)
                                    {
                                        oInvoiceHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;

                                        string filterInvDt = string.Format("ARInvoiceID = {0} AND ISNULL(GCTransactionDetailStatus,'') != '{1}'", invRcv.ARInvoiceID, Constant.TransactionStatus.VOID);
                                        List<ARInvoiceDt> lstInvoiceDt = BusinessLayer.GetARInvoiceDtList(filterInvDt, ctx);
                                        foreach (ARInvoiceDt oInvoiceDt in lstInvoiceDt)
                                        {
                                            oInvoiceDt.PaymentAmount = 0;
                                            oInvoiceDt.GCTransactionDetailStatus = Constant.TransactionStatus.APPROVED;
                                            oInvoiceDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            eInvoiceDt.Update(oInvoiceDt);
                                        }
                                    }
                                    else
                                    {
                                        oInvoiceHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                                    }
                                    oInvoiceHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    eInvoiceHd.Update(oInvoiceHd);
                                }

                                #endregion
                            }
                            else if (entityHD.GCTreasuryGroup == Constant.TreasuryGroup.DIRECT_PURCHASE && entityDt.DisplayOrder != 1)
                            {
                                #region Direct Purchase

                                string filterDP = string.Format("DirectPurchaseNo = '{0}'", entityDt.ReferenceNo);
                                DirectPurchaseHd dpHD = BusinessLayer.GetDirectPurchaseHdList(filterDP, ctx).FirstOrDefault();
                                dpHD.GLTransactionID = null;
                                dpHD.GLTransactionDtID = null;
                                dpHD.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                directPurchaseHdDao.Update(dpHD);

                                #endregion
                            }
                            else if (entityHD.GCTreasuryGroup == Constant.TreasuryGroup.REVENUE_SHARING && entityDt.DisplayOrder != 1)
                            {
                                #region Revenue Sharing

                                string filterRSPHD = string.Format("RSPaymentNo = '{0}'", entityDt.ReferenceNo);
                                TransRevenueSharingPaymentHd paymentHd = BusinessLayer.GetTransRevenueSharingPaymentHdList(filterRSPHD, ctx).FirstOrDefault();
                                paymentHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                paymentHd.GLTransactionID = null;
                                paymentHd.ApprovedBy = null;
                                paymentHd.ApprovedDate = DateTime.Parse("1900-01-01");
                                paymentHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                rspaymentHdDao.Update(paymentHd);

                                string filterDt = string.Format("RSPaymentID = {0} AND IsDeleted = 0", paymentHd.RSPaymentID);
                                List<TransRevenueSharingPaymentDt> lstPaymentDt = BusinessLayer.GetTransRevenueSharingPaymentDtList(filterDt, ctx);
                                foreach (TransRevenueSharingPaymentDt paymentDt in lstPaymentDt)
                                {
                                    paymentDt.GLTransactionDtID = null;
                                    paymentDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    rspaymentDtDao.Update(paymentDt);
                                }
                                #endregion
                            }
                        }

                        result = true;
                        glTransactionHdDao.Update(entityHD);
                        ctx.CommitTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "GAGAL VOID : Jurnal " + entityHD.JournalNo + " sudah diproses";
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
}