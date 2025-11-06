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
    public partial class BSuratCutiHamilRSPKSB : BaseDailyPortrait2Rpt
    {
        public BSuratCutiHamilRSPKSB()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vConsultVisit9 entityVisit = BusinessLayer.GetvConsultVisit9List(param[0])[0];
            vPatient entityP = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", entityVisit.MRN))[0];
            vHealthcare entityH1 = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID))[0];

            lblName.Text = entityP.PatientName;
            lblNoRM.Text = entityP.MedicalNo;
            lblDOB.Text = String.Format("{0}/{1} Tahun",entityP.DateOfBirthInString, entityP.AgeInYear);
            lblAddress.Text = entityP.HomeAddress;

            String Kandungan = param[1];
            String Kandungan2 = param[2];
            String Tanggal = param[3];
            lblKandungan1.Text = string.Format("Umur kehamilan : {0} minggu {1} hari", Kandungan, Kandungan2);
            lblPrintDate.Text = string.Format("{0}, {1}", entityH1.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            lblParamedicName.Text = entityVisit.ParamedicName;
            base.InitializeReport(param);
        }
    }
}
