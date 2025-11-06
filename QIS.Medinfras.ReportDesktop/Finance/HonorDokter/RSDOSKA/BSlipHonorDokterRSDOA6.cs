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
    public partial class BSlipHonorDokterRSDOA6 : BaseA6Rpt
    {
        public BSlipHonorDokterRSDOA6()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string filterHealthcare = string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID);
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(filterHealthcare).FirstOrDefault();

            TransRevenueSharingSummaryHd entity = BusinessLayer.GetTransRevenueSharingSummaryHdList(param[0]).FirstOrDefault();

            #region Sub Report

            // Detail List
            List<GetTRSSummaryDtTransRegistrationDetailRSDO> lstDetail = BusinessLayer.GetTRSSummaryDtTransRegistrationDetailRSDOList(entity.RSSummaryID);
            srTransRegistrationDetailRSDOA6.CanGrow = true;
            SubTransRegistrationDetailRSDOA6.InitializeReport(lstDetail);
            
            // Others Adjustment
            string filterAdj = string.Format("RSSummaryID = {0} AND GCRSAdjustmentType NOT IN ('{1}', '{2}', '{3}', '{4}') AND IsTaxed = 0 AND IsDeleted = 0",
                                                entity.RSSummaryID,
                                                Constant.RevenueSharingAdjustmentType.HNPK_HONOR_TETAP,
                                                Constant.RevenueSharingAdjustmentType.HNJM_SUBSIDI_JASMED_MINIMAL,
                                                Constant.RevenueSharingAdjustmentType.FONDS,
                                                Constant.RevenueSharingAdjustmentType.PPH_21);
            List<vTransRevenueSharingSummaryAdj> lstAdj = BusinessLayer.GetvTransRevenueSharingSummaryAdjList(filterAdj);

            srTransRevenueAdjustmentDetailPlusRSDOA6.CanGrow = true;
            SubTransRevenueAdjustmentDetailPlusRSDOA6.InitializeReport(lstAdj.Where(a => a.GCRSAdjustmentGroup == Constant.RevenueSharingAdjustmentGroup.PENAMBAHAN && a.GCRSAdjustmentType != Constant.RevenueSharingAdjustmentType.PENYESUAIAN_PAJAK_PENAMBAHAN).ToList());
            
            srTransRevenueAdjustmentDetailMinusRSDOA6.CanGrow = true;
            SubTransRevenueAdjustmentDetailMinusRSDOA6.InitializeReport(lstAdj.Where(a => a.GCRSAdjustmentGroup == Constant.RevenueSharingAdjustmentGroup.PENGURANGAN && a.GCRSAdjustmentType != Constant.RevenueSharingAdjustmentType.PENYESUAIAN_PAJAK_PENGURANGAN).ToList());

            #endregion


            #region Summary

            // Tax Balance
            string filterTaxBalance = string.Format("RSSummaryID = {0}", entity.RSSummaryID);
            List<ParamedicTaxBalance> lstTaxBalance = BusinessLayer.GetParamedicTaxBalanceList(filterTaxBalance);

            decimal totalTaxBalance = 0;
            if (lstTaxBalance.Count() > 0)
            {
                totalTaxBalance = lstTaxBalance.Sum(a => a.TotalRevenueSharingAmount);
            }
            else
            {
                totalTaxBalance = lstDetail.Sum(a => a.BrutoAmount);
            }

            decimal totalPenambahan = lstAdj.Where(a => a.GCRSAdjustmentGroup == Constant.RevenueSharingAdjustmentGroup.PENAMBAHAN && a.GCRSAdjustmentType != Constant.RevenueSharingAdjustmentType.PENYESUAIAN_PAJAK_PENAMBAHAN).Sum(a => a.AdjustmentAmount);
            decimal totalPengurangan = lstAdj.Where(a => a.GCRSAdjustmentGroup == Constant.RevenueSharingAdjustmentGroup.PENGURANGAN && a.GCRSAdjustmentType != Constant.RevenueSharingAdjustmentType.PENYESUAIAN_PAJAK_PENGURANGAN).Sum(a => a.AdjustmentAmount);
            decimal totalBulanIni = totalTaxBalance + totalPenambahan - totalPengurangan ;

            // Potongan RS
            decimal totalPotongan20 = lstDetail.Where(a => a.Remarks == "80%").Sum(a => a.BrutoAmount) * 1/5;
            decimal totalPotongan10 = lstDetail.Where(a => a.Remarks == "90%").Sum(a => a.BrutoAmount) * 1/10;
            decimal totalDiterimaRS = totalPotongan20 + totalPotongan10;

            // DPP
            decimal dppBulanIni = totalTaxBalance / 2;
            decimal komDppBulanLalu = lstTaxBalance.Sum(a => a.TaxBaseAccumulativeAmount -  a.TaxBaseAmount);
            decimal komDppBulanIni = lstTaxBalance.Sum(a => a.TaxBaseAccumulativeAmount);

            // PAJAK
            //string filterTax = string.Format("RSSummaryID = {0}", entity.RSSummaryID);
            //List<ParamedicTaxBalance> lstTax = BusinessLayer.GetParamedicTaxBalanceList(filterTax);

            //string currentYear = DateTime.Parse(DateTime.Now.ToString()).Year.ToString();
            //string currentMonth = DateTime.Parse(DateTime.Now.ToString()).Month.ToString();

            string todayJanuariPeriode = DateTime.Now.Year.ToString() + "01";
            string todayPeriode = DateTime.Now.ToString(Constant.FormatString.YYYYMM);

            int todayJanuariPeriodeInt = Convert.ToInt32(todayJanuariPeriode);
            int todayPeriodeInt = Convert.ToInt32(todayPeriode);

            /*periodno nya lebih kecil dari bulan ini dan lebih besar sama dengan dari periodno januari tiap tahunnya*/

            decimal poinF = lstTaxBalance.Where(a => Convert.ToInt32(a.PeriodNo) < todayPeriodeInt && Convert.ToInt32(a.PeriodNo) >= todayJanuariPeriodeInt).Sum(a => a.TaxAmount);


            /*periodno nya lebih kecil sama dengan dari bulan ini dan lebih besar sama dengan dari periodno januari tiap tahunnya*/

            decimal poinG = lstTaxBalance.Where(a => Convert.ToInt32(a.PeriodNo) <= todayPeriodeInt && Convert.ToInt32(a.PeriodNo) >= todayJanuariPeriodeInt).Sum(a => a.TaxAmount);

            
            //decimal pajak = lstTaxBalance.Where(currentYear < currentMonth).Sum(a => a.TaxAmount);
            decimal poinH = lstTaxBalance.Where(a => Convert.ToInt32(a.PeriodNo) == todayPeriodeInt).Sum(a => a.TaxAmount);
            //decimal komPajakBulanLalu1 = lstTaxBalance.Sum(a => a.TaxAmount);
            //decimal taxPercent = lstTaxBalance.Sum(a => a.TaxPercent);
            //decimal komPajakBulanIni = komPajakBulanLalu1 * taxPercent;
            //decimal pajakBulanIni = komPajakBulanIni - komPajakBulanLalu1;

            // Penyesuaian Pajak
            decimal penyesuaianPajak = lstAdj.Where(a => a.GCRSAdjustmentGroup == Constant.RevenueSharingAdjustmentGroup.PENAMBAHAN 
                && a.GCRSAdjustmentType == Constant.RevenueSharingAdjustmentType.PENYESUAIAN_PAJAK_PENAMBAHAN
                && a.GCRSAdjustmentGroup == Constant.RevenueSharingAdjustmentGroup.PENGURANGAN 
                && a.GCRSAdjustmentType == Constant.RevenueSharingAdjustmentType.PENYESUAIAN_PAJAK_PENGURANGAN).Sum(a => a.AdjustmentAmount);

            // Total Penerimaan Bersih
            decimal totalBersih = totalBulanIni - totalDiterimaRS - poinH - penyesuaianPajak;

            cTotalHonorSetelahPenyesuaian.Text = totalBulanIni.ToString(Constant.FormatString.NUMERIC_2);
            cPotongan20.Text = totalPotongan20.ToString(Constant.FormatString.NUMERIC_2);
            cPotongan10.Text = totalPotongan10.ToString(Constant.FormatString.NUMERIC_2);
            cTotalDiterimaRS.Text = totalDiterimaRS.ToString(Constant.FormatString.NUMERIC_2);
            cDPPBulanIni.Text = dppBulanIni.ToString(Constant.FormatString.NUMERIC_2);
            cKomulatifDPPBulanLalu.Text = komDppBulanLalu.ToString(Constant.FormatString.NUMERIC_2);
            cKomulatifDPPBulanIni.Text = komDppBulanIni.ToString(Constant.FormatString.NUMERIC_2);
            cKomulatifPajakBulanLalu.Text = poinF.ToString(Constant.FormatString.NUMERIC_2);
            cKomulatifPajakBulanIni.Text = poinG.ToString(Constant.FormatString.NUMERIC_2);
            cPajakBulanIni.Text = poinH.ToString(Constant.FormatString.NUMERIC_2);
            cPenyesuaianPajak.Text = penyesuaianPajak.ToString(Constant.FormatString.NUMERIC_2);
            cPenerimaanBersih.Text = totalBersih.ToString(Constant.FormatString.NUMERIC_2);

            // TTD 
            SettingParameter setvar = BusinessLayer.GetSettingParameterList(string.Format(
                                "ParameterCode = '{0}'", Constant.SettingParameter.FN_DIREKTUR_KEUANGAN)).FirstOrDefault();
            SettingParameterDt setvardt = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, setvar.ParameterCode);
            lblTTD1.Text = setvar.ParameterName;
            lblTTD2.Text = setvardt.ParameterValue;

            #endregion

            base.InitializeReport(param);
        }
    }
}
