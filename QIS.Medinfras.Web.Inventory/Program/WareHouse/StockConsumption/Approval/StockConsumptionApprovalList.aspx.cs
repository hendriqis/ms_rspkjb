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
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class StockConsumptionApprovalList : BasePageTrx
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.ITEM_CONSUMPTION_APPROVAL;
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
            filterExpression += String.Format("TransactionCode = '{0}' AND GCTransactionStatus = '{1}'", Constant.TransactionCode.ITEM_CONSUMPTION, Constant.TransactionStatus.WAIT_FOR_APPROVAL);
            int count = BusinessLayer.GetLocationUserRowCount(string.Format("UserID = {0} AND IsDeleted = 0", AppSession.UserLogin.UserID));

            if (count > 0)
                filterExpression += string.Format(" AND FromLocationID IN (SELECT LocationID FROM LocationUser WHERE UserID = {0} AND IsDeleted = 0)", AppSession.UserLogin.UserID);
            else
            {
                count = BusinessLayer.GetLocationUserRoleRowCount(string.Format("RoleID IN (SELECT RoleID FROM UserInRole WHERE UserID = {0} AND HealthcareID = '{1}') AND IsDeleted = 0", AppSession.UserLogin.UserID, AppSession.UserLogin.HealthcareID));
                if (count > 0)
                    filterExpression += string.Format(" AND FromLocationID IN (SELECT LocationID FROM LocationUserRole WHERE RoleID IN (SELECT RoleID FROM UserInRole WHERE UserID = {0} AND HealthcareID = '{1}') AND IsDeleted = 0)", AppSession.UserLogin.UserID, AppSession.UserLogin.HealthcareID);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemTransactionHd1RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vItemTransactionHd1> lstEntity = BusinessLayer.GetvItemTransactionHd1List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "TransactionID ASC");
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
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemTransactionHdDao itemHdDao = new ItemTransactionHdDao(ctx);
            ItemTransactionDtDao itemDtDao = new ItemTransactionDtDao(ctx);
            ItemRequestDtDao itemReqDtDao = new ItemRequestDtDao(ctx);
            if (type == "approve")
            {
                try
                {
                    string filterExpressionItemTransactionHd = String.Format("TransactionID IN ({0})", hdnParam.Value);
                    List<ItemTransactionHd> lstItemTransactionHd = BusinessLayer.GetItemTransactionHdList(filterExpressionItemTransactionHd, ctx);
                    foreach (ItemTransactionHd itemHd in lstItemTransactionHd)
                    {
                        List<ItemRequestDt> lstItemRequestDt = null;
                        if (itemHd.ItemRequestID != null && itemHd.ItemRequestID != 0)
                        {
                            string filterExpressionItemRequestHd = String.Format("ItemRequestID = {0} AND IsDeleted = 0 AND GCItemDetailStatus != '{1}'", itemHd.ItemRequestID, Constant.TransactionStatus.VOID);
                            lstItemRequestDt = BusinessLayer.GetItemRequestDtList(filterExpressionItemRequestHd, ctx);
                        }

                        string filterExpressionItemTransactionDt = String.Format("TransactionID IN ({0}) AND GCItemDetailStatus != '{1}'", hdnParam.Value, Constant.TransactionStatus.VOID);
                        List<ItemTransactionDt> lstTransactionDt = BusinessLayer.GetItemTransactionDtList(filterExpressionItemTransactionDt, ctx);
                        foreach (ItemTransactionDt itemDt in lstTransactionDt)
                        {
                            itemDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                            itemDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            itemDtDao.Update(itemDt);

                            if (lstItemRequestDt != null)
                            {
                                int countItem = lstItemRequestDt.Where(a => a.ItemID == itemDt.ItemID).Count();
                                if (countItem > 0)
                                {
                                    ItemRequestDt iRequestDt = lstItemRequestDt.Where(a => a.ItemID == itemDt.ItemID).FirstOrDefault();
                                    iRequestDt.ApprovedConsumptionQty += itemDt.Quantity;
                                    iRequestDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    itemReqDtDao.Update(iRequestDt);
                                }
                            }
                        }

                        itemHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                        itemHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        itemHdDao.Update(itemHd);
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
                try
                {
                    string[] listParam = hdnParam.Value.Split(',');
                    foreach (string param in listParam)
                    {
                        int ItemRequestID = Convert.ToInt32(param);
                        ItemTransactionHd entity = itemHdDao.Get(ItemRequestID);
                        entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        itemHdDao.Update(entity);
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