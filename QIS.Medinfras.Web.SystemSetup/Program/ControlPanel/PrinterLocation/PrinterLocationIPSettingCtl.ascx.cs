using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class PrinterLocationIPSettingCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnGCPrinterGroup.Value = param;
            String GCPrinterGroup = hdnGCPrinterGroup.Value;
            StandardCode sc = BusinessLayer.GetStandardCode(GCPrinterGroup);
            txtPrinterGroupCode.Text = sc.StandardCodeID;
            txtPrinterGroupName.Text = sc.StandardCodeName;

            BindGridView();

            txtIPAddress.Attributes.Add("validationgroup", "mpEntryPopup");
        }

        private void BindGridView()
        {
            grdView.DataSource = BusinessLayer.GetvPrinterGroupList(string.Format("GCPrinterGroup = '{0}' AND IsDeleted = 0", hdnGCPrinterGroup.Value));
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }

        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string param = e.Parameter;

            string result = param + "|";
            string errMessage = "";

            if (param == "save")
            {
                if (hdnID.Value.ToString() != "")
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param == "delete")
            {
                if (OnDeleteRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
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

        private void ControlToEntity(PrinterGroup entity)
        {
            if (IsValidIPAddress(txtIPAddress.Text))
            {
                entity.IPAddress = txtIPAddress.Text;
            }
            else
            {
                entity.IPAddress = null;
            }
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                PrinterGroup entity = new PrinterGroup();
                ControlToEntity(entity);
                entity.GCPrinterGroup = hdnGCPrinterGroup.Value;
                entity.IsDeleted = false;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertPrinterGroup(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                PrinterGroup entity = BusinessLayer.GetPrinterGroup(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.GCPrinterGroup = hdnGCPrinterGroup.Value;
                entity.IsDeleted = false;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePrinterGroup(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            try
            {
                PrinterGroup entity = BusinessLayer.GetPrinterGroup(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePrinterGroup(entity);
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