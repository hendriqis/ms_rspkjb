using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class DonationReceiveApprovalDetailList : BasePageTrx
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.CONSIGNMENT_RECEIVE_APPROVAL;
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
            hdnReceiveID.Value = Page.Request.QueryString["id"];
            vPurchaseReceiveHd entityItemReceive = BusinessLayer.GetvPurchaseReceiveHdList(String.Format("PurchaseReceiveID = '{0}'", Convert.ToInt32(hdnReceiveID.Value)))[0];
            EntityToControl(entityItemReceive);
        }

        private void EntityToControl(vPurchaseReceiveHd entity)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                //watermarkText = entity.TransactionStatusWatermark;
            }
            hdnReceiveID.Value = entity.PurchaseReceiveID.ToString();
            txtPurchaseReceiveNo.Text = entity.PurchaseReceiveNo;
            txtPurchaseReceiveDate.Text = entity.ReceivedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPurchaseReceiveTime.Text = entity.ReceivedTime;
            txtDateReferrence.Text = entity.ReferenceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnSupplierID.Value = entity.SupplierID.ToString();
            txtSupplierCode.Text = entity.SupplierCode;
            txtSupplierName.Text = entity.SupplierName;
            txtFacturNo.Text = entity.ReferenceNo;
            hdnLocationID.Value = entity.LocationID.ToString();
            txtLocationCode.Text = entity.LocationCode;
            txtLocationName.Text = entity.LocationName;
            txtTerm.Text = entity.TermName;
            txtCurrency.Text = entity.CurrencyCode;
            txtKurs.Text = entity.CurrencyRate.ToString();
            
            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnReceiveID.Value != "")
                filterExpression = string.Format("PurchaseReceiveID = {0}", hdnReceiveID.Value);


            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPurchaseReceiveDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            lstSelectedMember = hdnSelectedMember.Value.Split(',');
            List<vPurchaseReceiveDt> lstEntity = BusinessLayer.GetvPurchaseReceiveDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ItemName1 ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();

        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPurchaseReceiveDt entity = e.Row.DataItem as vPurchaseReceiveDt;
                CheckBox chkIsBonus = e.Row.FindControl("chkIsBonus") as CheckBox;
                //CheckBox chkIsAsset = e.Row.FindControl("chkIsAsset") as CheckBox;

                chkIsBonus.Checked = entity.IsBonusItem;
                //chkIsAsset.Checked = entity.is

            }
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
    }
}