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
    public partial class StockTakingApprovalDetailList : BasePageTrx
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;
        private string[] lstDeclinedMember = null;

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

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = false;
        }

        protected override void InitializeDataControl()
        {
            MPTrx master = (MPTrx)Master;
            menu = ((MPMain)master.Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

            hdnStockOpnameID.Value = Page.Request.QueryString["id"];
            vStockTakingHd entityHd = BusinessLayer.GetvStockTakingHdList(String.Format("StockTakingID = '{0}'", Convert.ToInt32(hdnStockOpnameID.Value)))[0];
            EntityToControl(entityHd);
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        private void EntityToControl(vStockTakingHd entity)
        {
            hdnStockOpnameID.Value = entity.StockTakingID.ToString();
            txtOrderNo.Text = entity.StockTakingNo;
            txtItemOrderDate.Text = entity.FormDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnLocationIDFrom.Value = entity.LocationID.ToString();
            txtLocationCode.Text = entity.LocationCode;
            txtLocationName.Text = entity.LocationName;
            txtNotes.Text = entity.Remarks;
            txtUserName.Text = entity.CreatedByName;
            BindGridView(1, true, ref PageCount);
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vStockTakingDt entity = e.Row.DataItem as vStockTakingDt;
                CheckBox chkIsSelected = e.Row.FindControl("chkIsSelected") as CheckBox;
                CheckBox chkIsDeclined = e.Row.FindControl("chkIsDeclined") as CheckBox;
                if (entity.GCItemDetailStatus == Constant.TransactionStatus.APPROVED || lstSelectedMember.Contains(entity.ItemID.ToString()) || entity.QuantityAdjustment == 0)
                    chkIsSelected.Checked = true;
                if (entity.GCItemDetailStatus == Constant.TransactionStatus.OPEN || lstDeclinedMember.Contains(entity.ItemID.ToString()) || entity.QuantityAdjustment != 0)
                    chkIsDeclined.Checked = true;

                if (entity.QuantityAdjustment == 0)
                {
                    chkIsSelected.Checked = true;
                }

                if (entity.QuantityAdjustment != 0)
                {
                    e.Row.BackColor = System.Drawing.Color.LightPink;
                }
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnStockOpnameID.Value != "")
                filterExpression = string.Format("StockTakingID = {0} AND GCItemDetailStatus NOT IN ('{1}','{2}')", hdnStockOpnameID.Value, Constant.TransactionStatus.VOID, Constant.TransactionStatus.OPEN);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvStockTakingDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            lstSelectedMember = hdnSelectedMember.Value.Split(',');
            lstDeclinedMember = hdnDeclinedMember.Value.Split(',');
            List<vStockTakingDt> lstEntity = BusinessLayer.GetvStockTakingDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
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
            StockTakingDtDao entityDtDao = new StockTakingDtDao(ctx);
            StockTakingHdDao entityHdDao = new StockTakingHdDao(ctx);

            try
            {
                string filterExpressionApproved = "1 = 0";
                string filterExpressionDeclined = "1 = 0";

                if (!String.IsNullOrEmpty(hdnSelectedMember.Value))
                {
                    filterExpressionApproved = String.Format("ItemID IN ({0}) AND StockTakingID = {1}", hdnSelectedMember.Value.Substring(1), hdnStockOpnameID.Value);
                }
                if (!String.IsNullOrEmpty(hdnDeclinedMember.Value))
                {
                    filterExpressionDeclined = String.Format("ItemID IN ({0}) AND StockTakingID = {1}", hdnDeclinedMember.Value.Substring(1), hdnStockOpnameID.Value);
                }
                //QtyAdjustment == 0 -> Approve
                //QtyAdjustment > 0 -> Open (SELISIH)

                List<StockTakingDt> lstStockDtApproved = BusinessLayer.GetStockTakingDtList(filterExpressionApproved);
                List<StockTakingDt> lstStockDtDeclined = BusinessLayer.GetStockTakingDtList(filterExpressionDeclined);

                foreach (StockTakingDt entityDt in lstStockDtApproved)
                {
                    entityDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(entityDt);
                }

                foreach (StockTakingDt entityDt in lstStockDtDeclined)
                {
                    if (entityDt.GCItemDetailStatus == Constant.TransactionStatus.OPEN || entityDt.GCItemDetailStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                    {
                        entityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entityDt);
                    }
                }

                if (lstStockDtDeclined.Count > 0)
                {
                    StockTakingHd entityHd = entityHdDao.Get(lstStockDtDeclined.FirstOrDefault().StockTakingID);
                    entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entityHd);
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

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }
    }
}