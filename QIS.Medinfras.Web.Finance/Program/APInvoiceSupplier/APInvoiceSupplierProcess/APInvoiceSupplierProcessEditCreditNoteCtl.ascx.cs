using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class APInvoiceSupplierProcessEditCreditNoteCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] parameter = param.Split('|');
            hdnPurchaseInvoiceDtID.Value = parameter[0];

            vSupplierCreditNote entityCreditNote = BusinessLayer.GetvSupplierCreditNoteList(string.Format("CreditNoteID = {0}", parameter[1])).FirstOrDefault();

            vPurchaseReturnHd entityReturnHd = BusinessLayer.GetvPurchaseReturnHdList(string.Format("PurchaseReturnID = {0}", entityCreditNote.PurchaseReturnID)).FirstOrDefault();
            hdnSupplierID.Value = entityReturnHd.BusinessPartnerID.ToString();
            txtSupplierCode.Text = entityReturnHd.BusinessPartnerCode;
            txtSupplierName.Text = entityReturnHd.SupplierName;
            hdnPurchaseReturnID.Value = entityReturnHd.PurchaseReturnID.ToString();
            txtPurchaseReturnNo.Text = entityReturnHd.PurchaseReturnNo;
            txtPurchaseReturnDate.Text = entityReturnHd.ReturnDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            chkPPN.Checked = entityReturnHd.IsIncludeVAT;

            if (entityCreditNote == null)
            {
                IsAdd = true;
                txtCNAmountBeforePPN.Text = (entityReturnHd.TransactionAmount * (100 + entityReturnHd.VATPercentage) / 100).ToString();
            }
            else
            {
                IsAdd = false;
                txtCreditNoteNo.Text = entityCreditNote.CreditNoteNo;
                hdnCreditNoteID.Value = entityCreditNote.CreditNoteID.ToString();
                txtCreditNoteDate.Text = entityCreditNote.CreditNoteDateInString;
                txtGCCreditNoteType.Text = entityCreditNote.CreditNoteType;
                txtReferenceNo.Text = entityCreditNote.ReferenceNo;
                txtReferenceDate.Text = entityCreditNote.ReferenceDateInString;
                txtTaxInvoiceNo.Text = entityCreditNote.TaxInvoiceNo;
                txtTaxInvoiceDate.Text = entityCreditNote.TaxInvoiceDateInString;
                chkPPN.Checked = entityCreditNote.IsIncludeVAT;
                hdnIsIncludeVAT.Value = entityCreditNote.IsIncludeVAT.ToString();
                hdnVATPercentage.Value = entityCreditNote.VATPercentage.ToString();
                txtCNAmountBeforePPN.Text = entityCreditNote.CNAmount.ToString();
                txtCNAmountAfterPPN.Text = entityCreditNote.TotalCreditNoteAmount.ToString("N2");
                txtRemarks.Text = entityCreditNote.Remarks;
            }
        }
        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtCNAmountBeforePPN, new ControlEntrySetting(true, true, true, 0));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(hdnIsIncludeVAT, new ControlEntrySetting(false, false, false, 0));
            SetControlEntrySetting(hdnVATPercentage, new ControlEntrySetting(false, false, false, 0));
        }

        private void ControlToEntity(SupplierCreditNote entity)
        {
            entity.CNAmount = Convert.ToDecimal(txtCNAmountBeforePPN.Text);
            entity.Remarks = txtRemarks.Text;
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = true;
            PurchaseInvoiceHdDao purchaseInvoiceHdDao = new PurchaseInvoiceHdDao(ctx);
            SupplierCreditNoteDao entityHdDao = new SupplierCreditNoteDao(ctx);
            PurchaseInvoiceDtDao purchaseInvoiceDtDao = new PurchaseInvoiceDtDao(ctx);
            try
            {
                PurchaseInvoiceHd entityInvoiceHd = purchaseInvoiceHdDao.Get(BusinessLayer.GetPurchaseInvoiceDt(Convert.ToInt32(hdnPurchaseInvoiceDtID.Value)).PurchaseInvoiceID);
                if (entityInvoiceHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    SupplierCreditNote entity = entityHdDao.Get(Convert.ToInt32(hdnCreditNoteID.Value));
                    ControlToEntity(entity);
                    //entity.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                    entityHdDao.Update(entity);

                    decimal nilai = entity.CNAmount;
                    decimal ppn = entity.VATPercentage;
                    decimal total = 0;
                    if (entity.IsIncludeVAT)
                    {
                        total = nilai + (nilai * ppn / 100);
                    }
                    else
                    {
                        total = nilai;
                    }

                    PurchaseInvoiceDt purchaseInvoiceDt = purchaseInvoiceDtDao.Get(Convert.ToInt32(hdnPurchaseInvoiceDtID.Value));
                    purchaseInvoiceDt.CreditNoteAmount = total;
                    purchaseInvoiceDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    purchaseInvoiceDtDao.Update(purchaseInvoiceDt);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Hutang supplier tidak dapat diubah. Harap refresh halaman ini.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();  
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
    }
}