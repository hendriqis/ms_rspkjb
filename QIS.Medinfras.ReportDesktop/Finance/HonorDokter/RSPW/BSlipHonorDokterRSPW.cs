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
    public partial class BSlipHonorDokterRSPW : BaseCustomDailyPotraitRpt
    {
        public BSlipHonorDokterRSPW()
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
            List<GetTRSSummaryDtTransRegistrationDetailRSPW> lstDetail = BusinessLayer.GetTRSSummaryDtTransRegistrationDetailRSPWList(entity.RSSummaryID);            
            srTransRegistrationDetail.CanGrow = true;
            subTransRegistrationDetailRSPW.InitializeReport(lstDetail);

            // Summary per Revenue
            List<GetTRSSummaryTransRegistrationPerRevenueRSPW> lstPerRevenue = BusinessLayer.GetTRSSummaryTransRegistrationPerRevenueRSPWList(entity.RSSummaryID);
            srTransRevenueDetail.CanGrow = true;
            subTransRevenueDetailRSPW.InitializeReport(lstPerRevenue);

            // Others Adjustment
            string filterAdj = string.Format("RSSummaryID = {0} AND GCRSAdjustmentType NOT IN ('{1}', '{2}', '{3}', '{4}') AND IsTaxed = 0 AND IsDeleted = 0",
                                                entity.RSSummaryID,
                                                Constant.RevenueSharingAdjustmentType.HNPK_HONOR_TETAP,
                                                Constant.RevenueSharingAdjustmentType.HNJM_SUBSIDI_JASMED_MINIMAL,
                                                Constant.RevenueSharingAdjustmentType.FONDS,
                                                Constant.RevenueSharingAdjustmentType.PPH_21);
            List<vTransRevenueSharingSummaryAdj> lstAdj = BusinessLayer.GetvTransRevenueSharingSummaryAdjList(filterAdj);

            srTransSummaryAdjustmentDetailPlus.CanGrow = true;
            subTransSummaryAdjustmentDetailPlusRSSES.InitializeReport(lstAdj.Where(a => a.GCRSAdjustmentGroup == Constant.RevenueSharingAdjustmentGroup.PENAMBAHAN && !a.IsTaxed).ToList());

            srTransSummaryAdjustmentDetailMinus.CanGrow = true;
            subTransSummaryAdjustmentDetailMinusRSSES.InitializeReport(lstAdj.Where(a => a.GCRSAdjustmentGroup == Constant.RevenueSharingAdjustmentGroup.PENGURANGAN && !a.IsTaxed).ToList());

            // Tax Balance
            List<GetTRSSummaryDtParamedicTaxBalanceRSAJ> lstTaxBalance = BusinessLayer.GetTRSSummaryDtParamedicTaxBalanceRSAJList(entity.RSSummaryID);
            srParamedicTaxBalance.CanGrow = true;
            subParamedicTaxBalanceRSPW.InitializeReport(lstTaxBalance);

            #endregion

            #region Summary End
            List<GetTRSSummaryDtSummaryTotal> lstTotal = BusinessLayer.GetTRSSummaryDtSummaryTotalList(entity.RSSummaryID);

            decimal totalHonorSetelahPajak = lstTotal.Sum(a => a.TotalHonorSetelahPajak);
            decimal totalAdjustmentPlus = lstTotal.Sum(a => a.TotalAdjustmentPlusAmount);
            decimal totalAdjustmentMinus = lstTotal.Sum(a => a.TotalAdjustmentMinusAmount);
            decimal honorBersih = lstTotal.Sum(a => a.HonorBersih);

            cTotalHonorSetelahPajak.Text = totalHonorSetelahPajak.ToString(Constant.FormatString.NUMERIC_2);
            cOthersAdjustmentPlus.Text = totalAdjustmentPlus.ToString(Constant.FormatString.NUMERIC_2);
            cOthersAdjustmentMinus.Text = totalAdjustmentMinus.ToString(Constant.FormatString.NUMERIC_2);
            cHonorBersih.Text = honorBersih.ToString(Constant.FormatString.NUMERIC_2);

            #endregion

            lblFooter.Text = string.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));

            base.InitializeReport(param);
        }

    }
}
