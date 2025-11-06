using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
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
    public partial class ItemDistributionApprovalDetailList : BasePageTrx
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.ITEM_DISTRIBUTION;
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

            hdnDistributionID.Value = Page.Request.QueryString["id"];
            vItemDistributionHd entityItemDistribution = BusinessLayer.GetvItemDistributionHdList(String.Format("DistributionID = '{0}'", Convert.ToInt32(hdnDistributionID.Value)))[0];
            EntityToControl(entityItemDistribution);
        }

        private void EntityToControl(vItemDistributionHd entity)
        {
            hdnDistributionID.Value = entity.DistributionID.ToString();
            txtDistributionNo.Text = entity.DistributionNo;
            txtItemDistributionDate.Text = entity.DeliveryDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtItemDistributionTime.Text = entity.DeliveryTime;
            hdnLocationIDFrom.Value = entity.FromLocationID.ToString();
            txtLocationCode.Text = entity.FromLocationCode;
            txtLocationName.Text = entity.FromLocationName;
            hdnLocationIDTo.Value = entity.ToLocationID.ToString();
            txtLocationCodeTo.Text = entity.ToLocationCode;
            txtLocationNameTo.Text = entity.ToLocationName;
            txtNotes.Text = entity.DeliveryRemarks;
            BindGridView(1, true, ref PageCount);
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vItemDistributionDt entity = e.Row.DataItem as vItemDistributionDt;
                CheckBox chkIsSelected = e.Row.FindControl("chkIsSelected") as CheckBox;
                if (entity.GCItemDetailStatus == Constant.TransactionStatus.APPROVED || lstSelectedMember.Contains(entity.ID.ToString()))
                    chkIsSelected.Checked = true;
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {

            string filterExpression = string.Format("DistributionID = {0} AND IsDeleted = 0", hdnDistributionID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemDistributionDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            lstSelectedMember = hdnSelectedMember.Value.Split(',');
            List<vItemDistributionDt> lstEntity = BusinessLayer.GetvItemDistributionDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
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
            ItemDistributionDtDao itemDtDao = new ItemDistributionDtDao(ctx);
            ItemRequestDtDao itemReqDtDao = new ItemRequestDtDao(ctx);
            try
            {
                string filterExpressionSetDefaultDt = String.Format("DistributionID = {0} AND IsDeleted = 0", hdnDistributionID.Value);
                List<ItemDistributionDt> lstItemDistributionDtSetDefault = BusinessLayer.GetItemDistributionDtList(filterExpressionSetDefaultDt, ctx);

                string filterExpressionItemDistributionDt = String.Format("ID IN ({0})", hdnSelectedMember.Value.Substring(1));
                List<ItemDistributionDt> lstItemDistributionDt = BusinessLayer.GetItemDistributionDtList(filterExpressionItemDistributionDt, ctx);

                List<ItemRequestDt> lstItemRequestDt = null;
                ItemDistributionHd itemTransHd = BusinessLayer.GetItemDistributionHd(Convert.ToInt32(hdnDistributionID.Value));
                if (itemTransHd.ItemRequestID != null && itemTransHd.ItemRequestID != 0)
                {
                    string filterExpressionItemRequestHd = String.Format("ItemRequestID = {0} AND IsDeleted = 0 AND GCItemDetailStatus != '{1}'", itemTransHd.ItemRequestID, Constant.TransactionStatus.VOID);
                    lstItemRequestDt = BusinessLayer.GetItemRequestDtList(filterExpressionItemRequestHd, ctx);
                }

                foreach (ItemDistributionDt itemDt in lstItemDistributionDtSetDefault)
                {
                    if (itemDt.GCItemDetailStatus == Constant.TransactionStatus.APPROVED && lstItemDistributionDt.Where(p => p.ID == itemDt.ID).Count() < 1)
                    {
                        itemDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                        itemDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        itemDtDao.Update(itemDt);
                    }
                }
                
                foreach (ItemDistributionDt itemDt in lstItemDistributionDt)
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
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            itemReqDtDao.Update(iRequestDt);
                        }
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