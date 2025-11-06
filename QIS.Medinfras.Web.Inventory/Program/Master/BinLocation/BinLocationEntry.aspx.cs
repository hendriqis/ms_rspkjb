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

        protected string filterExpressionLocation = "";

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
            filterExpressionLocation = string.Format("{0};{1};{2};", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Empty);
            if (param[0] == "edit")
            {
                IsAdd = false;
                String binLocationID = param[1];
                hdnID.Value = binLocationID;
                vBinLocation entity = BusinessLayer.GetvBinLocationList(string.Format("BinLocationID = {0}", binLocationID))[0];
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }
            txtBinLocationCode.Focus();
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtBinLocationCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtBinLocationName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtLocationCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtLocationName, new ControlEntrySetting(false, false, true));
        }

        private void EntityToControl(vBinLocation entity)
        {
            txtBinLocationCode.Text = entity.BinLocationCode;
            txtBinLocationName.Text = entity.BinLocationName;
            txtRemarks.Text = entity.Remarks;
            txtLocationCode.Text = entity.LocationCode;
            txtLocationName.Text = entity.LocationName;
            hdnLocationID.Value = entity.LocationID.ToString();
        }

        private void ControlToEntity(BinLocation entity)
        {
            entity.BinLocationCode = txtBinLocationCode.Text;
            entity.BinLocationName = txtBinLocationName.Text;
            entity.Remarks = txtRemarks.Text;
            entity.LocationID = Convert.ToInt32(hdnLocationID.Value);
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("BinLocationCode = '{0}'", txtBinLocationCode.Text);
            List<BinLocation> lst = BusinessLayer.GetBinLocationList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Bin Location with Code " + txtBinLocationCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            Int32 locationID = Convert.ToInt32(hdnID.Value);
            string FilterExpression = string.Format("BinLocationCode = '{0}' AND BinLocationID != {1}", txtBinLocationCode.Text, locationID);
            List<BinLocation> lst = BusinessLayer.GetBinLocationList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Location with Code " + txtBinLocationCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            BinLocationDao entityDao = new BinLocationDao(ctx);
            bool result = false;
            try
            {
                BinLocation entity = new BinLocation();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetBinLocationMaxID(ctx).ToString();
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
                BinLocation entity = BusinessLayer.GetBinLocation(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateBinLocation(entity);
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