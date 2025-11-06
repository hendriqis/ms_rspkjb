using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LRekapHonorDokterPerJenisDokter : BaseCustomDailyPotraitRpt
    {
        public LRekapHonorDokterPerJenisDokter()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');

            if (param[1] == "1")
            {
                lblHD.Text = string.Format("HONOR DOKTER SPESIALIS");
            }
            else
            {
                lblHD.Text = string.Format("HONOR DOKTER UMUM");
            }

            lblDate.Text = string.Format("BULAN {0}", Helper.YYYYMMDDToDate(temp[0]).ToString("MMMM yyyy"));
            base.InitializeReport(param);
        }

    }
}
