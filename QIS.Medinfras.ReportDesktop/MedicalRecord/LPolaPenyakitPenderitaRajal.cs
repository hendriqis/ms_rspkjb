using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPolaPenyakitPenderitaRajal : BaseCustomDailyPotraitRpt
    {
        public LPolaPenyakitPenderitaRajal()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] ageGroup = param;
            if (ageGroup[1] == "0")
            {
                lblKelompokUmur.Text = "Semua Umur";
            }
            else if (ageGroup[1] == "1")
            {
                lblKelompokUmur.Text = "0 - 28 Hari";
            }
            else if (ageGroup[1] == "2")
            {
                lblKelompokUmur.Text = "29 Hari - < 1 Tahun";
            }
            else if (ageGroup[1] == "3")
            {
                lblKelompokUmur.Text = "1 - 4 Tahun";
            }
            else if (ageGroup[1] == "4")
            {
                lblKelompokUmur.Text = "5 - 14 Tahun";
            }
            else if (ageGroup[1] == "5")
            {
                lblKelompokUmur.Text = "15 - 24 Tahun";
            }
            else if (ageGroup[1] == "6")
            {
                lblKelompokUmur.Text = "25 - 44 Tahun";
            }
            else if (ageGroup[1] == "7")
            {
                lblKelompokUmur.Text = "45 - 64 Tahun";
            }
            else if (ageGroup[1] == "8")
            {
                lblKelompokUmur.Text = "> 65 Tahun";
            }

            string[] temp = param[0].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            base.InitializeReport(param);
        }
    }
}
