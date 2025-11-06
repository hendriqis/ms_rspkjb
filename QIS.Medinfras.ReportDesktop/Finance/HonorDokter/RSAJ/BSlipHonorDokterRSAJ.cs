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
    public partial class BSlipHonorDokterRSAJ : BaseCustomDailyPotraitRpt
    {
        public BSlipHonorDokterRSAJ()
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
            List<GetTRSSummaryDtTransRegistrationDetailRSAJ> lstDetail = BusinessLayer.GetTRSSummaryDtTransRegistrationDetailRSAJList(entity.RSSummaryID);            
            srTransRegistrationDetailRSAJ.CanGrow = true;
            subTransRegistrationDetailRSAJNew.InitializeReport(lstDetail);

            // Summary per Revenue
            List<GetTRSSummaryTransRegistrationPerRevenue> lstPerRevenue = BusinessLayer.GetGetTRSSummaryTransRegistrationPerRevenueList(entity.RSSummaryID);
            if (lstPerRevenue.Count > 0)
            {
                srTransRevenueDetailRSAJNew.CanGrow = true;
                subTransRevenueDetailRSAJNew.InitializeReport(lstPerRevenue);
            }
            else
            {
                subTransRevenueDetailRSAJNew.Visible = false;
            }

            // Others Adjustment
            string filterAdj = string.Format("RSSummaryID = {0} AND GCRSAdjustmentType NOT IN ('{1}', '{2}', '{3}', '{4}') AND IsTaxed = 0 AND IsDeleted = 0",
                                                entity.RSSummaryID,
                                                Constant.RevenueSharingAdjustmentType.HNPK_HONOR_TETAP,
                                                Constant.RevenueSharingAdjustmentType.HNJM_SUBSIDI_JASMED_MINIMAL,
                                                Constant.RevenueSharingAdjustmentType.FONDS,
                                                Constant.RevenueSharingAdjustmentType.PPH_21);
            List<vTransRevenueSharingSummaryAdj> lstAdj = BusinessLayer.GetvTransRevenueSharingSummaryAdjList(filterAdj);

            var penambahanCount = lstAdj.Where(a => a.GCRSAdjustmentGroup == Constant.RevenueSharingAdjustmentGroup.PENAMBAHAN).Count();
            var penguranganCount = lstAdj.Where(a => a.GCRSAdjustmentGroup == Constant.RevenueSharingAdjustmentGroup.PENGURANGAN).Count();

            if (penambahanCount > 0)
            {
                srTransRevenueAdjustmentDetailPlusRSAJNew.CanGrow = true;
                subTransRevenueAdjustmentDetailPlusRSAJNew.InitializeReport(lstAdj.Where(a => a.GCRSAdjustmentGroup == Constant.RevenueSharingAdjustmentGroup.PENAMBAHAN).ToList());
            }
            else
            {
                srTransRevenueAdjustmentDetailPlusRSAJNew.CanGrow = false;
                subTransRevenueAdjustmentDetailPlusRSAJNew.Visible = false;
            }

            if (penguranganCount > 0)
            {
                srTransRevenueAdjustmentDetailMinusRSAJNew.CanGrow = true;
                subTransRevenueAdjustmentDetailMinusRSAJNew.InitializeReport(lstAdj.Where(a => a.GCRSAdjustmentGroup == Constant.RevenueSharingAdjustmentGroup.PENGURANGAN).ToList());
            }
            else
            {
                srTransRevenueAdjustmentDetailMinusRSAJNew.CanGrow = false;
                srTransRevenueAdjustmentDetailMinusRSAJNew.Visible = false;
            }

            // Tax Balance
            List<GetTRSSummaryDtParamedicTaxBalanceRSAJ> lstTaxBalance = BusinessLayer.GetTRSSummaryDtParamedicTaxBalanceRSAJList(entity.RSSummaryID);
            srParamedicTaxBalance.CanGrow = true;
            subParamedicTaxBalanceRSAJ.InitializeReport(lstTaxBalance);

            // Adjustment Fonds
            string filterFonds = string.Format("RSSummaryID = {0} AND GCRSAdjustmentType IN ('{1}') AND IsDeleted = 0", entity.RSSummaryID, Constant.RevenueSharingAdjustmentType.FONDS);
            List<vTransRevenueSharingSummaryAdj> lstFonds = BusinessLayer.GetvTransRevenueSharingSummaryAdjList(filterFonds);

            #endregion

            #region Summary End

            //decimal totalTaxBalance = lstTaxBalance.Sum(a => a.Total);
            //decimal totalOthersAdjustmentPlus = lstAdj.Where(a => a.GCRSAdjustmentGroup == Constant.RevenueSharingAdjustmentGroup.PENAMBAHAN).Sum(a => a.AdjustmentAmount);
            //decimal totalOthersAdjustmentMinus = lstAdj.Where(a => a.GCRSAdjustmentGroup == Constant.RevenueSharingAdjustmentGroup.PENGURANGAN).Sum(a => a.AdjustmentAmount);
            //decimal honorBersih = totalTaxBalance + totalOthersAdjustmentPlus - totalOthersAdjustmentMinus;

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
