using System;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BSuratKeteranganPemeriksaanCovid19 : BaseDailyPortraitRpt
    {
        public BSuratKeteranganPemeriksaanCovid19()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vCovidTest entity = BusinessLayer.GetvCovidTestList(string.Format("VisitID = {0}",param[0])).FirstOrDefault();
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];

            txtPatient.Text = entity.PatientName;
            txtDateOfBirth.Text = entity.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT);
            txtDate.Text = entity.ObservationDate.ToString(Constant.FormatString.DATE_FORMAT);
            txtPhysicalCheck.Text = entity.IsNormal;
            txtTerms.Text = param[1];

            if (!string.IsNullOrEmpty(entity.LastBloodPressureS) && !string.IsNullOrEmpty(entity.LastBloodPressureD))
            {
                lblBP.Text = string.Format("{0}/{1} mmHg", entity.LastBloodPressureS, entity.LastBloodPressureD);
            }
            else
            {
                lblBP.Text = "belum diperiksa";
            }
            if (!string.IsNullOrEmpty(entity.Nadi))
            {
                lblPulse.Text = string.Format("{0} bpm", entity.Nadi);
            }
            else
            {
                lblPulse.Text = "belum diperiksa";
            }
            if (!string.IsNullOrEmpty(entity.Pernafasan))
            {
                lblBreath.Text = string.Format("{0} rpm", entity.Pernafasan);
            }
            else
            {
                lblBreath.Text = "belum diperiksa";
            }
            if (!string.IsNullOrEmpty(entity.Suhu))
            {
                lblTemp.Text = string.Format("{0} °C", entity.Suhu);
            }
            else
            {
                lblTemp.Text = "belum diperiksa";
            }
            if (!string.IsNullOrEmpty(entity.SpO2))
            {
                lblO2.Text = string.Format("{0} %", entity.SpO2);
            }
            else
            {
                lblO2.Text = "belum diperiksa";
            }

            if (param[2] == "true")
            {
                cbAntibody.Checked = true;
                if (param[3] == "0")
                {
                    lblAntibody.Text = "REAKTIF";
                }
                else
                {
                    lblAntibody.Text = "NON REAKTIF";
                }
                lblABDate.Text = param[4];
            }
            else
            {
                cbAntibody.Checked = false;
                lblAntibody.Font = new Font(lblAntibody.Font, FontStyle.Strikeout | FontStyle.Bold);
                lblABDate.Text = "-";
            }

            if (param[5] == "true")
            {
                cbAntigen.Checked = true;
                if (param[6] == "0")
                {
                    lblAntigen.Text = "REAKTIF";
                }
                else
                {
                    lblAntigen.Text = "NON REAKTIF";
                }
                lblAGDate.Text = param[7];
            }
            else
            {
                cbAntigen.Checked = false;
                lblAntigen.Font = new Font(lblAntigen.Font, FontStyle.Strikeout | FontStyle.Bold);
                lblAGDate.Text = "-";
            }

            if (param[8] == "true")
            {
                cbPCR.Checked = true;
                if (param[9] == "0")
                {
                    lblPCR.Text = "REAKTIF";
                }
                else
                {
                    lblPCR.Text = "NON REAKTIF";
                }
                lblPCRDate.Text = param[10];
            }
            else
            {
                cbPCR.Checked = false;
                lblPCR.Font = new Font(lblPCR.Font, FontStyle.Strikeout | FontStyle.Bold);
                lblPCRDate.Text = "-";
            }


            DateTime dateNow = DateTime.Now;
            lblPrintDate.Text = string.Format("{0}, {1}",entityHealthcare.City, dateNow.ToString(Constant.FormatString.DATE_FORMAT));
            lblParamedicName.Text = entity.ParamedicName;

            base.InitializeReport(param);

        }
    }
}
