using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class IPPharmacyUnitEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.IP_PHARMACY_UNIT;
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
                SetControlProperties();
                IsAdd = false;
                string ID = param[1];

                IPPharmacyUnit entity = BusinessLayer.GetIPPharmacyUnit(Convert.ToInt32(ID));
                hdnID.Value = entity.ID.ToString();
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }

            txtIPAddress.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        private bool IsValidIPAddress(string ipAddress)
        {
            //create our match pattern
            string pattern = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";
            //create our Regular Expression object
            Regex check = new Regex(pattern);
            //boolean variable to hold the status
            bool valid = false;
            //check to make sure an ip address was provided
            if (ipAddress == "")
            {
                //no address provided so return false
                valid = false;
            }
            else
            {
                //address provided so use the IsMatch Method
                //of the Regular Expression object
                valid = check.IsMatch(ipAddress, 0);
            }
            //return the results
            return valid;
        }

        protected override void SetControlProperties()
        {
            List<vHealthcareServiceUnit> lst = BusinessLayer.GetvHealthcareServiceUnitList(String.Format("DepartmentID = '{0}' AND IsDeleted = 0", Constant.Facility.PHARMACY));
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboPharmacyUnit, lst, "ServiceUnitName", "HealthcareServiceUnitID");
            cboPharmacyUnit.SelectedIndex = 0;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtIPAddress, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboPharmacyUnit, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(IPPharmacyUnit entity)
        {
            txtIPAddress.Text = entity.IPAddress;
            cboPharmacyUnit.Value = entity.PharmacyHealthcareServiceUnitID;
        }

        private void ControlToEntity(IPPharmacyUnit entity)
        {
            if (IsValidIPAddress(txtIPAddress.Text))
            {
                entity.IPAddress = txtIPAddress.Text;
            }
            else
            {
                entity.IPAddress = null;
            }
            entity.PharmacyHealthcareServiceUnitID = Convert.ToInt32(cboPharmacyUnit.Value);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            try
            {
                IPPharmacyUnit entity = new IPPharmacyUnit();
                ControlToEntity(entity);
                entity.IsDeleted = false;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertIPPharmacyUnit(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                if (hdnID.Value.ToString() != "")
                {
                    IPPharmacyUnit entity = BusinessLayer.GetIPPharmacyUnit(Convert.ToInt32(hdnID.Value));
                    ControlToEntity(entity);
                    entity.IsDeleted = false;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateIPPharmacyUnit(entity);
                    return true;
                }
                return false;
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