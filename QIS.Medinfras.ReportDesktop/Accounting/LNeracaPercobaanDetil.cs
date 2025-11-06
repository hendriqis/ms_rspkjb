using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Linq;
using System.Globalization;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LNeracaPercobaanDetil : BaseDailyPortraitRpt
    {
        string coa = "";
        int year = 0;
        int month = 0;
        int PageIndex = 1;
        int NumRows = 1000;


        public LNeracaPercobaanDetil()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string monthName = "";

            if (param[2] == "13")
            {
                monthName = "Desember [Audited]";
            }
            else
            {
                monthName = Enumerable.Range(1, 12).Select(a => new
                {
                    MonthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(a),
                    MonthNumber = a
                }).FirstOrDefault(p => p.MonthNumber == Convert.ToInt32(param[2])).MonthName;
            }

            lblPeriod.Text = string.Format("Period : {0} {1}", monthName, param[1]);
            lblTotal.Text = string.Format("Total Bulan {0}", monthName);
            try
            {
                this.AdditionalReportSubTitle = BusinessLayer.GetChartOfAccount(Convert.ToInt16(param[0])).GLAccountName;
            }
            catch (Exception ex)
            {
                this.AdditionalReportSubTitle = string.Empty;
            }

            coa = param[0];
            year = Convert.ToInt32(param[1]);
            month = Convert.ToInt32(param[2]);

            int GLAccountID = Convert.ToInt32(coa);
            ChartOfAccount entity = BusinessLayer.GetChartOfAccount(GLAccountID);
            List<GetGLBalancePerGLAccount> listentity = BusinessLayer.GetGLBalancePerGLAccountList(GLAccountID, year, month, PageIndex, NumRows);

            decimal debitAmount = listentity.FirstOrDefault().BalanceEND;
            decimal creditAmount = 0;
            decimal totalEnd = 0;

            decimal BalanceEnd = listentity.LastOrDefault().BalanceEND;

            if (listentity.Count > 0)
            {
                foreach (GetGLBalancePerGLAccount item in listentity)
                {
                    debitAmount += item.DEBITAmount;
                    creditAmount += item.CREDITAmount;
                }
            }
            totalEnd = BalanceEnd;
            lblBalance.Text = totalEnd.ToString("N2");

            //lblBalance.Text = BalanceEnd.ToString("N2");

            //if (entity.Position == "D")
            //{
            //    totalEnd = debitAmount - creditAmount;
            //    lblBalance.Text = totalEnd.ToString("N2");
            //}
            //else
            //{
            //    totalEnd = creditAmount - debitAmount;
            //    lblBalance.Text = totalEnd.ToString("N2");
            //}

               

            base.InitializeReport(param);
        } 
    }
}
