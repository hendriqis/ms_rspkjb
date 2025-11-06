using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using System.Data;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPenjualanResepPerDokterRekap : BaseDailyPortraitRpt
    {
        List<GetPrescriptionPerDokterRekap> lstTotalVisit;
        decimal totalResepRekanan = 0;
        decimal totalResepRekananAmount = 0;
        decimal totalResepBPJS = 0;
        decimal totalResepBPJSAmount = 0;
        decimal totalResepPribadi = 0;
        decimal totalResepPribadiAmount = 0;
        public LPenjualanResepPerDokterRekap()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');

            lstTotalVisit = BusinessLayer.GetPrescriptionPerDokterRekap2List(param[0], param[1], param[2]);
            foreach (GetPrescriptionPerDokterRekap lstResep in lstTotalVisit)
            {
                String customerType = lstResep.CustomerType;
                if (lstResep.CustomerType == "Rekanan")
                {
                    totalResepRekanan = lstTotalVisit.AsEnumerable().Where(a => a.CustomerType == "Rekanan").Sum(a => a.HealthcareServiceUnitID);
                    totalResepRekananAmount = lstTotalVisit.Where(a => a.CustomerType == "Rekanan").Sum(a => a.TotalAmount);

                    if (totalResepRekanan > 0)
                    {
                        lblRekapRekanan.Text = string.Format("{0} RESEP", totalResepRekanan.ToString("#.#"));
                        lblRekapRekananAmount.Text = string.Format("{0}", totalResepRekananAmount.ToString("N2"));
                    }
                    else
                    {
                        lblRekapRekanan.Text = string.Format("");
                        lblRekapRekananAmount.Text = string.Format("");
                    }
                }
                else if (lstResep.CustomerType == "BPJS")
                {
                    totalResepBPJS = lstTotalVisit.AsEnumerable().Where(a => a.CustomerType == "BPJS").Sum(a => a.HealthcareServiceUnitID);
                    totalResepBPJSAmount = lstTotalVisit.Where(a => a.CustomerType == "BPJS").Sum(a => a.TotalAmount);

                    if (totalResepBPJS > 0)
                    {
                        lblRekapBPJS.Text = string.Format("{0} RESEP", totalResepBPJS.ToString("#.#"));
                        lblRekapBPJSAmount.Text = string.Format("{0}", totalResepBPJSAmount.ToString("N2"));
                    }
                    else
                    {
                        lblRekapBPJS.Text = string.Format("");
                        lblRekapBPJSAmount.Text = string.Format("");
                    }
                }
                else
                {
                    totalResepPribadi = lstTotalVisit.AsEnumerable().Where(a => a.CustomerType == "Pribadi").Sum(a => a.HealthcareServiceUnitID);
                    totalResepPribadiAmount = lstTotalVisit.Where(a => a.CustomerType == "Pribadi").Sum(a => a.TotalAmount);

                    if (totalResepPribadi > 0)
                    {
                        lblRekapPribadi.Text = string.Format("{0} RESEP", totalResepPribadi.ToString("#.#"));
                        lblRekapPribadiAmount.Text = string.Format("{0}", totalResepPribadiAmount.ToString("N2"));
                    }
                    else
                    {
                        lblRekapPribadi.Text = string.Format("");
                        lblRekapPribadiAmount.Text = string.Format("");
                    }
                }
            }
            base.InitializeReport(param);
        }
    }
}
