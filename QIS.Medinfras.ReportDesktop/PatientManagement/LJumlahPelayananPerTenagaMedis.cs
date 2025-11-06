using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using System.Linq;




namespace QIS.Medinfras.ReportDesktop
{
    public partial class LJumlahPelayananPerTenagaMedis : BaseCustomDailyLandscapeA3Rpt
    {
        public LJumlahPelayananPerTenagaMedis()
        {
            InitializeComponent();

        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            txtYear.Text = string.Format("{0} ",param[0]);

                if (param[1] == "1")
                {
                      txtmonth.Text = "Januari";
                }
                else if (param[1] == "2")
                {
                      txtmonth.Text = "Februari";
                }
                else if (param[1] == "3")
                {
                    txtmonth.Text = "Maret";
                }
                else if (param[1] == "4")
                {
                    txtmonth.Text = "April";
                }
                else if (param[1] == "5")
                {
                    txtmonth.Text = "Mei";
                }
                else if (param[1] == "6")
                {
                    txtmonth.Text = "Juni";
                }
                else if (param[1] == "7")
                {
                    txtmonth.Text = "Juli";
                }
                else if (param[1] == "8")
                {
                   txtmonth.Text  = "Agustus";
                }
                else if (param[1] == "9")
                {
                    txtmonth.Text = "September";
                }
                else if (param[1] == "10")
                {
                    txtmonth.Text = "Oktober";
                }
                else if (param[1] == "11")
                {
                    txtmonth.Text = "November";
                }
                else if (param[1] == "12")
                {
                    txtmonth.Text = "Desember";
                }

                
                if (param[2]== "")
                {
                    txtHsu.Text = string.Format("Unit Pelayanan : Semua");
                }
                else
                {
                    vHealthcareServiceUnit entity = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthCareServiceUnitID = '{0}'", param[2])).FirstOrDefault();

                    txtHsu.Text = string.Format("Unit Pelayanan : {0}", entity.ServiceUnitName);
                } 
                
            base.InitializeReport(param);
        }
    }
}
