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
    public partial class BNewPemesananBarangDenganNilai_RSSES : BaseDailyPortraitRpt
    {
        public BNewPemesananBarangDenganNilai_RSSES()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            base.InitializeReport(param);

            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID)).FirstOrDefault();

            vPurchaseOrderHd entity = BusinessLayer.GetvPurchaseOrderHdList(param[0]).FirstOrDefault();
            lblPurchaseOrderNo.Text = entity.PurchaseOrderNo;
            lblPurchaseOrderDate.Text = entity.OrderDateInString;
            lblNotes.Text = entity.Remarks;
            lblSupplierCode.Text = entity.BusinessPartnerCode;
            lblSupplierName.Text = entity.BusinessPartnerName;
            lblTerm.Text = entity.TermName;
            lblProductLine.Text = entity.ProductLineName;

            BusinessPartners bp = BusinessLayer.GetBusinessPartnersList(string.Format("BusinessPartnerID = '{0}'", entity.BusinessPartnerID)).FirstOrDefault();
            Address ad = BusinessLayer.GetAddressList(string.Format("AddressID = '{0}'", bp.AddressID)).FirstOrDefault();
            lblSupplierAddress.Text = ad.StreetName;
            lblPhoneNo.Text = ad.PhoneNo1;

            lblPOType.Text = entity.PurchaseOrderType;
            lblSyarat.Text = entity.PaymentRemarks;
            lblDeliveryDate.Text = entity.DeliveryDate.ToString(Constant.FormatString.DATE_FORMAT);

            if (entity.IsUrgent == true)
            {
                lblIsUrgent.Visible = true;
            }
            else 
            {
                lblIsUrgent.Visible = false;            
            }

            string filterExpression = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')",
                                                            AppSession.UserLogin.HealthcareID, //0
                                                            Constant.SettingParameter.IM_MAX_TRANSACTION_AMOUNT_PURCHASE_ORDER, //1
                                                            Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_PHARMACY, //2
                                                            Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_LOGISTIC, //3
                                                            Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_NUTRITION, //4
                                                            Constant.SettingParameter.IM_PHONE_EMAIL_PURCHASEORDER_PHARMACY, //5
                                                            Constant.SettingParameter.IM_PHONE_EMAIL_PURCHASEORDER_LOGISTIC, //6
                                                            Constant.SettingParameter.IM_PHONE_EMAIL_PURCHASEORDER_NUTRITION, //7
                                                            Constant.SettingParameter.IM_NAME_CREATE_PO_PHARMACY, //8
                                                            Constant.SettingParameter.IM_NAME_CREATE_PO_LOGISTIC, //9
                                                            Constant.SettingParameter.KEPALA_LOGISTIK_UMUM //10
                                                        );
            List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression);

            decimal amount = Convert.ToDecimal(lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_MAX_TRANSACTION_AMOUNT_PURCHASE_ORDER).FirstOrDefault().ParameterValue);

            string oLocationPH = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_PHARMACY).FirstOrDefault().ParameterValue;
            string oLocationLG = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_LOGISTIC).FirstOrDefault().ParameterValue;
            string oLocationNT = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_NUTRITION).FirstOrDefault().ParameterValue;
            
            #region Hitung Total

            decimal totalDiskon = 0;
            decimal total = entity.TransactionAmount;
            decimal charges = entity.ChargesAmount;
            decimal downpayment = entity.DownPaymentAmount;

            if (entity.FinalDiscountAmount > 0)
            {
                totalDiskon = entity.FinalDiscountAmount;
                total = entity.TotalDtAmount;
            }
            else
            {
                totalDiskon = (entity.FinalDiscount / 100 * entity.TransactionAmount);
            }

            //if (entity.FinalDiscount > 0)
            //{
            //    totalDiskon = (entity.FinalDiscount / 100 * entity.TransactionAmount);
            //}

            decimal ppn = 0;
            if (entity.IsIncludeVAT)
            {
                ppn = ((entity.VATPercentage * (entity.TransactionAmount - totalDiskon)) / 100);
            }
            else
            {
                ppn = 0;
            }

            decimal pph = 0;
            if (entity.IsPPHInPercentage)
            {
                pph = entity.TransactionAmount * entity.PPHPercentage / 100;
            }
            else
            {
                pph = entity.PPHAmount;
            }

            decimal totalPemesanan = total + ppn + pph - totalDiskon + charges;

            #endregion

            #region Total Header

            lblTotal.Text = total.ToString("N2");
            lblDiskon.Text = totalDiskon.ToString("N2");
            lblPPN.Text = ppn.ToString("N2");
            lblDownPayment.Text = entity.DownPaymentAmount.ToString("N2");
            lblTotalPemesanan.Text = totalPemesanan.ToString("N2");

            #endregion

            lblTTDRight.Text = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.KEPALA_LOGISTIK_UMUM).FirstOrDefault().ParameterValue;
            ttdRight.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/Signature/IM0009.png");
            ttdRight.Visible = true;

            if (entity.LocationID.ToString() == oLocationPH)
            {
                string[] ttdHeaderLst = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_PHONE_EMAIL_PURCHASEORDER_PHARMACY).FirstOrDefault().ParameterValue.Split('|');
                lblLogisticPhoneNo.Text = ttdHeaderLst[0];
                lblLogisticEmail.Text = ttdHeaderLst[1];

                lblTTDLeftCaption.Text = "Kepala Sub Bagian Logistik Medis";

                string[] ttdLeftLst = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_NAME_CREATE_PO_PHARMACY).FirstOrDefault().ParameterValue.Split('|');
                lblTTDLeft1.Text = ttdLeftLst[0];
                lblTTDLeft2.Text = ttdLeftLst[1];

                ttdLeft.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/Signature/IM0029.png");
                ttdLeft.Visible = true;

            }
            else if (entity.LocationID.ToString() == oLocationLG)
            {
                string[] ttdHeaderLst = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_PHONE_EMAIL_PURCHASEORDER_LOGISTIC).FirstOrDefault().ParameterValue.Split('|');
                lblLogisticPhoneNo.Text = ttdHeaderLst[0];
                lblLogisticEmail.Text = ttdHeaderLst[1];

                lblTTDLeftCaption.Text = "Kepala Sub Bagian Logistik Non Medis";

                lblTTDLeft1.Text = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_NAME_CREATE_PO_LOGISTIC).FirstOrDefault().ParameterValue;
                lblTTDLeft2.Text = "";

                ttdLeft.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/Signature/IM0030.png");
                ttdLeft.Visible = true;

            }
            else if (entity.LocationID.ToString() == oLocationNT)
            {
                string[] ttdHeaderLst = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_PHONE_EMAIL_PURCHASEORDER_NUTRITION).FirstOrDefault().ParameterValue.Split('|');
                lblLogisticPhoneNo.Text = ttdHeaderLst[0];
                lblLogisticEmail.Text = ttdHeaderLst[1];

                lblTTDLeftCaption.Text = "Kepala Sub Bagian Logistik Non Medis";

                lblTTDLeft1.Text = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_NAME_CREATE_PO_LOGISTIC).FirstOrDefault().ParameterValue;
                lblTTDLeft2.Text = "";

                ttdLeft.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/Signature/IM0030.png");
                ttdLeft.Visible = true;

            }
            else
            {
                lblTTDLeftCaption.Text = "";

                lblTTDLeft1.Text = "";
                lblTTDLeft2.Text = "";

                ttdLeft.ImageUrl = null;
                ttdLeft.Visible = false;

            }

        }
    }
}
