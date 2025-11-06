using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using DevExpress.XtraReports;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BNewPembelianTunai : BaseDailyPortraitRpt
    {
        public BNewPembelianTunai()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vDirectPurchaseHd entity = BusinessLayer.GetvDirectPurchaseHdList(String.Format(
                "DirectPurchaseID IN (SELECT DirectPurchaseID FROM vDirectPurchaseDt WHERE {0})", param[0]))[0];
            List<DirectPurchaseDt> lstDT = BusinessLayer.GetDirectPurchaseDtList(
                string.Format("DirectPurchaseID = {0} AND GCItemDetailStatus != '{1}'", entity.DirectPurchaseID, Constant.TransactionStatus.VOID));

            decimal total = lstDT.Sum(a => a.UnitPrice * a.Quantity);
            decimal disc = lstDT.Sum(a => (a.DiscountAmount + a.DiscountAmount2));

            lblTotalHarga.Text = total.ToString("N2");
            lblTotalDiskon.Text = disc.ToString("N2");
            lblItemType.Text = entity.ItemType;

            #region Hitung Total

            decimal totalAmount = entity.TransactionAmount;

            decimal totalDiskon = 0;
            if (entity.FinalDiscountAmount > 0 || entity.FinalDiscount > 0)
            {
                totalDiskon = Math.Round(((entity.FinalDiscountAmount) + (entity.FinalDiscount / 100 * entity.TransactionAmount)), 2);
            }

            decimal ppn = 0;
            if (entity.IsIncludeVAT)
            {
                ppn = Math.Round(((entity.VATPercentage * (entity.TransactionAmount - totalDiskon)) / 100), 2);
            }
            else
            {
                ppn = 0;
            }

            decimal totalPenerimaan = (totalAmount - totalDiskon) + ppn;

            #endregion

            lblTotal.Text = totalAmount.ToString("N2");
            lblDiskon.Text = totalDiskon.ToString("N2");
            StandardCode sc = BusinessLayer.GetStandardCode(entity.GCDirectPurchaseType);
            lblChargesType.Text = sc.StandardCodeName;
            lblPPN.Text = ppn.ToString("N2");
            lblTotalPenerimaan.Text = totalPenerimaan.ToString("N2");

            lblCreatedByName.Text = entity.CreatedByName;

            SettingParameterDt setvarDTL = BusinessLayer.GetSettingParameterDt(
                appSession.HealthcareID, Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_LOGISTIC);
            SettingParameterDt setvarDTP = BusinessLayer.GetSettingParameterDt(
                appSession.HealthcareID, Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_PHARMACY);

            if (entity.LocationCode == setvarDTL.ParameterValue)
            {
                SettingParameterDt lstParam = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.KEPALA_LOGISTIK_UMUM);
                if (lstParam.ParameterValue != "" && lstParam.ParameterValue != null)
                {
                    lblApprovedByName.Text = lstParam.ParameterValue;
                }
                else
                {
                    string filterExpression1 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.DIREKTUR_YANMED);
                    SettingParameterDt lstParam1 = BusinessLayer.GetSettingParameterDtList(filterExpression1).FirstOrDefault();
                    lblApprovedByName.Text = lstParam1.ParameterValue;
                }
            }
            else if (entity.LocationCode == setvarDTP.ParameterValue)
            {
                SettingParameterDt lstParam = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.KEPALA_LOGISTIK_OBAT);
                if (lstParam.ParameterValue != "" && lstParam.ParameterValue != null)
                {
                    lblApprovedByName.Text = lstParam.ParameterValue;
                }
                else
                {
                    string filterExpression1 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.DIREKTUR_YANMED);
                    SettingParameterDt lstParam1 = BusinessLayer.GetSettingParameterDtList(filterExpression1).FirstOrDefault();
                    lblApprovedByName.Text = lstParam1.ParameterValue;
                }
            }
            else
            {
                lblApprovedByName.Text = "";
            }

            base.InitializeReport(param);
        }
    }
}
