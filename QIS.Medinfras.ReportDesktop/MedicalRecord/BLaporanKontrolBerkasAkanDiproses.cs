using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BLaporanKontrolBerkasAkanDiproses : BaseCustomDailyPotraitRpt
    {
        public BLaporanKontrolBerkasAkanDiproses()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            List<vConsultVisit1> lstEntity = BusinessLayer.GetvConsultVisit1List(param[0].ToString());

            if (lstEntity.Count > 0)
            {

                String StartDate = lstEntity.FirstOrDefault().VisitDate.ToString("dd-MMM-yyyy");
                String EndDate = lstEntity.LastOrDefault().VisitDate.ToString("dd-MMM-yyyy");

                if (StartDate == EndDate)
                {
                    xrLabel3.Text = String.Format("Tanggal Kunjungan : {0}", StartDate);
                }
                else
                {
                    xrLabel3.Text = String.Format("Periode Kunjungan : {0} Sampai {1}", StartDate, EndDate);
                }
            }
            else 
            {
                xrLabel3.Text = String.Format("Periode Kunjungan : -");
            }

            base.InitializeReport(param);
        }
    }
}
