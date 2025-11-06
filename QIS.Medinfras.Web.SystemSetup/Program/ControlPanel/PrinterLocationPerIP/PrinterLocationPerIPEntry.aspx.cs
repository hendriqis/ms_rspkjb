using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class PrinterLocationPerIPEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.PRINTER_LOCATION_PER_IP;
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

                PrinterLocation entity = BusinessLayer.GetPrinterLocation(Convert.ToInt32(ID));
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
            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(String.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.PRINTER_TYPE));
            Methods.SetComboBoxField<StandardCode>(cboPrinterType, lst, "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtIPAddress, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboPrinterType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtPrinterName, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(PrinterLocation entity)
        {
            txtIPAddress.Text = entity.IPAddress;
            cboPrinterType.Value = entity.GCPrinterType;
            txtPrinterName.Text = entity.PrinterName;
            chkIsUsePrintingTools.Checked = entity.IsUsingPrintingTools;
        }

        private void ControlToEntity(PrinterLocation entity)
        {
            if (IsValidIPAddress(txtIPAddress.Text))
            {
                entity.IPAddress = txtIPAddress.Text;
            }
            else
            {
                entity.IPAddress = null;
            }
            entity.GCPrinterType = cboPrinterType.Value.ToString();
            entity.PrinterName = txtPrinterName.Text;
            entity.IsUsingPrintingTools = chkIsUsePrintingTools.Checked;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            try
            {
                PrinterLocation entity = new PrinterLocation();
                ControlToEntity(entity);
                entity.IsDeleted = false;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertPrinterLocation(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                if (hdnID.Value.ToString() != "")
                {
                    PrinterLocation entity = BusinessLayer.GetPrinterLocation(Convert.ToInt32(hdnID.Value));
                    ControlToEntity(entity);
                    entity.IsDeleted = false;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdatePrinterLocation(entity);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
    }
}