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
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ChangePatientTransactionStatusReopenCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;
            hdnRegistrationID.Value = hdnParam.Value.Split('|')[0];
            hdnTransactionID.Value = hdnParam.Value.Split('|')[1];
            List<StandardCode> lstReopenReason = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.CHARGES_CHANGE_STATUS_REASON));
            Methods.SetComboBoxField<StandardCode>(cboReopenReason, lstReopenReason, "StandardCodeName", "StandardCodeID");
            cboReopenReason.SelectedIndex = 0;

            string filterSetVar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.REOPEN_CHARGESHD_REOPEN_ISAPPROVE_CHARGESDT);
            List<SettingParameterDt> lstSetVarDt = BusinessLayer.GetSettingParameterDtList(filterSetVar);

            hdnIsReopenChargesReopenApprovedChargesDt.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.REOPEN_CHARGESHD_REOPEN_ISAPPROVE_CHARGESDT).FirstOrDefault().ParameterValue;
        }

        protected void cbpChargesReopen_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            if (OnReopenRecord(ref errMessage))
                result = "success";
            else
                result = "fail|" + errMessage;

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnReopenRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            LocationDao locationDao = new LocationDao(ctx);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            PrescriptionOrderHdDao presOrderHdDao = new PrescriptionOrderHdDao(ctx);
            PrescriptionOrderDtDao presOrderDtDao = new PrescriptionOrderDtDao(ctx);
            PrescriptionReturnOrderHdDao presReturnOrderHdDao = new PrescriptionReturnOrderHdDao(ctx);
            PrescriptionReturnOrderDtDao presReturnOrderDtDao = new PrescriptionReturnOrderDtDao(ctx);
            TestOrderHdDao orderHdDao = new TestOrderHdDao(ctx);
            TestOrderDtDao orderDtDao = new TestOrderDtDao(ctx);
            ChargesStatusLogDao statusLogDao = new ChargesStatusLogDao(ctx);
            PatientChargesDtInfoDao entityInfoDao = new PatientChargesDtInfoDao(ctx);
            VisitPackageBalanceHdDao balanceHdDao = new VisitPackageBalanceHdDao(ctx);
            VisitPackageBalanceDtDao balanceDtDao = new VisitPackageBalanceDtDao(ctx);
            try
            {
                List<PatientChargesHd> lstHd = BusinessLayer.GetPatientChargesHdList(string.Format("TransactionID IN ({0})", hdnTransactionID.Value));
                ChargesStatusLog statusLog = new ChargesStatusLog();

                string statusOld = "", statusNew = "";

                foreach (PatientChargesHd entityHd in lstHd)
                {
                    statusOld = entityHd.GCTransactionStatus;

                    int IsAllowOver = 0;
                    int OverQty = 0;
                    if (entityHd.PatientBillingID == null && entityHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                    {
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        List<PatientChargesDt> lstDt1 = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = {0} AND IsDeleted = 0", entityHd.TransactionID), ctx);
                        foreach (PatientChargesDt entityDt in lstDt1)
                        {
                            if (entityDt != null)
                            {
                                if (entityDt.ChargedQuantity < 0)
                                {
                                    if (entityDt.LocationID != null && entityDt.LocationID != 0)
                                    {
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        Location entityLocation = locationDao.Get(Convert.ToInt32(entityDt.LocationID));
                                        if (entityLocation.IsAllowOverIssued == false)
                                        {
                                            IsAllowOver += 1;
                                        }
                                    }
                                }
                            }
                        }

                        if (IsAllowOver > 0)
                        {
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            List<PatientChargesDt> lstDt2 = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = {0} AND IsDeleted = 0 AND LocationID IS NOT NULL", entityHd.TransactionID), ctx);
                            foreach (PatientChargesDt entityDt in lstDt2)
                            {
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                Location entityLocation = locationDao.Get(Convert.ToInt32(entityDt.LocationID));

                                ItemBalance entityItem = BusinessLayer.GetItemBalanceList(String.Format("ItemID = {0} AND LocationID = {1} AND IsDeleted = 0", entityDt.ItemID, entityLocation.LocationID), ctx).FirstOrDefault();
                                Decimal qtyEnd = 0;
                                if (entityItem != null)
                                {
                                    qtyEnd = entityItem.QuantityEND;
                                }
                                Decimal QtyStock = qtyEnd + entityDt.ChargedQuantity;
                                if (QtyStock < 0)
                                {
                                    OverQty += 1;
                                }
                            }

                            if (OverQty > 0)
                            {
                                errMessage = "Transaksi " + entityHd.TransactionNo + " tidak bisa diproses. Saldo tidak boleh minus.";
                                Exception ex = new Exception(errMessage);
                                Helper.InsertErrorLog(ex);
                                result = false;
                                ctx.RollBackTransaction();
                                break;
                            }
                            else
                            {
                                if (entityHd.TestOrderID != null && entityHd.TestOrderID != 0)
                                {
                                    TestOrderHd orderHd = orderHdDao.Get(Convert.ToInt32(entityHd.TestOrderID));
                                    if (orderHd.IsCreatedBySystem)
                                    {
                                        string filterOrderDt = string.Format("TestOrderID = {0} AND IsDeleted = 0 AND GCTestOrderStatus != '{1}'", entityHd.TestOrderID, Constant.TestOrderStatus.CANCELLED);
                                        List<TestOrderDt> orderDtList = BusinessLayer.GetTestOrderDtList(filterOrderDt, ctx);
                                        foreach (TestOrderDt orderDt in orderDtList)
                                        {
                                            orderDt.GCTestOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                            orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            orderDtDao.Update(orderDt);
                                        }

                                        orderHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                        orderHd.GCVoidReason = Constant.DeleteReason.OTHER;
                                        orderHd.VoidReason = "Linked transaction was deleted";
                                        orderHd.VoidBy = AppSession.UserLogin.UserID;
                                        orderHd.VoidDate = DateTime.Now;
                                        orderHd.GCOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                        orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        orderHdDao.Update(orderHd);

                                        entityHd.TestOrderID = null;
                                    }
                                }

                                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                                entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                entityHdDao.Update(entityHd);

                                // Update status PrescriptionOrderHd
                                if (entityHd.PrescriptionOrderID != null)
                                {
                                    PrescriptionOrderHd orderHd = presOrderHdDao.Get((int)entityHd.PrescriptionOrderID);
                                    orderHd.GCOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                                    orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    presOrderHdDao.Update(orderHd);
                                }

                                // Update status PrescriptionReturnOrderHd
                                if (entityHd.PrescriptionReturnOrderID != null)
                                {
                                    PrescriptionReturnOrderHd orderHd = presReturnOrderHdDao.Get((int)entityHd.PrescriptionReturnOrderID);
                                    orderHd.GCOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                                    orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    presReturnOrderHdDao.Update(orderHd);
                                }

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                List<PatientChargesDt> lstDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = {0} AND IsDeleted = 0", entityHd.TransactionID), ctx);
                                foreach (PatientChargesDt entityDt in lstDt)
                                {
                                    entityDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;

                                    if (entityDt.IsApproved)
                                    {
                                        if (entityDt.PrescriptionOrderDetailID == null && entityDt.PrescriptionReturnOrderDtID == null)
                                        {
                                            if (hdnIsReopenChargesReopenApprovedChargesDt.Value == "1")
                                            {
                                                entityDt.IsApproved = false;
                                            }
                                            else
                                            {
                                                entityDt.IsApproved = true;
                                            }
                                        }
                                        else
                                        {
                                            entityDt.IsApproved = false;
                                        }
                                    }

                                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    entityDtDao.Update(entityDt);

                                    PatientChargesDtInfo info = entityInfoDao.Get(entityDt.ID);
                                    if (info != null && info.VisitPackageBalanceTransactionID > 0)
                                    {
                                        VisitPackageBalanceHd balanceHd = BusinessLayer.GetVisitPackageBalanceHd(Convert.ToInt32(info.VisitPackageBalanceTransactionID));
                                        VisitPackageBalanceDt balanceDt = BusinessLayer.GetVisitPackageBalanceDtList(string.Format("TransactionID = {0} ORDER BY ID DESC", balanceHd.TransactionID)).FirstOrDefault();
                                        VisitPackageBalanceDt entityPackageDt = new VisitPackageBalanceDt();
                                        entityPackageDt.TransactionID = balanceHd.TransactionID;
                                        entityPackageDt.PatientChargesDtID = entityDt.ID;
                                        entityPackageDt.VisitID = entityHd.VisitID;
                                        entityPackageDt.QuantityBEGIN = balanceDt.QuantityEND;
                                        entityPackageDt.QuantityIN = balanceDt.QuantityOUT;
                                        entityPackageDt.QuantityOUT = 0;
                                        entityPackageDt.QuantityEND = entityPackageDt.QuantityBEGIN + entityPackageDt.QuantityIN - entityPackageDt.QuantityOUT;
                                        entityPackageDt.CreatedBy = AppSession.UserLogin.UserID;
                                        entityPackageDt.CreatedDate = DateTime.Now;
                                        balanceDtDao.Insert(entityPackageDt);

                                        balanceHd.Quantity = entityPackageDt.QuantityEND;
                                        balanceHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        balanceHd.LastUpdatedDate = DateTime.Now;
                                        balanceHdDao.Update(balanceHd);
                                    }

                                    // Update status PrescriptionOrderDt
                                    if (entityDt.PrescriptionOrderDetailID != null)
                                    {
                                        PrescriptionOrderDt orderDt = presOrderDtDao.Get((int)entityDt.PrescriptionOrderDetailID);
                                        orderDt.GCPrescriptionOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                                        orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        presOrderDtDao.Update(orderDt);
                                    }

                                    // Update status PrescriptionReturnOrderDt
                                    if (entityDt.PrescriptionReturnOrderDtID != null)
                                    {
                                        PrescriptionReturnOrderDt orderDt = presReturnOrderDtDao.Get((int)entityDt.PrescriptionReturnOrderDtID);
                                        orderDt.GCPrescriptionReturnOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                                        orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        presReturnOrderDtDao.Update(orderDt);
                                    }
                                }

                                statusNew = entityHd.GCTransactionStatus;

                                statusLog.VisitID = entityHd.VisitID;
                                statusLog.TransactionID = entityHd.TransactionID;
                                statusLog.GCReopenReason = cboReopenReason.Value.ToString();
                                if (cboReopenReason.Value.ToString() == Constant.ChargesChangeStatusReason.LAIN_LAIN)
                                {
                                    statusLog.ReopenReason = txtReason.Text;
                                }
                                statusLog.GCTransactionStatusOLD = statusOld;
                                statusLog.GCTransactionStatusNEW = statusNew;
                                statusLog.LogDate = DateTime.Now;
                                statusLog.UserID = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                statusLogDao.Insert(statusLog);
                            }
                        }
                        else
                        {
                            if (entityHd.TestOrderID != null && entityHd.TestOrderID != 0)
                            {
                                TestOrderHd orderHd = orderHdDao.Get(Convert.ToInt32(entityHd.TestOrderID));
                                if (orderHd.IsCreatedBySystem)
                                {
                                    string filterOrderDt = string.Format("TestOrderID = {0} AND IsDeleted = 0 AND GCTestOrderStatus != '{1}'", entityHd.TestOrderID, Constant.TestOrderStatus.CANCELLED);
                                    List<TestOrderDt> orderDtList = BusinessLayer.GetTestOrderDtList(filterOrderDt, ctx);
                                    foreach (TestOrderDt orderDt in orderDtList)
                                    {
                                        orderDt.GCTestOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                        orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        orderDtDao.Update(orderDt);
                                    }

                                    orderHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                    orderHd.GCVoidReason = Constant.DeleteReason.OTHER;
                                    orderHd.VoidReason = "Linked transaction was deleted";
                                    orderHd.VoidBy = AppSession.UserLogin.UserID;
                                    orderHd.VoidDate = DateTime.Now;
                                    orderHd.GCOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                    orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    orderHdDao.Update(orderHd);

                                    entityHd.TestOrderID = null;
                                }
                            }

                            entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                            entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityHdDao.Update(entityHd);

                            // Update status PrescriptionOrderHd
                            if (entityHd.PrescriptionOrderID != null)
                            {
                                PrescriptionOrderHd orderHd = presOrderHdDao.Get((int)entityHd.PrescriptionOrderID);
                                orderHd.GCOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                                orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                presOrderHdDao.Update(orderHd);
                            }

                            // Update status PrescriptionReturnOrderHd
                            if (entityHd.PrescriptionReturnOrderID != null)
                            {
                                PrescriptionReturnOrderHd orderHd = presReturnOrderHdDao.Get((int)entityHd.PrescriptionReturnOrderID);
                                orderHd.GCOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                                orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                presReturnOrderHdDao.Update(orderHd);
                            }

                            List<PatientChargesDt> lstDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = {0} AND IsDeleted = 0", entityHd.TransactionID), ctx);
                            foreach (PatientChargesDt entityDt in lstDt)
                            {
                                entityDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;

                                if (entityDt.IsApproved)
                                {
                                    if (entityDt.PrescriptionOrderDetailID == null && entityDt.PrescriptionReturnOrderDtID == null)
                                    {
                                        if (hdnIsReopenChargesReopenApprovedChargesDt.Value == "1")
                                        {
                                            entityDt.IsApproved = false;
                                        }
                                        else
                                        {
                                            entityDt.IsApproved = true;
                                        }
                                    }
                                    else
                                    {
                                        entityDt.IsApproved = false;
                                    }
                                }

                                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                entityDtDao.Update(entityDt);

                                PatientChargesDtInfo info = entityInfoDao.Get(entityDt.ID);
                                if (info != null && info.VisitPackageBalanceTransactionID > 0)
                                {
                                    VisitPackageBalanceHd balanceHd = BusinessLayer.GetVisitPackageBalanceHd(Convert.ToInt32(info.VisitPackageBalanceTransactionID));
                                    VisitPackageBalanceDt balanceDt = BusinessLayer.GetVisitPackageBalanceDtList(string.Format("TransactionID = {0} ORDER BY ID DESC", balanceHd.TransactionID)).FirstOrDefault();
                                    VisitPackageBalanceDt entityPackageDt = new VisitPackageBalanceDt();
                                    entityPackageDt.TransactionID = balanceHd.TransactionID;
                                    entityPackageDt.PatientChargesDtID = entityDt.ID;
                                    entityPackageDt.VisitID = entityHd.VisitID;
                                    entityPackageDt.QuantityBEGIN = balanceDt.QuantityEND;
                                    entityPackageDt.QuantityIN = balanceDt.QuantityOUT;
                                    entityPackageDt.QuantityOUT = 0;
                                    entityPackageDt.QuantityEND = entityPackageDt.QuantityBEGIN + entityPackageDt.QuantityIN - entityPackageDt.QuantityOUT;
                                    entityPackageDt.CreatedBy = AppSession.UserLogin.UserID;
                                    entityPackageDt.CreatedDate = DateTime.Now;
                                    balanceDtDao.Insert(entityPackageDt);

                                    balanceHd.Quantity = entityPackageDt.QuantityEND;
                                    balanceHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    balanceHd.LastUpdatedDate = DateTime.Now;
                                    balanceHdDao.Update(balanceHd);
                                }

                                // Update status PrescriptionOrderDt
                                if (entityDt.PrescriptionOrderDetailID != null)
                                {
                                    PrescriptionOrderDt orderDt = presOrderDtDao.Get((int)entityDt.PrescriptionOrderDetailID);
                                    orderDt.GCPrescriptionOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                                    orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    presOrderDtDao.Update(orderDt);
                                }

                                // Update status PrescriptionReturnOrderDt
                                if (entityDt.PrescriptionReturnOrderDtID != null)
                                {
                                    PrescriptionReturnOrderDt orderDt = presReturnOrderDtDao.Get((int)entityDt.PrescriptionReturnOrderDtID);
                                    orderDt.GCPrescriptionReturnOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                                    orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    presReturnOrderDtDao.Update(orderDt);
                                }
                            }

                            statusNew = entityHd.GCTransactionStatus;

                            statusLog.VisitID = entityHd.VisitID;
                            statusLog.TransactionID = entityHd.TransactionID;
                            statusLog.GCReopenReason = cboReopenReason.Value.ToString();
                            if (cboReopenReason.Value.ToString() == Constant.ChargesChangeStatusReason.LAIN_LAIN)
                            {
                                statusLog.ReopenReason = txtReason.Text;
                            }
                            statusLog.GCTransactionStatusOLD = statusOld;
                            statusLog.GCTransactionStatusNEW = statusNew;
                            statusLog.LogDate = DateTime.Now;
                            statusLog.UserID = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            statusLogDao.Insert(statusLog);
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = "Transaksi " + entityHd.TransactionNo + " sudah diproses, tidak dapat dibuka kembali.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                    }
                }

                if (result)
                {
                    ctx.CommitTransaction();
                }
                else
                {
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