using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Globalization;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BSlipHonorDokterRSPKSB : BaseCustomDailyPotraitRpt
    {
        public BSlipHonorDokterRSPKSB()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string filterHealthcare = string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID);
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(filterHealthcare).FirstOrDefault();

            TransRevenueSharingSummaryHd entity = BusinessLayer.GetTransRevenueSharingSummaryHdList(param[0]).FirstOrDefault();
            vParamedicMaster entityPM = BusinessLayer.GetvParamedicMasterList(string.Format("ParamedicID = {0}",entity.ParamedicID)).FirstOrDefault();

            lblJabatan.Text = string.Format("{0} {1}", entityPM.ParamedicMasterType, CultureInfo.CurrentCulture.TextInfo.ToTitleCase(entityPM.SpecialtyName.ToLower()));
            lblBulanTahun.Text = entity.RSSummaryDate.ToString("MMMM yyyy", new System.Globalization.CultureInfo("id-ID"));

            DateTime cashDate = entity.RSSummaryDate.AddMonths(1);
            DateTime fixedCashDate = new DateTime(cashDate.Year, cashDate.Month, 15);
            lblTGLCashDiterima.Text = fixedCashDate.ToString("dd MMMM yyyy", new System.Globalization.CultureInfo("id-ID"));

            DateTime nonCashDate = entity.RSSummaryDate.AddMonths(2);
            DateTime fixedNonCashDate = new DateTime(nonCashDate.Year, nonCashDate.Month, 10);
            lblTGLNonCashDiterima.Text = fixedNonCashDate.ToString("dd MMMM yyyy", new System.Globalization.CultureInfo("id-ID"));


            #region Sub Report

            // Others Adjustment
            string filterAdj = string.Format("RSSummaryID = {0} AND GCRSAdjustmentType NOT IN ('{1}', '{2}', '{3}', '{4}') AND IsDeleted = 0",
                                                entity.RSSummaryID,
                                                Constant.RevenueSharingAdjustmentType.HNPK_HONOR_TETAP,
                                                Constant.RevenueSharingAdjustmentType.HNJM_SUBSIDI_JASMED_MINIMAL,
                                                Constant.RevenueSharingAdjustmentType.FONDS,
                                                Constant.RevenueSharingAdjustmentType.PPH_21);
            List<vTransRevenueSharingSummaryAdj> lstAdj = BusinessLayer.GetvTransRevenueSharingSummaryAdjList(filterAdj);

            string filterAdjPph = string.Format("RSSummaryID = {0} AND IsDeleted = 0",
                                                entity.RSSummaryID);
            List<vTransRevenueSharingSummaryAdj> lstAdjPph = BusinessLayer.GetvTransRevenueSharingSummaryAdjList(filterAdjPph);


            var allowedTypesPlus = new[] { "X167^824", "X167^825", "X167^826", "X167^827", "X167^828", "X167^829", "X167^830", "X167^831", "X167^832", "X167^833"};
            var allowedTypesMinus = new[] { "X167^834", "X167^835", "X167^836", "X167^837", "X167^838", "X167^839", "X167^840" };
            var allowedTypesPPh = new[] { "X167^509" };

            srTransSummaryAdjustmentDetailPlus.CanGrow = true;
            subTransSummaryAdjustmentDetailPlusRSPKSB.InitializeReport(lstAdj.Where(a => a.GCRSAdjustmentGroup == Constant.RevenueSharingAdjustmentGroup.PENAMBAHAN && allowedTypesPlus.Contains(a.GCRSAdjustmentType)).ToList());

            if (lstAdj.Where(a => a.GCRSAdjustmentGroup == Constant.RevenueSharingAdjustmentGroup.PENGURANGAN && allowedTypesMinus.Contains(a.GCRSAdjustmentType)).ToList().Count > 0)
            {
                srTransSummaryAdjustmentDetailMinus.CanGrow = true;
                subTransSummaryAdjustmentDetailMinusRSPKSB.InitializeReport(lstAdj.Where(a => a.GCRSAdjustmentGroup == Constant.RevenueSharingAdjustmentGroup.PENGURANGAN && allowedTypesMinus.Contains(a.GCRSAdjustmentType)).ToList());
                xrTable12.Visible = false;
                xrTable8.Visible = true;
            }
            else
            {
                srTransSummaryAdjustmentDetailMinus.Visible = false;
                xrTable12.Visible = true;
                xrTable8.Visible = false;
            }

            #endregion

            List<GetTRSSummaryDtTransRegistrationDetail> lstTotal = BusinessLayer.GetTRSSummaryDtTransRegistrationDetailList(entity.RSSummaryID);

            decimal feeCash = lstTotal.Where(a => a.BusinessPartnerID == 1).Sum(a => a.RevenueSharingAmount);
            decimal feeNonCash = lstTotal.Where(a => a.BusinessPartnerID != 1).Sum(a => a.RevenueSharingAmount);
            decimal TotalAdjPlus = lstAdj.Where(a => a.GCRSAdjustmentGroup == Constant.RevenueSharingAdjustmentGroup.PENAMBAHAN && allowedTypesPlus.Contains(a.GCRSAdjustmentType)).Sum(a => a.AdjustmentAmount);
            decimal totalTerima = TotalAdjPlus + feeCash + feeNonCash;
            decimal PphProfesi = lstAdjPph.Where(a => a.GCRSAdjustmentGroup == Constant.RevenueSharingAdjustmentGroup.PENGURANGAN && allowedTypesPPh.Contains(a.GCRSAdjustmentType)).Sum(a => a.AdjustmentAmount); 
            decimal TotalAdjMinus = lstAdj.Where(a => a.GCRSAdjustmentGroup == Constant.RevenueSharingAdjustmentGroup.PENGURANGAN && allowedTypesMinus.Contains(a.GCRSAdjustmentType)).Sum(a => a.AdjustmentAmount);
            decimal TotalPotong = TotalAdjMinus + PphProfesi;
            decimal takeHomePay = totalTerima - TotalPotong;
            decimal feeCashDiterima = (feeCash + TotalAdjPlus) - (TotalAdjMinus + PphProfesi);
            decimal feeNonCashDiterima = (feeNonCash + TotalAdjPlus) - (TotalAdjMinus + PphProfesi);

            lblFeeCash.Text = feeCash.ToString(Constant.FormatString.NUMERIC_2);
            lblFeeNonCash.Text = feeNonCash.ToString(Constant.FormatString.NUMERIC_2);
            lblTotPenerimaan.Text = totalTerima.ToString(Constant.FormatString.NUMERIC_2);
            lblPphProfesi.Text = PphProfesi.ToString(Constant.FormatString.NUMERIC_2);
            lblPphProfesi1.Text = PphProfesi.ToString(Constant.FormatString.NUMERIC_2);
            lblTotPotongan.Text = TotalPotong.ToString(Constant.FormatString.NUMERIC_2);
            lblTakeHomePay.Text = takeHomePay.ToString(Constant.FormatString.NUMERIC_2);

            if (feeCashDiterima < 0)
            {
                lblfeeCashDiterima.Text = 0.ToString(Constant.FormatString.NUMERIC_2);

                if (feeNonCashDiterima < 0)
                {
                    lblfeeNonCashDiterima.Text = 0.ToString(Constant.FormatString.NUMERIC_2);
                }
                else
                {
                    lblfeeNonCashDiterima.Text = feeNonCashDiterima.ToString(Constant.FormatString.NUMERIC_2);
                }
            }
            else 
            {
                lblfeeCashDiterima.Text = feeCashDiterima.ToString(Constant.FormatString.NUMERIC_2);
                lblfeeNonCashDiterima.Text = feeNonCash.ToString(Constant.FormatString.NUMERIC_2);
            }

            SettingParameter setvar = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode = '{0}'", Constant.SettingParameter.SA0176)).FirstOrDefault();
            lblHRD.Text = setvar.ParameterValue;

            base.InitializeReport(param);
        }
    }
}
