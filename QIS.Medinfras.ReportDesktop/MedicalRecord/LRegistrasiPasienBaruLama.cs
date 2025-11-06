using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LRegistrasiPasienBaruLama : BaseCustomDailyPotraitRpt
    {
        public LRegistrasiPasienBaruLama()
        {
            InitializeComponent();
        }

        private void cPasienLama_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            bool pBaru = Convert.ToBoolean(GetCurrentColumnValue("IsNewPatient"));
            if (!pBaru)
            {
                cPasienLama.Text = ((char)0x221A).ToString();
            }
            else
            {
                cPasienLama.Text = "-";
            }
        }

        private void cPasienBaru_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            bool pBaru = Convert.ToBoolean(GetCurrentColumnValue("IsNewPatient"));
            if (pBaru)
            {
                cPasienBaru.Text = ((char)0x221A).ToString();
            }
            else
            {
                cPasienBaru.Text = "-";
            }
        }

    }
}
