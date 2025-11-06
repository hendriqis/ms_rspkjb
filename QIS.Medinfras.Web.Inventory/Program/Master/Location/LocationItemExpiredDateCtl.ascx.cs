using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class LocationItemExpiredDateCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            String[] lstParam = param.Split('|');
            hdnID.Value = lstParam[0];
            BindGridView();
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
            if (hdnID.Value != "")
                filterExpression = string.Format("ID = {0}", hdnID.Value);

            List<ItemBalanceDtExpired> lstEntity = BusinessLayer.GetItemBalanceDtExpiredList(filterExpression);
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
                if (hdnID.Value.ToString() != "" && hdnBatchNumber.Value.ToString() != "")
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

        private void ControlToEntity(ItemBalanceDtExpired entity)
        {
            entity.BatchNumber = txtBatchNumber.Text;
            entity.ExpiredDate = Helper.GetDatePickerValue(txtExpiredDate.Text);
            entity.QuantityEND = Convert.ToInt32(txtQuantity.Text);
            entity.QuantityBEGIN = 1;
        }

        protected bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                ItemBalanceDtExpired entity = new ItemBalanceDtExpired();
                entity.ID = Convert.ToInt32(hdnID.Value);
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertItemBalanceDtExpired(entity);
            }
            catch (Exception ex) 
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }

        protected bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                ItemBalanceDtExpired entity = BusinessLayer.GetItemBalanceDtExpired(Convert.ToInt32(hdnID.Value));
                entity.ExpiredDate = Helper.GetDatePickerValue(txtExpiredDate.Text);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateItemBalanceDtExpired(entity);
            }
            catch (Exception ex) 
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }

        protected bool OnSaveDeleteRecord(ref string errMessage) 
        {
            bool result = true;
            try
            {
                BusinessLayer.DeleteItemBalanceDtExpired(Convert.ToInt32(hdnID.Value));
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }
    }
}