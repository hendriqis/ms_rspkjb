using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class SupplierItemEntryCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnBusinessPartnerID.Value = param;

            BusinessPartners hsu = BusinessLayer.GetBusinessPartners(Convert.ToInt32(param));
            txtCustomerName.Text = string.Format("{0} - {1}", hsu.BusinessPartnerCode, hsu.BusinessPartnerName);

            BindGridView();
            txtItemCode.Attributes.Add("validationgroup", "mpEntryPopup");
            txtLeadTime.Attributes.Add("validationgroup", "mpEntryPopup");
        }

        private void BindGridView()
        {
            grdView.DataSource = BusinessLayer.GetvSupplierItemList(string.Format("BusinessPartnerID = {0} AND IsDeleted = 0 ORDER BY ItemName1 ASC", hdnBusinessPartnerID.Value));
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

        private void ControlToEntity(SupplierItem entity)
        {
            entity.ItemID = Convert.ToInt32(hdnItemID.Value);
            entity.SupplierItemCode = txtSupplierItemCode.Text;
            entity.SupplierItemName = txtSupplierItemName.Text;
            entity.LeadTime = Convert.ToByte(txtLeadTime.Text);
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                SupplierItem entity = new SupplierItem();
                ControlToEntity(entity);
                entity.BusinessPartnerID = Convert.ToInt32(hdnBusinessPartnerID.Value);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertSupplierItem(entity);
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
                SupplierItem entity = BusinessLayer.GetSupplierItem(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateSupplierItem(entity);
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
                SupplierItem entity = BusinessLayer.GetSupplierItem(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateSupplierItem(entity);
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