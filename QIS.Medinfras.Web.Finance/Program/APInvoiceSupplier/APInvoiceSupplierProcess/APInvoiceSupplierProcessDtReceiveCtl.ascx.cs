using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class APInvoiceSupplierProcessDtReceiveCtl : BaseViewPopupCtl
    {
        protected string GetVATPercentage()
        {
            return hdnVATPercentage.Value;
        }

        public override void InitializeDataControl(string param)
        {
            hdnVATPercentage.Value = BusinessLayer.GetSettingParameter(Constant.SettingParameter.VAT_PERCENTAGE).ParameterValue;

            List<StandardCode> listStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}') AND IsDeleted = 0", Constant.StandardCode.CURRENCY_CODE, Constant.StandardCode.CHARGES_TYPE));
            List<Term> listTerm = BusinessLayer.GetTermList(string.Format("1=1"));
            Methods.SetComboBoxField<StandardCode>(cboChargesType, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.CHARGES_TYPE).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboCurrency, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.CURRENCY_CODE).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<Term>(cboTerm, listTerm, "TermName", "TermID");
            cboChargesType.SelectedIndex = 0;
            cboCurrency.SelectedIndex = 0;

            string[] temp = param.Split('|');
            hdnIDCtl.Value = temp[0];
            hdnPurchaseReceiveIDCtl.Value = temp[1];
            hdnPurchaseOrderIDCtl.Value = temp[2];

            PurchaseReceiveHd entity = BusinessLayer.GetPurchaseReceiveHd(Convert.ToInt32(hdnPurchaseReceiveIDCtl.Value));
            txtPurchaseReceiveNo.Text = entity.PurchaseReceiveNo;
            txtPurchaseReceiveDate.Text = entity.ReceivedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPurchaseReceiveTime.Text = entity.ReceivedTime;
            txtReferenceNo.Text = entity.ReferenceNo;
            txtDateReferrence.Text = entity.ReferenceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPPH.Text = entity.PPHAmount.ToString();
            txtDPReferrenceNo.Text = entity.DownPaymentReferenceNo;
            txtDP.Text = entity.DownPaymentAmount.ToString();
            txtCharges.Text = entity.ChargesAmount.ToString();
            txtStamp.Text = entity.StampAmount.ToString();
            cboChargesType.Value = entity.GCChargesType.ToString();
            cboTerm.Value = entity.TermID.ToString();
            txtNotes.Text = entity.Remarks;
            cboCurrency.Value = entity.GCCurrencyCode.ToString();
            txtKurs.Text = entity.CurrencyRate.ToString();
            chkPPN.Checked = entity.IsIncludeVAT;
            decimal transactionAmount = entity.TransactionAmount - entity.DiscountAmount;
            txtTotalOrder.Text = transactionAmount.ToString();
            txtFinalDiscount.Text = entity.FinalDiscount.ToString();

            hdnVATPercentage.Value = entity.VATPercentage.ToString("0.##");

            if (entity.TransactionCode == Constant.TransactionCode.CONSIGNMENT_RECEIVE)
            {
                trDiskonFinal.Style.Remove("display");
                trDiskonFinal1.Style.Remove("display");
                trJenisPembiayaan.Style.Remove("display");
                trOngkosKirim.Style.Remove("display");
                trPph.Style.Add("display", "none");
                trBiayaMaterai.Style.Add("display", "none");
            }
            else
            {
                trDiskonFinal.Style.Add("display", "none");
                trDiskonFinal1.Style.Add("display", "none");
                trJenisPembiayaan.Style.Add("display", "none");
                trOngkosKirim.Style.Add("display", "none");
                trPph.Style.Remove("display");
                trBiayaMaterai.Style.Remove("display");
            }

            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("PurchaseReceiveID = {0} AND GCItemDetailStatus != '{1}'", hdnPurchaseReceiveIDCtl.Value, Constant.TransactionStatus.VOID);
            List<vPurchaseReceiveDt> lstEntity = BusinessLayer.GetvPurchaseReceiveDtList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void cbpPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }
    }
}