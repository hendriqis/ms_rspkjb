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
    public partial class ServiceUnitAutoBillItemDailyEntryCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnHealthcareServiceUnitIDCtlAutoDaily.Value = param;
            vHealthcareServiceUnit entity = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", hdnHealthcareServiceUnitIDCtlAutoDaily.Value)).FirstOrDefault();
            txtHealthcareName.Text = entity.HealthcareName;
            txtServiceUnitName.Text = entity.ServiceUnitName;

            BindGridView();


            txtItemCodeAutoDaily.Attributes.Add("validationgroup", "mpEntryPopup");
            txtQtyAutoDaily.Attributes.Add("validationgroup", "mpEntryPopup");
        }

        private void BindGridView()
        {
            grdView.DataSource = BusinessLayer.GetvServiceUnitAutoBillItemAdditionalDailyList(string.Format("HealthcareServiceUnitID = {0} AND IsDeleted = 0 ORDER BY ItemName1 ASC", hdnHealthcareServiceUnitIDCtlAutoDaily.Value));
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
                if (hdnIDAutoDaily.Value.ToString() != "")
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

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                ServiceUnitAutoBillItemAdditionalDaily entity = new ServiceUnitAutoBillItemAdditionalDaily();
                entity.Quantity = Convert.ToDecimal(txtQtyAutoDaily.Text);
                entity.ItemID = Convert.ToInt32(hdnItemIDAutoDaily.Value);
                entity.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitIDCtlAutoDaily.Value);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertServiceUnitAutoBillItemAdditionalDaily(entity);
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
                ServiceUnitAutoBillItemAdditionalDaily entity = BusinessLayer.GetServiceUnitAutoBillItemAdditionalDaily(Convert.ToInt32(hdnIDAutoDaily.Value));
                entity.Quantity = Convert.ToDecimal(txtQtyAutoDaily.Text);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateServiceUnitAutoBillItemAdditionalDaily(entity);
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
                ServiceUnitAutoBillItemAdditionalDaily entity = BusinessLayer.GetServiceUnitAutoBillItemAdditionalDaily(Convert.ToInt32(hdnIDAutoDaily.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateServiceUnitAutoBillItemAdditionalDaily(entity);
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