using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class SubSlipHonorDokterRSDOSOBAA6 : DevExpress.XtraReports.UI.XtraReport
    {
        List<vTransRevenueSharingSummaryHd> lstDataBind = null;

        public SubSlipHonorDokterRSDOSOBAA6()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<vTransRevenueSharingSummaryHd> lst, String rsSummaryID)
        {
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID))[0];
            cHealthcareName.Text = oHealthcare.HealthcareName;
            cHealthcareAddress.Text = oHealthcare.StreetName;
            cHealthcareCityZipCodes.Text = string.Format("{0} {1}", oHealthcare.City, oHealthcare.ZipCode);
            cHealthcarePhone.Text = string.Format("Telp/Fax : {0}/{1}", oHealthcare.PhoneNo1, oHealthcare.FaxNo1);
            cHealthcareName2.Text = oHealthcare.HealthcareName;

            vTransRevenueSharingSummaryHd entity = BusinessLayer.GetvTransRevenueSharingSummaryHdList(rsSummaryID).FirstOrDefault();

            #region Sub Report

            // Detail List
            List<GetTRSSummaryDtTransRegistrationDetail3> lstDetail = BusinessLayer.GetTRSSummaryDtTransRegistrationDetail3List(entity.RSSummaryID);
            srTransRegistrationDetailRSDOSOBAA6.CanGrow = true;
            SubTransRegistrationDetailRSDOSOBAA6.InitializeReport(lstDetail);

            // Others Adjustment
            string filterAdj = string.Format("RSSummaryID = {0} AND GCRSAdjustmentType NOT IN ('{1}', '{2}', '{3}', '{4}') AND IsTaxed = 0 AND IsDeleted = 0",
                                                entity.RSSummaryID,
                                                Constant.RevenueSharingAdjustmentType.HNPK_HONOR_TETAP,
                                                Constant.RevenueSharingAdjustmentType.HNJM_SUBSIDI_JASMED_MINIMAL,
                                                Constant.RevenueSharingAdjustmentType.FONDS,
                                                Constant.RevenueSharingAdjustmentType.PPH_21);
            List<vTransRevenueSharingSummaryAdj> lstAdj = BusinessLayer.GetvTransRevenueSharingSummaryAdjList(filterAdj);
            #endregion


            #region Summary

            // Tax Balance
            List<GetTransRevenueSharingSummaryReportRSDOSOBA> lstHonor = BusinessLayer.GetTransRevenueSharingSummaryReportRSDOSOBAList(entity.RSSummaryID);

            decimal totalBulanIni = lstHonor.Sum(a => a.TotalBRUTOCurrMonth);
            decimal totalDiterimaRS = lstHonor.Sum(a => a.TotalBRUTORSCurrMonth);

            //DPP
            decimal dppBulanIni = lstHonor.Sum(a => a.TotalDPPCurrMonth);
            decimal komDppBulanLalu = lstHonor.Sum(a => a.TotalDPPCumulativeBeforeCurrMonth);
            decimal komDppBulanIni = lstHonor.Sum(a => a.TotalDPPCumulativeToCurrMonth);

            //Pajak
            decimal komPajakBulanLalu = lstHonor.Sum(a => a.TotalTAXCumulativeBeforeCurrMonth);
            decimal komPajakBulanIni = lstHonor.Sum(a => a.TotalTAXCumulativeToCurrMonth);
            decimal pajakBulanIni = lstHonor.Sum(a => a.TotalTAXCurrMonth);

            //Penambahan
            decimal penambahan = lstHonor.Sum(a => a.TotalAdjAdditionCurrMonth);

            //Pengurangan
            decimal pengurangan = lstHonor.Sum(a => a.TotalAdjDeductionCurrMonth);

            decimal pembayaran = lstHonor.Sum(a => a.TotalNETTDRCurrMonth);

            cTotalHonorSetelahPenyesuaian.Text = totalBulanIni.ToString(Constant.FormatString.NUMERIC_2);
            cTotalDiterimaRS.Text = totalDiterimaRS.ToString(Constant.FormatString.NUMERIC_2);
            cDPPBulanIni.Text = dppBulanIni.ToString(Constant.FormatString.NUMERIC_2);
            cKomulatifDPPBulanLalu.Text = komDppBulanLalu.ToString(Constant.FormatString.NUMERIC_2);
            cKomulatifDPPBulanIni.Text = komDppBulanIni.ToString(Constant.FormatString.NUMERIC_2);
            cKomulatifPajakBulanLalu.Text = komPajakBulanLalu.ToString(Constant.FormatString.NUMERIC_2);
            cKomulatifPajakBulanIni.Text = komPajakBulanIni.ToString(Constant.FormatString.NUMERIC_2);
            cPajakBulanIni.Text = pajakBulanIni.ToString(Constant.FormatString.NUMERIC_2);
            cPenambahan.Text = penambahan.ToString(Constant.FormatString.NUMERIC_2);
            cPengurangan.Text = pengurangan.ToString(Constant.FormatString.NUMERIC_2);
            cPembayaran.Text = pembayaran.ToString(Constant.FormatString.NUMERIC_2);

            // TTD 
            SettingParameter setvar = BusinessLayer.GetSettingParameterList(string.Format(
                                "ParameterCode = '{0}'", Constant.SettingParameter.FN_DIREKTUR_KEUANGAN)).FirstOrDefault();
            SettingParameterDt setvardt = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, setvar.ParameterCode);
            //lblTTD1.Text = setvar.ParameterName;
            //lblTTD2.Text = setvardt.ParameterValue;

            #endregion

            #region Footer
            lblTanggal.Text = string.Format("{0}, {1}", oHealthcare.City, entity.RSSummaryDate.AddDays(3).ToString(Constant.FormatString.DATE_FORMAT));
            #endregion

            lstDataBind = lst;

            this.DataSource = lst;
        }
    }
}
