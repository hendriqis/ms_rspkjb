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
    public partial class PromotionSchemeDepartmentServiceUnitEntryCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] temp = param.Split('|');

            hdnPromotionSchemeID.Value = temp[0];
            hdnDepartmentID.Value = temp[1];

            PromotionScheme entity = BusinessLayer.GetPromotionScheme(Convert.ToInt32(hdnPromotionSchemeID.Value));
            txtPromotionScheme.Text = string.Format("{0} - {1}", entity.PromotionSchemeCode, entity.PromotionSchemeName);

            Department entity2 = BusinessLayer.GetDepartment(hdnDepartmentID.Value);
            txtDepartment.Text = string.Format("{0} - {1}", entity2.DepartmentID, entity2.DepartmentName);

            List<SettingParameter> lstParam = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}','{2}')",
    Constant.SettingParameter.TARIFF_COMPONENT1_TEXT, Constant.SettingParameter.TARIFF_COMPONENT2_TEXT, Constant.SettingParameter.TARIFF_COMPONENT3_TEXT));
           
            hdnTariffComp1Text.Value = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.TARIFF_COMPONENT1_TEXT).ParameterValue;
            hdnTariffComp2Text.Value = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.TARIFF_COMPONENT2_TEXT).ParameterValue;
            hdnTariffComp3Text.Value = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.TARIFF_COMPONENT3_TEXT).ParameterValue;

            BindGridView();

            txtServiceUnitCode.Attributes.Add("validationgroup", "mpEntryPopup");
        }

        private void BindGridView()
        {
            lvwView.DataSource = BusinessLayer.GetvPromotionSchemeServiceUnitList(string.Format("PromotionSchemeID = {0} AND IsDeleted = 0 ORDER BY ServiceUnitName ASC", hdnPromotionSchemeID.Value));
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

        private void ControlToEntity(PromotionSchemeServiceUnit entity)
        {
            entity.ServiceUnitID = Convert.ToInt32(hdnServiceUnitID.Value);

            entity.IsUsePromotionPrice1 = chkIsUsePromotionPrice1.Checked;
            entity.Tariff1 = txtTariff1.Text == "" ? 0 : Convert.ToDecimal(txtTariff1.Text);
            entity.Tariff1Comp1 = txtTariff1Comp1.Text == "" ? 0 : Convert.ToDecimal(txtTariff1Comp1.Text);
            entity.Tariff1Comp2 = txtTariff1Comp2.Text == "" ? 0 : Convert.ToDecimal(txtTariff1Comp2.Text);
            entity.Tariff1Comp3 = txtTariff1Comp3.Text == "" ? 0 : Convert.ToDecimal(txtTariff1Comp3.Text);
            entity.IsDiscountInPercentage1 = chkIsDiscountInPercentage1.Checked;
            entity.DiscountAmount1 = txtDiscountAmount1.Text == "" ? 0 : Convert.ToDecimal(txtDiscountAmount1.Text);


            entity.IsUsePromotionPrice2 = chkIsUsePromotionPrice2.Checked;
            entity.Tariff2 = txtTariff2.Text == "" ? 0 : Convert.ToDecimal(txtTariff2.Text);
            entity.IsDiscountInPercentage2 = chkIsDiscountInPercentage2.Checked;
            entity.DiscountAmount2 = txtDiscountAmount2.Text == "" ? 0 : Convert.ToDecimal(txtDiscountAmount2.Text);

            entity.IsUsePromotionPrice3 = chkIsUsePromotionPrice3.Checked;
            entity.Tariff3 = txtTariff3.Text == "" ? 0 : Convert.ToDecimal(txtTariff3.Text);
            entity.IsDiscountInPercentage3 = chkIsDiscountInPercentage3.Checked;
            entity.DiscountAmount3 = txtDiscountAmount3.Text == "" ? 0 : Convert.ToDecimal(txtDiscountAmount3.Text);

            if (!string.IsNullOrEmpty(hdnRevenueSharingID.Value) && hdnRevenueSharingID.Value != "0")
            {
                entity.RevenueSharingID = Convert.ToInt32(hdnRevenueSharingID.Value);
            }
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                PromotionSchemeServiceUnit entity = new PromotionSchemeServiceUnit();
                ControlToEntity(entity);
                entity.PromotionSchemeID = Convert.ToInt32(hdnPromotionSchemeID.Value);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertPromotionSchemeServiceUnit(entity);
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
                PromotionSchemeServiceUnit entity = BusinessLayer.GetPromotionSchemeServiceUnit(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePromotionSchemeServiceUnit(entity);
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
                PromotionSchemeServiceUnit entity = BusinessLayer.GetPromotionSchemeServiceUnit(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePromotionSchemeServiceUnit(entity);
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