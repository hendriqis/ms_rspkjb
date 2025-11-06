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
    public partial class BNewPemesananBarangTanpaNilaiRSUKRIDA : BaseDailyPortraitRpt
    {
        public BNewPemesananBarangTanpaNilaiRSUKRIDA()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            base.InitializeReport(param);

            vPurchaseOrderHd entity = BusinessLayer.GetvPurchaseOrderHdList(param[0])[0];
            lblPurchaseOrderNo.Text = entity.PurchaseOrderNo;
            lblPurchaseOrderDate.Text = entity.OrderDateInString;
            lblNotes.Text = entity.Remarks;
            lblSyarat.Text = entity.PaymentRemarks;
            lblSupplierCode.Text = entity.BusinessPartnerCode;
            lblSupplierName.Text = entity.BusinessPartnerName;

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

            decimal totalDiskon = 0;
            decimal total = entity.TransactionAmount;
            decimal ongkosKirim = entity.ChargesAmount;

            // ditutup AG 20210208
            //if (entity.FinalDiscount > 0)
            //{
            //    totalDiskon = (entity.FinalDiscount / 100 * entity.TransactionAmount);
            //}

            if (entity.FinalDiscountAmount > 0)
            {
                totalDiskon = entity.FinalDiscountAmount;
                total = entity.TotalDtAmount;
            }
            else
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

            decimal totalPemesanan = (total - totalDiskon) + ppn + ongkosKirim + entity.PPHAmount;

            #endregion

            if (entity.GCPurchaseOrderType != Constant.PurchaseOrderType.LOGISTIC && entity.GCPurchaseOrderType != Constant.PurchaseOrderType.ASSET && entity.GCPurchaseOrderType != Constant.PurchaseOrderType.FOOD)
            {
                lblReportTitle.Text = "SURAT PEMESANAN OBAT DAN ALKES";

                xrLabel18.Visible = false;
                lblApprover.Visible = false;
                xrLine3.Visible = false;

                string filterExpressionApprover = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_USERID_APPROVE_PO);
                SettingParameterDt lstParamApprover = BusinessLayer.GetSettingParameterDtList(filterExpressionApprover).FirstOrDefault();
                if (entity.ApprovedBy.ToString() == lstParamApprover.ParameterValue)
                {
                    ttdDirektur.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/Signature/IM0025.png");
                    ttdDirektur.Visible = true;
                }
                else
                {
                    ttdDirektur.Visible = false;
                }

                string filterExpression1 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.DIREKTUR_YANMED);
                SettingParameterDt lstParam1 = BusinessLayer.GetSettingParameterDtList(filterExpression1).FirstOrDefault();
                lblDirektur.Text = lstParam1.ParameterValue;

                string filterExpressionProposer = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_USERID_PROPOSE_PO);
                SettingParameterDt lstParamProposer = BusinessLayer.GetSettingParameterDtList(filterExpressionProposer).FirstOrDefault();
                if (entity.ProposedBy.ToString() == lstParamProposer.ParameterValue)
                {
                    ttdApoteker.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/Signature/IM0026.png");
                    ttdApoteker.Visible = true;
                }
                else
                {
                    ttdApoteker.Visible = false;
                }

                string filterExpression3 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.PHARMACIST);
                SettingParameterDt lstParam3 = BusinessLayer.GetSettingParameterDtList(filterExpression3).FirstOrDefault();
                lblApoteker.Text = lstParam3.ParameterValue;

                string filterExpression4 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.PHARMACIST_LICENSE_NO);
                SettingParameterDt lstParam4 = BusinessLayer.GetSettingParameterDtList(filterExpression4).FirstOrDefault();
                lblSIK.Text = lstParam4.ParameterValue;

                string filterExpressionCreatorPh = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_USERID_CREATE_PO_PHARMACY);
                SettingParameterDt lstParamCreatorPh = BusinessLayer.GetSettingParameterDtList(filterExpressionCreatorPh).FirstOrDefault();
                string filterExpressionCreatorNamePh = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_NAME_CREATE_PO_PHARMACY);
                SettingParameterDt lstParamCreatorNamePh = BusinessLayer.GetSettingParameterDtList(filterExpressionCreatorNamePh).FirstOrDefault();

                if (entity.CreatedBy.ToString() == lstParamCreatorPh.ParameterValue)
                {
                    ttdKaLog.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/Signature/IM0027.png");
                    ttdKaLog.Visible = true;

                    lblKepalaLogistik.Text = lstParamCreatorNamePh.ParameterValue;

                    string filterExpression5 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_LOGISTIK_NO_LISENSI);
                    SettingParameterDt lstParam5 = BusinessLayer.GetSettingParameterDtList(filterExpression5).FirstOrDefault();
                    lblSIKKaLog.Text = lstParam5.ParameterValue;
                }
                else
                {
                    ttdKaLog.Visible = false;

                    lblKepalaLogistik.Text = "";
                    lblSIKKaLog.Text = "";
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

                string filterExpression1 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.DIREKTUR_YANMED);
                SettingParameterDt lstParam1 = BusinessLayer.GetSettingParameterDtList(filterExpression1).FirstOrDefault();
                lblDirektur.Text = lstParam1.ParameterValue;

                string filterExpression3 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.DIREKTUR_KEUANGAN);
                SettingParameterDt lstParam3 = BusinessLayer.GetSettingParameterDtList(filterExpression3).FirstOrDefault();
                lblApoteker.Text = lstParam3.ParameterValue;

                lblSIK.Visible = false;

                string filterExpressionCreatorLg = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_USERID_CREATE_PO_LOGISTIC);
                SettingParameterDt lstParamCreatorLg = BusinessLayer.GetSettingParameterDtList(filterExpressionCreatorLg).FirstOrDefault();
                string filterExpressionCreatorNameLg = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_NAME_CREATE_PO_LOGISTIC);
                SettingParameterDt lstParamCreatorNameLg = BusinessLayer.GetSettingParameterDtList(filterExpressionCreatorNameLg).FirstOrDefault();

                if (entity.CreatedBy.ToString() == lstParamCreatorLg.ParameterValue)
                {
                    ttdKaLog.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/Signature/IM0028.png");
                    ttdKaLog.Visible = true;

                    lblKepalaLogistik.Text = lstParamCreatorNameLg.ParameterValue;
                    lblSIKKaLog.Text = "";
                }
                else
                {
                    ttdKaLog.Visible = false;

                    lblKepalaLogistik.Text = "";
                    lblSIKKaLog.Text = "";
                }
            }
        }

        private void lblNotes_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (String.IsNullOrEmpty(lblNotes.Text))
            {
                xrLabel17.Text = "";
                xrLabel16.Text = "";
            }
        }

        private void lblSyarat_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (String.IsNullOrEmpty(lblSyarat.Text))
            {
                xrLabel1.Text = "";
                xrLabel2.Text = "";
            }
        }
    }
}
