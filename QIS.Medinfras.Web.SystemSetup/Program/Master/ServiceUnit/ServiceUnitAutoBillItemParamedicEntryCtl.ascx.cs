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
    public partial class ServiceUnitAutoBillItemParamedicEntryCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnHealthcareServiceUnitIDAutoBillParamedic.Value = param;
            vHealthcareServiceUnit entity = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", hdnHealthcareServiceUnitIDAutoBillParamedic.Value)).FirstOrDefault();
            txtHealthcareName.Text = entity.HealthcareName;
            txtServiceUnitName.Text = entity.ServiceUnitName;

            BindGridView();

            txtParamedicCodeAutoBillParamedic.Attributes.Add("validationgroup", "mpEntryPopup");
            txtItemCodeAutoBillParamedic.Attributes.Add("validationgroup", "mpEntryPopup");
            txtQty.Attributes.Add("validationgroup", "mpEntryPopup");
        }

        private void BindGridView()
        {
            grdView.DataSource = BusinessLayer.GetvAutoBillItemParamedicList(string.Format("HealthcareServiceUnitID = {0} AND IsDeleted = 0 ORDER BY ParamedicName ASC, ItemName1 ASC", hdnHealthcareServiceUnitIDAutoBillParamedic.Value));
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

        private void ControlToEntity(AutoBillItemParamedic entity)
        {
            entity.Quantity = Convert.ToDecimal(txtQty.Text);
            entity.IsAutoPayment = chkIsAutoPayment.Checked;
            entity.IsAdministrationItem = chkIsIsAdministrationItem.Checked;
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                AutoBillItemParamedic entity = new AutoBillItemParamedic();
                ControlToEntity(entity);
                entity.ParamedicID = Convert.ToInt32(hdnParamedicIDAutoBillParamedic.Value);
                entity.ItemID = Convert.ToInt32(hdnItemIDAutoBillParamedic.Value);
                entity.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitIDAutoBillParamedic.Value);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entity.LastUpdatedDate = DateTime.Now;

                string filterCheck = string.Format("ParamedicID = '{0}' AND ItemID = '{1}' AND HealthcareServiceUnitID = '{2}' AND IsDeleted = 0", entity.ParamedicID, entity.ItemID, entity.HealthcareServiceUnitID);
                List<AutoBillItemParamedic> lstCheck = BusinessLayer.GetAutoBillItemParamedicList(filterCheck);
                if (lstCheck.Count <= 0)
                {
                    BusinessLayer.InsertAutoBillItemParamedic(entity);
                    return true;
                }
                else
                {
                    errMessage = string.Format("Data untuk item dan dokter ini sudah ada.");
                    return false;
                }
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
                AutoBillItemParamedic entity = BusinessLayer.GetAutoBillItemParamedic(Convert.ToInt32(hdnAutoBillItemID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entity.LastUpdatedDate = DateTime.Now;
                BusinessLayer.UpdateAutoBillItemParamedic(entity);
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
                AutoBillItemParamedic entity = BusinessLayer.GetAutoBillItemParamedic(Convert.ToInt32(hdnAutoBillItemID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateAutoBillItemParamedic(entity);
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