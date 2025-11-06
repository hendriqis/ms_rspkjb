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
    public partial class BSlipHonorDokterRSPBT : BaseCustomDailyPotraitRpt
    {
        public BSlipHonorDokterRSPBT()
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
            List<GetTRSSummaryDtTransRegistrationDetailRSPBT> lstDetail = BusinessLayer.GetTRSSummaryDtTransRegistrationDetailRSPBTList(entity.RSSummaryID);            
            srTransRegistrationDetailRSPBT.CanGrow = true;
            subTransRegistrationDetailRSPBT.InitializeReport(lstDetail);

            // Summary per Revenue
            List<GetTRSSummaryTransRegistrationPerRevenue> lstPerRevenue = BusinessLayer.GetGetTRSSummaryTransRegistrationPerRevenueList(entity.RSSummaryID);
            srTransRevenueDetailRSPBT.CanGrow = true;
            subTransRevenueDetailRSPBT.InitializeReport(lstPerRevenue);

            // Others Adjustment
            string filterAdj = string.Format("RSSummaryID = {0} AND GCRSAdjustmentType NOT IN ('{1}', '{2}', '{3}', '{4}') AND IsTaxed = 0 AND IsDeleted = 0",
                                                entity.RSSummaryID,
                                                Constant.RevenueSharingAdjustmentType.HNPK_HONOR_TETAP,
                                                Constant.RevenueSharingAdjustmentType.HNJM_SUBSIDI_JASMED_MINIMAL,
                                                Constant.RevenueSharingAdjustmentType.FONDS,
                                                Constant.RevenueSharingAdjustmentType.PPH_21);
            List<vTransRevenueSharingSummaryAdj> lstAdj = BusinessLayer.GetvTransRevenueSharingSummaryAdjList(filterAdj);            
            srTransSummaryAdjustmentDetailRSPBT.CanGrow = true;
            subTransSummaryAdjustmentDetailRSPBT.InitializeReport(lstAdj);

            // Tax Balance
            string filterTaxBalance = string.Format("RSSummaryID = {0}", entity.RSSummaryID);
            List<ParamedicTaxBalance> lstTaxBalance = BusinessLayer.GetParamedicTaxBalanceList(filterTaxBalance);
            srParamedicTaxBalance.CanGrow = true;
            subParamedicTaxBalanceRSPBT.InitializeReport(lstTaxBalance);

            // Adjustment Fonds
            string filterFonds = string.Format("RSSummaryID = {0} AND GCRSAdjustmentType IN ('{1}') AND IsDeleted = 0", entity.RSSummaryID, Constant.RevenueSharingAdjustmentType.FONDS);
            List<vTransRevenueSharingSummaryAdj> lstFonds = BusinessLayer.GetvTransRevenueSharingSummaryAdjList(filterFonds);

            #endregion

            #region Summary End

            decimal totalTaxBalance = lstTaxBalance.Sum(a => a.cfTotalHonorSetelahPajakRSPBT);
            decimal honorTanpaPotongan = lstDetail.Where(a => !a.IsChargesHasHospitalFee).Sum(b => b.TaxBalanceEndAmount);
            decimal honorKenaPotongan = lstDetail.Where(a => a.IsChargesHasHospitalFee).Sum(b => b.TaxBalanceEndAmount);
            decimal totalOthersAdjustment = lstAdj.Sum(a => a.AdjustmentAmount);
            decimal honorBersih = totalTaxBalance - totalOthersAdjustment;

            cTotalHonorSetelahPajak.Text = totalTaxBalance.ToString(Constant.FormatString.NUMERIC_2);
            //cHonorTanpaPotongan.Text = honorTanpaPotongan.ToString(Constant.FormatString.NUMERIC_2);
            //cHonorKenaPotongan.Text = honorKenaPotongan.ToString(Constant.FormatString.NUMERIC_2);
            //cTotalFonds.Text = totalFonds.ToString(Constant.FormatString.NUMERIC_2);
            cOthersAdjustment.Text = totalOthersAdjustment.ToString(Constant.FormatString.NUMERIC_2);
            cHonorBersih.Text = honorBersih.ToString(Constant.FormatString.NUMERIC_2);

            #endregion

            lblFooter.Text = string.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));

            base.InitializeReport(param);
        }

    }
}
