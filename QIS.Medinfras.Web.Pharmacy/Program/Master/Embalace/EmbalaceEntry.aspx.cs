using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class EmbalaceEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Pharmacy.EMBALACE;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                hdnEmbalaceID.Value = Request.QueryString["id"];
                SetControlProperties();
                EmbalaceHd entity = BusinessLayer.GetEmbalaceHd(Convert.ToInt32(hdnEmbalaceID.Value));
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }

            chkIsUnitPrice.Visible = false;
            txtEmbalaceCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(String.Format("ParentID IN ('{0}') AND IsDeleted = 0", Constant.StandardCode.DRUG_FORM));
            lst.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboSignaLabel, lst.Where(p => p.ParentID == Constant.StandardCode.DRUG_FORM).ToList(), "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtEmbalaceCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtEmbalaceName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtItemCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtItemName, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtTariff, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(hdnItemID, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(chkIsUnitPrice, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsUsingRangePrice, new ControlEntrySetting(true, true, false,true));
            SetControlEntrySetting(txtBUD, new ControlEntrySetting(true, true, true, Helper.GetCurrentDate().ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
        }

        private void EntityToControl(EmbalaceHd entity)
        {
            txtEmbalaceCode.Text = entity.EmbalaceCode;
            txtEmbalaceName.Text = entity.EmbalaceName;
            txtTariff.Text = Convert.ToDecimal(entity.Tariff).ToString();
            cboSignaLabel.Value = entity.GCSignaLabel;           
            chkIsUsingRangePrice.Checked = entity.IsUsingRangePricing;
            chkIsUnitPrice.Checked = entity.IsUnitPrice;
            if (entity.ItemID != null)
            {
                ItemMaster obj = BusinessLayer.GetItemMaster((int)entity.ItemID);
                txtItemCode.Text = obj.ItemCode;
                txtItemName.Text = obj.ItemName1;
                hdnItemID.Value = entity.ItemID.ToString();
            }
            else
            {
                txtItemCode.Text = string.Empty;
                txtItemName.Text = string.Empty;
                hdnItemID.Value = string.Empty;
            }
            txtBUD.Text = entity.BUD.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        }

        private void ControlToEntity(EmbalaceHd entity)
        {
            entity.EmbalaceCode = txtEmbalaceCode.Text;
            entity.EmbalaceName = txtEmbalaceName.Text;
            entity.Tariff = Convert.ToDecimal(txtTariff.Text);
            if (cboSignaLabel.Value != null)
            {
                entity.GCSignaLabel = cboSignaLabel.Value.ToString();   
            }
            if (!String.IsNullOrEmpty(hdnItemID.Value))
            {
                entity.ItemID = Convert.ToInt32(hdnItemID.Value);                
            }

            entity.IsUsingRangePricing = chkIsUsingRangePrice.Checked;
            entity.IsUnitPrice = chkIsUnitPrice.Checked;
            entity.BUD = Helper.GetDatePickerValue(txtBUD);
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("EmbalaceCode = '{0}' AND IsDeleted = 0", txtEmbalaceCode.Text);
            List<EmbalaceHd> lst = BusinessLayer.GetEmbalaceHdList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Embalace with code " + txtEmbalaceCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("EmbalaceCode = '{0}' AND EmbalaceID != {1} AND IsDeleted = 0", txtEmbalaceCode.Text,hdnEmbalaceID.Value);
            List<EmbalaceHd> lst = BusinessLayer.GetEmbalaceHdList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Embalace with code " + txtEmbalaceCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            try
            {
                EmbalaceHd entity = new EmbalaceHd();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertEmbalaceHd(entity);
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
            }
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                EmbalaceHd entity = BusinessLayer.GetEmbalaceHd(Convert.ToInt32(hdnEmbalaceID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateEmbalaceHd(entity);
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