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
    public partial class VaccinationTypeEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.VACCINATION_TYPE;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                VaccinationType entity = BusinessLayer.GetVaccinationType(Convert.ToInt32(ID));
                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtVaccinationTypeCode.Focus();
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.VACCINATION_GROUP));
            Methods.SetComboBoxField<StandardCode>(cboVaccinationGroup, lstStandardCode, "StandardCodeName", "StandardCodeID");
            cboVaccinationGroup.SelectedIndex = 0;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtVaccinationTypeCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtVaccinationTypeName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtShortName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboVaccinationGroup, new ControlEntrySetting(true, true, true));

            SetControlEntrySetting(txtDisplayOrder, new ControlEntrySetting(true, true, true, 0));
            SetControlEntrySetting(txtDisplayColor, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtDisplayColorPicker, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(VaccinationType entity)
        {
            txtVaccinationTypeCode.Text = entity.VaccinationTypeCode;
            txtVaccinationTypeName.Text = entity.VaccinationTypeName;
            txtShortName.Text = entity.ShortName;
            cboVaccinationGroup.Value = entity.GCVaccinationGroup;

            txtDisplayOrder.Text = entity.DisplayOrder.ToString();
            txtDisplayColor.Text = txtDisplayColorPicker.Text = entity.DisplayColor;
            txtRemarks.Text = entity.Remarks;
        }

        private void ControlToEntity(VaccinationType entity)
        {
            entity.VaccinationTypeCode = txtVaccinationTypeCode.Text;
            entity.VaccinationTypeName = txtVaccinationTypeName.Text;
            entity.ShortName = txtShortName.Text;
            entity.GCVaccinationGroup = cboVaccinationGroup.Value.ToString();

            entity.DisplayOrder = Convert.ToInt16(txtDisplayOrder.Text);
            entity.DisplayColor = txtDisplayColor.Text;
            entity.Remarks = txtRemarks.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("VaccinationTypeCode = '{0}'", txtVaccinationTypeCode.Text);
            List<VaccinationType> lst = BusinessLayer.GetVaccinationTypeList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Vaccination with Code " + txtVaccinationTypeCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            Int32 ID = Convert.ToInt32(hdnID.Value);
            string FilterExpression = string.Format("VaccinationTypeCode = '{0}' AND VaccinationTypeID != {1}", txtVaccinationTypeCode.Text, ID);
            List<VaccinationType> lst = BusinessLayer.GetVaccinationTypeList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Vaccination with Code " + txtVaccinationTypeCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            VaccinationTypeDao entityDao = new VaccinationTypeDao(ctx);
            bool result = false;
            try
            {
                VaccinationType entity = new VaccinationType();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetVaccinationTypeMaxID(ctx).ToString();
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
                VaccinationType entity = BusinessLayer.GetVaccinationType(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateVaccinationType(entity);
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