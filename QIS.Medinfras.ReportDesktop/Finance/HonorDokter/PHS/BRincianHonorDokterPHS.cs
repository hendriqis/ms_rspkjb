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
    public partial class BRincianHonorDokterPHS : BaseCustomDailyPotraitRpt
    {
        public BRincianHonorDokterPHS()
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
            List<GetTRSSummaryDtTransRegistrationDetailPHS> lstDetail = BusinessLayer.GetTRSSummaryDtTransRegistrationDetailPHSList(entity.RSSummaryID);            
            srTransRegistrationDetailPHS.CanGrow = true;
            SubTransRegistrationDetailPHS.InitializeReport(lstDetail);

            // Summary per Revenue
            List<GetTRSSummaryTransRegistrationPerRevenue> lstPerRevenue = BusinessLayer.GetGetTRSSummaryTransRegistrationPerRevenueList(entity.RSSummaryID);
            //srTransRevenueDetail.CanGrow = true;
            //subTransRevenueDetail.InitializeReport(lstPerRevenue);

            // Others Adjustment
            string filterAdj = string.Format("RSSummaryID = {0} AND GCRSAdjustmentType NOT IN ('{1}', '{2}', '{3}', '{4}') AND IsTaxed = 0 AND IsDeleted = 0",
                                                entity.RSSummaryID,
                                                Constant.RevenueSharingAdjustmentType.HNPK_HONOR_TETAP,
                                                Constant.RevenueSharingAdjustmentType.HNJM_SUBSIDI_JASMED_MINIMAL,
                                                Constant.RevenueSharingAdjustmentType.FONDS,
                                                Constant.RevenueSharingAdjustmentType.PPH_21);
            List<vTransRevenueSharingSummaryAdj> lstAdj = BusinessLayer.GetvTransRevenueSharingSummaryAdjList(filterAdj);

            srTransSummaryAdjustmentDetailPlusPHS.CanGrow = true;
            SubTransSummaryAdjustmentDetailPlusPHS.InitializeReport(lstAdj.Where(a => a.GCRSAdjustmentGroup == Constant.RevenueSharingAdjustmentGroup.PENAMBAHAN && a.GCRSAdjustmentType != Constant.RevenueSharingAdjustmentType.PENYESUAIAN_PAJAK_PENAMBAHAN && !a.IsTaxed).ToList());

            srTransSummaryAdjustmentDetailMinusPHS.CanGrow = true;
            SubTransSummaryAdjustmentDetailMinusPHS.InitializeReport(lstAdj.Where(a => a.GCRSAdjustmentGroup == Constant.RevenueSharingAdjustmentGroup.PENGURANGAN && a.GCRSAdjustmentType != Constant.RevenueSharingAdjustmentType.PENYESUAIAN_PAJAK_PENGURANGAN && !a.IsTaxed).ToList());

            List<vTransRevenueSharingSummaryAdj> lstAdjPlus2 = lstAdj.Where(a => a.GCRSAdjustmentGroup == Constant.RevenueSharingAdjustmentGroup.PENAMBAHAN && a.GCRSAdjustmentType == Constant.RevenueSharingAdjustmentType.PENYESUAIAN_PAJAK_PENAMBAHAN).ToList();
            if (lstAdjPlus2.Count() > 0)
            {
                srTransSummaryAdjustmentDetailPlusPHS2.Visible = true;
                srTransSummaryAdjustmentDetailPlusPHS2.CanGrow = true;
                subTransRevenueAdjustmentDetailPlusPHS2.InitializeReport(lstAdjPlus2);
            }
            else
            {
                srTransSummaryAdjustmentDetailPlusPHS2.Visible = false;
            }

            List<vTransRevenueSharingSummaryAdj> lstAdjMin2 = lstAdj.Where(a => a.GCRSAdjustmentGroup == Constant.RevenueSharingAdjustmentGroup.PENGURANGAN && a.GCRSAdjustmentType == Constant.RevenueSharingAdjustmentType.PENYESUAIAN_PAJAK_PENGURANGAN).ToList();
            if (lstAdjMin2.Count() > 0)
            {
                srTransSummaryAdjustmentDetailMinusPHS2.Visible = true;
                srTransSummaryAdjustmentDetailMinusPHS2.CanGrow = true;
                subTransRevenueAdjustmentDetailMinusPHS2.InitializeReport(lstAdjMin2);
            }
            else
            {
                srTransSummaryAdjustmentDetailMinusPHS2.Visible = false;
            }

            // Tax Balance
            string filterTaxBalance = string.Format("RSSummaryID = {0}", entity.RSSummaryID);
            List<ParamedicTaxBalance> lstTaxBalance = BusinessLayer.GetParamedicTaxBalanceList(filterTaxBalance);
            srParamedicTaxBalancePHS.CanGrow = true;
            subParamedicTaxBalancePHS.InitializeReport(lstTaxBalance);


            // Adjustment Fonds
            string filterFonds = string.Format("RSSummaryID = {0} AND GCRSAdjustmentType IN ('{1}') AND IsDeleted = 0", entity.RSSummaryID, Constant.RevenueSharingAdjustmentType.FONDS);
            List<vTransRevenueSharingSummaryAdj> lstFonds = BusinessLayer.GetvTransRevenueSharingSummaryAdjList(filterFonds);
            //srTransSummaryAdjustmentFonds.CanGrow = true;
            //subTransRevenueAdjustmentFonds.InitializeReport(lstFonds);

            #endregion

            #region Summary End


            List<GetTRSSummaryDtTransRegistrationDetailPHS> lstHonor = BusinessLayer.GetTRSSummaryDtTransRegistrationDetailPHSList(entity.RSSummaryID);
            string filterTax = string.Format("RSSummaryID = {0}", entity.RSSummaryID);
            List<ParamedicTaxBalance> lstTax = BusinessLayer.GetParamedicTaxBalanceList(filterTax);

            decimal revenueSharingAmount = 0;
            if (lstHonor.Count() > 0)
            {
                revenueSharingAmount = lstHonor.Sum(a => a.RevenueSharingAmount) - lstTax.Sum(b => b.TaxAmount + b.ExtraTaxAmount);
            }

            decimal totalTaxBalance = 0;
            if (lstTaxBalance.Count() > 0)
            {
                totalTaxBalance = lstTaxBalance.Sum(a => a.cfTotalHonorSetelahPajak);
            }
            else
            {
                totalTaxBalance = lstDetail.Sum(a => a.RevenueSharingAmount);
            }

            //decimal honorKenaPotongan = (lstDetail.Where(a => a.IsChargesHasHospitalFee).Sum(b => b.TaxBalanceEndAmount));
            //decimal honorTanpaPotongan = totalTaxBalance - honorKenaPotongan;

            //decimal honorTanpaPotongan = lstDetail.Where(a => !a.IsChargesHasHospitalFee).Sum(b => b.TaxBalanceEndAmount);
            //decimal honorKenaPotongan = (lstDetail.Where(a => a.IsChargesHasHospitalFee).Sum(b => b.TaxBalanceEndAmount)) + (lstPerRevenue.Where(a => a.RevenueSharingCode.Contains("ADJ-")).Sum(b => b.TaxBalanceEndAmount));
            //decimal honorKenaPotongan = lstPerRevenue.Sum(a => a.TaxBalanceEndAmount);

            //decimal totalFonds = lstFonds.Sum(a => a.AdjustmentAmount);
            decimal totalOthersAdjustmentPlus = lstAdj.Where(a => a.GCRSAdjustmentGroup == Constant.RevenueSharingAdjustmentGroup.PENAMBAHAN).Sum(a => a.AdjustmentAmount);
            decimal totalOthersAdjustmentMinus = lstAdj.Where(a => a.GCRSAdjustmentGroup == Constant.RevenueSharingAdjustmentGroup.PENGURANGAN).Sum(a => a.AdjustmentAmount);
            decimal honorBersih = revenueSharingAmount  + totalOthersAdjustmentPlus - totalOthersAdjustmentMinus;

            cTotalHonorSetelahPajak.Text = revenueSharingAmount.ToString(Constant.FormatString.NUMERIC_2);
            //cHonorTanpaPotongan.Text = honorTanpaPotongan.ToString(Constant.FormatString.NUMERIC_2);
            //cHonorKenaPotongan.Text = honorKenaPotongan.ToString(Constant.FormatString.NUMERIC_2);
            //cTotalFonds.Text = totalFonds.ToString(Constant.FormatString.NUMERIC_2);
            cOthersAdjustmentPlus.Text = totalOthersAdjustmentPlus.ToString(Constant.FormatString.NUMERIC_2);
            cOthersAdjustmentMinus.Text = totalOthersAdjustmentMinus.ToString(Constant.FormatString.NUMERIC_2);
            cHonorBersih.Text = honorBersih.ToString(Constant.FormatString.NUMERIC_2);

            #endregion

            //lblFooter.Text = string.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));

            base.InitializeReport(param);
        }

    }
}
