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
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class DirectPurchaseApprovalDetailList : BasePageTrx
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.DIRECT_PURCHASE_APPROVAL;
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
        
        protected string GetVATPercentageLabel()
        {
            return hdnVATPercentage.Value;
        }

        protected override void InitializeDataControl()
        {
            MPTrx master = (MPTrx)Master;
            menu = ((MPMain)master.Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

            hdnVATPercentage.Value = BusinessLayer.GetSettingParameter(Constant.SettingParameter.VAT_PERCENTAGE).ParameterValue;

            List<StandardCode> listStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.DIRECT_PURCHASE_TYPE));
            Methods.SetComboBoxField<StandardCode>(cboDirectPurchaseType, listStandardCode.ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
            cboDirectPurchaseType.SelectedIndex = 0;

            hdnDirectPurchaseID.Value = Page.Request.QueryString["id"];
            vDirectPurchaseHd entityPOHD = BusinessLayer.GetvDirectPurchaseHdList(String.Format("DirectPurchaseID = '{0}'", Convert.ToInt32(hdnDirectPurchaseID.Value)))[0];
            EntityToControl(entityPOHD);
        }

        private void EntityToControl(vDirectPurchaseHd entity)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;
            }

            hdnDirectPurchaseID.Value = entity.DirectPurchaseID.ToString();
            txtDirectPurchaseNo.Text = entity.DirectPurchaseNo;
            txtDirectPurchaseDate.Text = entity.PurchaseDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            if (entity.ReferenceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != "01-01-1900")
                txtReferenceDate.Text = entity.ReferenceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            else
                txtReferenceDate.Text = "";
            txtReferenceNo.Text = entity.ReferenceNo;

            hdnProductLineID.Value = entity.ProductLineID.ToString();
            txtProductLineCode.Text = entity.ProductLineCode;
            txtProductLineName.Text = entity.ProductLineName;
            hdnProductLineItemType.Value = entity.GCItemType;

            hdnSupplierID.Value = entity.BusinessPartnerID.ToString();
            txtSupplierCode.Text = entity.BusinessPartnerCode;
            txtSupplierName.Text = entity.BusinessPartnerName;
            hdnLocationID.Value = entity.LocationID.ToString();
            txtLocationCode.Text = entity.LocationCode;
            txtLocationName.Text = entity.LocationName;
            hdnLocationItemGroupID.Value = entity.LocationItemGroupID.ToString();
            chkIsUrgent.Checked = entity.IsUrgent;
            cboDirectPurchaseType.Value = entity.GCDirectPurchaseType;
            txtNotes.Text = entity.Remarks;
            chkPPN.Checked = entity.IsIncludeVAT;

            if (entity.FinalDiscount > 0)
            {
                rblDiscountType.SelectedIndex = 0;
                txtFinalDiscountInPercentage.Text = entity.FinalDiscount.ToString();
            }
            else if (entity.FinalDiscountAmount > 0)
            {
                rblDiscountType.SelectedIndex = 1;
                txtFinalDiscount.Text = entity.FinalDiscountAmount.ToString();
            }
            else
            {
                rblDiscountType.SelectedIndex = 0;
                txtFinalDiscount.Text = "";
                txtFinalDiscountInPercentage.Text = "";
            }

            txtTotalPurchase.Text = entity.TransactionAmount.ToString();

            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = "1 = 0";
            if (hdnDirectPurchaseID.Value != "")
                filterExpression = string.Format("DirectPurchaseID = {0} AND GCItemDetailStatus != '{1}'", hdnDirectPurchaseID.Value, Constant.TransactionStatus.VOID);
            
            List<vDirectPurchaseDt> lstEntity = BusinessLayer.GetvDirectPurchaseDtList(filterExpression, int.MaxValue, 1);
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
                    BindGridView();
                    result = "changepage";
                }
                else // refresh
                {
                    vDirectPurchaseHd entityPOHD = BusinessLayer.GetvDirectPurchaseHdList(String.Format("DirectPurchaseID = '{0}'", Convert.ToInt32(hdnDirectPurchaseID.Value)))[0];
                    EntityToControl(entityPOHD);

                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

    }
}