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
    public partial class LocationPermissionEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.LOCATION_PERMISSION;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                SetControlProperties();
                RestrictionHd entity = BusinessLayer.GetRestrictionHd(Convert.ToInt32(ID));
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtRestrictionCode.Focus();
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.RESTRICTION_TYPE));
            Methods.SetComboBoxField<StandardCode>(cboRestrictionType, lstSc, "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtRestrictionCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRestrictionName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboRestrictionType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(RestrictionHd entity)
        {
            txtRestrictionCode.Text = entity.RestrictionCode;
            txtRestrictionName.Text = entity.RestrictionName;
            cboRestrictionType.Value = entity.GCRestrictionType;
            txtNotes.Text = entity.Remarks;
        }

        private void ControlToEntity(RestrictionHd entity)
        {
            entity.RestrictionCode = txtRestrictionCode.Text;
            entity.RestrictionName = txtRestrictionName.Text;
            entity.GCRestrictionType = cboRestrictionType.Value.ToString();
            entity.Remarks = txtNotes.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("RestrictionCode = '{0}'", txtRestrictionCode.Text);
            List<RestrictionHd> lst = BusinessLayer.GetRestrictionHdList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Restriction with Code " + txtRestrictionCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("RestrictionCode = '{0}' AND RestrictionID != {1}", txtRestrictionCode.Text, hdnID.Value);
            List<RestrictionHd> lst = BusinessLayer.GetRestrictionHdList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Restriction with Code " + txtRestrictionCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            RestrictionHdDao entityDao = new RestrictionHdDao(ctx);
            bool result = false;
            try
            {
                RestrictionHd entity = new RestrictionHd();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetRestrictionHdMaxID(ctx).ToString();
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
                RestrictionHd entity = BusinessLayer.GetRestrictionHd(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateRestrictionHd(entity);
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