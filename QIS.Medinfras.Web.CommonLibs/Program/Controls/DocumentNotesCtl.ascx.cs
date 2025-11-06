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

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class DocumentNotesCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            String[] paramSplit = param.Split('|');

            String ParamID = paramSplit[0];
            String ParamDocumentID = paramSplit[1];

            hdnParamID.Value = ParamID;
            hdnParamDocumentID.Value = ParamDocumentID;

            if (ParamID == "CO")
            {
                vCustomerContract customercontract = BusinessLayer.GetvCustomerContractList(string.Format("ContractID = {0}", ParamDocumentID)).FirstOrDefault();
                txtDocumentNo.Text = customercontract.ContractNo;
            }
            else if (ParamID == "PO")
            {
                vPurchaseOrderHd purchaseorder = BusinessLayer.GetvPurchaseOrderHdList(string.Format("PurchaseOrderID = {0}", ParamDocumentID)).FirstOrDefault();
                txtDocumentNo.Text = purchaseorder.PurchaseOrderNo;
            }

            BindGridView();
            SetControlProperties();
        }

        private void SetControlProperties()
        {
            txtDocumentNoteDate.Attributes.Add("validationgroup", "mpDocumentNotes");
            txtDocumentNoteText.Attributes.Add("validationgroup", "mpDocumentNotes");
            txtDocumentNoteDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        }

        private void BindGridView()
        {
            if (hdnParamID.Value == "CO")
            {
                grdDocumentNotes.DataSource = BusinessLayer.GetvDocumentLogList(
                string.Format("DocumentID = {0} AND IsDeleted = 0 AND GCDocumentType = '{1}'", hdnParamDocumentID.Value, Constant.DocumentNoteType.CUSTOMER_CONTRACT));
                grdDocumentNotes.DataBind();
            }
            else if (hdnParamID.Value == "PO")
            {
                grdDocumentNotes.DataSource = BusinessLayer.GetvDocumentLogList(
                string.Format("DocumentID = {0} AND IsDeleted = 0 AND GCDocumentType = '{1}'", hdnParamDocumentID.Value, Constant.DocumentNoteType.PURCHASE_ORDER));
                grdDocumentNotes.DataBind();
            }
        }

        protected void cbpDocumentNotes_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string result = "";
            string errMessage = "";
            if (e.Parameter == "save")
            {
                result = "save|";
                if (hdnDocumentNoteID.Value.ToString() != "")
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
            else
            {
                result = "delete|";
                if (OnDeleteRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(DocumentLog entity)
        {
            entity.DocumentID = Convert.ToInt32(hdnParamDocumentID.Value);

            if (hdnParamID.Value == "CO")
            {
                entity.GCDocumentType = Constant.DocumentNoteType.CUSTOMER_CONTRACT;
            }
            else if (hdnParamID.Value == "PO")
            {
                entity.GCDocumentType = Constant.DocumentNoteType.PURCHASE_ORDER;
            }

            string tempDate = Helper.GetDatePickerValue(txtDocumentNoteDate.Text).ToString(Constant.FormatString.DATE_FORMAT_112);
            string tempTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT_FULL);
            DateTime dt = Helper.YYYYMMDDHourToDate(tempDate + " " + tempTime);
            entity.LogDate = dt;

            entity.LogRemarks = txtDocumentNoteText.Text;
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                DocumentLog entity = new DocumentLog();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertDocumentLog(entity);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }
            return result;
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                DocumentLog entity = BusinessLayer.GetDocumentLog(Convert.ToInt32(hdnDocumentNoteID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateDocumentLog(entity);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }
            return result;
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                DocumentLog entity = BusinessLayer.GetDocumentLog(Convert.ToInt32(hdnDocumentNoteID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateDocumentLog(entity);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }
            return result;
        }
    }
}