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
    public partial class ManufacturerEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.MANUFACTURER;
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
                Manufacturer entity = BusinessLayer.GetManufacturer(Convert.ToInt32(ID));
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }
            txtManufacturerCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtManufacturerCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtManufacturerName, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(Manufacturer entity)
        {
            txtManufacturerCode.Text = entity.ManufacturerCode;
            txtManufacturerName.Text = entity.ManufacturerName;
        }

        private void ControlToEntity(Manufacturer entity)
        {
            entity.ManufacturerCode = txtManufacturerCode.Text;
            entity.ManufacturerName = txtManufacturerName.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("ManufacturerCode = '{0}'", txtManufacturerCode.Text);
            List<Manufacturer> lst = BusinessLayer.GetManufacturerList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Manufacturer with Code " + txtManufacturerCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("ManufacturerCode = '{0}' AND ManufacturerID != {1}", txtManufacturerCode.Text, hdnID.Value);
            List<Manufacturer> lst = BusinessLayer.GetManufacturerList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Manufacturer with Code " + txtManufacturerCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            ManufacturerDao entityDao = new ManufacturerDao(ctx);
            bool result = false;
            try
            {
                Manufacturer entity = new Manufacturer();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetManufacturerMaxID(ctx).ToString();
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
                Manufacturer entity = BusinessLayer.GetManufacturer(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateManufacturer(entity);
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