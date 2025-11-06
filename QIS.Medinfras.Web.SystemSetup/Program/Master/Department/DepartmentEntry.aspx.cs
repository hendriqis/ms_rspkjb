using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class DepartmentEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.FACILITY;
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
                String departmentID = Request.QueryString["id"];
                hdnID.Value = departmentID;
                Department entity = BusinessLayer.GetDepartment(departmentID);
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }
            txtDepartmentCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtDepartmentCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtDepartmentName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtShortName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtInitial, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(chkIsActive, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsAllowPatientRegistration, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsAllowPrescriptionOrder, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtGLAccountSegmentNo, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtIHSOrganizationID, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(Department entity)
        {
            txtDepartmentCode.Text = entity.DepartmentID;
            txtDepartmentName.Text = entity.DepartmentName;
            txtShortName.Text = entity.ShortName;
            txtInitial.Text = entity.Initial;
            chkIsAllowPatientRegistration.Checked = entity.IsHasRegistration;
            chkIsAllowPrescriptionOrder.Checked = entity.IsHasPrescription;
            chkIsActive.Checked = entity.IsActive;
            txtGLAccountSegmentNo.Text = entity.GLAccountNoSegment;
            txtIHSOrganizationID.Text = entity.IHSOrganizationID;
        }

        private void ControlToEntity(Department entity)
        {
            entity.DepartmentName = txtDepartmentName.Text;
            entity.ShortName = txtShortName.Text;
            entity.Initial = txtInitial.Text;
            entity.IsHasRegistration = chkIsAllowPatientRegistration.Checked;
            entity.IsHasPrescription = chkIsAllowPrescriptionOrder.Checked;
            entity.IsActive = chkIsActive.Checked;
            entity.GLAccountNoSegment = txtGLAccountSegmentNo.Text;
            entity.IHSOrganizationID = txtIHSOrganizationID.Text;
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                Department entity = BusinessLayer.GetDepartment(hdnID.Value);
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateDepartment(entity);
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