using System;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BReturBarangKonsinyasiDenganNilai : BaseDailyPortraitRpt
    {
        public BReturBarangKonsinyasiDenganNilai()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vPurchaseReturnHd entity = BusinessLayer.GetvPurchaseReturnHdList(param[0])[0];
            lblCreatedByName.Text = entity.CreatedByName;

            decimal total = entity.TransactionAmount;
            decimal ppn = 0;
            if (entity.IsIncludeVAT)
            {
                ppn = ((entity.VATPercentage * (entity.TransactionAmount)) / 100);
            }
            else
            {
                ppn = 0;
            }

            lblPPN.Text = ppn.ToString("N2");

            decimal totalPemesanan = total + ppn;
            lblTotalPemesanan.Text = totalPemesanan.ToString("N2");

            string filterExpression = string.Format(" ParameterCode IN ('{0}')", "IM0009");
            List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression);
            lblApprovedByName.Text = lstParam.Where(lst => lst.ParameterCode == "IM0009").FirstOrDefault().ParameterValue;
            base.InitializeReport(param);
        }

    }
}
