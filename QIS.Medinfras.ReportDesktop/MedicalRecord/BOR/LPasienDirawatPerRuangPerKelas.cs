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
    public partial class LPasienDirawatPerRuangPerKelas : BaseCustomDailyLandscapeA3Rpt
    {
        int Year = 0;
        int Month = 0;
        public LPasienDirawatPerRuangPerKelas()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            Year = Convert.ToInt32(param[0]);
            Month = Convert.ToInt32(param[1]);

            base.InitializeReport(param);
        }

        private void lblBOR_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int jmlhHariPerwatan = 0;
            int jmlhTempatTidur = 0;
            int jmlHari = Convert.ToInt32(GetCurrentColumnValue("JmlhHari"));
            decimal BOR = 0;

            List<GetIndicatorHospitalServiceUnitClassCarePerMonthPerYear> lst = BusinessLayer.GetIndicatorHospitalServiceUnitClassCarePerMonthPerYear(Year, Month);

            foreach (GetIndicatorHospitalServiceUnitClassCarePerMonthPerYear hd in lst)
            {
                jmlhHariPerwatan += hd.JmlhHariPerwatan;
                jmlhTempatTidur += hd.JmlhTempatTidur;
            }

            if (jmlhHariPerwatan != 0 || jmlhTempatTidur != 0)
            {
                decimal jmlTTPerHari = jmlhTempatTidur * jmlHari;
                BOR = ((decimal)jmlhHariPerwatan / jmlTTPerHari) * 100;
            }
            lblBOR.Text = BOR.ToString("N2");
        }

        private void lblLOS_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int jmlhLamaDirawat = 0;
            int jmlhPasienKeluarLP = 0;
            decimal LOS = 0;

            List<GetIndicatorHospitalServiceUnitClassCarePerMonthPerYear> lst = BusinessLayer.GetIndicatorHospitalServiceUnitClassCarePerMonthPerYear(Year, Month);

            foreach (GetIndicatorHospitalServiceUnitClassCarePerMonthPerYear hd in lst)
            {
                jmlhLamaDirawat += hd.JmlhLamaDirawat;
                jmlhPasienKeluarLP += hd.JmlhPasienKeluarLP;
            }

            if (jmlhPasienKeluarLP != 0)
            {
                LOS = ((decimal)jmlhLamaDirawat / (decimal)jmlhPasienKeluarLP);
            }
            lblLOS.Text = LOS.ToString("N2");
        }

        private void lblBTO_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int jmlhPasienKeluarLP = 0;
            int jmlhTempatTidur = 0;
            decimal BTO = 0;

            List<GetIndicatorHospitalServiceUnitClassCarePerMonthPerYear> lst = BusinessLayer.GetIndicatorHospitalServiceUnitClassCarePerMonthPerYear(Year, Month);

            foreach (GetIndicatorHospitalServiceUnitClassCarePerMonthPerYear hd in lst)
            {
                jmlhPasienKeluarLP += hd.JmlhPasienKeluarLP;
                jmlhTempatTidur += hd.JmlhTempatTidur;
            }

            if (jmlhTempatTidur != 0)
            {
                BTO = ((decimal)jmlhPasienKeluarLP / (decimal)jmlhTempatTidur);
            }
            lblBTO.Text = BTO.ToString("N2");
        }

        private void lblTOI_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int jmlhTempatTidur = 0;
            int jmlHari = Convert.ToInt32(GetCurrentColumnValue("JmlhHari"));
            int jmlhHariPerwatan = 0;
            int jmlhPasienKeluarLP = 0;
            decimal TOI = 0;

            List<GetIndicatorHospitalServiceUnitClassCarePerMonthPerYear> lst = BusinessLayer.GetIndicatorHospitalServiceUnitClassCarePerMonthPerYear(Year, Month);

            foreach (GetIndicatorHospitalServiceUnitClassCarePerMonthPerYear hd in lst)
            {
                jmlhTempatTidur += hd.JmlhTempatTidur;
                jmlhHariPerwatan += hd.JmlhHariPerwatan;
                jmlhPasienKeluarLP += hd.JmlhPasienKeluarLP;
            }

            if (jmlhPasienKeluarLP != 0)
            {
                decimal jmlTTPerHari = jmlhTempatTidur * jmlHari;
                decimal jmlTTPerHariPerawatan = jmlTTPerHari - (decimal)jmlhHariPerwatan;
                TOI = jmlTTPerHariPerawatan / (decimal)jmlhPasienKeluarLP;
            }
            lblTOI.Text = TOI.ToString("N2");
        }

        private void lblNDR_L_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int jmlhPasienKeluarMati48L = 0;
            int jmlhPasienKeluarLP = 0;
            decimal NDR_L = 0;

            List<GetIndicatorHospitalServiceUnitClassCarePerMonthPerYear> lst = BusinessLayer.GetIndicatorHospitalServiceUnitClassCarePerMonthPerYear(Year, Month);

            foreach (GetIndicatorHospitalServiceUnitClassCarePerMonthPerYear hd in lst)
            {
                jmlhPasienKeluarMati48L += hd.JmlhPasienKeluarMati48L;
                jmlhPasienKeluarLP += hd.JmlhPasienKeluarLP;
            }

            if (jmlhPasienKeluarLP != 0)
            {
                decimal jmlKeluar = (decimal)jmlhPasienKeluarMati48L / (decimal)jmlhPasienKeluarLP;
                NDR_L = jmlKeluar * 1000;
            }
            lblNDR_L.Text = NDR_L.ToString("N2");
        }

        private void lblNDR_P_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int jmlhPasienKeluarMati48P = 0;
            int jmlhPasienKeluarLP = 0;
            decimal NDR_P = 0;

            List<GetIndicatorHospitalServiceUnitClassCarePerMonthPerYear> lst = BusinessLayer.GetIndicatorHospitalServiceUnitClassCarePerMonthPerYear(Year, Month);

            foreach (GetIndicatorHospitalServiceUnitClassCarePerMonthPerYear hd in lst)
            {
                jmlhPasienKeluarMati48P += hd.JmlhPasienKeluarMati48P;
                jmlhPasienKeluarLP += hd.JmlhPasienKeluarLP;
            }

            if (jmlhPasienKeluarLP != 0)
            {
                decimal jmlKeluar = (decimal)jmlhPasienKeluarMati48P / (decimal)jmlhPasienKeluarLP;
                NDR_P = jmlKeluar * 1000;
            }
            lblNDR_P.Text = NDR_P.ToString("N2");
        }

        private void lblGDR_L_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int jmlhPasienKeluarMatiL = 0;
            int jmlhPasienKeluarLP = 0;
            decimal GDR_L = 0;

            List<GetIndicatorHospitalServiceUnitClassCarePerMonthPerYear> lst = BusinessLayer.GetIndicatorHospitalServiceUnitClassCarePerMonthPerYear(Year, Month);

            foreach (GetIndicatorHospitalServiceUnitClassCarePerMonthPerYear hd in lst)
            {
                jmlhPasienKeluarMatiL += hd.JmlhPasienKeluarMatiL;
                jmlhPasienKeluarLP += hd.JmlhPasienKeluarLP;
            }

            if (jmlhPasienKeluarLP != 0)
            {
                decimal jmlKeluar = (decimal)jmlhPasienKeluarMatiL / (decimal)jmlhPasienKeluarLP;
                GDR_L = jmlKeluar * 1000;
            }
            lblGDR_L.Text = GDR_L.ToString("N2");
        }

        private void lblGDR_P_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int jmlhPasienKeluarMatiP = 0;
            int jmlhPasienKeluarLP = 0;
            decimal GDR_P = 0;

            List<GetIndicatorHospitalServiceUnitClassCarePerMonthPerYear> lst = BusinessLayer.GetIndicatorHospitalServiceUnitClassCarePerMonthPerYear(Year, Month);

            foreach (GetIndicatorHospitalServiceUnitClassCarePerMonthPerYear hd in lst)
            {
                jmlhPasienKeluarMatiP += hd.JmlhPasienKeluarMatiP;
                jmlhPasienKeluarLP += hd.JmlhPasienKeluarLP;
            }

            if (jmlhPasienKeluarLP != 0)
            {
                decimal jmlKeluar = (decimal)jmlhPasienKeluarMatiP / (decimal)jmlhPasienKeluarLP;
                GDR_P = jmlKeluar * 1000;
            }
            lblGDR_P.Text = GDR_P.ToString("N2");
        }
    }
}
