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
    public partial class PromotionSchemeItemGroupEntryCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnPromotionSchemeID.Value = param;
            PromotionScheme entity = BusinessLayer.GetPromotionScheme(Convert.ToInt32(hdnPromotionSchemeID.Value));
            txtPromotionScheme.Text = string.Format("{0} - {1}", entity.PromotionSchemeCode, entity.PromotionSchemeName);

            List<SettingParameter> lstParam = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}','{2}')",
    Constant.SettingParameter.TARIFF_COMPONENT1_TEXT, Constant.SettingParameter.TARIFF_COMPONENT2_TEXT, Constant.SettingParameter.TARIFF_COMPONENT3_TEXT));
           
            hdnTariffComp1Text.Value = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.TARIFF_COMPONENT1_TEXT).ParameterValue;
            hdnTariffComp2Text.Value = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.TARIFF_COMPONENT2_TEXT).ParameterValue;
            hdnTariffComp3Text.Value = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.TARIFF_COMPONENT3_TEXT).ParameterValue;

            List<StandardCode> lstCodes = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}') AND IsActive = 1 AND IsDeleted = 0 AND StandardCodeID IN ('{1}','{2}','{3}','{4}')",
    Constant.StandardCode.ITEM_TYPE, Constant.ItemType.PELAYANAN, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM));
            Methods.SetComboBoxField(cboItemType, lstCodes, "StandardCodeName", "StandardCodeID");

            BindGridView();

            txtItemGroupCode.Attributes.Add("validationgroup", "mpEntryPopup");
            cboItemType.Attributes.Add("validationgroup", "mpEntryPopup");
        }

        private void BindGridView()
        {
            lvwView.DataSource = BusinessLayer.GetvPromotionSchemeItemGroupList(string.Format("PromotionSchemeID = {0} AND IsDeleted = 0 ORDER BY GCItemType,ItemGroupName1 ASC", hdnPromotionSchemeID.Value));
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

        private void ControlToEntity(PromotionSchemeItemGroup entity)
        {
            entity.ItemGroupID = Convert.ToInt32(hdnItemGroupID.Value);
            entity.GCItemType = cboItemType.Value.ToString();
            entity.IsUsePromotionPrice = chkIsUsePromotionPrice.Checked;
            entity.Tariff = txtTariff.Text == "" ? 0 : Convert.ToDecimal(txtTariff.Text);
            entity.TariffComp1 = txtTariffComp1.Text == "" ? 0 : Convert.ToDecimal(txtTariffComp1.Text);
            entity.TariffComp2 = txtTariffComp2.Text == "" ? 0 : Convert.ToDecimal(txtTariffComp2.Text);
            entity.TariffComp3 = txtTariffComp3.Text == "" ? 0 : Convert.ToDecimal(txtTariffComp3.Text);
            entity.IsDiscountInPercentage = chkIsDiscountInPercentage.Checked;
            entity.DiscountAmount = txtDiscountAmount.Text == "" ? 0 : Convert.ToDecimal(txtDiscountAmount.Text);


            if (!string.IsNullOrEmpty(hdnRevenueSharingID.Value) && hdnRevenueSharingID.Value != "0")
            {
                entity.RevenueSharingID = Convert.ToInt32(hdnRevenueSharingID.Value);
            }
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                PromotionSchemeItemGroup entity = new PromotionSchemeItemGroup();
                ControlToEntity(entity);
                entity.PromotionSchemeID = Convert.ToInt32(hdnPromotionSchemeID.Value);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertPromotionSchemeItemGroup(entity);
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
                PromotionSchemeItemGroup entity = BusinessLayer.GetPromotionSchemeItemGroup(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePromotionSchemeItemGroup(entity);
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
                PromotionSchemeItemGroup entity = BusinessLayer.GetPromotionSchemeItemGroup(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePromotionSchemeItemGroup(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        protected string GetTariffComponent1Text()
        {
            return string.IsNullOrEmpty(hdnTariffComp1Text.Value) ? "Sarana" : hdnTariffComp1Text.Value;
        }

        protected string GetTariffComponent2Text()
        {
            return string.IsNullOrEmpty(hdnTariffComp2Text.Value) ? "Pelayanan" : hdnTariffComp2Text.Value;
        }

        protected string GetTariffComponent3Text()
        {
            return string.IsNullOrEmpty(hdnTariffComp3Text.Value) ? "Lain-lain" : hdnTariffComp3Text.Value;
        }
    }
}