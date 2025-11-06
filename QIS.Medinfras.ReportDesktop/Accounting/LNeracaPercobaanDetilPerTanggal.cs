using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Linq;
using System.Globalization;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LNeracaPercobaanDetilPerTanggal : BaseDailyPortraitRpt
    {
        public LNeracaPercobaanDetilPerTanggal()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string accountFrom = BusinessLayer.GetChartOfAccount(Convert.ToInt16(param[0])).GLAccountName;
            string accountTo = BusinessLayer.GetChartOfAccount(Convert.ToInt16(param[1])).GLAccountName;
            string[] temp = param[2].Split(';');

            lblPeriodeAccount.Text = string.Format("{0} s/d {1}", accountFrom, accountTo);
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));


            int GLAccountIDFrom = Convert.ToInt16(param[0]);
            int GLAccountIDTo = Convert.ToInt16(param[1]);

            List<GetGLBalancePerGLAccountPerDate> listentity = BusinessLayer.GetGLBalancePerGLAccountPerDateList(GLAccountIDFrom, GLAccountIDTo, param[2]);

            decimal BalanceEnd = listentity.LastOrDefault().EndAmount;
            decimal totalEnd = 0;

            if (listentity.Count > 0)
            {
                foreach (GetGLBalancePerGLAccountPerDate item in listentity)
                {
                    totalEnd += item.EndAmount;
                }
            }
            lblBalance.Text = totalEnd.ToString("#,##0.00;(#,##0.00)");

            base.InitializeReport(param);
        }
    }
}
