using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class ARNoteCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                string[] lstParam = param.Split('|');

                hdnTransactionCode.Value = lstParam[0];
                hdnTransactionID.Value = lstParam[1];

                if (hdnTransactionCode.Value == Constant.TransactionCode.AR_INVOICE_PAYER || hdnTransactionCode.Value == Constant.TransactionCode.AR_INVOICE_NON_OPERATIONAL || hdnTransactionCode.Value == Constant.TransactionCode.AR_INVOICE_PAYER_ADJUSTMENT)
                {
                    string filterAII = string.Format("TransactionCode = '{0}' AND ARInvoiceID = {1}", hdnTransactionCode.Value, hdnTransactionID.Value);
                    vARInvoiceHd aii = BusinessLayer.GetvARInvoiceHdList(filterAII).FirstOrDefault();

                    txtTransactionNo.Text = aii.ARInvoiceNo;
                    hdnBusinessPartnerID.Value = aii.BusinessPartnerID.ToString();
                    txtBusinessPartnerName.Text = aii.BusinessPartnerName;
                }
                else if (hdnTransactionCode.Value == Constant.TransactionCode.AR_INVOICE_PATIENT || hdnTransactionCode.Value == Constant.TransactionCode.AR_INVOICE_PATIENT_ADJUSTMENT)
                {
                    string filterAII = string.Format("TransactionCode = '{0}' AND ARInvoiceID = {1}", hdnTransactionCode.Value, hdnTransactionID.Value);
                    vARInvoiceHd aii = BusinessLayer.GetvARInvoiceHdList(filterAII).FirstOrDefault();

                    txtTransactionNo.Text = aii.ARInvoiceNo;
                    hdnBusinessPartnerID.Value = aii.BusinessPartnerID.ToString();
                    txtBusinessPartnerName.Text = string.Format("{0} - ({1}) {2}", aii.BusinessPartnerName, aii.MedicalNo, aii.PatientName);
                }

                BindGridView();
                SetControlProperties();
            } 
        }

        private void SetControlProperties()
        {
            txtARNoteDate.Attributes.Add("validationgroup", "mpARNotes");
            txtARNoteTime.Attributes.Add("validationgroup", "mpARNotes");
            txtNoteText.Attributes.Add("validationgroup", "mpARNotes");
            txtARNoteDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtARNoteTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("TransactionCode = '{0}' AND TransactionID = {1} AND IsDeleted = 0", hdnTransactionCode.Value, hdnTransactionID.Value);
            
            grdARNotes.DataSource = BusinessLayer.GetvARNoteList(filterExpression);
            grdARNotes.DataBind();
        }

        protected void cbpARNotes_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string result = "";
            string errMessage = "";
            if (e.Parameter == "save")
            {
                result = "save|";
                if (hdnARNoteID.Value.ToString() != "")
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

        private void ControlToEntity(ARNote entity)
        {
            entity.NoteDate = Helper.GetDatePickerValue(txtARNoteDate);
            entity.NoteTime = txtARNoteTime.Text;
            entity.NoteText = txtNoteText.Text;
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                ARNote entity = new ARNote();
                ControlToEntity(entity);

                entity.TransactionCode = hdnTransactionCode.Value;
                entity.TransactionID = Convert.ToInt32(hdnTransactionID.Value);
                entity.BusinessPartnerID = Convert.ToInt32(hdnBusinessPartnerID.Value);

                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertARNote(entity);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                result = false;
            }
            return result;
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                ARNote entity = BusinessLayer.GetARNote(Convert.ToInt32(hdnARNoteID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateARNote(entity);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                result = false;
            }
            return result;
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                ARNote entity = BusinessLayer.GetARNote(Convert.ToInt32(hdnARNoteID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateARNote(entity);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                result = false;
            }
            return result;
        }
    }
}