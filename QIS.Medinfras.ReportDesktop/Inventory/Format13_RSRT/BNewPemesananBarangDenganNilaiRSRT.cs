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
    public partial class BNewPemesananBarangDenganNilaiRSRT : BaseDailyPortraitRpt
    {
        public BNewPemesananBarangDenganNilaiRSRT()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            base.InitializeReport(param);

            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID)).FirstOrDefault();

            vPurchaseOrderHd entity = BusinessLayer.GetvPurchaseOrderHdList(param[0]).FirstOrDefault();
            lblPurchaseOrderNo.Text = entity.PurchaseOrderNo;
            lblPurchaseOrderDate.Text = entity.OrderDateInString;
            lblNotes.Text = entity.Remarks;
            lblSupplierCode.Text = entity.BusinessPartnerCode;
            lblSupplierName.Text = entity.BusinessPartnerName;
            lblTerm.Text = entity.TermName;
            lblProductLine.Text = entity.ProductLineName;

            BusinessPartners bp = BusinessLayer.GetBusinessPartnersList(string.Format("BusinessPartnerID = '{0}'", entity.BusinessPartnerID)).FirstOrDefault();
            Address ad = BusinessLayer.GetAddressList(string.Format("AddressID = '{0}'", bp.AddressID)).FirstOrDefault();
            lblSupplierAddress.Text = ad.StreetName;
            lblPhoneNo.Text = ad.PhoneNo1;

            lblPOType.Text = entity.PurchaseOrderType;
            lblSyarat.Text = entity.PaymentRemarks;
            lblDeliveryDate.Text = entity.DeliveryDate.ToString(Constant.FormatString.DATE_FORMAT);
            lblPrintDate.Text = string.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));

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
            string oLocation = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_PHARMACY).FirstOrDefault().ParameterValue;
            
            #region Hitung Total

            decimal totalDiskon = 0;
            decimal total = entity.TransactionAmount;
            decimal charges = entity.ChargesAmount;
            decimal downpayment = entity.DownPaymentAmount;

            if (entity.FinalDiscountAmount > 0)
            {
                totalDiskon = entity.FinalDiscountAmount;
                total = entity.TotalDtAmount;
            }
            else
            {
                totalDiskon = (entity.FinalDiscount / 100 * entity.TransactionAmount);
            }

            //if (entity.FinalDiscount > 0)
            //{
            //    totalDiskon = (entity.FinalDiscount / 100 * entity.TransactionAmount);
            //}

            decimal ppn = 0;
            if (entity.IsIncludeVAT)
            {
                ppn = ((entity.VATPercentage * (entity.TransactionAmount - totalDiskon)) / 100);
            }
            else
            {
                ppn = 0;
            }

            decimal pph = 0;
            if (entity.IsPPHInPercentage)
            {
                pph = entity.TransactionAmount * entity.PPHPercentage / 100;
            }
            else
            {
                pph = entity.PPHAmount;
            }

            decimal totalPemesanan = total + ppn + pph - totalDiskon - downpayment + charges;

            #endregion

            #region Total Header

            lblTotal.Text = total.ToString("N2");
            lblDiskon.Text = totalDiskon.ToString("N2");
            lblPPN.Text = ppn.ToString("N2");
            lblTotalPemesanan.Text = totalPemesanan.ToString("N2");

            #endregion


            if (entity.GCPurchaseOrderType == Constant.PurchaseOrderType.DRUGMS || entity.GCPurchaseOrderType == Constant.PurchaseOrderType.ALKES || entity.GCPurchaseOrderType == Constant.PurchaseOrderType.COVID
                    || entity.GCPurchaseOrderType == Constant.PurchaseOrderType.OBAT_RAJAL || entity.GCPurchaseOrderType == Constant.PurchaseOrderType.OBAT_RANAP
                    || entity.GCPurchaseOrderType == Constant.PurchaseOrderType.ALKES_RAJAL || entity.GCPurchaseOrderType == Constant.PurchaseOrderType.ALKES_RANAP)
            {
                lblReportTitle.Text = "SURAT PEMESANAN OBAT DAN ALKES";

                string filterExpressionApprover = string.Format("ParameterCode IN ('{0}', '{1}')", Constant.SettingParameter.MANAGER_FARMASI, Constant.SettingParameter.IM_NAME_CREATE_PO_PHARMACY);
                List<SettingParameterDt> lstFarmasi = BusinessLayer.GetSettingParameterDtList(filterExpressionApprover);

                SettingParameterDt kepFarmasi = lstFarmasi.Where(p => p.ParameterCode == Constant.SettingParameter.MANAGER_FARMASI).FirstOrDefault();
                SettingParameterDt poname = lstFarmasi.Where(p => p.ParameterCode == Constant.SettingParameter.IM_NAME_CREATE_PO_PHARMACY).FirstOrDefault();
               
                string sipa = "";
                string nama_po = "";

                if(!string.IsNullOrEmpty(poname.ParameterValue))
                {
                    string[] data = poname.ParameterValue.Split('|');
                    if(data.Length >0){
                        nama_po= data[0];
                        sipa = data[1];
                    }
                }

                lblKepalaLogistik.Text = nama_po;
                lblJabatan.Text = sipa;
                lblPenanggungjawab.Text = "Apoteker Penanggung Jawab,";
                lblUser.Text = entity.CreatedByName; // AppSession.UserLogin.UserFullName;
            }
            else if (entity.GCPurchaseOrderType == Constant.PurchaseOrderType.FOOD)
            {
                lblReportTitle.Text = "SURAT PEMESANAN BARANG";

                string filterExpressionApprover = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_INSTALASI_GIZI);
                  List<SettingParameterDt> lstSetPar = BusinessLayer.GetSettingParameterDtList(filterExpressionApprover).ToList();

                  SettingParameterDt kepLogistik = lstSetPar.Where(p => p.ParameterCode == Constant.SettingParameter.KEPALA_INSTALASI_GIZI).FirstOrDefault();
                //SettingParameterDt kepDir = lstSetPar.Where(p => p.ParameterCode == Constant.SettingParameter.IM_NAMA_JABATAN_DIREKTUR).FirstOrDefault();

                lblPenanggungjawab.Visible = false;
                lblKepalaLogistik.Visible = false;
                lblJabatan.Visible = false;
                //lblKepalaLogistik.Text = kepLogistik.ParameterValue;
                //lblJabatan.Text = "Ka.Div. U & P";
                lblUser.Text = entity.CreatedByName;
                //lblJabatan1.Text = "Logistik Umum";
                lblAcc.Text = kepLogistik.ParameterValue;
                lblJabatan3.Text = "Ka.Div. U & P";
            }
            else if (entity.GCPurchaseOrderType == Constant.PurchaseOrderType.MARKETING)
            {
                lblReportTitle.Text = "SURAT PEMESANAN BARANG";

                string filterExpressionApprover = string.Format("ParameterCode IN ('{0}', '{1}')", Constant.SettingParameter.PJ_KA_PEMASARAN, Constant.SettingParameter.PJ_KA_BIRO_PENGEMBANGAN);
                List<SettingParameterDt> lstSetPar = BusinessLayer.GetSettingParameterDtList(filterExpressionApprover).ToList();

                SettingParameterDt kepLogistik = lstSetPar.Where(p => p.ParameterCode == Constant.SettingParameter.PJ_KA_PEMASARAN).FirstOrDefault();
                SettingParameterDt kepDir = lstSetPar.Where(p => p.ParameterCode == Constant.SettingParameter.PJ_KA_BIRO_PENGEMBANGAN).FirstOrDefault();

                lblKepalaLogistik.Text = kepLogistik.ParameterValue;
                lblJabatan.Text = "Ka.Bag. Pemasaran";
                lblUser.Text = entity.CreatedByName;
                lblJabatan1.Text = "Pemasaran";
                lblAcc.Text = kepDir.ParameterValue;
                lblJabatan3.Text = "Ka.Biro Pengembangan";
            }
            else
            {
                lblReportTitle.Text = "SURAT PEMESANAN BARANG";

                string filterExpressionApprover = string.Format("ParameterCode IN ('{0}', '{1}')", Constant.SettingParameter.PJ_PEMBELIAN_BARANG, Constant.SettingParameter.IM_NAMA_JABATAN_DIREKTUR);
                List<SettingParameterDt> lstSetPar = BusinessLayer.GetSettingParameterDtList(filterExpressionApprover).ToList();

                SettingParameterDt kepLogistik = lstSetPar.Where(p => p.ParameterCode == Constant.SettingParameter.PJ_PEMBELIAN_BARANG).FirstOrDefault();
                SettingParameterDt kepDir = lstSetPar.Where(p => p.ParameterCode == Constant.SettingParameter.IM_NAMA_JABATAN_DIREKTUR).FirstOrDefault();

                lblKepalaLogistik.Text = kepLogistik.ParameterValue;
                lblJabatan.Text = "Ka.Div. U & P";
                lblUser.Text = entity.CreatedByName;
                lblJabatan1.Text = "Logistik Umum";
                lblAcc.Text = kepDir.ParameterValue;
                lblJabatan3.Text = "Direktur";

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
