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

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class VaccinationTypeEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.VACCINATION_TYPE;
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
                SetControlProperties();
                hdnID.Value = ID;
                VaccinationType entity = BusinessLayer.GetVaccinationType(Convert.ToInt32(hdnID.Value));
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }

            txtVaccinationCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }


        protected override void SetControlProperties()
        {
            List<StandardCode> lststd = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}','{2}') AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.VACCINATION_GROUP, Constant.StandardCode.VACCINATION_CVX_GROUP, Constant.StandardCode.VACCINATION_CVX_NAME));
            Methods.SetComboBoxField<StandardCode>(cboVaccinationGroup, lststd.Where(sc => sc.ParentID == Constant.StandardCode.VACCINATION_GROUP).ToList(), "StandardCodeName", "StandardCodeID");
            cboVaccinationGroup.SelectedIndex = 0;

            Methods.SetComboBoxField<StandardCode>(cboCVXGroup, lststd.Where(sc => sc.ParentID == Constant.StandardCode.VACCINATION_CVX_GROUP).ToList(), "StandardCodeName", "StandardCodeID");
            cboCVXGroup.SelectedIndex = 0;

            Methods.SetComboBoxField<StandardCode>(cboCVXName, lststd.Where(sc => sc.ParentID == Constant.StandardCode.VACCINATION_CVX_NAME).ToList(), "StandardCodeName", "StandardCodeID");
            cboCVXName.SelectedIndex = 0;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtVaccinationCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtVaccinationName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtShortName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboVaccinationGroup, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboCVXGroup, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboCVXName, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(txtDisplayOrder, new ControlEntrySetting(true, true, true,0));
            SetControlEntrySetting(txtDisplayColor, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtDisplayColorPicker, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(VaccinationType entity)
        {
            txtVaccinationCode.Text = entity.VaccinationTypeCode;
            txtVaccinationName.Text = entity.VaccinationTypeName;
            txtShortName.Text = entity.ShortName;
            cboVaccinationGroup.Value = entity.GCVaccinationGroup;
            cboCVXGroup.Value = entity.GCCVXGroup;
            cboCVXName.Value = entity.GCCVXName;

            txtDisplayOrder.Text = entity.DisplayOrder.ToString();
            txtDisplayColor.Text = txtDisplayColorPicker.Text = entity.DisplayColor;
            txtRemarks.Text = entity.Remarks;
            chkIsCovid19.Checked = entity.IsCovid19;
        }

        private void ControlToEntity(VaccinationType entity)
        {
            entity.VaccinationTypeCode = txtVaccinationCode.Text;
            entity.VaccinationTypeName = txtVaccinationName.Text;
            entity.ShortName = txtShortName.Text;
            entity.GCVaccinationGroup = cboVaccinationGroup.Value.ToString();

            if (cboCVXGroup.Value != null && cboCVXGroup.Value.ToString() != "0")
                entity.GCCVXGroup = cboCVXGroup.Value.ToString();
            else
                entity.GCCVXGroup = null;

            if (cboCVXName.Value != null && cboCVXName.Value.ToString() != "0")
                entity.GCCVXName = cboCVXName.Value.ToString();
            else
                entity.GCCVXName = null;

            entity.DisplayOrder = Convert.ToInt16(txtDisplayOrder.Text);
            entity.DisplayColor = Request.Form[txtDisplayColor.UniqueID];
            entity.Remarks = txtRemarks.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("VaccinationTypeCode = '{0}' AND IsDeleted = 0", txtVaccinationCode.Text);
            List<VaccinationType> lst = BusinessLayer.GetVaccinationTypeList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Vaccination with Code " + txtVaccinationCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = false;
            IDbContext ctx = DbFactory.Configure(true);
            VaccinationTypeDao entityDao = new VaccinationTypeDao(ctx);
            try
            {
                VaccinationType entity = new VaccinationType();
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
                VaccinationType entity = BusinessLayer.GetVaccinationType(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateVaccinationType(entity);
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