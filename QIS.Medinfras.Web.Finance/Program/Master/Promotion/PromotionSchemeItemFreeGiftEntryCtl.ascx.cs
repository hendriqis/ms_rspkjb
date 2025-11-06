using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class PromotionSchemeItemFreeGiftEntryCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnPromotionSchemeID.Value = param;
            PromotionScheme entity = BusinessLayer.GetPromotionScheme(Convert.ToInt32(hdnPromotionSchemeID.Value));
            txtPromotionScheme.Text = string.Format("{0} - {1}", entity.PromotionSchemeCode, entity.PromotionSchemeName);          

            List<StandardCode> lstCodes = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}') AND IsActive = 1 AND IsDeleted = 0 AND StandardCodeID IN ('{1}','{2}','{3}')",
    Constant.StandardCode.ITEM_TYPE, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM));
            Methods.SetComboBoxField(cboItemType, lstCodes, "StandardCodeName", "StandardCodeID");

            BindGridView();

            txtItemCode.Attributes.Add("validationgroup", "mpEntryPopup");
            txtQty.Attributes.Add("validationgroup", "mpEntryPopup");
            txtItemUnit.Attributes.Add("validationgroup", "mpEntryPopup");
            cboItemType.Attributes.Add("validationgroup", "mpEntryPopup");

        }

        private void BindGridView()
        {
            lvwView.DataSource = BusinessLayer.GetvPromotionSchemeItemFreeGiftList(string.Format("PromotionSchemeID = {0} AND IsDeleted = 0 ORDER BY GCItemType,ItemName1 ASC", hdnPromotionSchemeID.Value));
            lvwView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }
            
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string param = e.Parameter;

            string result = param + "|";
            string errMessage = "";

            if (param == "save")
            {
                if (hdnID.Value.ToString() != "")
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param == "delete")
            {
                if (OnDeleteRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(PromotionSchemeItemFreeGift entity)
        {
            entity.ItemID = Convert.ToInt32(hdnItemID.Value);
            entity.GCItemType = cboItemType.Value.ToString();
            entity.GCItemUnit = hdnGCItemUnit.Value;
            entity.Quantity = Convert.ToDecimal(txtQty.Text);
            entity.ConversionFactor = Convert.ToDecimal(hdnConversionFactor.Value);
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                PromotionSchemeItemFreeGift entity = new PromotionSchemeItemFreeGift();
                ControlToEntity(entity);
                entity.PromotionSchemeID = Convert.ToInt32(hdnPromotionSchemeID.Value);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertPromotionSchemeItemFreeGift(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                PromotionSchemeItemFreeGift entity = BusinessLayer.GetPromotionSchemeItemFreeGift(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePromotionSchemeItemFreeGift(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            try
            {
                PromotionSchemeItemFreeGift entity = BusinessLayer.GetPromotionSchemeItemFreeGift(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePromotionSchemeItemFreeGift(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
    }
}