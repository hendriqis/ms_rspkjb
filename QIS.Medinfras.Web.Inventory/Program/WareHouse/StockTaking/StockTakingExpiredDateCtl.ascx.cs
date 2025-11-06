using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class StockTakingExpiredDateCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            String[] lstParam = param.Split('|');
            hdnStockTakingID.Value = lstParam[0];
            hdnItemID.Value = lstParam[1];
            hdnQuantityEnd.Value = ((int)Convert.ToDouble(lstParam[2])).ToString();
            BindGridView();
            Helper.SetControlEntrySetting(txtBatchNumber, new ControlEntrySetting(true, true, true), "mpTrxCtlPopup");
            Helper.SetControlEntrySetting(txtExpiredDate, new ControlEntrySetting(true, true, true), "mpTrxCtlPopup");
            Helper.SetControlEntrySetting(txtQuantity, new ControlEntrySetting(true, true, true), "mpTrxCtlPopup");
        }

        protected override void OnLoad(EventArgs e)
        {
            //base.OnLoad(e);
            //if (grdView.Rows.Count < 1)
            //    BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = "1 = 0";
            if (hdnStockTakingID.Value != "" && hdnItemID.Value != "")
                filterExpression = string.Format("StockTakingID = {0} AND ItemID = {1}", hdnStockTakingID.Value,hdnItemID.Value);

            List<StockTakingDtExpired> lstEntity = BusinessLayer.GetStockTakingDtExpiredList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        protected void cbpPopupProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";

            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";

            if (param[0] == "save")
            {
                if (hdnItemID.Value.ToString() != "" && hdnBatchNumber.Value.ToString() != "")
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
            else if (param[0] == "delete")
            {
                if (OnSaveDeleteRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(StockTakingDtExpired entity)
        {
            entity.BatchNumber = txtBatchNumber.Text;
            entity.ExpiredDate = Helper.GetDatePickerValue(txtExpiredDate.Text);
            entity.Quantity = Convert.ToInt32(txtQuantity.Text);
        }

        protected bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                StockTakingDtExpired entity = new StockTakingDtExpired();
                entity.StockTakingID = Convert.ToInt32(hdnStockTakingID.Value);
                entity.ItemID = Convert.ToInt32(hdnItemID.Value);
                ControlToEntity(entity);
                BusinessLayer.InsertStockTakingDtExpired(entity);
            }
            catch (Exception ex) 
            {
                result = false;
                errMessage = ex.Message;
            }
            return result;
        }

        protected bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                StockTakingDtExpired entity = BusinessLayer.GetStockTakingDtExpired(Convert.ToInt32(hdnStockTakingID.Value),Convert.ToInt32(hdnItemID.Value), Request.Form[txtBatchNumber.UniqueID]);
                entity.Quantity = Convert.ToInt32(txtQuantity.Text);
                entity.ExpiredDate = Helper.GetDatePickerValue(txtExpiredDate.Text);
                BusinessLayer.UpdateStockTakingDtExpired(entity);
            }
            catch (Exception ex) 
            {
                result = false;
                errMessage = ex.Message;
            }
            return result;
        }

        protected bool OnSaveDeleteRecord(ref string errMessage) 
        {
            bool result = true;
            try
            {
                BusinessLayer.DeleteStockTakingDtExpired(Convert.ToInt32(hdnStockTakingID.Value),Convert.ToInt32(hdnItemID.Value), Request.Form[txtBatchNumber.UniqueID]);
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
            }
            return result;
        }
    }
}