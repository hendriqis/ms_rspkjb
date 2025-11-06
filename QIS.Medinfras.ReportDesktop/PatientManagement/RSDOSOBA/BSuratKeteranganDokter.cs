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
    public partial class BSuratKeteranganDokter : BaseDailyPortrait2Rpt
    {
        public BSuratKeteranganDokter()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vConsultVisit9 entityVisit = BusinessLayer.GetvConsultVisit9List(param[0])[0];
            vPatient entityP = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", entityVisit.MRN))[0];
            vHealthcare entityH1 = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID))[0];

            lblName.Text = entityP.PatientName;
            lblDOB.Text = String.Format("{0} Tahun", entityP.AgeInYear);
            lblAddress.Text = entityP.HomeAddress;

            String Kandungan = param[1];
            String Tanggal = param [2];
            lblDate.Text = string.Format("Cuti hamil sesuai dengan ketentuan yang berlaku, Hari perkiraan lahir : {0}", Tanggal);
            lblKandungan1.Text = string.Format("Umur kehamilan : {0} minggu.", Kandungan);
            lblKandungan.Text = string.Format("Saat ini hamil {0} minggu, dalam keadaan sehat dan dapat melakukan perjalanan dengan pesawat terbang.", Kandungan);
            lblPrintDate.Text = string.Format("{0}, {1}", entityH1.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            lblParamedicName.Text = entityVisit.ParamedicName;
            base.InitializeReport(param);
        }
    }
}
