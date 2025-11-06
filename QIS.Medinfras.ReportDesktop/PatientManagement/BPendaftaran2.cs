using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BPendaftaran2 : BaseRpt
    {
        public BPendaftaran2()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            lblReportProperties.Text = string.Format("MEDINFRAS - {0}, {1}, User ID:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), "admin");

            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param[0]))[0];
            lblRegistrationDateTime.Text = string.Format("{0} {1}", entity.RegistrationDateInString, entity.RegistrationTime);
            lblRegistrationNo.Text = string.Format("No.Reg. : {0}", entity.RegistrationNo);
            lblQueueNo.Text = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit=1",param[0]))[0].QueueNo.ToString();
            lblPatientName.Text = string.Format("{0} ({1})", entity.PatientName, entity.Sex);
            lblMedicalNo.Text = entity.MedicalNo;
            //lblAge.Text = Helper.GetPatientAge(words, entity.DateOfBirth);
            lblServiceUnit.Text = entity.ServiceUnitName;
            lblPhysician.Text = entity.ParamedicName;
            lblBusinessPartnerName.Text = entity.BusinessPartnerName;
            lblIsPrintingPatientCard.Text = entity.IsPrintingPatientCard ? "YA" : "TIDAK";
        }
    }
}
