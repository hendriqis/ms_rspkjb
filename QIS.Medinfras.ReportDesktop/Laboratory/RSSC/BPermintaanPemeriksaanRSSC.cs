using System;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BPermintaanPemeriksaanRSSC : BaseDailyPortraitRpt
    {
        public BPermintaanPemeriksaanRSSC()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string filterExpression = "";

            if (param[1] != "" && param[1] != "0")
            {
                filterExpression = string.Format("TestOrderID = {0}", param[1]);
                
                vTestOrderHd entityOrderHd = BusinessLayer.GetvTestOrderHdList(filterExpression).FirstOrDefault();
                lblNoOrder.Text = entityOrderHd.TestOrderNo;
                lblTglOrder.Text = entityOrderHd.TestOrderDateTimeInString;
            }
            else
            {
                lblNoOrder.Text = "";
                lblTglOrder.Text = "";
            }

            base.InitializeReport(param);

        }
    }
}
