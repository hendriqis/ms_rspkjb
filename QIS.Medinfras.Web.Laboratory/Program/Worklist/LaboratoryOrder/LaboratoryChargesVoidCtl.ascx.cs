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

namespace QIS.Medinfras.Web.Laboratory.Program
{
    public partial class LaboratoryChargesVoidCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;
            hdnRegistrationID.Value = hdnParam.Value.Split('|')[0];
            hdnTransactionID.Value = hdnParam.Value.Split('|')[1];
            List<StandardCode> lstVoidReason = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.DELETE_REASON));
            Methods.SetComboBoxField<StandardCode>(cboVoidReason, lstVoidReason, "StandardCodeName", "StandardCodeID");
            cboVoidReason.SelectedIndex = 0;
        }

        protected void cbpChargesVoid_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
            PatientChargesHdDao chargesDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            TestOrderHdDao orderHdDao = new TestOrderHdDao(ctx);
            TestOrderDtDao orderDtDao = new TestOrderDtDao(ctx);
            //ChargesStatusLogDao logDao = new ChargesStatusLogDao(ctx);
            try
            {
                //ChargesStatusLog log = new ChargesStatusLog();
                //string statusOld = "", statusNew = "";
                PatientChargesHd entity = chargesDao.Get(Convert.ToInt32(hdnTransactionID.Value));
                //statusOld = entity.GCTransactionStatus;
                if (entity.PatientBillingID == null && entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    entity.GCVoidReason = cboVoidReason.Value.ToString();
                    if (cboVoidReason.Value.ToString() == Constant.DeleteReason.OTHER)
                    {
                        entity.VoidReason = txtReason.Text;
                    }
                    entity.VoidBy = AppSession.UserLogin.UserID;
                    entity.VoidDate = DateTime.Now;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    chargesDao.Update(entity);

                    List<PatientChargesDt> lstPatientChargesDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = {0} AND IsDeleted = 0 AND GCTransactionDetailStatus <> '{1}'", entity.TransactionID, Constant.TransactionStatus.VOID), ctx);
                    foreach (PatientChargesDt patientChargesDt in lstPatientChargesDt)
                    {
                        //PatientChargesDt entity = entityDtDao.Get(patientChargesDt.ID);
                        patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                        patientChargesDt.IsApproved = false;
                        patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(patientChargesDt);
                    }

                    //statusNew = entity.GCTransactionStatus;

                    //log.VisitID = entity.VisitID;
                    //log.TransactionID = entity.TransactionID;
                    //log.LogDate = DateTime.Now;
                    //log.UserID = AppSession.UserLogin.UserID;
                    //log.GCTransactionStatusOLD = statusOld;
                    //log.GCTransactionStatusNEW = statusNew;
                    //logDao.Insert(log); 

                    //Update Status TestOrderHd
                    if (entity.TestOrderID != null)
                    {
                        TestOrderHd orderHd = orderHdDao.Get((int)entity.TestOrderID);
                        if (orderHd.GCTransactionStatus != Constant.TransactionStatus.VOID)
                        {
                            orderHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                            orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            orderHdDao.Update(orderHd);

                            List<TestOrderDt> lstTestOrderDt = BusinessLayer.GetTestOrderDtList(string.Format("TestOrderID = {0} AND IsDeleted = 0", entity.TestOrderID.ToString()), ctx);
                            foreach (TestOrderDt orderDt in lstTestOrderDt)
                            {
                                if (orderDt != null && orderDt.GCTestOrderStatus != Constant.TestOrderStatus.CANCELLED)
                                {
                                    if (!orderDt.IsDeleted)
                                    {
                                        orderDt.GCTestOrderStatus = Constant.TestOrderStatus.OPEN;
                                        orderDt.BusinessPartnerID = null;
                                        orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        orderDtDao.Update(orderDt);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    errMessage = "Transaksi Sudah Diproses. Tidak Bisa Dibatalkan";
                    result = false;
                }
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
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