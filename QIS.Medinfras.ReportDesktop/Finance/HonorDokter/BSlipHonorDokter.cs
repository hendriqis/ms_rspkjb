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
    public partial class BSlipHonorDokter : BaseCustomDailyPotraitRpt
    {
        public BSlipHonorDokter()
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
            List<GetTRSSummaryDtTransRegistrationDetail> lstDetail = BusinessLayer.GetTRSSummaryDtTransRegistrationDetailList(entity.RSSummaryID);            
            srTransRegistrationDetail.CanGrow = true;
            subTransRegistrationDetail.InitializeReport(lstDetail);

            // Summary per Revenue
            List<GetTRSSummaryTransRegistrationPerRevenue> lstPerRevenue = BusinessLayer.GetGetTRSSummaryTransRegistrationPerRevenueList(entity.RSSummaryID);
            srTransRevenueDetail.CanGrow = true;
            subTransRevenueDetail.InitializeReport(lstPerRevenue);

            // Others Adjustment
            string filterAdj = string.Format("RSSummaryID = {0} AND GCRSAdjustmentType NOT IN ('{1}', '{2}', '{3}', '{4}') AND IsTaxed = 0 AND IsDeleted = 0",
                                                entity.RSSummaryID,
                                                Constant.RevenueSharingAdjustmentType.HNPK_HONOR_TETAP,
                                                Constant.RevenueSharingAdjustmentType.HNJM_SUBSIDI_JASMED_MINIMAL,
                                                Constant.RevenueSharingAdjustmentType.FONDS,
                                                Constant.RevenueSharingAdjustmentType.PPH_21);
            List<vTransRevenueSharingSummaryAdj> lstAdj = BusinessLayer.GetvTransRevenueSharingSummaryAdjList(filterAdj);

            srTransSummaryAdjustmentDetailPlus.CanGrow = true;
            subTransSummaryAdjustmentDetailPlus.InitializeReport(lstAdj.Where(a => a.GCRSAdjustmentGroup == Constant.RevenueSharingAdjustmentGroup.PENAMBAHAN).ToList());

            srTransSummaryAdjustmentDetailMinus.CanGrow = true;
            subTransSummaryAdjustmentDetailMinus.InitializeReport(lstAdj.Where(a => a.GCRSAdjustmentGroup == Constant.RevenueSharingAdjustmentGroup.PENGURANGAN).ToList());

            // Tax Balance
            List<GetTRSSummaryDtParamedicTaxBalance> lstTaxBalance = BusinessLayer.GetTRSSummaryDtParamedicTaxBalanceList(entity.RSSummaryID);
            srParamedicTaxBalance.CanGrow = true;
            subParamedicTaxBalance.InitializeReport(lstTaxBalance);

            // Adjustment Fonds
            string filterFonds = string.Format("RSSummaryID = {0} AND GCRSAdjustmentType IN ('{1}') AND IsDeleted = 0", entity.RSSummaryID, Constant.RevenueSharingAdjustmentType.FONDS);
            List<vTransRevenueSharingSummaryAdj> lstFonds = BusinessLayer.GetvTransRevenueSharingSummaryAdjList(filterFonds);
            srTransSummaryAdjustmentFonds.CanGrow = true;
            subTransRevenueAdjustmentFonds.InitializeReport(lstFonds);

            #endregion

            #region Summary End

            List<GetTRSSummaryDtSummaryTotal> lstTotal = BusinessLayer.GetTRSSummaryDtSummaryTotalList(entity.RSSummaryID);
            decimal totalHonorSetelahPajak = lstTotal.Sum(a => a.TotalHonorSetelahPajak);
            decimal honorTanpaPotongan = lstTotal.Sum(a => a.TotalHonorTanpaPotongan);
            decimal honorKenaPotongan = lstTotal.Sum(a => a.TotalHonorKenaPotongan);
            decimal totalFonds = lstTotal.Sum(a => a.TotalFondsAmount);
            decimal totalOthersAdjustmentPlus = lstTotal.Sum(a => a.TotalAdjustmentPlusAmount);
            decimal totalOthersAdjustmentMinus = lstTotal.Sum(a => a.TotalAdjustmentMinusAmount);
            decimal honorBersih = lstTotal.Sum(a => a.HonorBersih);

            cTotalHonorSetelahPajak.Text = totalHonorSetelahPajak.ToString(Constant.FormatString.NUMERIC_2);
            cHonorTanpaPotongan.Text = honorTanpaPotongan.ToString(Constant.FormatString.NUMERIC_2);
            cHonorKenaPotongan.Text = honorKenaPotongan.ToString(Constant.FormatString.NUMERIC_2);
            cTotalFonds.Text = totalFonds.ToString(Constant.FormatString.NUMERIC_2);
            cOthersAdjustmentPlus.Text = totalOthersAdjustmentPlus.ToString(Constant.FormatString.NUMERIC_2);
            cOthersAdjustmentMinus.Text = totalOthersAdjustmentMinus.ToString(Constant.FormatString.NUMERIC_2);
            cHonorBersih.Text = honorBersih.ToString(Constant.FormatString.NUMERIC_2);

            #endregion

            lblFooter.Text = string.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));

            base.InitializeReport(param);
        }

    }
}
