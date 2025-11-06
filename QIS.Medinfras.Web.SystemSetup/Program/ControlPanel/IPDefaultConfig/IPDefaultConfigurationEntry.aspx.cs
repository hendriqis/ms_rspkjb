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
    public partial class IPDefaultConfigurationEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.IP_DEFAULT_CONFIG2;
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

                IPDefaultConfig entity = BusinessLayer.GetIPDefaultConfigList(string.Format("ID = {0}", ID))[0];
                List<vIPDefaultConfig> lstAddress = BusinessLayer.GetvIPDefaultConfigList(string.Format("ID = {0}", entity.ID));
                vIPDefaultConfig entityIP = new vIPDefaultConfig();
                if (lstAddress.Count > 0)
                {
                    entityIP = BusinessLayer.GetvIPDefaultConfigList(string.Format("ID = {0}", entity.ID))[0];
                }
                hdnID.Value = entity.ID.ToString();
                EntityToControl(entity, entityIP);
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
            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(String.Format("ParentID IN ('{0}') AND IsDeleted = 0", Constant.StandardCode.CASHIER_GROUP));
            Methods.SetComboBoxField<StandardCode>(cboCashierGroup, lst.Where(p => p.ParentID == Constant.StandardCode.CASHIER_GROUP).ToList(), "StandardCodeName", "StandardCodeID");

            List<Department> lstDept = BusinessLayer.GetDepartmentList("");
            lstDept.Insert(0, new Department { DepartmentName = "", DepartmentID = "" });
            Methods.SetComboBoxField<Department>(cboDepartment, lstDept, "DepartmentName", "DepartmentID");

            List<Location> lstLoc = BusinessLayer.GetLocationList("");
            lstLoc.Insert(0, new Location { LocationCode = "", LocationName = "" });
            Methods.SetComboBoxField<Location>(cboLocation, lstLoc, "LocationName", "LocationID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtIPAddress, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboDepartment, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboCashierGroup, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboLocation, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtServiceUnitCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtServiceUnitName, new ControlEntrySetting(false, false, false));


        }

        private void EntityToControl(IPDefaultConfig entity, vIPDefaultConfig entityIP)
        {
            txtIPAddress.Text = entity.IPAddress;
            txtServiceUnitName.Text = entityIP.ServiceUnitName;
            cboCashierGroup.Value = entity.GCCashierGroup;
            cboDepartment.Value = entity.DepartmentID;
            cboLocation.Value = entityIP.LocationID;

            if (entity.HealthcareServiceUnitID != 0 && entity.HealthcareServiceUnitID != null)
            {
                vHealthcareServiceUnit vsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", entity.HealthcareServiceUnitID)).FirstOrDefault();
                txtServiceUnitCode.Text = vsu.ServiceUnitCode;
                txtServiceUnitName.Text = vsu.ServiceUnitName;
                hdnServiceUnitID.Value = Convert.ToString(vsu.DispensaryServiceUnitID);
            }
        }

        private void ControlToEntity(IPDefaultConfig entity)
        {
            entity.IPAddress = txtIPAddress.Text;
            entity.DepartmentID = cboDepartment.Value.ToString();
            entity.LocationID = Convert.ToInt32(cboLocation.Value);
            entity.HealthcareServiceUnitID = Convert.ToInt32(hdnServiceUnitID.Value);
            entity.DispensaryUnitID = Convert.ToInt32(txtServiceUnitCode.Text);
            if (cboCashierGroup.Value != null && cboCashierGroup.Value.ToString() != "")
                entity.GCCashierGroup = cboCashierGroup.Value.ToString();
            else
                entity.GCCashierGroup = null;
            
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            try
            {
                IPDefaultConfig entity = new IPDefaultConfig();
                ControlToEntity(entity);
                entity.IsDeleted = false;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertIPDefaultConfig(entity);
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
                    IPDefaultConfig entity = BusinessLayer.GetIPDefaultConfigList(string.Format("ID='{0}'", hdnID.Value)).FirstOrDefault();
                    ControlToEntity(entity);
                    entity.IsDeleted = false;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateIPDefaultConfig(entity);
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