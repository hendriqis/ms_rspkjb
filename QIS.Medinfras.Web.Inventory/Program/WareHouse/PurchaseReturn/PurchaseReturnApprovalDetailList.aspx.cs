using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class PurchaseReturnApprovalDetailList : BasePageTrx
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.PURCHASE_RETURN;
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

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = false;
        }

        protected override void InitializeDataControl()
        {
            MPTrx master = (MPTrx)Master;
            menu = ((MPMain)master.Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

            hdnPurchaseReturnID.Value = Page.Request.QueryString["id"];
            vPurchaseReturnHd entityPurchaseReturn = BusinessLayer.GetvPurchaseReturnHdList(String.Format("PurchaseReturnID = '{0}'", Convert.ToInt32(hdnPurchaseReturnID.Value)))[0];
            EntityToControl(entityPurchaseReturn);
        }

        private void EntityToControl(vPurchaseReturnHd entity)
        {
            hdnPurchaseReturnID.Value = entity.PurchaseReturnID.ToString();
            txtReturnNo.Text = entity.PurchaseReturnNo;
            txtPurchaseReceiveNo.Text = entity.PurchaseReceiveNo;
            txtPurchaseReturnDate.Text = entity.ReturnDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnSupplierID.Value = entity.BusinessPartnerID.ToString();
            txtSupplierCode.Text = entity.BusinessPartnerCode;
            txtSupplierName.Text = entity.SupplierName;
            txtReferenceNo.Text = entity.ReferenceNo;
            txtReferenceDate.Text = entity.ReferenceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnLocationID.Value = entity.LocationID.ToString();
            txtLocationCode.Text = entity.LocationCode;
            txtLocationName.Text = entity.LocationName;
            txtReturnType.Text = entity.PurchaseReturnType;
            chkPPN.Checked = entity.IsIncludeVAT;
            BindGridView(1, true, ref PageCount);
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPurchaseReturnDt entity = e.Row.DataItem as vPurchaseReturnDt;
                CheckBox chkIsSelected = e.Row.FindControl("chkIsSelected") as CheckBox;
                if (entity.GCItemDetailStatus == Constant.TransactionStatus.APPROVED || lstSelectedMember.Contains(entity.ID.ToString()))
                    chkIsSelected.Checked = true;
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnPurchaseReturnID.Value != "")
                filterExpression = string.Format("PurchaseReturnID = {0} AND GCItemDetailStatus <> '{1}'", hdnPurchaseReturnID.Value, Constant.TransactionStatus.VOID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPurchaseReturnDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            lstSelectedMember = hdnSelectedMember.Value.Split(',');
            List<vPurchaseReturnDt> lstEntity = BusinessLayer.GetvPurchaseReturnDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
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
            PurchaseReturnDtDao purchaseReturnDtDao = new PurchaseReturnDtDao(ctx);
            try
            {
                string filterExpressionSetDefaultDt = String.Format("PurchaseReturnID = {0} AND GCItemDetailStatus <> '{1}'", hdnPurchaseReturnID.Value, Constant.TransactionStatus.VOID);
                List<PurchaseReturnDt> lstPurchaseReturnDtSetDefault = BusinessLayer.GetPurchaseReturnDtList(filterExpressionSetDefaultDt);

                List<PurchaseReturnDt> lstPurchaseReturnDt = null;
                 if (hdnSelectedMember.Value != "")
                {
                    string filterExpressionPurchaseReturnDt = String.Format("ID IN ({0})", hdnSelectedMember.Value.Substring(1));
                    lstPurchaseReturnDt = BusinessLayer.GetPurchaseReturnDtList(filterExpressionPurchaseReturnDt);
                }

                foreach (PurchaseReturnDt purchaseDt in lstPurchaseReturnDtSetDefault)
                {
                    if (lstPurchaseReturnDt == null || (lstPurchaseReturnDt != null && lstPurchaseReturnDt.Where(p => p.ID == purchaseDt.ID).Count() < 1))
                    {
                        purchaseDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                        purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        purchaseReturnDtDao.Update(purchaseDt);
                    }
                }

                if (lstPurchaseReturnDt != null)
                {
                    foreach (PurchaseReturnDt purchaseDt in lstPurchaseReturnDt)
                    {
                        purchaseDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                        purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        purchaseReturnDtDao.Update(purchaseDt);
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