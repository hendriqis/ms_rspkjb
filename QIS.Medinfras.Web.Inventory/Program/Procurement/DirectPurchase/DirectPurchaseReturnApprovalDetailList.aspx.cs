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
    public partial class DirectPurchaseReturnApprovalDetailList : BasePageTrx
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.DIRECT_PURCHASE_RETURN_APPROVAL;
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

            List<StandardCode> listStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}') AND IsDeleted = 0", Constant.StandardCode.PURCHASE_RETURN_TYPE, Constant.StandardCode.PURCHASE_RETURN_REASON));
            Methods.SetComboBoxField<StandardCode>(cboReturnType, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.PURCHASE_RETURN_TYPE).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
            
            hdnDirectPurchaseReturnID.Value = Page.Request.QueryString["id"];
            vDirectPurchaseReturnHd entityPOHD = BusinessLayer.GetvDirectPurchaseReturnHdList(String.Format("DirectPurchaseReturnID = '{0}'", Convert.ToInt32(hdnDirectPurchaseReturnID.Value)))[0];
            EntityToControl(entityPOHD);
        }

        private void EntityToControl(vDirectPurchaseReturnHd entity)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;
            }

            hdnDirectPurchaseReturnID.Value = entity.DirectPurchaseReturnID.ToString();
            txtDirectPurchaseReturnNo.Text = entity.DirectPurchaseReturnNo;
            hdnDirectPurchaseID.Value = entity.DirectPurchaseID.ToString();
            txtDirectPurchaseNo.Text = entity.DirectPurchaseNo;
            txtPurchaseReturnDate.Text = entity.ReturnDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            if (entity.ReferenceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
                txtReferenceDate.Text = entity.ReferenceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            else
                txtReferenceDate.Text = "";
            txtReferenceNo.Text = entity.ReferenceNo;
            hdnSupplierID.Value = entity.BusinessPartnerID.ToString();
            txtSupplierCode.Text = entity.BusinessPartnerCode;
            txtSupplierName.Text = entity.SupplierName;
            txtReferenceNo.Text = entity.ReferenceNo;
            hdnLocationID.Value = entity.LocationID.ToString();
            txtLocationCode.Text = entity.LocationCode;
            txtLocationName.Text = entity.LocationName;
            cboReturnType.Value = entity.GCDirectPurchaseReturnType.ToString();
            txtNotes.Text = entity.Remarks;
            txtFinalDiscountInPercentage.Text = entity.FinalDiscount.ToString();
            chkPPN.Checked = entity.IsIncludeVAT;
            txtTotalRetur.Text = entity.TransactionAmount.ToString();

            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = "1 = 0";
            if (hdnDirectPurchaseReturnID.Value != "")
                filterExpression = string.Format("DirectPurchaseReturnID = {0} AND GCItemDetailStatus != '{1}'", hdnDirectPurchaseReturnID.Value, Constant.TransactionStatus.VOID);
            
            List<vDirectPurchaseReturnDt> lstEntity = BusinessLayer.GetvDirectPurchaseReturnDtList(filterExpression, int.MaxValue, 1);
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
                    vDirectPurchaseReturnHd entityPOHD = BusinessLayer.GetvDirectPurchaseReturnHdList(String.Format("DirectPurchaseReturnID = '{0}'", Convert.ToInt32(hdnDirectPurchaseReturnID.Value)))[0];
                    EntityToControl(entityPOHD);

                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

    }
}