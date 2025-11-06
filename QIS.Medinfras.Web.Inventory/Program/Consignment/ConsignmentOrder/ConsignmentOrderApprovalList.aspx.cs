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
    public partial class ConsignmentOrderApprovalList : BasePageTrx
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.CONSIGNMENT_ORDER_APPROVAL;
        }

        private GetUserMenuAccess menu;

        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        protected override void InitializeDataControl()
        {
            MPTrx master = (MPTrx)Master;
            menu = ((MPMain)master.Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += String.Format("GCTransactionStatus = '{0}' AND TransactionCode = '{1}'", Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionCode.CONSIGNMENT_ORDER);

            int count = BusinessLayer.GetLocationUserRowCount(string.Format("UserID = {0} AND IsDeleted = 0", AppSession.UserLogin.UserID));

            if (count > 0)
                filterExpression += string.Format(" AND LocationID IN (SELECT LocationID FROM LocationUser WHERE UserID = {0} AND IsDeleted = 0)", AppSession.UserLogin.UserID);
            else
            {
                count = BusinessLayer.GetLocationUserRoleRowCount(string.Format("RoleID IN (SELECT RoleID FROM UserInRole WHERE UserID = {0} AND HealthcareID = '{1}') AND IsDeleted = 0", AppSession.UserLogin.UserID, AppSession.UserLogin.HealthcareID));
                if (count > 0)
                    filterExpression += string.Format(" AND LocationID IN (SELECT LocationID FROM LocationUserRole WHERE RoleID IN (SELECT RoleID FROM UserInRole WHERE UserID = {0} AND HealthcareID = '{1}') AND IsDeleted = 0)", AppSession.UserLogin.UserID, AppSession.UserLogin.HealthcareID);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPurchaseOrderHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPurchaseOrderHd> lstEntity = BusinessLayer.GetvPurchaseOrderHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "OrderDate DESC");
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

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            if (type == "approve")
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                PurchaseOrderHdDao purchaseHdDao = new PurchaseOrderHdDao(ctx);
                PurchaseOrderDtDao purchaseDtDao = new PurchaseOrderDtDao(ctx);
                AuditLogDao entityAuditLogDao = new AuditLogDao(ctx);
                AuditLog entityAuditLog = new AuditLog();
                try
                {
                    string filterExpressionPurchaseOrderHd = String.Format("PurchaseOrderID IN ({0})", hdnParam.Value);
                    List<PurchaseOrderHd> lstPurchaseOrderHd = BusinessLayer.GetPurchaseOrderHdList(filterExpressionPurchaseOrderHd, ctx);
                    foreach (PurchaseOrderHd purchaseHd in lstPurchaseOrderHd)
                    {
                        entityAuditLog.OldValues = JsonConvert.SerializeObject(purchaseHd);

                        string filterExpressionPurchaseOrderDt = String.Format("PurchaseOrderID IN ({0})", purchaseHd.PurchaseOrderID);
                        List<PurchaseOrderDt> lstPurchaseOrderDt = BusinessLayer.GetPurchaseOrderDtList(filterExpressionPurchaseOrderDt, ctx);
                        foreach (PurchaseOrderDt purchaseDt in lstPurchaseOrderDt)
                        {
                            purchaseDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                            purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            purchaseDtDao.Update(purchaseDt);
                        }

                        purchaseHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                        purchaseHd.ApprovedBy = AppSession.UserLogin.UserID;
                        purchaseHd.ApprovedDate = DateTime.Now;
                        purchaseHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        purchaseHdDao.Update(purchaseHd);

                        entityAuditLog.ObjectType = Constant.BusinessObjectType.PURCHASE_ORDER;
                        entityAuditLog.NewValues = JsonConvert.SerializeObject(purchaseHd);
                        entityAuditLog.UserID = AppSession.UserLogin.UserID;
                        entityAuditLog.LogDate = DateTime.Now;
                        entityAuditLog.TransactionID = purchaseHd.PurchaseOrderID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityAuditLogDao.Insert(entityAuditLog);
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
            else
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                PurchaseOrderHdDao entityDao = new PurchaseOrderHdDao(ctx);
                PurchaseOrderDtDao purchaseDtDao = new PurchaseOrderDtDao(ctx);
                AuditLogDao entityAuditLogDao = new AuditLogDao(ctx);
                AuditLog entityAuditLog = new AuditLog();
                try
                {
                    string[] listParam = hdnParam.Value.Split(',');
                    string filterExpressionPurchaseOrderHd = String.Format("PurchaseOrderID IN ({0})", hdnParam.Value);

                    foreach (string param in listParam)
                    {
                        int PurchaseOrderID = Convert.ToInt32(param);

                        PurchaseOrderHd entity = entityDao.Get(PurchaseOrderID);

                        entityAuditLog.OldValues = JsonConvert.SerializeObject(entity);

                        string filterExpressionPurchaseOrderDt = String.Format("PurchaseOrderID IN ({0})", entity.PurchaseOrderID);
                        List<PurchaseOrderDt> lstPurchaseOrderDt = BusinessLayer.GetPurchaseOrderDtList(filterExpressionPurchaseOrderDt, ctx);
                        foreach (PurchaseOrderDt purchaseDt in lstPurchaseOrderDt)
                        {
                            purchaseDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                            purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            purchaseDtDao.Update(purchaseDt);
                        }

                        entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityDao.Update(entity);

                        entityAuditLog.ObjectType = Constant.BusinessObjectType.PURCHASE_ORDER;
                        entityAuditLog.NewValues = JsonConvert.SerializeObject(entity);
                        entityAuditLog.UserID = AppSession.UserLogin.UserID;
                        entityAuditLog.LogDate = DateTime.Now;
                        entityAuditLog.TransactionID = entity.PurchaseOrderID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityAuditLogDao.Insert(entityAuditLog);
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
}