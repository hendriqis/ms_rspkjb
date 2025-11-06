using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class PurchaseOrderCloseCtl : BaseViewPopupCtl
    {
        protected string filterExpressionSupplier = "";

        public override void InitializeDataControl(string param)
        {
            hdnPurchaseOrderIDCtlClosed.Value = param;

            string filterExpression = string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.CLOSED_PO_REASON);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboClosedPOReason, lstStandardCode.Where(a => a.ParentID == Constant.StandardCode.CLOSED_PO_REASON).ToList(), "StandardCodeName", "StandardCodeID");
            cboClosedPOReason.SelectedIndex = 0;
        }

        protected void cbpClosePO_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            if (OnProcessRecord(ref errMessage))
                result = "success";
            else
                result = "fail|" + errMessage;

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnProcessRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseOrderHdDao POHdDao = new PurchaseOrderHdDao(ctx);
            PurchaseOrderDtDao PODtDao = new PurchaseOrderDtDao(ctx);
            try
            {
                PurchaseOrderHd POHd = POHdDao.Get(Convert.ToInt32(hdnPurchaseOrderIDCtlClosed.Value));
                POHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                POHd.GCClosedReason = cboClosedPOReason.Value.ToString();
                if (cboClosedPOReason.Value.ToString() == Constant.POClosedReason.OTHER)
                {
                    POHd.ClosedReason= txtReason.Text;
                }
                POHd.ClosedBy = AppSession.UserLogin.UserID;
                POHd.ClosedDate = DateTime.Now;
                POHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                POHdDao.Update(POHd);

                string filterExpressionPO = String.Format("PurchaseOrderID = {0} AND IsDeleted = 0", hdnPurchaseOrderIDCtlClosed.Value);
                List<PurchaseOrderDt> lstPurchaseOrderDt = BusinessLayer.GetPurchaseOrderDtList(filterExpressionPO, ctx);
                foreach (PurchaseOrderDt PODt in lstPurchaseOrderDt)
                {
                    PODt.GCItemDetailStatus = Constant.TransactionStatus.CLOSED;
                    PODt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    PODtDao.Update(PODt);
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