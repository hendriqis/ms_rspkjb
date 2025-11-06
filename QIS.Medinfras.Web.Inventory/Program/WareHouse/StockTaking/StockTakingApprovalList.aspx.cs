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
    public partial class StockTakingApprovalList : BasePageTrx
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.STOCK_OPNAME_APPROVAL;
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
            BindGridView(1, true, ref PageCount);
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += String.Format("GCTransactionStatus = '{0}'", Constant.TransactionStatus.WAIT_FOR_APPROVAL);
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
                int rowCount = BusinessLayer.GetvStockTakingHd1RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vStockTakingHd1> lstEntity = BusinessLayer.GetvStockTakingHd1List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "FormDate DESC");
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

        protected void cbpViewDt_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridViewDt(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnID.Value != "")
                filterExpression = string.Format("StockTakingID = {0} AND GCItemDetailStatus NOT IN ('{1}','{2}')", hdnID.Value, Constant.TransactionStatus.VOID, Constant.TransactionStatus.OPEN);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvStockTakingDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vStockTakingDt> lstEntity = BusinessLayer.GetvStockTakingDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "BinLocationCode ASC,ItemName1 ASC");
            grdViewDt.DataSource = lstEntity;
            grdViewDt.DataBind();
        }

        protected void grdViewDt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vStockTakingDt entity = e.Row.DataItem as vStockTakingDt;
                if (entity.QuantityAdjustment != 0)
                {
                    e.Row.BackColor = System.Drawing.Color.LightPink;
                }
            }
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            if (type == "approve")
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                StockTakingHdDao stockTakingHdDao = new StockTakingHdDao(ctx);
                StockTakingDtDao stockTakingDtDao = new StockTakingDtDao(ctx);
                try
                {
                    string filterExpressionHd = String.Format("StockTakingID IN ({0})", hdnParam.Value);
                    string filterExpressionDt = String.Format("StockTakingID IN ({0}) AND GCItemDetailStatus = '{1}' AND QuantityAdjustment != 0", hdnParam.Value, Constant.TransactionStatus.WAIT_FOR_APPROVAL);
                    List<StockTakingDt> lstDt = BusinessLayer.GetStockTakingDtList(filterExpressionDt, ctx);

                    int countOutOfStockMinus = lstDt.Where(a => a.QuantityEND < 0).Count();
                    if (countOutOfStockMinus == 0)
                    {
                        foreach (StockTakingDt entityDt in lstDt)
                        {
                            if (entityDt.GCItemDetailStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                            {
                                entityDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                stockTakingDtDao.Update(entityDt);
                            }
                            else
                            {
                                result = false;
                                errMessage = "Maaf, Stock Opname tidak dapat di-approve karena ada item dengan status NON-PROPOSED.";
                                break;
                            }
                        }

                        if (result)
                        {
                            List<StockTakingHd> lstHd = BusinessLayer.GetStockTakingHdList(filterExpressionHd, ctx);
                            foreach (StockTakingHd entityHd in lstHd)
                            {
                                entityHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                                entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                stockTakingHdDao.Update(entityHd);
                            }
                            ctx.CommitTransaction();
                        }
                        else
                        {
                            ctx.RollBackTransaction();
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = "Stock Opname tidak dapat di-approve karena ada " + countOutOfStockMinus.ToString() + " detail item stok yang minus.";
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
                StockTakingHdDao entityHdDao = new StockTakingHdDao(ctx);
                StockTakingDtDao entityDtDao = new StockTakingDtDao(ctx);
                try
                {
                    string[] listParam = hdnParam.Value.Split(',');
                    foreach (string param in listParam)
                    {
                        int id = Convert.ToInt32(param);
                        StockTakingHd entity = entityHdDao.Get(id);
                        entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityHdDao.Update(entity);
                    }

                    string filterExpression = String.Format("StockTakingID IN ({0}) AND GCItemDetailStatus != '{1}' AND QuantityAdjustment != 0", hdnParam.Value, Constant.TransactionStatus.VOID);
                    List<StockTakingDt> lstStockTakingDt = BusinessLayer.GetStockTakingDtList(filterExpression, ctx);
                    foreach (StockTakingDt stockTakingDt in lstStockTakingDt)
                    {
                        stockTakingDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                        stockTakingDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(stockTakingDt);
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

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }
    }
}