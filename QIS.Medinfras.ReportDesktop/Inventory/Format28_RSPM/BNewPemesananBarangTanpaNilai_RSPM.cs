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
    public partial class BNewPemesananBarangTanpaNilai_RSPM : BaseDailyPortraitRpt
    {
        public BNewPemesananBarangTanpaNilai_RSPM()
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

            string filterExpression = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}', '{5}')",
                                                            AppSession.UserLogin.HealthcareID, //0
                                                            Constant.SettingParameter.SA0134, //1
                                                            Constant.SettingParameter.IM_MANAGER_LOGISTIK, //2
                                                            Constant.SettingParameter.IM_MANAGER_FARMASI, //3
                                                            Constant.SettingParameter.IM_SPV_RT_LOGISTIK, //4
                                                            Constant.SettingParameter.APOTEKER_PENANGGUNG_JAWAB //5
                                                        );
            List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression);

            string wadirNonMedis = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.SA0134).FirstOrDefault().ParameterValue;
            string managerUmum = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_MANAGER_LOGISTIK).FirstOrDefault().ParameterValue;
            string apj = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.APOTEKER_PENANGGUNG_JAWAB).FirstOrDefault().ParameterValue;
            string spvRTLogistik = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_SPV_RT_LOGISTIK).FirstOrDefault().ParameterValue;

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

            decimal totalPemesanan = (total - totalDiskon) + ppn;

            #endregion

            if (entity.GCPurchaseOrderType == Constant.PurchaseOrderType.DRUGMS || entity.GCPurchaseOrderType == Constant.PurchaseOrderType.ALKES || entity.GCPurchaseOrderType == Constant.PurchaseOrderType.OBAT_RANAP || entity.GCPurchaseOrderType == Constant.PurchaseOrderType.OBAT_RAJAL)
            {
                lblMTTD2.Visible = false;
                lblMTTD3.Visible = false;
                lblTTD2.Visible = false;
                lblTTD3.Visible = false;
                lblMTTD1.Text = "Apoteker Penanggung Jawab";
                lblMTTD4.Text = "Staff Pemesan";

                lblTTD1.Text = apj;
                lblTTD4.Text = entity.CreatedByName;
            }
            else
            {
                if (totalPemesanan > 5000000)
                {
                    lblMTTD1.Text = "Wakil Direktur Non Medis";
                    lblMTTD2.Text = "Manager Umum";
                    lblMTTD3.Text = "Supervisor Rumah Tangga & Logistik";
                    lblMTTD4.Text = "Staff Logistik";

                    lblTTD1.Text = wadirNonMedis;
                    lblTTD2.Text = managerUmum;
                    lblTTD3.Text = spvRTLogistik;
                    lblTTD4.Text = entity.CreatedByName;
                }
                else
                {
                    lblMTTD3.Visible = false;
                    lblTTD3.Visible = false;
                    lblMTTD1.Text = "Manager Umum";
                    lblMTTD2.Text = "Supervisor Rumah Tangga & Logistik";
                    lblMTTD4.Text = "Staff Logistik";

                    lblTTD1.Text = managerUmum;
                    lblTTD2.Text = spvRTLogistik;
                    lblTTD4.Text = entity.CreatedByName;
                }
            }

        }

    }
}
