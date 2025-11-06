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
    public partial class ItemRequestApprovalList : BasePageTrx
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.ITEM_REQUEST_APPROVAL;
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
            filterExpression += String.Format("GCTransactionStatus = '{0}'", Constant.TransactionStatus.WAIT_FOR_APPROVAL);
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
                int rowCount = BusinessLayer.GetvItemRequestHd1RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vItemRequestHd1> lstEntity = BusinessLayer.GetvItemRequestHd1List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "TransactionDate DESC");
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
                ItemRequestHdDao itemHdDao = new ItemRequestHdDao(ctx);
                ItemRequestDtDao itemDtDao = new ItemRequestDtDao(ctx);
                try
                {
                    string filterExpressionItemRequestHd = String.Format("ItemRequestID IN ({0})", hdnParam.Value);
                    List<ItemRequestHd> lstItemRequestHd = BusinessLayer.GetItemRequestHdList(filterExpressionItemRequestHd);
                    foreach (ItemRequestHd itemHd in lstItemRequestHd)
                    {
                        itemHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                        itemHd.ApprovedDate = DateTime.Now;
                        itemHd.ApprovedBy = AppSession.UserLogin.UserID;
                        itemHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        itemHdDao.Update(itemHd);
                    }

                    string filterExpressionItemRequestDt = String.Format("ItemRequestID IN ({0}) AND IsDeleted = 0", hdnParam.Value);
                    List<ItemRequestDt> lstItemRequestDt = BusinessLayer.GetItemRequestDtList(filterExpressionItemRequestDt, ctx);
                    foreach (ItemRequestDt itemDt in lstItemRequestDt)
                    {
                        itemDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                        itemDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        itemDtDao.Update(itemDt);
                    }

                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    result = false;
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
                ItemRequestHdDao entityDao = new ItemRequestHdDao(ctx);
                ItemRequestDtDao itemDtDao = new ItemRequestDtDao(ctx);
                try
                {
                    string[] listParam = hdnParam.Value.Split(',');
                    foreach (string param in listParam)
                    {
                        int ItemRequestID = Convert.ToInt32(param);
                        ItemRequestHd entity = entityDao.Get(ItemRequestID);
                        entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDao.Update(entity);

                        string filterExpressionItemRequestDt = String.Format("ItemRequestID IN ({0}) AND IsDeleted = 0", ItemRequestID);
                        List<ItemRequestDt> lstItemRequestDt = BusinessLayer.GetItemRequestDtList(filterExpressionItemRequestDt, ctx);
                        foreach (ItemRequestDt itemDt in lstItemRequestDt)
                        {
                            itemDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                            itemDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            itemDtDao.Update(itemDt);
                        }
                    }
                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    result = false;
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