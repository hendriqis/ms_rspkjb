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

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class ItemDistributionApprovalList : BasePageTrx
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.ITEM_DISTRIBUTION_APPROVAL;
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
            filterExpression += String.Format("GCDistributionStatus = '{0}'", Constant.DistributionStatus.WAIT_FOR_APPROVAL);
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
                int rowCount = BusinessLayer.GetvItemDistributionHd1RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vItemDistributionHd1> lstEntity = BusinessLayer.GetvItemDistributionHd1List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "DeliveryDate DESC");
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
                ItemDistributionHdDao itemHdDao = new ItemDistributionHdDao(ctx);
                ItemDistributionDtDao itemDtDao = new ItemDistributionDtDao(ctx);
                ItemRequestDtDao itemReqDtDao = new ItemRequestDtDao(ctx);
                try
                {
                    string filterExpressionItemDistributionHd = String.Format("DistributionID IN ({0})", hdnParam.Value);

                    List<ItemDistributionHd> lstItemDistributionHd = BusinessLayer.GetItemDistributionHdList(filterExpressionItemDistributionHd, ctx);
                    foreach (ItemDistributionHd itemHd in lstItemDistributionHd)
                    {
                        itemHd.GCDistributionStatus = Constant.DistributionStatus.ON_DELIVERY;
                        itemHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        itemHdDao.Update(itemHd);

                        List<ItemRequestDt> lstItemRequestDt = null;

                        if (itemHd.ItemRequestID != null && itemHd.ItemRequestID != 0)
                        {
                            string filterExpressionItemRequestHd = String.Format("ItemRequestID = {0} AND IsDeleted = 0 AND GCItemDetailStatus != '{1}'", itemHd.ItemRequestID, Constant.TransactionStatus.VOID);
                            lstItemRequestDt = BusinessLayer.GetItemRequestDtList(filterExpressionItemRequestHd, ctx);
                        }

                        List<ItemDistributionDt> lstItemDistributionDt = BusinessLayer.GetItemDistributionDtList(filterExpressionItemDistributionHd, ctx);
                        foreach (ItemDistributionDt itemDt in lstItemDistributionDt.Where(a => !a.IsDeleted))
                        {
                            itemDt.GCItemDetailStatus = Constant.DistributionStatus.ON_DELIVERY;
                            itemDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            itemDtDao.Update(itemDt);

                            if (lstItemRequestDt != null)
                            {
                                int countItem = lstItemRequestDt.Where(a => a.ItemID == itemDt.ItemID).Count();
                                if (countItem > 0)
                                {
                                    ItemRequestDt iRequestDt = lstItemRequestDt.Where(a => a.ItemID == itemDt.ItemID).FirstOrDefault();
                                    iRequestDt.ApprovedDistributionQty += itemDt.Quantity;
                                    iRequestDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    itemReqDtDao.Update(iRequestDt);
                                }
                            }
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
            else
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                ItemDistributionHdDao entityDao = new ItemDistributionHdDao(ctx);
                try
                {
                    string[] listParam = hdnParam.Value.Split(',');
                    foreach (string param in listParam)
                    {
                        int ItemDistributionID = Convert.ToInt32(param);
                        ItemDistributionHd entity = entityDao.Get(ItemDistributionID);
                        entity.GCDistributionStatus = Constant.DistributionStatus.OPEN;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
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