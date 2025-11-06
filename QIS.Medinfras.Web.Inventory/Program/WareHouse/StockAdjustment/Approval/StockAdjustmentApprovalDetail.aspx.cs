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
    public partial class StockAdjustmentApprovalDetail : BasePageTrx
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.ITEM_ADJUSTMENT_APPROVAL;
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

            hdnAdjustmentID.Value = Page.Request.QueryString["id"];
            vItemTransactionHd entityItemRequest = BusinessLayer.GetvItemTransactionHdList(String.Format("TransactionID = {0}", Convert.ToInt32(hdnAdjustmentID.Value)))[0];
            EntityToControl(entityItemRequest);
        }

        private void EntityToControl(vItemTransactionHd entity)
        {
            hdnAdjustmentID.Value = entity.TransactionID.ToString();
            txtAdjustmentNo.Text = entity.TransactionNo;
            txtAdjustmentDate.Text = entity.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnLocationID.Value = entity.FromLocationID.ToString();
            txtLocationCode.Text = entity.FromLocationCode;
            txtLocationName.Text = entity.FromLocationName;

            txtAdjustmentType.Text = entity.AdjustmentType;
            txtRemarks.Text = entity.Remarks;
            BindGridView(1, true, ref PageCount);
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vItemTransactionDt entity = e.Row.DataItem as vItemTransactionDt;
                CheckBox chkIsSelected = e.Row.FindControl("chkIsSelected") as CheckBox;
                if (entity.GCItemDetailStatus == Constant.TransactionStatus.APPROVED || lstSelectedMember.Contains(entity.ID.ToString()))
                    chkIsSelected.Checked = true;
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnAdjustmentID.Value != "")
                filterExpression = string.Format("TransactionID = {0} AND GCItemDetailStatus != '{1}'", hdnAdjustmentID.Value, Constant.TransactionStatus.VOID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemTransactionDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            lstSelectedMember = hdnSelectedMember.Value.Split(',');
            List<vItemTransactionDt> lstEntity = BusinessLayer.GetvItemTransactionDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ItemName1 ASC");
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
            ItemTransactionDtDao itemDtDao = new ItemTransactionDtDao(ctx);
            try
            {
                string filterExpressionSetDefaultDt = String.Format("TransactionID = {0} AND GCItemDetailStatus != '{1}'", hdnAdjustmentID.Value, Constant.TransactionStatus.VOID);
                List<ItemTransactionDt> lstItemTransactionDtSetDefault = BusinessLayer.GetItemTransactionDtList(filterExpressionSetDefaultDt);

                string filterExpressionItemTransactionDt = String.Format("ID IN ({0})", hdnSelectedMember.Value.Substring(1));
                List<ItemTransactionDt> lstItemTransactionDt = BusinessLayer.GetItemTransactionDtList(filterExpressionItemTransactionDt);

                foreach (ItemTransactionDt itemDt in lstItemTransactionDtSetDefault)
                {
                    if (itemDt.GCItemDetailStatus == Constant.TransactionStatus.APPROVED && lstItemTransactionDt.Where(p => p.ID == itemDt.ID).Count() < 1)
                    {
                        itemDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                        itemDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        itemDtDao.Update(itemDt);
                    }
                }

                foreach (ItemTransactionDt itemDt in lstItemTransactionDt)
                {
                    itemDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                    itemDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    itemDtDao.Update(itemDt);
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