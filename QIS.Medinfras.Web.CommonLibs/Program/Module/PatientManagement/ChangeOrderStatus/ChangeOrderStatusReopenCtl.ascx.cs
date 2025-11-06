using System;
using System.Collections.Generic;
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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ChangeOrderStatusReopenCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;
            hdnRegistrationID.Value = hdnParam.Value.Split('|')[0];
            hdnOrderID.Value = hdnParam.Value.Split('|')[1];
            hdnOrderType.Value = hdnParam.Value.Split('|')[2];
            List<StandardCode> lstReopenReason = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.CHARGES_CHANGE_STATUS_REASON));
            Methods.SetComboBoxField<StandardCode>(cboReopenReason, lstReopenReason, "StandardCodeName", "StandardCodeID");
            cboReopenReason.SelectedIndex = 0;
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
            TestOrderHdDao entityTestOrderHdDao = new TestOrderHdDao(ctx);
            PrescriptionOrderHdDao entityPrescriptionOrderHdDao = new PrescriptionOrderHdDao(ctx);
            PrescriptionReturnOrderHdDao entityPrescriptionReturnOrderHdDao = new PrescriptionReturnOrderHdDao(ctx);
            ServiceOrderHdDao entityServiceOrderHdDao = new ServiceOrderHdDao(ctx);

            //ChargesStatusLogDao statusLogDao = new ChargesStatusLogDao(ctx);

            try
            {
                List<vPatientOrderAll> lstEntity = BusinessLayer.GetvPatientOrderAllList(String.Format("OrderID IN ({0}) AND OrderType = '{1}'", hdnOrderID.Value, hdnOrderType.Value));
                
                //ChargesStatusLog statusLog = new ChargesStatusLog();
                //string statusOld = "", statusNew = "";

                foreach (vPatientOrderAll entity in lstEntity)
                {
                    if (entity.OrderType == "TO")
                    {
                        //statusOld = entity.GCTransactionStatus;

                        TestOrderHd entityto = new TestOrderHd();
                        entityto = BusinessLayer.GetTestOrderHdList(String.Format("TestOrderID = {0} AND GCTransactionStatus IN ('{1}')",
                                            entity.OrderID, Constant.TransactionStatus.WAIT_FOR_APPROVAL)).FirstOrDefault();

                        entityto.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        entityto.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityTestOrderHdDao.Update(entityto);

                        //statusNew = entityto.GCTransactionStatus;

                        //statusLog.VisitID = entityto.VisitID;
                        //statusLog.TransactionID = entityto.TestOrderID;
                        //statusLog.GCReopenReason = cboReopenReason.Value.ToString();
                        //if (cboReopenReason.Value.ToString() == Constant.ChargesChangeStatusReason.LAIN_LAIN)
                        //{
                        //    statusLog.ReopenReason = txtReason.Text;
                        //}
                        //statusLog.GCTransactionStatusOLD = statusOld;
                        //statusLog.GCTransactionStatusNEW = statusNew;
                        //statusLog.LogDate = DateTime.Now;
                        //statusLog.UserID = AppSession.UserLogin.UserID;
                        //statusLogDao.Insert(statusLog);
                    }
                    else if (entity.OrderType == "PO")
                    {
                        //statusOld = entity.GCTransactionStatus;

                        PrescriptionOrderHd entityPo = new PrescriptionOrderHd();
                        entityPo = BusinessLayer.GetPrescriptionOrderHdList(String.Format("PrescriptionOrderID = {0} AND GCTransactionStatus IN ('{1}')",
                                            entity.OrderID, Constant.TransactionStatus.WAIT_FOR_APPROVAL)).FirstOrDefault();

                        entityPo.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        entityPo.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityPrescriptionOrderHdDao.Update(entityPo);

                        //statusNew = entityPo.GCTransactionStatus;

                        //statusLog.VisitID = entityPo.VisitID;
                        //statusLog.TransactionID = entityPo.PrescriptionOrderID;
                        //statusLog.GCReopenReason = cboReopenReason.Value.ToString();
                        //if (cboReopenReason.Value.ToString() == Constant.ChargesChangeStatusReason.LAIN_LAIN)
                        //{
                        //    statusLog.ReopenReason = txtReason.Text;
                        //}
                        //statusLog.GCTransactionStatusOLD = statusOld;
                        //statusLog.GCTransactionStatusNEW = statusNew;
                        //statusLog.LogDate = DateTime.Now;
                        //statusLog.UserID = AppSession.UserLogin.UserID;
                        //statusLogDao.Insert(statusLog);
                    }
                    else if (entity.OrderType == "RO")
                    {
                        //statusOld = entity.GCTransactionStatus;

                        PrescriptionReturnOrderHd entityRo = new PrescriptionReturnOrderHd();
                        entityRo = BusinessLayer.GetPrescriptionReturnOrderHdList(String.Format("PrescriptionReturnOrderID = {0} AND GCTransactionStatus IN ('{1}')",
                                            entity.OrderID, Constant.TransactionStatus.WAIT_FOR_APPROVAL)).FirstOrDefault();

                        entityRo.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        entityRo.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityPrescriptionReturnOrderHdDao.Update(entityRo);

                        //statusNew = entityRo.GCTransactionStatus;

                        //statusLog.VisitID = entityRo.VisitID;
                        //statusLog.TransactionID = entityRo.PrescriptionReturnOrderID;
                        //statusLog.GCReopenReason = cboReopenReason.Value.ToString();
                        //if (cboReopenReason.Value.ToString() == Constant.ChargesChangeStatusReason.LAIN_LAIN)
                        //{
                        //    statusLog.ReopenReason = txtReason.Text;
                        //}
                        //statusLog.GCTransactionStatusOLD = statusOld;
                        //statusLog.GCTransactionStatusNEW = statusNew;
                        //statusLog.LogDate = DateTime.Now;
                        //statusLog.UserID = AppSession.UserLogin.UserID;
                        //statusLogDao.Insert(statusLog);
                    }
                    else if (entity.OrderType == "SO")
                    {
                        //statusOld = entity.GCTransactionStatus;

                        ServiceOrderHd entitySo = new ServiceOrderHd();
                        entitySo = BusinessLayer.GetServiceOrderHdList(String.Format("ServiceOrderID = {0} AND GCTransactionStatus IN ('{1}')",
                                            entity.OrderID, Constant.TransactionStatus.WAIT_FOR_APPROVAL)).FirstOrDefault();

                        entitySo.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        entitySo.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityServiceOrderHdDao.Update(entitySo);

                        //statusNew = entitySo.GCTransactionStatus;

                        //statusLog.VisitID = entitySo.VisitID;
                        //statusLog.TransactionID = entitySo.ServiceOrderID;
                        //statusLog.GCReopenReason = cboReopenReason.Value.ToString();
                        //if (cboReopenReason.Value.ToString() == Constant.ChargesChangeStatusReason.LAIN_LAIN)
                        //{
                        //    statusLog.ReopenReason = txtReason.Text;
                        //}
                        //statusLog.GCTransactionStatusOLD = statusOld;
                        //statusLog.GCTransactionStatusNEW = statusNew;
                        //statusLog.LogDate = DateTime.Now;
                        //statusLog.UserID = AppSession.UserLogin.UserID;
                        //statusLogDao.Insert(statusLog);
                    }
                    else
                    {
                        errMessage = "Order Sudah Diproses. Tidak Bisa Dibuka Kembali.";
                        result = false;
                    }
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
    }
}