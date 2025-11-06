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
    public partial class BNewPemesananBarangDenganNilai_RSSK : BaseDailyPortraitRpt
    {
        public BNewPemesananBarangDenganNilai_RSSK()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            base.InitializeReport(param);

            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID)).FirstOrDefault();

            vPurchaseOrderHd entity = BusinessLayer.GetvPurchaseOrderHdList(param[0]).FirstOrDefault();

            string filterExp = string.Format("PurchaseOrderID = {0} AND ProductLineID IN (SELECT ProductLineID FROM ProductLine WHERE IsFixedAsset = 1 AND IsDeleted = 0)", entity.PurchaseOrderID);
            List<vPurchaseOrderHd> lstProductLine = BusinessLayer.GetvPurchaseOrderHdList(filterExp);

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
            lblTanggal.Text = string.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));

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

            if (entity.ProductLineCode == "INV-01" || entity.ProductLineCode == "INV-02")
            {
                lblReportTitle.Text = "SURAT PEMESANAN OBAT";

                string filterExpressionApprover = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_LOGISTIK_MEDIK);
                string filterExpressionApoteker = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.PHARMACIST);
                List<SettingParameter> kepLogistik = BusinessLayer.GetSettingParameterList(filterExpressionApprover);
                List<SettingParameter> kepApt = BusinessLayer.GetSettingParameterList(filterExpressionApoteker);

                lblPengadaan.Text = kepLogistik.FirstOrDefault().ParameterValue;
                lblJabatan1.Text = "";

                lblMengetahui1.Visible = false;
                xrLine2.Visible = false;
                lblJabatanMengetahui1.Visible = false;

                lblMengetahui.Text = "";
                lblJabatanMengetahui.Text = "";
            }
            else if (entity.ProductLineCode == "INV-03")
            {
                lblReportTitle.Text = "SURAT PEMESANAN ALKES";

                string filterExpressionApprover = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_LOGISTIK_MEDIK);
                string filterExpressionApoteker = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.PHARMACIST);
                List<SettingParameter> kepLogistik = BusinessLayer.GetSettingParameterList(filterExpressionApprover);
                List<SettingParameter> kepApt = BusinessLayer.GetSettingParameterList(filterExpressionApoteker);

                lblPengadaan.Text = kepLogistik.FirstOrDefault().ParameterValue;
                lblJabatan1.Text = "";

                lblMengetahui1.Visible = false;
                xrLine2.Visible = false;
                lblJabatanMengetahui1.Visible = false;

                lblMengetahui.Text = "";
                lblJabatanMengetahui.Text = "";
            }
            else if (entity.ProductLineCode == "INV-04" || entity.ProductLineCode == "INV-05")
            {
                lblReportTitle.Text = "SURAT PEMESANAN GAS MEDIS";

                string filterExpressionApprover = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_LOGISTIK_MEDIK);
                string filterExpressionApoteker = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.PHARMACIST);
                List<SettingParameter> kepLogistik = BusinessLayer.GetSettingParameterList(filterExpressionApprover);
                List<SettingParameter> kepApt = BusinessLayer.GetSettingParameterList(filterExpressionApoteker);

                lblPengadaan.Text = kepLogistik.FirstOrDefault().ParameterValue;
                lblJabatan1.Text = "";

                lblMengetahui1.Visible = false;
                xrLine2.Visible = false;
                lblJabatanMengetahui1.Visible = false;

                lblMengetahui.Text = "";
                lblJabatanMengetahui.Text = "";
            }
            else if (entity.ProductLineCode == "INV-06")
            {
                lblReportTitle.Text = "SURAT PEMESANAN BAHAN MAKANAN";

                string filterExpressionApprover = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_LOGISTIK_MEDIK);
                string filterExpressionGizi = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_UNIT_LOGISTIK_BAHAN_MAKANAN);
                List<SettingParameter> kepLogistik = BusinessLayer.GetSettingParameterList(filterExpressionApprover);
                List<SettingParameter> kepGiz = BusinessLayer.GetSettingParameterList(filterExpressionGizi);

                lblMengetahui.Text = kepLogistik.FirstOrDefault().ParameterValue;
                lblJabatanMengetahui.Text = "";

                lblMengetahui1.Visible = false;
                xrLine2.Visible = false;
                lblJabatanMengetahui1.Visible = false;

                lblPengadaan.Text = kepGiz.FirstOrDefault().ParameterValue;
                lblJabatan1.Text = "";
            }
            else if (entity.ProductLineCode == "INV-07" || entity.ProductLineCode == "INV-08" || entity.ProductLineCode == "INV-09" || entity.ProductLineCode == "INV-10"
                     || entity.ProductLineCode == "INV-11" || entity.ProductLineCode == "INV-12" || entity.ProductLineCode == "INV-13")
            {
                lblReportTitle.Text = "SURAT PEMESANAN BARANG UMUM";

                string filterExpressionApprover = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_LOGISTIK_MEDIK);
                string filterExpressionUmum = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_UNIT_LOGISTIK_UMUM);
                List<SettingParameter> kepLogistik = BusinessLayer.GetSettingParameterList(filterExpressionApprover);
                List<SettingParameter> kepUm = BusinessLayer.GetSettingParameterList(filterExpressionUmum);

                lblMengetahui1.Visible = false;
                xrLine2.Visible = false;
                lblJabatanMengetahui1.Visible = false;

                lblMengetahui.Text = kepLogistik.FirstOrDefault().ParameterValue;
                lblJabatanMengetahui.Text = "";

                lblPengadaan.Text = kepUm.FirstOrDefault().ParameterValue;
                lblJabatan1.Text = "";
            }
            else if (lstProductLine.Count > 0)
            {
                lblReportTitle.Text = "SURAT PEMESANAN BARANG/INVENTARIS";

                string filterExpressionApprover = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_LOGISTIK_MEDIK);
                string filterExpressionFinance = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.FN_KEPALABAGIAN_KEUANGAN);
                string filterExpressionDirektur = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.DIREKTUR_UMUM);
                List<SettingParameter> kepLogistik = BusinessLayer.GetSettingParameterList(filterExpressionApprover);
                List<SettingParameter> kepFinance = BusinessLayer.GetSettingParameterList(filterExpressionFinance);
                List<SettingParameter> direktur = BusinessLayer.GetSettingParameterList(filterExpressionDirektur);

                lblMengetahui.Text = kepFinance.FirstOrDefault().ParameterValue;
                lblJabatanMengetahui.Text = "";

                lblMengetahui1.Text = direktur.FirstOrDefault().ParameterValue;
                lblJabatanMengetahui1.Text = "";

                lblPengadaan.Text = kepLogistik.FirstOrDefault().ParameterValue;
                lblJabatan1.Text = "";
            }
            else 
            {
                lblReportTitle.Text = "SURAT PEMESANAN BARANG UMUM";

                string filterExpressionApprover = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_LOGISTIK_MEDIK);
                string filterExpressionUmum = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_UNIT_LOGISTIK_UMUM);
                List<SettingParameter> kepLogistik = BusinessLayer.GetSettingParameterList(filterExpressionApprover);
                List<SettingParameter> kepUm = BusinessLayer.GetSettingParameterList(filterExpressionUmum);

                lblMengetahui1.Visible = false;
                xrLine2.Visible = false;
                lblJabatanMengetahui1.Visible = false;

                lblMengetahui.Text = kepLogistik.FirstOrDefault().ParameterValue;
                lblJabatanMengetahui.Text = "";

                lblPengadaan.Text = kepUm.FirstOrDefault().ParameterValue;
                lblJabatan1.Text = "";
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
