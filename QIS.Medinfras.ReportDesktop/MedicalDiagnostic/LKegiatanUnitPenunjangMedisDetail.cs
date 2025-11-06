using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using QIS.Medinfras.Web.Common;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LKegiatanUnitPenunjangMedisDetail : BaseCustomDailyLandscapeA3Rpt
    {
        public LKegiatanUnitPenunjangMedisDetail()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            string temp1 = param[2];
            string temp2 = param[3];
            string hasil1;
            string hasil2;

            if (temp1 == "1")
            { 
                hasil1 = "RAWAT INAP";
            }
            else
            {
                hasil1 = "NON RAWAT INAP";
            }
            if (temp2 == "1")
            {
                hasil2 = "KARYAWAN";
            }
            else
            {
                hasil2 = "UMUM";
            }

            lblHeader.Text = string.Format("TINDAKAN PASIEN {0} ({1})",hasil1,hasil2);

            base.InitializeReport(param);
        }

    }
}
