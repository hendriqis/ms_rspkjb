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
    public partial class BNewPemesananBarangTanpaNilai_RSFM : BaseDailyPortraitRpt
    {
        public BNewPemesananBarangTanpaNilai_RSFM()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            base.InitializeReport(param);

            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID)).FirstOrDefault();

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

            if (entity.GCPurchaseOrderType == Constant.PurchaseOrderType.DRUGMS || entity.GCPurchaseOrderType == Constant.PurchaseOrderType.ALKES || entity.GCPurchaseOrderType == Constant.PurchaseOrderType.COVID
                    || entity.GCPurchaseOrderType == Constant.PurchaseOrderType.OBAT_RAJAL || entity.GCPurchaseOrderType == Constant.PurchaseOrderType.OBAT_RANAP
                    || entity.GCPurchaseOrderType == Constant.PurchaseOrderType.ALKES_RAJAL || entity.GCPurchaseOrderType == Constant.PurchaseOrderType.ALKES_RANAP)
            {
                lblReportTitle.Text = "SURAT PEMESANAN OBAT DAN ALKES";

                string filterExpressionApprover = string.Format("ParameterCode IN ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')", Constant.SettingParameter.DIREKTUR_YANMED, Constant.SettingParameter.IM_NAMA_JABATAN_DIREKTUR, Constant.SettingParameter.FN_KEPALABAGIAN_KEUANGAN,
                    Constant.SettingParameter.SA0177, Constant.SettingParameter.IM_APPROVER_PURCHASE_RECEIVE_MEDIK, Constant.SettingParameter.MANAGER_FARMASI);

                List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(filterExpressionApprover);

                SettingParameter direktur = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.DIREKTUR_YANMED).FirstOrDefault();
                SettingParameter jabatanDirektur = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.IM_NAMA_JABATAN_DIREKTUR).FirstOrDefault();

                SettingParameter kepalaKeuanganAkuntansi = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.FN_KEPALABAGIAN_KEUANGAN).FirstOrDefault();
                SettingParameter jabatanKepalaKeuanganAkuntansi = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.FN_KEPALABAGIAN_KEUANGAN).FirstOrDefault();

                SettingParameter kepalaPenunjangMedis = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.SA0177).FirstOrDefault();
                SettingParameter jabatanKepalaPenunjangMedis = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.SA0177).FirstOrDefault();

                SettingParameter kepalaPembelian = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.IM_APPROVER_PURCHASE_RECEIVE_MEDIK).FirstOrDefault();
                SettingParameter jabatanKepalaPembelian = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.IM_APPROVER_PURCHASE_RECEIVE_MEDIK).FirstOrDefault();

                SettingParameter kepalaFarmasi = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.MANAGER_FARMASI).FirstOrDefault();
                SettingParameter jabatanKepalaFarmasi = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.MANAGER_FARMASI).FirstOrDefault();

                lblDirektur.Text = direktur.ParameterValue;
                lblJabatanDirektur.Text = jabatanDirektur.ParameterValue;

                lblKabagKeu.Text = kepalaKeuanganAkuntansi.ParameterValue;
                lblJabatanKabagKeu.Text = jabatanKepalaKeuanganAkuntansi.Notes;

                lblDisetujui3.Text = kepalaPenunjangMedis.ParameterValue;
                lblJabatanDisetujui3.Text = jabatanKepalaPenunjangMedis.Notes;

                lblPembelian.Text = kepalaPembelian.ParameterValue;
                lblJabatanPembelian.Text = jabatanKepalaPembelian.Notes;

                lblMinta.Text = kepalaFarmasi.ParameterValue;
                lblJabatanMinta.Text = jabatanKepalaFarmasi.Notes;

                lblDisetujui4.Visible = false;
                lblJabatanDisetujui4.Visible = false;

                lblDisetujui5.Visible = false;
                lblDirektur2.Visible = false;
                lblJabatanDirektur2.Visible = false;

                lblPembelian2.Visible = false;
                lblJabatanPembelian2.Visible = false;

                lblMinta2.Visible = false;
                lblJabatanMinta2.Visible = false;

                xrLabel38.Visible = false;
                xrLabel39.Visible = false;
                xrLabel43.Visible = false;
            }
            else if (entity.GCPurchaseOrderType == Constant.PurchaseOrderType.LOGISTIC)
            {
                lblReportTitle.Text = "SURAT PEMESANAN BARANG";

                string filterExpressionApprover = string.Format("ParameterCode IN ('{0}','{1}','{2}')", Constant.SettingParameter.MANAGER_KEUANGAN, Constant.SettingParameter.DIREKTUR_YANMED, Constant.SettingParameter.IM_NAMA_JABATAN_DIREKTUR);

                List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(filterExpressionApprover);

                SettingParameter kepalaPembelian = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.MANAGER_KEUANGAN).FirstOrDefault();
                SettingParameter jabatanKepalaPembelian = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.MANAGER_KEUANGAN).FirstOrDefault();

                SettingParameter direktur = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.DIREKTUR_YANMED).FirstOrDefault();
                SettingParameter jabatanDirektur = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.IM_NAMA_JABATAN_DIREKTUR).FirstOrDefault();

                lblDisetujui4.Text = kepalaPembelian.ParameterValue;
                lblJabatanDisetujui4.Text = jabatanKepalaPembelian.Notes;

                lblPembelian2.Text = "";
                lblJabatanPembelian2.Text = "Atasan Langsung";

                lblMinta2.Text = entity.CreatedByName;
                lblJabatanMinta2.Text = "Ruang / Instalasi";

                lblDisetujui.Visible = false;
                lblDisetujui1.Visible = false;
                lblDisetujui2.Visible = false;

                lblDibuatOleh.Visible = false;
                lblDimintaOleh.Visible = false;

                lblDirektur.Visible = false;
                lblJabatanDirektur.Visible = false;

                lblKabagKeu.Visible = false;
                lblJabatanKabagKeu.Visible = false;

                lblDisetujui3.Visible = false;
                lblJabatanDisetujui3.Visible = false;

                lblPembelian.Visible = false;
                lblJabatanPembelian.Visible = false;

                lblMinta.Visible = false;
                lblJabatanMinta.Visible = false;

                string filterExp = string.Format("PurchaseOrderID = {0}", entity.PurchaseOrderID);
                List<vPurchaseOrderDt> lstDt = BusinessLayer.GetvPurchaseOrderDtList(filterExp);
                decimal payment = 5000000;

                if (totalPemesanan >= payment)
                {
                    lblDirektur2.Text = direktur.ParameterValue;
                    lblJabatanDirektur2.Text = jabatanDirektur.ParameterValue;
                }
                else
                {
                    lblDirektur2.Visible = false;
                    lblJabatanDirektur2.Visible = false;
                    lblDisetujui5.Visible = false;
                }
            }
            else
            {
                lblReportTitle.Text = "SURAT PEMESANAN BARANG";

                string filterExpressionApprover = string.Format("ParameterCode IN ('{0}','{1}','{2}')", Constant.SettingParameter.SA0177, Constant.SettingParameter.DIREKTUR_YANMED, Constant.SettingParameter.IM_NAMA_JABATAN_DIREKTUR);

                List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(filterExpressionApprover);

                SettingParameter kepalaPembelian = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.SA0177).FirstOrDefault();
                SettingParameter jabatanKepalaPembelian = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.SA0177).FirstOrDefault();

                SettingParameter direktur = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.DIREKTUR_YANMED).FirstOrDefault();
                SettingParameter jabatanDirektur = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.IM_NAMA_JABATAN_DIREKTUR).FirstOrDefault();

                lblDisetujui4.Text = kepalaPembelian.ParameterValue;
                lblJabatanDisetujui4.Text = jabatanKepalaPembelian.Notes;

                lblPembelian2.Text = "";
                lblJabatanPembelian2.Text = "Atasan Langsung";

                lblMinta2.Text = entity.CreatedByName;
                lblJabatanMinta2.Text = "Petugas Pembelian";

                lblDisetujui.Visible = false;
                lblDisetujui1.Visible = false;
                lblDisetujui2.Visible = false;

                lblDibuatOleh.Visible = false;
                lblDimintaOleh.Visible = false;

                lblDirektur.Visible = false;
                lblJabatanDirektur.Visible = false;

                lblKabagKeu.Visible = false;
                lblJabatanKabagKeu.Visible = false;

                lblDisetujui3.Visible = false;
                lblJabatanDisetujui3.Visible = false;

                lblPembelian.Visible = false;
                lblJabatanPembelian.Visible = false;

                lblMinta.Visible = false;
                lblJabatanMinta.Visible = false;

                string filterExp = string.Format("PurchaseOrderID = {0}", entity.PurchaseOrderID);
                List<vPurchaseOrderDt> lstDt = BusinessLayer.GetvPurchaseOrderDtList(filterExp);
                decimal payment = 5000000;

                if (totalPemesanan >= payment)
                {
                    lblDirektur2.Text = direktur.ParameterValue;
                    lblJabatanDirektur2.Text = jabatanDirektur.ParameterValue;
                }
                else
                {
                    lblDirektur2.Visible = false;
                    lblJabatanDirektur2.Visible = false;
                    lblDisetujui5.Visible = false;
                }
            }
        }

        private void lblNotes_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (String.IsNullOrEmpty(lblNotes.Text))
            {
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
