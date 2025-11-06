using System;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BNewPemesananBarangTanpaNilaiBROS : BaseDailyPortraitRpt
    {
        public BNewPemesananBarangTanpaNilaiBROS()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            base.InitializeReport(param);

            vPurchaseOrderHd entity = BusinessLayer.GetvPurchaseOrderHdList(param[0])[0];
            List<PurchaseOrderDt> entityDT = BusinessLayer.GetPurchaseOrderDtList(string.Format("PurchaseOrderID = {0} AND IsDeleted = 0", entity.PurchaseOrderID));
            lblPurchaseOrderNo.Text = entity.PurchaseOrderNo;
            lblPurchaseOrderDate.Text = entity.OrderDateInString;
            lblNotes.Text = entity.Remarks;
            lblSyarat.Text = entity.PaymentRemarks;
            lblSupplierCode.Text = entity.BusinessPartnerCode;
            lblSupplierName.Text = entity.BusinessPartnerName;
            lblUser.Text = entity.CreatedByName;//AppSession.UserLogin.UserFullName;
            lblProductLine.Text = entity.ProductLineName;
            lblLocation.Text = entity.LocationName;

            if (entity.IsUrgent)
            {
                lblIsUrgent.Visible = true;
            }
            else 
            {
                lblIsUrgent.Visible = false;            
            }

            string filterExpression = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_MAX_TRANSACTION_AMOUNT_PURCHASE_ORDER);
            SettingParameterDt lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression).FirstOrDefault();
            Decimal amount = Convert.ToDecimal(lstParam.ParameterValue);

            #region Hitung Total

            decimal total = entity.TransactionAmount;

            decimal totalDiskon = 0;
            if (entity.FinalDiscount > 0)
            {
                totalDiskon = (entity.FinalDiscount / 100 * entity.TransactionAmount);
            }

            decimal ppn = 0;
            if (entity.IsIncludeVAT)
            {
                ppn = ((entity.VATPercentage * (entity.TransactionAmount - totalDiskon)) / 100);
            }
            else
            {
                ppn = 0;
            }

            decimal totalPemesanan = ((total - totalDiskon) + ppn) - entity.DownPaymentAmount;

            #endregion

            #region Total Header

            lblTotal.Text = total.ToString("N2");
            lblDiskon.Text = totalDiskon.ToString("N2");
            lblPPN.Text = ppn.ToString("N2");
            lblDownPayment.Text = entity.DownPaymentAmount.ToString("N2");
            lblTotalPemesanan.Text = totalPemesanan.ToString("N2");

            #endregion

            if (entity.GCPurchaseOrderType != Constant.PurchaseOrderType.LOGISTIC && entity.GCPurchaseOrderType != Constant.PurchaseOrderType.ASSET)
            {
                lblReportTitle.Text = "SURAT PEMESANAN OBAT DAN ALKES";

                xrLabel18.Visible = false;
                lblApprover.Visible = false;
                xrLine3.Visible = false;

                string filterExpressionApprover = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_USERID_APPROVE_PO);
                SettingParameterDt lstParamApprover = BusinessLayer.GetSettingParameterDtList(filterExpressionApprover).FirstOrDefault();
                if (entity.ApprovedBy.ToString() == lstParamApprover.ParameterValue)
                {
                    //ttdDirektur.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/Signature/IM0025.png");
                    //ttdDirektur.Visible = true;
                }
                else
                {
                    //ttdDirektur.Visible = false;
                }

                string filterExpression1 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.MANAJER_YANMED);
                SettingParameterDt lstParam1 = BusinessLayer.GetSettingParameterDtList(filterExpression1).FirstOrDefault();
                lblDirektur.Text = lstParam1.ParameterValue;

                string filterExpressionProposer = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_USERID_PROPOSE_PO);
                SettingParameterDt lstParamProposer = BusinessLayer.GetSettingParameterDtList(filterExpressionProposer).FirstOrDefault();

                string filterExpression3 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.APOTEKER_PENANGGUNG_JAWAB);
                SettingParameterDt lstParam3 = BusinessLayer.GetSettingParameterDtList(filterExpression3).FirstOrDefault();
                lblApoteker.Text = lstParam3.ParameterValue;

                string filterExpression4 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.PHARMACIST_LICENSE_NO);
                SettingParameterDt lstParam4 = BusinessLayer.GetSettingParameterDtList(filterExpression4).FirstOrDefault();

                string filterExpressionCreatorPh = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_USERID_CREATE_PO_PHARMACY);
                SettingParameterDt lstParamCreatorPh = BusinessLayer.GetSettingParameterDtList(filterExpressionCreatorPh).FirstOrDefault();
                string filterExpressionCreatorNamePh = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_NAME_CREATE_PO_PHARMACY);
                SettingParameterDt lstParamCreatorNamePh = BusinessLayer.GetSettingParameterDtList(filterExpressionCreatorNamePh).FirstOrDefault();

                if (entity.CreatedBy.ToString() == lstParamCreatorPh.ParameterValue)
                {
                    string filterExpression5 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_LOGISTIK_NO_LISENSI);
                    SettingParameterDt lstParam5 = BusinessLayer.GetSettingParameterDtList(filterExpression5).FirstOrDefault();
                }
                else
                {

                }
            }
            else
            {
                lblReportTitle.Text = "SURAT PEMESANAN BARANG";

                if (totalPemesanan >= amount)
                {
                    string filterExpression0 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_APPROVER_PURCHASE_ORDER);
                    SettingParameterDt lstParam0 = BusinessLayer.GetSettingParameterDtList(filterExpression0).FirstOrDefault();
                    lblApprover.Text = lstParam0.ParameterValue;
                }
                else
                {
                    xrLabel18.Visible = false;
                    lblApprover.Visible = false;
                    xrLine3.Visible = false;
                }

                string filterExpression1 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.MANAJER_YANMED);
                SettingParameterDt lstParam1 = BusinessLayer.GetSettingParameterDtList(filterExpression1).FirstOrDefault();
                lblDirektur.Text = lstParam1.ParameterValue;

                string filterExpression3 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.DIREKTUR_KEUANGAN);
                SettingParameterDt lstParam3 = BusinessLayer.GetSettingParameterDtList(filterExpression3).FirstOrDefault();
                lblApoteker.Text = lstParam3.ParameterValue;

                string filterExpressionCreatorLg = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_USERID_CREATE_PO_LOGISTIC);
                SettingParameterDt lstParamCreatorLg = BusinessLayer.GetSettingParameterDtList(filterExpressionCreatorLg).FirstOrDefault();
                string filterExpressionCreatorNameLg = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_NAME_CREATE_PO_LOGISTIC);
                SettingParameterDt lstParamCreatorNameLg = BusinessLayer.GetSettingParameterDtList(filterExpressionCreatorNameLg).FirstOrDefault();
            }

        }

        private void lblNotes_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (String.IsNullOrEmpty(lblNotes.Text)) {
                xrLabel17.Text = "";
                xrLabel16.Text = "";
            }
        }

        private void lblSyarat_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (String.IsNullOrEmpty(lblSyarat.Text)) {
                xrLabel19.Text = "";
                xrLabel22.Text = "";
            }
        }

    }
}
