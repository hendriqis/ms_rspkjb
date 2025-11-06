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

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class BinLocationEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.BIN_LOCATION;
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
                BinLocation entity = BusinessLayer.GetBinLocationList(string.Format("LocationID = {0}", locationID))[0];
                EntityToControl(entity);
                //hdnHealthcareID.Value = entity.HealthcareID;
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
            string filterExp = string.Format("GCItemType IN ('{0}','{1}','{2}') AND IsDeleted = 0", Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.SUPPLIES, Constant.ItemGroupMaster.LOGISTIC);
            if (cboGCLocationGroup.Value != null)
            {
                if (!string.IsNullOrEmpty(cboGCLocationGroup.Value.ToString()))
                {
                    if (cboGCLocationGroup.Value.ToString() == Constant.LocationGroup.DRUG_AND_MEDICAL_SUPPLIES)
                        filterExp = string.Format("GCItemType IN ('{0}','{1}') AND IsDeleted = 0", Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.SUPPLIES);
                    else
                        filterExp = string.Format("GCItemType IN ('{0}') AND IsDeleted = 0", Constant.ItemGroupMaster.LOGISTIC);
                } 
            }
            return filterExp;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtBinLocationCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtBinLocationName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(BinLocation entity)
        {
            txtBinLocationCode.Text = entity.BinLocationCode;
            txtBinLocationName.Text = entity.BinLocationName;
            txtRemarks.Text = entity.Remarks;
        }

        private void ControlToEntity(BinLocation entity)
        {
            entity.BinLocationCode = txtBinLocationCode.Text;
            entity.BinLocationName = txtBinLocationName.Text;
            entity.Remarks = txtRemarks.Text;
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