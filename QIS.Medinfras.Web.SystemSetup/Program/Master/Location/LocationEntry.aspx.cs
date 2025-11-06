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
    public partial class LocationEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.LOCATION;
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
                if (entity.ParentID > 0)
                {
                    Location entityParent = BusinessLayer.GetLocation((int)entity.ParentID);
                    txtParentCode.Text = entityParent.LocationCode;
                    txtParentName.Text = entityParent.LocationName;
                }
                hdnHealthcareID.Value = entity.HealthcareID;
            }
            else
            {
                hdnHealthcareID.Value = param[1];
                IsAdd = true;
            }
            txtLocationCode.Focus();
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtLocationCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtLocationName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtShortName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(hdnRestrictionID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtRestrictionCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtRestrictionName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnItemGroupID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtItemGroupCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtItemGroupName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnParentID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtParentCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtParentName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(chkIsAllowOverIssued, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsAvailable, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsHeader, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsHoldForTransaction, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsNettable, new ControlEntrySetting(true, true, false));
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
            hdnParentID.Value = entity.ParentID.ToString();
            chkIsAllowOverIssued.Checked = entity.IsAllowOverIssued;
            chkIsAvailable.Checked = entity.IsAvailable;
            chkIsHeader.Checked = entity.IsHeader;
            chkIsHoldForTransaction.Checked = entity.IsHoldForTransaction;
            chkIsNettable.Checked = entity.IsNettable;

            hdnItemGroupID.Value = entity.ItemGroupID.ToString();
            hdnRestrictionID.Value = entity.RestrictionID.ToString();
        }

        private void ControlToEntity(Location entity)
        {
            entity.LocationCode = txtLocationCode.Text;
            entity.LocationName = txtLocationName.Text;
            entity.ShortName = txtShortName.Text;
            if (hdnItemGroupID.Value == "" || hdnItemGroupID.Value == "0")
                entity.ItemGroupID = null;
            else
                entity.ItemGroupID = Convert.ToInt32(hdnItemGroupID.Value);
            if (hdnRestrictionID.Value == "" || hdnRestrictionID.Value == "0")
                entity.RestrictionID = null;
            else
                entity.RestrictionID = Convert.ToInt32(hdnRestrictionID.Value);
            if (hdnParentID.Value == "" || hdnParentID.Value == "0")
                entity.ParentID = null;
            else
                entity.ParentID = Convert.ToInt32(hdnParentID.Value);

            entity.IsAllowOverIssued = chkIsAllowOverIssued.Checked;
            entity.IsAvailable = chkIsAvailable.Checked;
            entity.IsHeader = chkIsHeader.Checked;
            entity.IsHoldForTransaction = chkIsHoldForTransaction.Checked;
            entity.IsNettable = chkIsNettable.Checked;
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
                entityDao.Insert(entity);
                retval = BusinessLayer.GetLocationMaxID(ctx).ToString();
                ctx.CommitTransaction();
                result = true;
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                result = false;
                errMessage = ex.Message;
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
                return false;
            }
        }
    }
}