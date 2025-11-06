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
    public partial class BSuratKeteranganVaksin : BaseDailyPortrait2Rpt
    {
        public BSuratKeteranganVaksin()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            //Registration entity = BusinessLayer.GetRegistrationList(string.Format(param[0]))[0];
            vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(string.Format(param[0]))[0];
            vPatient entityP = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", entityCV.MRN))[0];
            vHealthcare entityH1 = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID))[0];
            vHealthcareServiceUnit entityHSU = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = {0}" , appSession.HealthcareID))[0];
            PatientChargesHd entityPC = BusinessLayer.GetPatientChargesHdList(string.Format("VisitID = {0}", entityCV.VisitID))[0];

            lblName.Text = entityP.PatientName;
            lblDOB.Text = String.Format("{0} Tahun", entityP.AgeInYear);
            lblAddress.Text = entityP.HomeAddress;

            String Vaksin = param[1];
            String Ke = param [2];
            lblVaksin1.Text = string.Format("Telah mendapat {0} ke : {1}", Vaksin, Ke);
            lblVaksin2.Text = string.Format("pada tanggal : {0} di {1}", entityPC.TransactionDate.ToString(Constant.FormatString.DATE_FORMAT) , entityH1.HealthcareName);
            lblPrintDate.Text = string.Format("{0}, {1}", entityH1.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            lblParamedicName.Text = string.Format("({0})", entityCV.ParamedicName);
            base.InitializeReport(param);
        }
    }
}
