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
    public partial class PurchaseRequestApprovalDetailList : BasePageTrx
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;
        private string[] lstDeclinedMember = null;

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

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = false;
        }

        protected override void InitializeDataControl()
        {
            MPTrx master = (MPTrx)Master;
            menu = ((MPMain)master.Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

            hdnRequestID.Value = Page.Request.QueryString["id"];
            vPurchaseRequestHd entityItemRequest = BusinessLayer.GetvPurchaseRequestHdList(String.Format("PurchaseRequestID = '{0}'", Convert.ToInt32(hdnRequestID.Value)))[0];
            EntityToControl(entityItemRequest);
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            if (entityItemRequest.IsUrgent)
            {
                tdUrgent.Style.Remove("display");
            }
            else
            {
                tdUrgent.Style.Add("display", "none");
            }
        }

        private void EntityToControl(vPurchaseRequestHd entity)
        {
            hdnRequestID.Value = entity.PurchaseRequestID.ToString();
            txtOrderNo.Text = entity.PurchaseRequestNo;
            txtItemOrderDate.Text = entity.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtItemOrderTime.Text = entity.TransactionTime;
            hdnLocationIDFrom.Value = entity.FromLocationID.ToString();
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
                vPurchaseRequestDt entity = e.Row.DataItem as vPurchaseRequestDt;
                CheckBox chkIsSelected = e.Row.FindControl("chkIsSelected") as CheckBox;
                CheckBox chkIsDeclined = e.Row.FindControl("chkIsDeclined") as CheckBox;
                if (entity.GCItemDetailStatus == Constant.TransactionStatus.APPROVED || lstSelectedMember.Contains(entity.ID.ToString()))
                    chkIsSelected.Checked = true;
                if (entity.GCItemDetailStatus == Constant.TransactionStatus.OPEN || lstDeclinedMember.Contains(entity.ID.ToString()))
                    chkIsDeclined.Checked = true;
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnRequestID.Value != "")
                filterExpression = string.Format("PurchaseRequestID = {0} AND IsDeleted = 0", hdnRequestID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPurchaseRequestDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            lstSelectedMember = hdnSelectedMember.Value.Split(',');
            lstDeclinedMember = hdnDeclinedMember.Value.Split(',');
            List<vPurchaseRequestDt> lstEntity = BusinessLayer.GetvPurchaseRequestDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ItemName1 ASC");
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
            PurchaseRequestDtDao purchaseDtDao = new PurchaseRequestDtDao(ctx);
            try
            {
//              string filterExpressionApproved = String.Format("ID IN ({0})", hdnSelectedMember.Value.Substring(1));
                string filterExpressionApproved = "1 = 0";
                string filterExpressionDeclined = "1 = 0";

                if (!String.IsNullOrEmpty(hdnSelectedMember.Value))
                {
                    filterExpressionApproved = String.Format("ID IN ({0})", hdnSelectedMember.Value.Substring(1));
                } 
                if (!String.IsNullOrEmpty(hdnDeclinedMember.Value))
                {
                    filterExpressionDeclined = String.Format("ID IN ({0})", hdnDeclinedMember.Value.Substring(1));
                } 

                List<PurchaseRequestDt> lstPurchaseRequestDtApproved = BusinessLayer.GetPurchaseRequestDtList(filterExpressionApproved);
                List<PurchaseRequestDt> lstPurchaseRequestDtDeclined = BusinessLayer.GetPurchaseRequestDtList(filterExpressionDeclined);

                foreach (PurchaseRequestDt purchaseDt in lstPurchaseRequestDtApproved)
                {
                    purchaseDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                    purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    purchaseDtDao.Update(purchaseDt);
                }

                foreach (PurchaseRequestDt purchaseDt in lstPurchaseRequestDtDeclined)
                {
                    if (purchaseDt.GCItemDetailStatus == Constant.TransactionStatus.OPEN || purchaseDt.GCItemDetailStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
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

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }
    }
}