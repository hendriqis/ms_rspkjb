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
    public partial class BKeteranganMedis : BaseCustomDailyPotraitRpt
    {
        public BKeteranganMedis()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(param[0])[0];
            string MRN = string.Format("{0}", entityReg.MRN);
            string RegistrationID = string.Format("{0}", entityReg.RegistrationID);
            //vPatient entityPat = BusinessLayer.GetvPatientList(string.Format("MRN = {0} AND GCOCCUPATION LIKE '{1}'", MRN, Constant.StandardCode.OCCUPATION))[0]; 
            vPatient entityPat = BusinessLayer.GetvPatientList(string.Format("MRN = {0} ", MRN))[0];
            ConsultVisit entitycv = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0} ", RegistrationID))[0];

            vRegistrationPatientPerPeriod entityphd = BusinessLayer.GetvRegistrationPatientPerPeriodList(string.Format("VisitID = {0}", entityReg.VisitID)).FirstOrDefault();

            Healthcare hl = BusinessLayer.GetHealthcare(appSession.HealthcareID);

            string FirstName = entityPat.FirstName;
            string MiddleName = entityPat.MiddleName;
            string LastName = entityPat.LastName;
            string RM = entityPat.MedicalNo;
            string StreetName = entityPat.StreetName;
            string RT = entityPat.RT;
            string RW = entityPat.RW;
            string Country = entityPat.County;
            string City = entityPat.City;
            string GCOccupation = entityPat.GCOccupation;
            string State = entityPat.State;
            int AgeYear = entityPat.AgeInYear;
            int AgeMonth = entityPat.AgeInMonth;
            int AgeDays = entityPat.AgeInDay;

            lblNama.Text = string.Format("{0} {1} {2} ", FirstName, MiddleName, LastName);
            lblUmur.Text = string.Format("{0} Tahun {1} Bulan {2} Hari", AgeYear, AgeMonth, AgeDays);
            lblAdress.Text = string.Format("{0} {1} {2} {3} {4} {5}", StreetName, RT, RW, Country, City, State);
            lblDate.Text = string.Format("{0}", DateTime.Now.ToString("dd-MMM-yyyy"));
            lblWork.Text = entityphd.Pekerjaan;
            lblDiagnose.Text = entityphd.PatientDiagnosis;
            lblTglTo.Text = Convert.ToString(entitycv.VisitDate.ToString(Constant.FormatString.DATE_FORMAT));
            lblParamedic.Text = entityphd.ParamedicName;

            if (entitycv.DischargeDate != null && entitycv.DischargeDate.ToString(Constant.ConstantDate.DEFAULT_NULL) != "01-Jan-1900")
            {
                lblTglFrom.Text = "";
            }
            else
            {
                lblTglFrom.Text = Convert.ToString(entitycv.DischargeDate.ToString(Constant.FormatString.DATE_FORMAT));

            }

            xrLabel4.Text = string.Format("telah di rawat di {0} sejak tanggal : ", hl.HealthcareName);
           
            base.InitializeReport(param);
        }
    }
}
