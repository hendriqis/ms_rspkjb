using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class ServiceUnitAutoBillItemEntryCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnHealthcareServiceUnitID.Value = param;
            vHealthcareServiceUnit entity = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", hdnHealthcareServiceUnitID.Value)).FirstOrDefault();
            txtHealthcareName.Text = entity.HealthcareName;
            txtServiceUnitName.Text = entity.ServiceUnitName;

            BindGridView();


            txtItemCode.Attributes.Add("validationgroup", "mpEntryPopup");
            txtQty.Attributes.Add("validationgroup", "mpEntryPopup");
        }

        private void BindGridView()
        {
            grdView.DataSource = BusinessLayer.GetvServiceUnitAutoBillItemList(string.Format("HealthcareServiceUnitID = {0} AND IsDeleted = 0 ORDER BY ItemName1 ASC", hdnHealthcareServiceUnitID.Value));
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
                if (hdnAutoBillItemID.Value.ToString() != "")
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

        private void ControlToEntity(ServiceUnitAutoBillItem entity)
        {
            entity.Quantity = Convert.ToDecimal(txtQty.Text);
            entity.IsAutoPayment = chkIsAutoPayment.Checked;
            entity.IsAdministrationItem = chkIsIsAdministrationItem.Checked;
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                ServiceUnitAutoBillItem entity = new ServiceUnitAutoBillItem();
                ControlToEntity(entity);
                entity.ItemID = Convert.ToInt32(hdnItemID.Value);
                entity.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entity.LastUpdatedDate = DateTime.Now;
                BusinessLayer.InsertServiceUnitAutoBillItem(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                ServiceUnitAutoBillItem entity = BusinessLayer.GetServiceUnitAutoBillItem(Convert.ToInt32(hdnAutoBillItemID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entity.LastUpdatedDate = DateTime.Now;
                BusinessLayer.UpdateServiceUnitAutoBillItem(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            try
            {
                ServiceUnitAutoBillItem entity = BusinessLayer.GetServiceUnitAutoBillItem(Convert.ToInt32(hdnAutoBillItemID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateServiceUnitAutoBillItem(entity);
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