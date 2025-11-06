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
    public partial class LWaktuTungguFarmasiRekapRSSES : BaseCustomDailyPotraitRpt
    {
        private decimal TepatWaktu30;
        private decimal Terlambat30;
        private decimal TotalTepat;
        private decimal TepatWaktu60;
        private decimal Terlambat60;
        private decimal TotalTelat;
        private decimal Total30Tepat;
        private decimal Total30Telat;
        private decimal Total60Tepat;
        private decimal Total60Telat;
        private decimal Total;
        private decimal Total2;

        public LWaktuTungguFarmasiRekapRSSES()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            String[] temp = param[3].Split(';');
            String hsuID = temp[0];
            string srvUnit = "";

            string filterHSU = "";
            if (hsuID != "")
            {
                filterHSU = string.Format("HealthcareServiceUnitID = {0}", hsuID);
            }
            else
            {
                filterHSU = string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsDeleted = 0 AND IsUsingRegistration = 1", appSession.HealthcareID, Constant.Facility.PHARMACY);
            }

            List<vHealthcareServiceUnit> lstHSU = BusinessLayer.GetvHealthcareServiceUnitList(filterHSU);
            foreach (vHealthcareServiceUnit hsu in lstHSU)
            {
                srvUnit += string.Format("{0} | ", hsu.ServiceUnitName);
            }
            srvUnit = srvUnit.Remove(srvUnit.Length - 3);
            lblServiceUnit.Text = srvUnit;

            SettingParameterDt SetVarWaktu = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.FM_WAKTU_TUNGGU_PELAYANAN_RESEP_FARMASI);
            SettingParameterDt SetVarWaktuRacikan = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.FM_WAKTU_TUNGGU_PELAYANAN_RESEP_FARMASI_RACIKAN);
            lblKurangDari.Text = string.Format("<={0} Menit", SetVarWaktu.ParameterValue);
            lblLebihDari.Text = string.Format(">{0} Menit", SetVarWaktu.ParameterValue);
            lblKurangDari60.Text = string.Format("<={0} Menit", SetVarWaktuRacikan.ParameterValue);
            lblLebihDari60.Text = string.Format(">{0} Menit", SetVarWaktuRacikan.ParameterValue);
            
            base.InitializeReport(param);
        }
        private void lblPersenTepatWaktu_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            TotalTepat = Convert.ToDecimal(lblTotalTepat.Summary.GetResult().ToString());
            TotalTelat = Convert.ToDecimal(lblTotalTelat.Summary.GetResult().ToString());
            Total = TotalTepat + TotalTelat;

            if (Total != 0)
            {
                decimal result = (TotalTepat / Total) * 100;

                if (result.ToString("#.##").Equals("1"))
                {
                    lblPersenTotalTepatWaktu.Text = "0.99" + "%";
                }
                else if (1 - result < 1 && 1 - result > 0)
                {
                    lblPersenTotalTepatWaktu.Text = "0" + result.ToString("#.##") + "%";
                }
                else if (result == 0)
                {
                    lblPersenTotalTepatWaktu.Text = "0.00" + "%";
                }
                else
                {
                    lblPersenTotalTepatWaktu.Text = result.ToString("#.##") + "%";
                }
            }
            else
            {
                lblPersenTotalTepatWaktu.Text = "0.00" + "%";
            }
        }

        private void lblPersenTotal_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Total30Tepat = Convert.ToDecimal(lblLebihKecil30.Summary.GetResult().ToString());
            Total30Telat = Convert.ToDecimal(lblLebihBesar30.Summary.GetResult().ToString());
            Total60Tepat = Convert.ToDecimal(lblLebihKecil60.Summary.GetResult().ToString());
            Total60Telat = Convert.ToDecimal(lblLebihBesar60.Summary.GetResult().ToString());
            Total = Total30Tepat + Total30Telat;
            Total2 = Total60Tepat + Total60Telat;
            if (Total != 0)
            {
                decimal result = (Total30Tepat / Total) * 100;

                if (result.ToString("#.##").Equals("1"))
                {
                    lblPersenTotal30.Text = "0.99" + "%";
                }
                else if (1 - result < 1 && 1 - result > 0)
                {
                    lblPersenTotal30.Text = "0" + result.ToString("#.##") + "%";
                }
                else if (result == 0)
                {
                    lblPersenTotal30.Text = "0.00" + "%";
                }
                else
                {
                    lblPersenTotal30.Text = result.ToString("#.##") + "%";
                }
            }
            else
            {
                lblPersenTotal30.Text = "0.00" + "%";
            }
            if (Total != 0)
            {
                decimal result = (Total30Telat / Total) * 100;

                if (result.ToString("#.##").Equals("1"))
                {
                    lblPersenTotalLebih30.Text = "0.99" + "%";
                }
                else if (1 - result < 1 && 1 - result > 0)
                {
                    lblPersenTotalLebih30.Text = "0" + result.ToString("#.##") + "%";
                }
                else if (result == 0)
                {
                    lblPersenTotalLebih30.Text = "0.00" + "%";
                }
                else
                {
                    lblPersenTotalLebih30.Text = result.ToString("#.##") + "%";
                }
            }
            else
            {
                lblPersenTotalLebih30.Text = "0.00" + "%";
            }
            if (Total2 != 0)
            {
                decimal result = (Total60Tepat / Total2) * 100;

                if (result.ToString("#.##").Equals("1"))
                {
                    lblPersenTotal60.Text = "0.99" + "%";
                }
                else if (1 - result < 1 && 1 - result > 0)
                {
                    lblPersenTotal60.Text = "0" + result.ToString("#.##") + "%";
                }
                else if (result == 0)
                {
                    lblPersenTotal60.Text = "0.00" + "%";
                }
                else
                {
                    lblPersenTotal60.Text = result.ToString("#.##") + "%";
                }
            }
            else
            {
                lblPersenTotal60.Text = "0.00" + "%";
            }
            if (Total2 != 0)
            {
                decimal result = (Total60Telat / Total2) * 100;

                if (result.ToString("#.##").Equals("1"))
                {
                    lblPersenTotalLebih60.Text = "0.99" + "%";
                }
                else if (1 - result < 1 && 1 - result > 0)
                {
                    lblPersenTotalLebih60.Text = "0" + result.ToString("#.##") + "%";
                }
                else if (result == 0)
                {
                    lblPersenTotalLebih60.Text = "0.00" + "%";
                }
                else
                {
                    lblPersenTotalLebih60.Text = result.ToString("#.##") + "%";
                }
            }
            else
            {
                lblPersenTotalLebih60.Text = "0.00" + "%";
            }
        }

        private void lblPersenTerlambat_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            TotalTepat = Convert.ToDecimal(lblTotalTepat.Summary.GetResult().ToString());
            TotalTelat = Convert.ToDecimal(lblTotalTelat.Summary.GetResult().ToString());
            Total = TotalTepat + TotalTelat;

            if (Total != 0)
            {
                decimal result = (TotalTelat / Total) * 100;

                if (result.ToString("#.##").Equals("1"))
                {
                    lblPersenTotalTerlambat.Text = "0.99" + "%";
                }
                else if (1 - result < 1 && 1 - result > 0)
                {
                    lblPersenTotalTerlambat.Text = "0" + result.ToString("#.##") + "%";
                }
                else if (result == 0)
                {
                    lblPersenTotalTerlambat.Text = "0.00" + "%";
                }
                else
                {
                    lblPersenTotalTerlambat.Text = result.ToString("#.##") + "%";
                }
            }
            else
            {
                lblPersenTotalTerlambat.Text = "0.00" + "%";
            }
        }
    }
}
