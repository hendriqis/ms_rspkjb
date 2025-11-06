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
    public partial class ZipCodesEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.ZIPCODES;
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
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                ZipCodes entity = BusinessLayer.GetZipCodes(Convert.ToInt32(ID));
                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtZipCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}') AND IsDeleted = 0 ORDER BY StandardCodeName ASC", Constant.StandardCode.PROVINCE, Constant.StandardCode.NATION_GROUP));
            Methods.SetComboBoxField<StandardCode>(cboGCProvince, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.PROVINCE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCNation, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.NATION_GROUP).ToList(), "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtZipCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtStreetName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCountyCodeReference, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCounty, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDistrict, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(chkIsInRegionDistrict, new ControlEntrySetting(true, true, false, true));
            SetControlEntrySetting(txtCity, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboGCProvince, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboGCNation, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtLatitude, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtLongitude, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(ZipCodes entity)
        {
            txtZipCode.Text = entity.ZipCode;
            txtStreetName.Text = entity.StreetName;
            txtCountyCodeReference.Text = entity.CountyCodeReference;
            txtDistrict.Text = entity.District;
            txtCounty.Text = entity.County;
            txtCity.Text = entity.City;
            cboGCProvince.Value = entity.GCProvince;
            cboGCNation.Value = entity.GCNation;
            txtLatitude.Text = entity.Latitude.ToString();
            txtLongitude.Text = entity.Longitude.ToString();
            chkIsInRegionDistrict.Checked = entity.IsInRegionDistrict;
        }

        private void ControlToEntity(ZipCodes entity)
        {
            entity.ZipCode = txtZipCode.Text;
            entity.StreetName = txtStreetName.Text;
            entity.CountyCodeReference = txtCountyCodeReference.Text;
            entity.District = txtDistrict.Text;
            entity.County = txtCounty.Text;
            entity.City = txtCity.Text;
            entity.GCProvince = cboGCProvince.Value.ToString();
            entity.GCNation = cboGCNation.Value.ToString();
            entity.Latitude = Convert.ToDecimal(txtLatitude.Text);
            entity.Longitude = Convert.ToDecimal(txtLongitude.Text);
            entity.IsInRegionDistrict = chkIsInRegionDistrict.Checked;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            //string FilterExpression = string.Format("ZipCode = '{0}'", txtZipCode.Text);
            //List<ZipCodes> lst = BusinessLayer.GetZipCodesList(FilterExpression);

            //if (lst.Count > 0)
            //    errMessage = " ZIP Code " + txtZipCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            //string FilterExpression = string.Format("ZipCode = '{0}' AND ID != {1}", txtZipCode.Text, hdnID.Value);
            //List<ZipCodes> lst = BusinessLayer.GetZipCodesList(FilterExpression);

            //if (lst.Count > 0)
            //    errMessage = " ZIP Code " + txtZipCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            ZipCodesDao entityDao = new ZipCodesDao(ctx);
            bool result = false;
            try
            {
                ZipCodes entity = new ZipCodes();
                ControlToEntity(entity);
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
                ZipCodes entity = BusinessLayer.GetZipCodes(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateZipCodes(entity);
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