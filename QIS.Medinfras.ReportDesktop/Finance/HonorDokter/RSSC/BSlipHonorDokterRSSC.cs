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
    public partial class BSlipHonorDokterRSSC : BaseCustomDailyLandscapeRpt
    {
        public BSlipHonorDokterRSSC()
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
            List<GetTRSSummaryDtTransRegistrationDetail2> lstDetail = BusinessLayer.GetTRSSummaryDtTransRegistrationDetail2List(entity.RSSummaryID);
            srTransRegistrationDetail.CanGrow = true;
            subTransRegistrationDetail.InitializeReport(lstDetail.Where(a => a.ID != -1).ToList());

            // Others Adjustment
            string filterAdj = string.Format("RSSummaryID = {0} AND IsFromUpload = 0 AND IsTaxed = 0 AND IsDeleted = 0",
                                                entity.RSSummaryID);
            List<vTransRevenueSharingSummaryAdj> lstAdj = BusinessLayer.GetvTransRevenueSharingSummaryAdjList(filterAdj);
            List<vTransRevenueSharingSummaryAdj> lstAdjForList = lstAdj.Where(t => t.GCRSAdjustmentType != Constant.RevenueSharingAdjustmentType.HNPK_HONOR_TETAP
                                                        && t.GCRSAdjustmentType != Constant.RevenueSharingAdjustmentType.HNJM_SUBSIDI_JASMED_MINIMAL
                                                        && t.GCRSAdjustmentType != Constant.RevenueSharingAdjustmentType.FONDS
                                                        && t.GCRSAdjustmentType != Constant.RevenueSharingAdjustmentType.PPH_21).ToList();

            srTransSummaryAdjustmentDetailRSSC.CanGrow = true;
            subTransSummaryAdjustmentDetailRSSC.InitializeReport(lstAdjForList);

            // Tax Balance
            string filterTaxBalance = string.Format("RSSummaryID = {0}", entity.RSSummaryID);
            List<ParamedicTaxBalance> lstTaxBalance = BusinessLayer.GetParamedicTaxBalanceList(filterTaxBalance);
            srParamedicTaxBalanceRSSC.CanGrow = true;
            subParamedicTaxBalance.InitializeReport(lstTaxBalance);

            #endregion

            #region Summary End

            decimal totalTaxBalance = lstDetail.Sum(a => a.RevenueSharingAmount) - lstTaxBalance.Sum(a => a.TaxAmount);
            decimal honorSebelumPajak = lstDetail.Sum(a => a.RevenueSharingAmount);
            decimal pajak = lstTaxBalance.Sum(a => a.TaxAmount);
            //if (lstTaxBalance.Count() > 0)
            //{
            //    totalTaxBalance = lstTaxBalance.Sum(a => a.cfTotalHonorSetelahPajak);
            //}
            //else
            //{
            //    totalTaxBalance = lstDetail.Sum(a => a.RevenueSharingAmount);
            //}

            decimal honorKenaPotongan = (lstDetail.Where(a => a.IsChargesHasHospitalFee).Sum(b => b.TaxBalanceEndAmount));
            decimal honorTanpaPotongan = totalTaxBalance - honorKenaPotongan;

            decimal totalOthersAdjustmentPlus = lstAdjForList.Where(a => a.GCRSAdjustmentGroup == Constant.RevenueSharingAdjustmentGroup.PENAMBAHAN).Sum(a => a.AdjustmentAmount);
            decimal totalOthersAdjustmentMinus = lstAdjForList.Where(a => a.GCRSAdjustmentGroup == Constant.RevenueSharingAdjustmentGroup.PENGURANGAN).Sum(a => a.AdjustmentAmount);
            decimal honorBersih = totalTaxBalance + totalOthersAdjustmentPlus - totalOthersAdjustmentMinus;

            cHonorSebelumPajak.Text = honorSebelumPajak.ToString(Constant.FormatString.NUMERIC_2);
            cPajak.Text = pajak.ToString(Constant.FormatString.NUMERIC_2);
            cTotalHonorSetelahPajak.Text = totalTaxBalance.ToString(Constant.FormatString.NUMERIC_2);
            cOthersAdjustmentPlus.Text = totalOthersAdjustmentPlus.ToString(Constant.FormatString.NUMERIC_2);
            cOthersAdjustmentMinus.Text = totalOthersAdjustmentMinus.ToString(Constant.FormatString.NUMERIC_2);
            cHonorBersih.Text = honorBersih.ToString(Constant.FormatString.NUMERIC_2);

            #endregion

            lblFooter.Text = string.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            
            SettingParameterDt setvarDt = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.FN0168);
            lblTTD.Text = setvarDt.ParameterValue;

            base.InitializeReport(param);
        }

    }
}
