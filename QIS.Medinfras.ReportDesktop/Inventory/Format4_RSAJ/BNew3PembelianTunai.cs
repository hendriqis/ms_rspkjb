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
    public partial class BNew3PembelianTunai : BaseDailyPortraitRpt
    {
        public BNew3PembelianTunai()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vDirectPurchaseHd entity = BusinessLayer.GetvDirectPurchaseHdList(String.Format(
                "DirectPurchaseID IN (SELECT DirectPurchaseID FROM vDirectPurchaseDt WHERE {0})", param[0]))[0];
            List<DirectPurchaseDt> lstDT = BusinessLayer.GetDirectPurchaseDtList(
                string.Format("DirectPurchaseID = {0} AND GCItemDetailStatus != '{1}'", entity.DirectPurchaseID, Constant.TransactionStatus.VOID));

            lblCreatedByName.Text = entity.CreatedByName;

            string filterExpression1 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_APPROVER_PURCHASE_RECEIVE_PHARMACY);
            SettingParameterDt lstParam1 = BusinessLayer.GetSettingParameterDtList(filterExpression1).FirstOrDefault();
            lblApprovedByName.Text = lstParam1.ParameterValue;

            decimal total = lstDT.Sum(a => a.UnitPrice * a.Quantity);
            decimal disc = lstDT.Sum(a => ((a.UnitPrice * a.Quantity) * (a.DiscountPercentage / 100)));

            lblTotalHarga.Text = total.ToString("N2");
            lblTotalDiskon.Text = disc.ToString("N2");

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

            base.InitializeReport(param);
        }
    }
}
