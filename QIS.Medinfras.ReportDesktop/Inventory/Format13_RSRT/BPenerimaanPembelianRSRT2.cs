using System;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.ReportDesktop;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BPenerimaanPembelianRSRT2 : BaseDailyPortraitRpt
    {
        public BPenerimaanPembelianRSRT2()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vPurchaseReceiveHd entityHd = BusinessLayer.GetvPurchaseReceiveHdList(param[0])[0];

            lblSupplierCode.Text = String.Format("{0} - {1}", entityHd.SupplierCode, entityHd.SupplierName);
            lblNoFaktur.Text = entityHd.ReferenceNo;
            lblTanggalFaktur.Text = entityHd.ReferenceDate.ToString(Constant.FormatString.DATE_FORMAT);
            lblRemarks.Text = entityHd.Remarks;

            //lblDiskon.Text = entityHd.FinalDiscount.ToString("N2");
            if (entityHd.IsIncludeVAT)
            {
                lblPPN.Text = ((entityHd.VATPercentage * (entityHd.TransactionAmount - entityHd.FinalDiscount)) / 100).ToString("N2");
            }
            else
            {
                lblPPN.Text = "0.00";
            }
            StandardCode sc = BusinessLayer.GetStandardCode(entityHd.GCChargesType);
            lblChargesType.Text = sc.StandardCodeName;

            if (entityHd.GCChargesType == Constant.ChargesType.MATERAI)
            {
                lblCharges.Text = entityHd.StampAmount.ToString("N2");
            }
            else 
            {
               lblCharges.Text = entityHd.ChargesAmount.ToString("N2");
            }

            lblTotalPenerimaan.Text = entityHd.NetTransactionAmount.ToString("N2");

            lblCreatedByName.Text = entityHd.CreatedByName;
            lblInfo.Text = string.Format("Jakarta, {0}", DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));

            string log = string.Format("ParameterCode IN ('{0}','{1}','{2}')", Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_LOGISTIC, Constant.SettingParameter.IM_DEFAULT_KODE_LOKASI_MEDIK, Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_PHARMACY);
            List<SettingParameterDt> lstlogistic = BusinessLayer.GetSettingParameterDtList(log);
            SettingParameterDt logistic = lstlogistic.Where(t => t.ParameterCode == Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_LOGISTIC).FirstOrDefault();
            SettingParameterDt medic = lstlogistic.Where(t => t.ParameterCode == Constant.SettingParameter.IM_DEFAULT_KODE_LOKASI_MEDIK).FirstOrDefault();
            SettingParameterDt pharmacy = lstlogistic.Where(t => t.ParameterCode == Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_PHARMACY).FirstOrDefault();

            List<SettingParameterDt> lstSetvarDt = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}','{1}','{2}','{3}','{4}','{5}')", 
                Web.Common.Constant.SettingParameter.IM_APPROVER_PURCHASE_RECEIVE_LOGISTIC, Web.Common.Constant.SettingParameter.IM_APPROVER_PURCHASE_RECEIVE_PHARMACY,
                Constant.SettingParameter.KEPALA_LOGISTIK_UMUM, Constant.SettingParameter.KEPALA_LOGISTIK_OBAT, 
                Constant.SettingParameter.KEPALA_LOGISTIK_MEDIK, Constant.SettingParameter.IM_APPROVER_PURCHASE_RECEIVE_MEDIK));

            vLocation locLogistic = new vLocation();
            if (logistic.ParameterValue != null && logistic.ParameterValue != "" && logistic.ParameterValue != "0")
            {
                locLogistic = BusinessLayer.GetvLocationList(string.Format("LocationID = '{0}'", logistic.ParameterValue)).FirstOrDefault();
            }

            vLocation locMedic = new vLocation();
            if (medic.ParameterValue != null && medic.ParameterValue != "" && medic.ParameterValue != "0")
            {
                locMedic = BusinessLayer.GetvLocationList(string.Format("LocationID = '{0}'", medic.ParameterValue)).FirstOrDefault();
            }

            vLocation locPharmacy = new vLocation();
            if (pharmacy.ParameterValue != null && pharmacy.ParameterValue != "" && pharmacy.ParameterValue != "0")
            {
                locPharmacy = BusinessLayer.GetvLocationList(string.Format("LocationID = '{0}'", pharmacy.ParameterValue)).FirstOrDefault();
            }

            if (entityHd.LocationID == locPharmacy.LocationID)
            {
                SettingParameterDt lstParam1 = lstSetvarDt.Where(t => t.ParameterCode == Constant.SettingParameter.KEPALA_LOGISTIK_OBAT).FirstOrDefault();
                SettingParameterDt lstParam2 = lstSetvarDt.Where(t => t.ParameterCode == Web.Common.Constant.SettingParameter.IM_APPROVER_PURCHASE_RECEIVE_PHARMACY).FirstOrDefault();
                lblKALogistik.Text = entityHd.ApprovedByName;
//                lblKALogistik.Text = lstParam1.ParameterValue;
                lblNamaPJMedik.Text = lstParam2.ParameterValue;
                xrLabel40.Text = "KA. Farmasi";
                xrLabel39.Text = "Farmasi Gudang";
                xrLabel36.Text = "Pembelian";
            }
            else if (entityHd.LocationID == locMedic.LocationID)
            {
                SettingParameterDt lstParam1 = lstSetvarDt.Where(t => t.ParameterCode == Constant.SettingParameter.KEPALA_LOGISTIK_MEDIK).FirstOrDefault();
                SettingParameterDt lstParam2 = lstSetvarDt.Where(t => t.ParameterCode == Web.Common.Constant.SettingParameter.IM_APPROVER_PURCHASE_RECEIVE_MEDIK).FirstOrDefault();
                lblKALogistik.Text = lstParam1.ParameterValue;
                lblNamaPJMedik.Text = lstParam2.ParameterValue;
                xrLabel40.Text = "PJ Gudang Medik";
                xrLabel39.Text = "KA. Logistik";
                xrLabel36.Text = "Petugas";
            }
            else if (entityHd.LocationID == locLogistic.LocationID)
            {
                SettingParameterDt lstParam1 = lstSetvarDt.Where(t => t.ParameterCode == Constant.SettingParameter.KEPALA_LOGISTIK_UMUM).FirstOrDefault();
                SettingParameterDt lstParam2 = lstSetvarDt.Where(t => t.ParameterCode == Web.Common.Constant.SettingParameter.IM_APPROVER_PURCHASE_RECEIVE_LOGISTIC).FirstOrDefault();
                lblKALogistik.Text = lstParam1.ParameterValue;
                lblNamaPJMedik.Text = lstParam2.ParameterValue;
                xrLabel40.Text = "PJ Pengadaan";
                xrLabel39.Text = "KA. Logistik Umum";
                xrLabel36.Text = "Petugas";
            }
            else
            {
                SettingParameterDt lstParam1 = lstSetvarDt.Where(t => t.ParameterCode == Constant.SettingParameter.KEPALA_LOGISTIK_UMUM).FirstOrDefault();
                SettingParameterDt lstParam2 = lstSetvarDt.Where(t => t.ParameterCode == Web.Common.Constant.SettingParameter.IM_APPROVER_PURCHASE_RECEIVE_LOGISTIC).FirstOrDefault();
                lblKALogistik.Text = lstParam1.ParameterValue;
                lblNamaPJMedik.Text = lstParam2.ParameterValue;
                xrLabel40.Text = "PJ Pengadaan";
                xrLabel39.Text = "KA. Logistik Umum";
                xrLabel36.Text = "Petugas";
            }

            base.InitializeReport(param);
        }
    }
}