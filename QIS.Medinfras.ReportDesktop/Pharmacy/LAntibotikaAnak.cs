using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LAntibotikaAnak : BaseDailyLandscapeRpt
    {
        public LAntibotikaAnak()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            if ((!String.IsNullOrEmpty(param[2])))
            {
                if (param[2] == "0")
                {
                    txtStatus.Text = string.Format("Anak");
                }
                else
                {
                    txtStatus.Text = string.Format("Dewasa");
                }
            }
            else 
            {
                txtStatus.Text = string.Format("Dewasa");
            }

          

            base.InitializeReport(param);
        }

    }
}
