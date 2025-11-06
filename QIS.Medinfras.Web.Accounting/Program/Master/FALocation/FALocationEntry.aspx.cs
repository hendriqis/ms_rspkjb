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

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class FALocationEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.FA_LOCATION;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                FALocation entity = BusinessLayer.GetFALocation(Convert.ToInt32(ID));
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }
            txtFALocationCode.Focus();
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtFALocationCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtFALocationName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(FALocation entity)
        {
            txtFALocationCode.Text = entity.FALocationCode;
            txtFALocationName.Text = entity.FALocationName;
            txtRemarks.Text = entity.Remarks;
        }

        private void ControlToEntity(FALocation entity)
        {
            entity.FALocationCode = txtFALocationCode.Text;
            entity.FALocationName = txtFALocationName.Text;
            entity.Remarks = txtRemarks.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("FALocationCode = '{0}'", txtFALocationCode.Text);
            List<FALocation> lst = BusinessLayer.GetFALocationList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Location With Code " + txtFALocationCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("FALocationCode = '{0}' AND FALocationID != {1}", txtFALocationCode.Text, hdnID.Value);
            List<FALocation> lst = BusinessLayer.GetFALocationList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Location With Code " + txtFALocationCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            FALocationDao entityDao = new FALocationDao(ctx);
            try
            {
                FALocation entity = new FALocation();
                ControlToEntity(entity);
                entity.HealthcareID = AppSession.UserLogin.HealthcareID;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                retval = entityDao.InsertReturnPrimaryKeyID(entity).ToString();

                ctx.CommitTransaction();
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
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            FALocationDao entityDao = new FALocationDao(ctx);
            try
            {
                FALocation entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);

                ctx.CommitTransaction();
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
    }
}