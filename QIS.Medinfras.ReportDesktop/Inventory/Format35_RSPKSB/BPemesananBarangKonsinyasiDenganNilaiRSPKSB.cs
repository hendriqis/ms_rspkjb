using System;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BPemesananBarangKonsinyasiDenganNilaiRSPKSB : BaseDailyPortraitRpt
    {
        public BPemesananBarangKonsinyasiDenganNilaiRSPKSB()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            base.InitializeReport(param);

            vPurchaseOrderHd entity = BusinessLayer.GetvPurchaseOrderHdList(param[0])[0];
            List<PurchaseOrderDt> entityDT = BusinessLayer.GetPurchaseOrderDtList(string.Format("PurchaseOrderID = {0} AND IsDeleted = 0", entity.PurchaseOrderID));
            lblPurchaseOrderNo.Text = entity.PurchaseOrderNo;
            lblPurchaseOrderDate.Text = entity.OrderDateInString;
            lblNotes.Text = entity.Remarks;
            lblSupplierCode.Text = entity.BusinessPartnerCode;
            lblSupplierName.Text = entity.BusinessPartnerName;
            lblTerm.Text = entity.TermName;

            BusinessPartners bp = BusinessLayer.GetBusinessPartnersList(string.Format("BusinessPartnerID = '{0}'", entity.BusinessPartnerID)).FirstOrDefault();
            Address ad = BusinessLayer.GetAddressList(string.Format("AddressID = '{0}'", bp.AddressID)).FirstOrDefault();
            lblSupplierAddress.Text = ad.StreetName;
            lblPhoneNo.Text = ad.PhoneNo1;

            StandardCode sc = BusinessLayer.GetStandardCodeList(string.Format("StandardCodeID = '{0}'", entity.GCPurchaseOrderType)).FirstOrDefault();
            lblPOType.Text = sc.StandardCodeName;
            lblSyarat.Text = entity.PaymentRemarks;
            lblDeliveryDate.Text = entity.DeliveryDate.ToString(Constant.FormatString.DATE_FORMAT);
            lblPrintDate.Text = string.Format("Bekasi, {0}", DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));

            if (entity.IsUrgent == true)
            {
                lblIsUrgent.Visible = true;
            }
            else 
            {
                lblIsUrgent.Visible = false;            
            }

            string filterExpression = string.Format("ParameterCode IN ('{0}', '{1}')", Constant.SettingParameter.IM_MAX_TRANSACTION_AMOUNT_PURCHASE_ORDER,
                        Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_PHARMACY);
            List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression);
            //Decimal amount = Convert.ToDecimal(lstParam.ParameterValue[0]);
            Decimal amount = Convert.ToDecimal(lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_MAX_TRANSACTION_AMOUNT_PURCHASE_ORDER).FirstOrDefault().ParameterValue);
            //String location = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_PHARMACY).FirstOrDefault().ParameterValue; //Konsinyasi RSSBB - yg TTD tidak berdasarkan lokasi tetapi hanya berdasarkan Jenis Persediaan

            #region Hitung Total

            decimal total = entity.TransactionAmount;

            decimal totalDiskon = 0;
            if (entity.FinalDiscount > 0)
            {
                totalDiskon = (entity.FinalDiscount / 100 * entity.TransactionAmount);
            }

            decimal ppn = 0;
            if (entity.IsIncludeVAT)
            {
                ppn = ((entity.VATPercentage * (entity.TransactionAmount - totalDiskon)) / 100);
            }
            else
            {
                ppn = 0;
            }

            decimal totalPemesanan = (total - totalDiskon) + ppn;

            #endregion

            #region Total Header

            lblTotal.Text = total.ToString("N2");
            lblDiskon.Text = totalDiskon.ToString("N2");
            lblPPN.Text = ppn.ToString("N2");
            lblTotalPemesanan.Text = totalPemesanan.ToString("N2");

            #endregion

            if (entity.GCPurchaseOrderType != Constant.PurchaseOrderType.LOGISTIC && entity.GCPurchaseOrderType != Constant.PurchaseOrderType.ASSET)
            {
                lblReportTitle.Text = "SURAT PEMESANAN OBAT DAN ALKES";

                string filterExpressionDirektur = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.DIREKTUR_YANMED);
                string filterExpressionApprover = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.MANAGER_FARMASI);
                string filterExpressionKeuangan = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.MANAGER_KEUANGAN);
                List<SettingParameter> direktur = BusinessLayer.GetSettingParameterList(filterExpressionDirektur);
                List<SettingParameter> keuangan = BusinessLayer.GetSettingParameterList(filterExpressionKeuangan);

                lblUser.Text = entity.CreatedByName;

                lblMengetahui.Text = direktur.FirstOrDefault().ParameterValue;
                lblJabatanMengetahui.Text = direktur.FirstOrDefault().Notes;

                lblMengetahui1.Text = keuangan.FirstOrDefault().ParameterValue;
                lblJabatanMengetahui1.Text = keuangan.FirstOrDefault().Notes;
            }
            else
            {
                lblReportTitle.Text = "SURAT PEMESANAN BARANG";

                string filterExpressionDirektur = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.DIREKTUR_YANMED);
                List<SettingParameter> direktur = BusinessLayer.GetSettingParameterList(filterExpressionDirektur);

                lblUser.Text = entity.CreatedByName;

                lblMengetahui.Text = direktur.FirstOrDefault().ParameterValue;
                lblJabatanMengetahui.Text = direktur.FirstOrDefault().Notes;

                lblMengetahui1.Visible = false;
                xrLine2.Visible = false;
                lblJabatanMengetahui1.Visible = false;
            }
        }

        private void lblNotes_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (String.IsNullOrEmpty(lblNotes.Text)) {
                xrLabel17.Text = "";
                xrLabel16.Text = "";
            }
        }

        private void lblRemarks_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String PO = lblPOType.Text;
            if (PO == "Bahan Makanan")
            {
                lblRemarks.Text = "Sebelum pukul 08:00";
            }
            else
            {
                lblRemarks.Text = "Sebelum pukul 12:00";
            }
        }

    }
}
