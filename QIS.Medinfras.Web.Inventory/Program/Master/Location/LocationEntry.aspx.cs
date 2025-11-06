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

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class LocationEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.LOCATION;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            String[] param = Request.QueryString["id"].Split('|');
            if (param[0] == "edit")
            {
                IsAdd = false;
                String locationID = param[1];
                hdnID.Value = locationID;
                vLocation entity = BusinessLayer.GetvLocationList(string.Format("LocationID = {0}", locationID))[0];

                EntityToControl(entity);
                hdnHealthcareID.Value = entity.HealthcareID;
            }
            else
            {
                hdnHealthcareID.Value = param[1];
                IsAdd = true;
            }
            txtLocationCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected string OnGetItemGroupFilterExpression()
        {
            string filterExp = string.Format("GCItemType IN ('{0}','{1}','{2}','{3}') AND IsDeleted = 0", Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.SUPPLIES, Constant.ItemGroupMaster.LOGISTIC, Constant.ItemGroupMaster.NUTRITION);
            if (cboGCLocationGroup.Value != null)
            {
                if (!string.IsNullOrEmpty(cboGCLocationGroup.Value.ToString()))
                {
                    if (cboGCLocationGroup.Value.ToString() == Constant.LocationGroup.DRUG_AND_MEDICAL_SUPPLIES)
                        filterExp = string.Format("GCItemType IN ('{0}','{1}') AND IsDeleted = 0", Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.SUPPLIES);
                    else if (cboGCLocationGroup.Value.ToString() == Constant.LocationGroup.NUTRITION)
                        filterExp = string.Format("GCItemType IN ('{0}') AND IsDeleted = 0", Constant.ItemGroupMaster.NUTRITION);
                    else
                        filterExp = string.Format("GCItemType IN ('{0}') AND IsDeleted = 0", Constant.ItemGroupMaster.LOGISTIC);
                } 
            }
            return filterExp;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtLocationCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtLocationName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtShortName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboGCLocationGroup, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboTransactionType, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(hdnRestrictionID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtRestrictionCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtRestrictionName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnItemGroupID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtItemGroupCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtItemGroupName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnRequestLocationID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtRequestLocationCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtRequestLocationName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnDistributionLocationID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtDistributionLocationCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDistributionLocationName, new ControlEntrySetting(false, false, false));
            //SetControlEntrySetting(hdnParentID, new ControlEntrySetting(true, true));
            //SetControlEntrySetting(txtParentCode, new ControlEntrySetting(true, true, false));
            //SetControlEntrySetting(txtParentName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(chkIsAllowOverIssued, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsAvailable, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsHeader, new ControlEntrySetting(false, false, false,true));
            SetControlEntrySetting(chkIsHoldForTransaction, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsNettable, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsMinMaxReadOnly, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkisUsingLocationAverageQty, new ControlEntrySetting(true, true, false));

            List<StandardCode> lstLocationGroup = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.LOCATION_GROUP));
            Methods.SetComboBoxField<StandardCode>(cboGCLocationGroup, lstLocationGroup, "StandardCodeName", "StandardCodeID", DevExpress.Web.ASPxEditors.DropDownStyle.DropDown);

            string filterExpression = string.Format("ParentID IN ('{0}') AND IsDeleted = 0", Constant.StandardCode.TRANSACTION_TYPE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboTransactionType, lstStandardCode, "StandardCodeName", "StandardCodeID");

            List<StandardCode> lstHealthcareUnit = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.HEALTHCARE_UNIT));
            lstHealthcareUnit.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboHealthcareUnit, lstHealthcareUnit, "StandardCodeName", "StandardCodeID", DevExpress.Web.ASPxEditors.DropDownStyle.DropDown);
            cboHealthcareUnit.SelectedIndex = 0;
        }

        private void EntityToControl(vLocation entity)
        {
            txtLocationCode.Text = entity.LocationCode;
            txtLocationName.Text = entity.LocationName;
            txtShortName.Text = entity.ShortName;
            txtRestrictionCode.Text = entity.RestrictionCode;
            txtRestrictionName.Text = entity.RestrictionName;
            txtItemGroupCode.Text = entity.ItemGroupCode;
            txtItemGroupName.Text = entity.ItemGroupName1;
            txtDistributionLocationCode.Text = entity.DistributionLocationCode;
            txtDistributionLocationName.Text = entity.DistributionLocationName;
            txtRequestLocationCode.Text = entity.RequestLocationCode;
            txtRequestLocationName.Text = entity.RequestLocationName;
            cboGCLocationGroup.Value = entity.GCLocationGroup;
            cboTransactionType.Value = entity.GCItemRequestType;
            cboHealthcareUnit.Value = entity.GCHealthcareUnit;

            //hdnParentID.Value = entity.ParentID.ToString();
            chkIsAllowOverIssued.Checked = entity.IsAllowOverIssued;
            chkIsAvailable.Checked = entity.IsAvailable;
            chkIsHeader.Checked = entity.IsHeader;
            chkIsHoldForTransaction.Checked = entity.IsHoldForTransaction;
            chkIsNettable.Checked = entity.IsNettable;
            chkIsMinMaxReadOnly.Checked = entity.IsMinMaxReadOnly;
            chkisUsingLocationAverageQty.Checked = entity.isUsingLocationAverageQty;

            hdnItemGroupID.Value = entity.ItemGroupID.ToString();
            hdnRestrictionID.Value = entity.RestrictionID.ToString();
        }

        private void ControlToEntity(Location entity)
        {
            entity.LocationCode = txtLocationCode.Text;
            entity.LocationName = txtLocationName.Text;
            entity.ShortName = txtShortName.Text;
            
            if (cboGCLocationGroup.Value != null)
            {
                entity.GCLocationGroup = cboGCLocationGroup.Value.ToString();
            }

            if (cboTransactionType.Value != null)
            {
                entity.GCItemRequestType = cboTransactionType.Value.ToString();
            }

            if (cboHealthcareUnit.Value != null)
            {
                entity.GCHealthcareUnit = cboHealthcareUnit.Value.ToString();
            }

            if (hdnItemGroupID.Value == "" || hdnItemGroupID.Value == "0")
                entity.ItemGroupID = null;
            else
                entity.ItemGroupID = Convert.ToInt32(hdnItemGroupID.Value);
            if (hdnRestrictionID.Value == "" || hdnRestrictionID.Value == "0")
                entity.RestrictionID = null;
            else
                entity.RestrictionID = Convert.ToInt32(hdnRestrictionID.Value);

            if (hdnDistributionLocationID.Value == "" || hdnDistributionLocationID.Value == "0")
                entity.DistributionLocationID = null;
            else
                entity.DistributionLocationID = Convert.ToInt32(hdnDistributionLocationID.Value);

            if (hdnRequestLocationID.Value == "" || hdnRequestLocationID.Value == "0")
                entity.RequestLocationID = null;
            else
                entity.RequestLocationID = Convert.ToInt32(hdnRequestLocationID.Value);
            //if (hdnParentID.Value == "" || hdnParentID.Value == "0")
            //    entity.ParentID = null;
            //else
            //    entity.ParentID = Convert.ToInt32(hdnParentID.Value);

            entity.IsAllowOverIssued = chkIsAllowOverIssued.Checked;
            entity.IsAvailable = chkIsAvailable.Checked;
            entity.IsHeader = true;
            entity.IsHoldForTransaction = chkIsHoldForTransaction.Checked;
            entity.IsNettable = chkIsNettable.Checked;
            entity.IsMinMaxReadOnly = chkIsMinMaxReadOnly.Checked;
            entity.IsUsingLocationAverageQty = chkisUsingLocationAverageQty.Checked;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("LocationCode = '{0}'", txtLocationCode.Text);
            List<Location> lst = BusinessLayer.GetLocationList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Location with Code " + txtLocationCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            Int32 locationID = Convert.ToInt32(hdnID.Value);
            string FilterExpression = string.Format("LocationCode = '{0}' AND LocationID != {1}", txtLocationCode.Text, locationID);
            List<Location> lst = BusinessLayer.GetLocationList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Location with Code " + txtLocationCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            LocationDao entityDao = new LocationDao(ctx);
            bool result = false;
            try
            {
                Location entity = new Location();
                ControlToEntity(entity);
                entity.HealthcareID = hdnHealthcareID.Value;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                retval = entityDao.InsertReturnPrimaryKeyID(entity).ToString();
                ctx.CommitTransaction();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                Location entity = BusinessLayer.GetLocation(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateLocation(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }
    }
}