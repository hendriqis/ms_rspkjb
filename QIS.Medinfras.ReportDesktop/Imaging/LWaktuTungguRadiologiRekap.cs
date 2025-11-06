using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LWaktuTungguRadiologiRekap : BaseCustomDailyPotraitRpt
    {
        private decimal TepatWaktu;
        private decimal Terlambat;
        private decimal Total;

        public LWaktuTungguRadiologiRekap()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            SettingParameterDt SetVarWaktu = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.LB_WAKTU_TUNGGU_LABORATORIUM);
            lblKurangDari.Text = string.Format("<={0} jam", SetVarWaktu.ParameterValue);
            lblLebihDari.Text = string.Format(">{0} jam", SetVarWaktu.ParameterValue);
            lblTepatWaktu.Text = string.Format("2. Jumlah tepat waktu <={0} jam", SetVarWaktu.ParameterValue);
            lblTerlambat.Text = string.Format("3. Jumlah terlambat >{0} jam", SetVarWaktu.ParameterValue);

            base.InitializeReport(param);
        }
        private void lblPersenTepatWaktu_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Total = Convert.ToDecimal(lblTotal.Summary.GetResult().ToString());
            TepatWaktu = Convert.ToDecimal(lblLebihKecil.Summary.GetResult().ToString());
            if (Total == 0)
            {
                lblPersenTepatWaktu.Text = "0";
            }
            else
            {
                decimal result = (TepatWaktu / Total) * 100;

                if (result.ToString("#.##").Equals("1"))
                {
                    lblPersenTepatWaktu.Text = "0.99" + "%";
                }
                else if (1 - result < 1 && 1 - result > 0)
                {
                    lblPersenTepatWaktu.Text = "0" + result.ToString("#.##") + "%";
                }
                else if (result == 0)
                {
                    lblPersenTepatWaktu.Text = "0.00" + "%";
                }
                else
                {
                    lblPersenTepatWaktu.Text = result.ToString("#.##") + "%";
                }
            }
        }

        private void lblPersenTotal_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Total = Convert.ToDecimal(lblTotal.Summary.GetResult().ToString());
            if (Total == 0)
            {
                lblPersenTotal.Text = "0";
            }
            else
            {
                decimal result = (Total / Total) * 100;

                if (result.ToString("#.##").Equals("1"))
                {
                    lblPersenTotal.Text = "0.99" + "%";
                }
                else if (1 - result < 1 && 1 - result > 0)
                {
                    lblPersenTotal.Text = "0" + result.ToString("#.##") + "%";
                }
                else if (result == 0)
                {
                    lblPersenTotal.Text = "0.00" + "%";
                }
                else
                {
                    lblPersenTotal.Text = result.ToString("#.##") + "%";
                }
            }
        }

        private void lblPersenTerlambat_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Total = Convert.ToDecimal(lblTotal.Summary.GetResult().ToString());
            Terlambat = Convert.ToDecimal(lblLebihBesar.Summary.GetResult().ToString());

            if (Total == 0)
            {
                lblPersenTerlambat.Text = "0";
            }else
            {
                decimal result = (Terlambat / Total) * 100;

                if (result.ToString("#.##").Equals("1"))
                {
                    lblPersenTerlambat.Text = "0.99" + "%";
                }
                else if (1 - result < 1 && 1 - result > 0)
                {
                    lblPersenTerlambat.Text = "0" + result.ToString("#.##") + "%";
                }
                else if (result == 0)
                {
                    lblPersenTerlambat.Text = "0.00" + "%";
                }
                else
                {
                    lblPersenTerlambat.Text = result.ToString("#.##") + "%";
                }
            }
        }
    }
}
