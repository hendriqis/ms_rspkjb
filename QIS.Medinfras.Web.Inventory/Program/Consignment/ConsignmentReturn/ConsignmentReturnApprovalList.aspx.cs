using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class ConsignmentReturnApprovalList : BasePageTrx
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.CONSIGNMENT_RETURN_APPROVAL;
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
            filterExpression += String.Format("GCTransactionStatus = '{0}' AND TransactionCode = '{1}'", Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionCode.CONSIGNMENT_RETURN);
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
                int rowCount = BusinessLayer.GetvPurchaseReturnHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPurchaseReturnHd> lstEntity = BusinessLayer.GetvPurchaseReturnHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ReturnDate DESC");
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
                PurchaseReturnHdDao purchaseReturnHdDao = new PurchaseReturnHdDao(ctx);
                PurchaseReturnDtDao purchaseReturnDtDao = new PurchaseReturnDtDao(ctx);
                try
                {
                    string filterExpressionPurchaseReturnHd = String.Format("PurchaseReturnID IN ({0})", hdnParam.Value);
                    int count = 0;
                    string purchaseReturnNoInfo = "";
                    List<PurchaseReturnHd> lstPurchaseReturnHd = BusinessLayer.GetPurchaseReturnHdList(filterExpressionPurchaseReturnHd);
                    foreach (PurchaseReturnHd purchaseHd in lstPurchaseReturnHd)
                    {
                        string filterExp = string.Format("PurchaseReturnID = {0} AND GCItemDetailStatus != '{1}'", purchaseHd.PurchaseReturnID, Constant.TransactionStatus.VOID);
                        List<PurchaseReturnDt> lstPurchaseReturnDt = BusinessLayer.GetPurchaseReturnDtList(filterExp);
                        foreach (PurchaseReturnDt purchaseDt in lstPurchaseReturnDt)
                        {
                            decimal returnQty = purchaseDt.Quantity * purchaseDt.ConversionFactor;
                            string filterExpression = String.Format("LocationID = {0} AND ItemID = {1} AND IsDeleted = 0", purchaseHd.LocationID, purchaseDt.ItemID);
                            ItemBalance lstItemBalance = BusinessLayer.GetItemBalanceList(filterExpression, ctx).FirstOrDefault();
                            decimal currentStock = lstItemBalance.QuantityEND;
                            if (currentStock < returnQty)
                            {
                                count = count + 1;
                                result = false;

                                if (String.IsNullOrEmpty(purchaseReturnNoInfo))
                                {
                                    purchaseReturnNoInfo = string.Format("{0}", purchaseHd.PurchaseReturnNo);
                                }
                                else
                                {
                                    purchaseReturnNoInfo += string.Format(" ,{0}", purchaseHd.PurchaseReturnNo);
                                }
                            }
                            else
                            {
                                purchaseDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                                purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                purchaseReturnDtDao.Update(purchaseDt);
                            }
                        }

                        purchaseHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                        purchaseHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        purchaseReturnHdDao.Update(purchaseHd);
                    }

                    if (count == 0)
                    {
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Retur dgn nomor <b>" + purchaseReturnNoInfo + "</b> tidak dapat di-approve karena stok tidak mencukupi.";
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
            else
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                PurchaseReturnHdDao entityDao = new PurchaseReturnHdDao(ctx);
                try
                {
                    string[] listParam = hdnParam.Value.Split(',');
                    foreach (string param in listParam)
                    {
                        int PurchaseReturnID = Convert.ToInt32(param);
                        PurchaseReturnHd entity = entityDao.Get(PurchaseReturnID);
                        entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDao.Update(entity);
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
