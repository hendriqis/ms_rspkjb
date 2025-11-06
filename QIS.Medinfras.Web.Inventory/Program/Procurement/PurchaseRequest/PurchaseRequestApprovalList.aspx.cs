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
    public partial class PurchaseRequestApprovalList : BasePageTrx
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.PURCHASE_REQUEST_APPROVAL;
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
            MPTrx2 master = (MPTrx2)Master;
            menu = ((MPMain)master.Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

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
                filterExpression += string.Format(" AND FromLocationID IN (SELECT LocationID FROM LocationUser WHERE UserID = {0} AND IsDeleted = 0)", AppSession.UserLogin.UserID);
            else
            {
                count = BusinessLayer.GetLocationUserRoleRowCount(string.Format("RoleID IN (SELECT RoleID FROM UserInRole WHERE UserID = {0} AND HealthcareID = '{1}') AND IsDeleted = 0", AppSession.UserLogin.UserID, AppSession.UserLogin.HealthcareID));
                if (count > 0)
                    filterExpression += string.Format(" AND FromLocationID IN (SELECT LocationID FROM LocationUserRole WHERE RoleID IN (SELECT RoleID FROM UserInRole WHERE UserID = {0} AND HealthcareID = '{1}') AND IsDeleted = 0)", AppSession.UserLogin.UserID, AppSession.UserLogin.HealthcareID);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPurchaseRequestHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPurchaseRequestHd> lstEntity = BusinessLayer.GetvPurchaseRequestHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex,"TransactionDate DESC");
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
                PurchaseRequestHdDao purchaseHdDao = new PurchaseRequestHdDao(ctx);
                PurchaseRequestDtDao purchaseDtDao = new PurchaseRequestDtDao(ctx);
                try
                {
                    string filterExpressionPurchaseRequestHd = String.Format("PurchaseRequestID IN ({0})", hdnParam.Value);
                    string filterExpressionPurchaseRequestDt = String.Format("PurchaseRequestID IN ({0}) AND IsDeleted = 0", hdnParam.Value);
                    List<PurchaseRequestDt> lstPurchaseRequestDt = BusinessLayer.GetPurchaseRequestDtList(filterExpressionPurchaseRequestDt);
                    foreach (PurchaseRequestDt purchaseDt in lstPurchaseRequestDt)
                    {
                        if (purchaseDt.GCItemDetailStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                        {
                            purchaseDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                            purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            purchaseDtDao.Update(purchaseDt);
                        }
                        else if (purchaseDt.GCItemDetailStatus == Constant.TransactionStatus.OPEN)
                        {
                            result = false;
                            errMessage = "Maaf, transaksi tidak dapat diproses karena masih ada item dengan status OPEN";
                        }
                    }

                    if (result)
                    {
                        List<PurchaseRequestHd> lstPurchaseRequestHd = BusinessLayer.GetPurchaseRequestHdList(filterExpressionPurchaseRequestHd);
                        foreach (PurchaseRequestHd purchaseHd in lstPurchaseRequestHd)
                        {
                            purchaseHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                            purchaseHd.ApprovedBy = AppSession.UserLogin.UserID;
                            purchaseHd.ApprovedDate = DateTime.Now;
                            purchaseHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            purchaseHdDao.Update(purchaseHd);
                        }
                        ctx.CommitTransaction();
                    }
                    else ctx.RollBackTransaction();
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
                PurchaseRequestHdDao entityDao = new PurchaseRequestHdDao(ctx);
                PurchaseRequestDtDao purchaseDtDao = new PurchaseRequestDtDao(ctx);
                try
                {
                    string[] listParam = hdnParam.Value.Split(',');
                    foreach (string param in listParam)
                    {
                        int PurchaseRequestID = Convert.ToInt32(param);
                        PurchaseRequestHd entity = entityDao.Get(PurchaseRequestID);
                        entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDao.Update(entity);
                    }

                    string filterExpressionPurchaseRequestHd = String.Format("PurchaseRequestID IN ({0}) AND IsDeleted = 0", hdnParam.Value);
                    List<PurchaseRequestDt> lstPurchaseRequestDt = BusinessLayer.GetPurchaseRequestDtList(filterExpressionPurchaseRequestHd);
                    foreach (PurchaseRequestDt purchaseDt in lstPurchaseRequestDt)
                    {
                        if (purchaseDt.GCItemDetailStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                        {
                            purchaseDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                            purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            purchaseDtDao.Update(purchaseDt);
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

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }
    }
}