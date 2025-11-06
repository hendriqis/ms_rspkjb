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
    public partial class BNew4PembelianTunai : BaseDailyPortraitRpt
    {
        public BNew4PembelianTunai()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vDirectPurchaseHd entity = BusinessLayer.GetvDirectPurchaseHdList(String.Format(
                "DirectPurchaseID IN (SELECT DirectPurchaseID FROM vDirectPurchaseDt WHERE {0})", param[0]))[0];
            List<DirectPurchaseDt> lstDT = BusinessLayer.GetDirectPurchaseDtList(
                string.Format("DirectPurchaseID = {0} AND GCItemDetailStatus != '{1}'", entity.DirectPurchaseID, Constant.TransactionStatus.VOID));
            vHealthcare entityHealthCare = BusinessLayer.GetvHealthcareList(String.Format(
                "HealthCareID = {0}", appSession.HealthcareID))[0];

            //lblCreatedByName.Text = entity.CreatedByName;

            string filterExpression1 = string.Format("ParameterCode IN ('{0}')", "SA0020");
            List<SettingParameterDt> lstParam1 = BusinessLayer.GetSettingParameterDtList(filterExpression1);
            //lblApprovedByName.Text = lstParam1.Where(lst => lst.ParameterCode == "SA0020").FirstOrDefault().ParameterValue;
            lblHospital.Text = entityHealthCare.HealthcareName;
            lblAddress.Text = entityHealthCare.StreetName;
            lblPhone.Text = entityHealthCare.PhoneNo1;
            lblFax.Text = entityHealthCare.FaxNo1;
            lblProductLine.Text = entity.ProductLineName;

            //string filterExpression = string.Format("ParameterCode IN ('{0}')", "IM0009");
            //List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression);
            //lblApprovedByName.Text = lstParam.Where(lst => lst.ParameterCode == "IM0009").FirstOrDefault().ParameterValue;

            decimal total = lstDT.Sum(a => a.UnitPrice * a.Quantity);
            decimal disc = lstDT.Sum(a => ((a.UnitPrice * a.Quantity) * (a.DiscountPercentage / 100)));

            //lblTotalHarga.Text = total.ToString("N2");
            //lblTotalDiskon.Text = disc.ToString("N2");

            #region Hitung Total

            decimal totalAmount = entity.TransactionAmount;

            decimal totalDiskon = 0;
            if (entity.FinalDiscount > 0 && entity.FinalDiscountAmount <= 0)
            {
                totalDiskon = (entity.FinalDiscount / 100 * entity.TransactionAmount);
            }
            else if (entity.FinalDiscount <= 0 && entity.FinalDiscountAmount > 0)
            {
                totalDiskon = entity.FinalDiscountAmount;
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

            decimal totalPenerimaan = (totalAmount - totalDiskon) + ppn;

            #endregion

            lblTotal.Text = totalAmount.ToString("N2");
            lblDiskon.Text = totalDiskon.ToString("N2");
            StandardCode sc = BusinessLayer.GetStandardCode(entity.GCDirectPurchaseType);
            //lblChargesType.Text = sc.StandardCodeName;
            lblPPN.Text = ppn.ToString("N2");
            lblTotalPenerimaan.Text = totalPenerimaan.ToString("N2");
            lblTerbilang.Text = string.Format("Terbilang : #{0}#", Function.NumberInWords(Convert.ToInt32(totalPenerimaan), true));

            base.InitializeReport(param);
        }
    }
}
