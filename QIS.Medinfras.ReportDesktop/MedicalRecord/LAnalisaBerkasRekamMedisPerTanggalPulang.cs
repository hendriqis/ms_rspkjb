using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LAnalisaBerkasRekamMedisPerTanggalPulang : BaseCustomDailyPotraitRpt
    {

        private Decimal JumlahLengkap = 0;
        private Decimal JumlahTidakLengkap = 0;
        private Decimal JumlahTotal = 0;

        public LAnalisaBerkasRekamMedisPerTanggalPulang()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            List<vVisitMRFolderStatusPerPatient> listEntity = BusinessLayer.GetvVisitMRFolderStatusPerPatientList(String.Format("Month = {0} AND Year = {1} AND DepartmentID = '{2}'", param[1].ToString(), param[0].ToString(), param[2].ToString()));
            JumlahLengkap = listEntity.Where(x => x.IsCompleted == true).Count(x => x.IsCompleted);
            JumlahTotal = listEntity.Count;
            JumlahTidakLengkap = JumlahTotal - JumlahLengkap;
            string MonthName = "";
            Int32 Bulan = Convert.ToInt32(param[1].ToString());

            if (Bulan == 1)
            {
                MonthName = "Januari";
            }
            else if (Bulan == 2)
            {
                MonthName = "Februari";
            }
            else if (Bulan == 3)
            {
                MonthName = "Maret";
            }
            else if (Bulan == 4)
            {
                MonthName = "April";
            }
            else if (Bulan == 5)
            {
                MonthName = "Mei";
            }
            else if (Bulan == 6)
            {
                MonthName = "Juni";
            }
            else if (Bulan == 7)
            {
                MonthName = "Juli";
            }
            else if (Bulan == 8)
            {
                MonthName = "Agustus";
            }
            else if (Bulan == 9)
            {
                MonthName = "September";
            }
            else if (Bulan == 10)
            {
                MonthName = "Oktober";
            }
            else if (Bulan == 11)
            {
                MonthName = "November";
            }
            else if (Bulan == 12)
            {
                MonthName = "Desember";
            }

            lblPeriod.Text = string.Format("Periode : {0} {1}", MonthName, param[0].ToString());
            base.InitializeReport(param);
        }

        private void lblJumlahLengkap_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lblJumlahLengkap.Text = JumlahLengkap.ToString();
        }

        private void lblPersentaseLengkap_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Decimal JumlahTotalTemp = JumlahTotal;

            if (JumlahTotalTemp == 0)
            {
                JumlahTotalTemp = JumlahTotalTemp + 1;
            }

            Decimal result = (JumlahLengkap / JumlahTotalTemp) * 100;

            if (result.ToString("#.##").Equals("1"))
            {
                lblPersentaseLengkap.Text = "0.99";
            }
            else if (1 - result < 1 && 1 - result > 0)
            {
                lblPersentaseLengkap.Text = "0" + result.ToString("#.##");
            }
            else if (result == 0)
            {
                lblPersentaseLengkap.Text = "0.00";
            }
            else
            {
                lblPersentaseLengkap.Text = result.ToString("#.##");
            }
        }

        private void lblPersentaseLengkapSubTotal_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (lblJumlahLengkapSubTotal.Summary.GetResult() != null)
            {
                Decimal JumlahLengkapSubTotal = (Decimal)lblJumlahLengkapSubTotal.Summary.GetResult();
                Decimal JumlahTotalSub = Convert.ToDecimal(lblJumlahTotalSubTotal.Summary.GetResult().ToString());
                Decimal result = 0;

                if (JumlahTotalSub == 0)
                {
                    JumlahTotalSub = JumlahTotalSub + 1;
                }

                result = (JumlahLengkapSubTotal / JumlahTotalSub) * 100;

                if (result.ToString("#.##").Equals("1"))
                {
                    lblPersentaseLengkapSubTotal.Text = "0.99";
                }
                else if (1 - result < 1 && 1 - result > 0)
                {
                    lblPersentaseLengkapSubTotal.Text = "0" + result.ToString("#.##");
                }
                else if (result == 0)
                {
                    lblPersentaseLengkapSubTotal.Text = "0.00";
                }
                else
                {
                    lblPersentaseLengkapSubTotal.Text = result.ToString("#.##");
                }
            }
        }

        private void lblJumlahTidakLengkap_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lblJumlahTidakLengkap.Text = JumlahTidakLengkap.ToString();
        }

        private void lblPersentaseTidakLengkap_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Decimal JumlahTotalTemp = JumlahTotal;

            if (JumlahTotalTemp == 0)
            {
                JumlahTotalTemp = JumlahTotalTemp + 1;
            }

            Decimal result = (JumlahTidakLengkap / JumlahTotalTemp) * 100;

            if (result.ToString("#.##").Equals("1"))
            {
                lblPersentaseTidakLengkap.Text = "0.99";
            }
            else if (1 - result < 1 && 1 - result > 0)
            {
                lblPersentaseTidakLengkap.Text = "0" + result.ToString("#.##");
            }
            else if (result == 0)
            {
                lblPersentaseTidakLengkap.Text = "0.00";
            }
            else
            {
                lblPersentaseTidakLengkap.Text = result.ToString("#.##");
            }
        }

        private void lblJumlahTidakLengkapSubTotal_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (lblJumlahLengkapSubTotal.Summary.GetResult() != null)
            {
                Decimal JumlahLengkapSubTotal = (Decimal)lblJumlahLengkapSubTotal.Summary.GetResult();
                Decimal JumlahTotalSub = Convert.ToDecimal(lblJumlahTotalSubTotal.Summary.GetResult().ToString());
                lblJumlahTidakLengkapSubTotal.Text = Convert.ToString(JumlahTotalSub - JumlahLengkapSubTotal);
            }
        }

        private void lblPersentaseTidakLengkapSubTotal_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (lblJumlahLengkapSubTotal.Summary.GetResult() != null)
            {
                Decimal JumlahLengkapSubTotal = (Decimal)lblJumlahLengkapSubTotal.Summary.GetResult();
                Decimal JumlahTotalSub = Convert.ToDecimal(lblJumlahTotalSubTotal.Summary.GetResult().ToString());
                Decimal JumlahTidakLengkapSubTotal = JumlahTotalSub - JumlahLengkapSubTotal;
                Decimal result = 0;

                if (JumlahTotalSub == 0)
                {
                    JumlahTotalSub = JumlahTotalSub + 1;
                }

                result = (JumlahTidakLengkapSubTotal / JumlahTotalSub) * 100;

                if (result.ToString("#.##").Equals("1"))
                {
                    lblPersentaseTidakLengkapSubTotal.Text = "0.99";
                }
                else if (1 - result < 1 && 1 - result > 0)
                {
                    lblPersentaseTidakLengkapSubTotal.Text = "0" + result.ToString("#.##");
                }
                else if (result == 0)
                {
                    lblPersentaseTidakLengkapSubTotal.Text = "0.00";
                }
                else
                {
                    lblPersentaseTidakLengkapSubTotal.Text = result.ToString("#.##");
                }
            }
        }

        private void rowSubtotal_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            rowSubtotal.Visible = false;
        }
    }
}
