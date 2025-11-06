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
    public partial class ItemRequestApprovalDetailList : BasePageTrx
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.ITEM_REQUEST;
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

            hdnOrderID.Value = Page.Request.QueryString["id"];
            vItemRequestHd entityItemRequest = BusinessLayer.GetvItemRequestHdList(String.Format("ItemRequestID = '{0}'", Convert.ToInt32(hdnOrderID.Value)))[0];
            EntityToControl(entityItemRequest);
        }

        private void EntityToControl(vItemRequestHd entity)
        {
            hdnOrderID.Value = entity.ItemRequestID.ToString();
            txtOrderNo.Text = entity.ItemRequestNo;
            txtItemOrderDate.Text = entity.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtItemOrderTime.Text = entity.TransactionTime;
            hdnLocationIDFrom.Value = entity.FromLocationCode;
            txtLocationCode.Text = entity.FromLocationCode;
            txtLocationName.Text = entity.FromLocationName;
            hdnLocationIDTo.Value = entity.ToLocationCode;
            txtLocationCodeTo.Text = entity.ToLocationCode;
            txtLocationNameTo.Text = entity.ToLocationName;
            txtNotes.Text = entity.Remarks;
            BindGridView(1, true, ref PageCount);
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vItemRequestDt entity = e.Row.DataItem as vItemRequestDt;
                CheckBox chkIsSelected = e.Row.FindControl("chkIsSelected") as CheckBox;
                if (entity.GCItemDetailStatus == Constant.TransactionStatus.APPROVED || lstSelectedMember.Contains(entity.ID.ToString()))
                    chkIsSelected.Checked = true;
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnOrderID.Value != "")
                filterExpression = string.Format("ItemRequestID = {0} AND IsDeleted = 0", hdnOrderID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemRequestDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            lstSelectedMember = hdnSelectedMember.Value.Split(',');
            List<vItemRequestDt> lstEntity = BusinessLayer.GetvItemRequestDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ItemName1 ASC");
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
                ItemRequestDtDao itemDtDao = new ItemRequestDtDao(ctx);
                try
                {
                    string filterExpressionSetDefaultDt = String.Format("ItemRequestID = {0} AND IsDeleted = 0", hdnOrderID.Value);
                    List<ItemRequestDt> lstItemRequestDtSetDefault = BusinessLayer.GetItemRequestDtList(filterExpressionSetDefaultDt);

                    string filterExpressionItemRequestDt = String.Format("ID IN ({0})", hdnSelectedMember.Value.Substring(1));
                    List<ItemRequestDt> lstItemRequestDt = BusinessLayer.GetItemRequestDtList(filterExpressionItemRequestDt);

                    foreach (ItemRequestDt itemDt in lstItemRequestDtSetDefault)
                    {
                        if (itemDt.GCItemDetailStatus == Constant.TransactionStatus.APPROVED && lstItemRequestDt.Where(p => p.ID == itemDt.ID).Count() < 1)
                        {
                            itemDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                            itemDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            itemDtDao.Update(itemDt);
                        }
                    }

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
                ItemRequestDtDao itemDtDao = new ItemRequestDtDao(ctx);
                try
                {
                    string filterExpressionSetDefaultDt = String.Format("ItemRequestID = {0} AND IsDeleted = 0", hdnOrderID.Value);
                    List<ItemRequestDt> lstItemRequestDtSetDefault = BusinessLayer.GetItemRequestDtList(filterExpressionSetDefaultDt);

                    string filterExpressionItemRequestDt = String.Format("ID IN ({0})", hdnSelectedMember.Value.Substring(1));
                    List<ItemRequestDt> lstItemRequestDt = BusinessLayer.GetItemRequestDtList(filterExpressionItemRequestDt);

                    foreach (ItemRequestDt itemDt in lstItemRequestDtSetDefault)
                    {
                        if (itemDt.GCItemDetailStatus == Constant.TransactionStatus.APPROVED && lstItemRequestDt.Where(p => p.ID == itemDt.ID).Count() < 1)
                        {
                            itemDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                            itemDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            itemDtDao.Update(itemDt);
                        }
                    }

                    foreach (ItemRequestDt itemDt in lstItemRequestDt)
                    {
                        itemDt.GCItemDetailStatus = Constant.TransactionStatus.VOID;
                        itemDt.IsDeleted = true;
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
        }
    }
}