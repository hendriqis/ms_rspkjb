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
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class UpdateEDAndBatchNoCtl : BaseViewPopupCtl
    {
        private const string DEFAULT_GRDVIEW_FILTER = "BusinessPartnerID > 1 AND GCBusinessPartnerType = '{0}' AND (HealthcareID = '{1}' OR HealthcareID IS NULL) AND IsDeleted = 0";
        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            hdnPurchaseReceiveIDCtl.Value = param;
            vPurchaseReceiveHd entity = BusinessLayer.GetvPurchaseReceiveHdList(string.Format("PurchaseReceiveID = {0}", hdnPurchaseReceiveIDCtl.Value)).FirstOrDefault();
            txtPurchaseReceiveNo.Text = entity.PurchaseReceiveNo;
            Helper.SetControlEntrySetting(txtBatchNumberCtl, new ControlEntrySetting(true, true, true), "mpEntryCtl");
            Helper.SetControlEntrySetting(txtExpiredDateCtl, new ControlEntrySetting(true, true, true), "mpEntryCtl");
            Helper.SetControlEntrySetting(txtQuantityCtl, new ControlEntrySetting(true, true, true), "mpEntryCtl");
            //SetControlProperties();
            //BindGridView(1, true, ref PageCount);
        }

        //private void SetControlProperties()
        //{
        //}

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            Int32 receiveID = 0;
            Int32 orderID = 0;
            Int32 itemID = 0;

            if (!String.IsNullOrEmpty(hdnPurchaseReceiveIDCtl.Value))
            {
                receiveID = Convert.ToInt32(hdnPurchaseReceiveIDCtl.Value);
            }

            if (!String.IsNullOrEmpty(hdnPurchaseOrderID.Value))
            {
                orderID = Convert.ToInt32(hdnPurchaseOrderID.Value);
            }

            if (!String.IsNullOrEmpty(hdnItemID.Value))
            {
                itemID = Convert.ToInt32(hdnItemID.Value);
            }

            string filterExpression = String.Format("PurchaseReceiveID = {0} AND PurchaseOrderID = {1} AND ItemID = {2}", receiveID, orderID, itemID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPurchaseReceiveDtExpiredRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 8);
            }

            List<vPurchaseReceiveDtExpired> lstEntity = BusinessLayer.GetvPurchaseReceiveDtExpiredList(filterExpression, 8, pageIndex, "ExpiredDate ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            int pageCount = 1;

            string[] param = e.Parameter.Split('|');

            string result = param[0] + "|";
            string errMessage = "";

            if (param[0] == "changepage")
            {
                BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                result = "changepage";
            }
            else if (param[0] == "refresh")
            {
                BindGridView(1, true, ref pageCount);
                result = "refresh|" + pageCount;
            }
            else
            {
                if (param[0] == "save")
                {
                    if (hdnIDCtl.Value.ToString() != "" && hdnBatchNumberCtl.Value.ToString() != "")
                    {
                        if (OnSaveEditRecord(ref errMessage))
                        {
                            result += "success";
                            BindGridView(1, true, ref pageCount);
                        }
                        else
                        {
                            result += string.Format("fail|{0}", errMessage);
                        }
                    }
                    else
                    {
                        if (OnSaveAddRecord(ref errMessage))
                        {
                            result += "success";
                            BindGridView(1, true, ref pageCount);
                        }
                        else
                        {
                            result += string.Format("fail|{0}", errMessage);
                        }
                    }
                }
                else if (param[0] == "delete")
                {
                    if (OnDeleteRecord(ref errMessage))
                    {
                        result += "success";
                        BindGridView(1, true, ref pageCount);
                    }
                    else
                    {
                        result += string.Format("fail|{0}", errMessage);
                    }
                }
            }
            result += "|" + pageCount;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(PurchaseReceiveDtExpired entity)
        {
            entity.BatchNumber = txtBatchNumberCtl.Text;
            entity.ExpiredDate = Helper.GetDatePickerValue(txtExpiredDateCtl.Text);
            entity.Quantity = Convert.ToDecimal(txtQuantityCtl.Text);
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                PurchaseReceiveDtExpired entity = new PurchaseReceiveDtExpired();
                entity.ID = Convert.ToInt32(hdnIDCtl.Value);
                ControlToEntity(entity);

                PurchaseReceiveDtExpired entityCheck = BusinessLayer.GetPurchaseReceiveDtExpired(entity.ID, entity.BatchNumber);
                if (entityCheck == null)
                {
                    BusinessLayer.InsertPurchaseReceiveDtExpired(entity);
                }
                else
                {
                    result = false;
                    errMessage = "Maaf, Batch Number Ini Sudah Digunakan";
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
            }
            return result;
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                PurchaseReceiveDtExpired entity = BusinessLayer.GetPurchaseReceiveDtExpired(Convert.ToInt32(hdnIDCtl.Value), txtBatchNumberCtl.Text);
                entity.Quantity = Convert.ToDecimal(txtQuantityCtl.Text);
                entity.ExpiredDate = Helper.GetDatePickerValue(txtExpiredDateCtl.Text);
                BusinessLayer.UpdatePurchaseReceiveDtExpired(entity);
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                BusinessLayer.DeletePurchaseReceiveDtExpired(Convert.ToInt32(hdnIDCtl.Value), hdnBatchNumberCtl.Value);
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