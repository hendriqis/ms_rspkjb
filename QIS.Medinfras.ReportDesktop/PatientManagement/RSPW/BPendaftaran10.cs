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
    public partial class BPendaftaran10 : BaseRpt
    {
        private string businessPartner = "";
        private string tipeBPJS = BusinessLayer.GetSettingParameterDt("001", Constant.SettingParameter.FN_TIPE_CUSTOMER_BPJS).ParameterValue;
        private string department = "";

        public BPendaftaran10()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            //lblReportProperties.Text = string.Format("MEDINFRAS - {0}, {1}, User ID:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), "admin");

            //SettingParameterDt setvarDT = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.FN_LABEL_HONOR_DOKTER);

            List<vConsultVisit9> lstEntity = BusinessLayer.GetvConsultVisit9List(string.Format("RegistrationID = {0}", param[0]));
            vConsultVisit9 entityVisist = lstEntity.FirstOrDefault();
            vRegistration5 entity = BusinessLayer.GetvRegistration5List(string.Format("RegistrationID = {0}", entityVisist.RegistrationID))[0];
            lblRegistrationNo.Text = string.Format("*{0}*", entityVisist.RegistrationNo);
            lblQueueNo.Text = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit=1", param[0]))[0].QueueNo.ToString();
            lblRegistrationDateTime.Text = string.Format("{0} / {1}", entityVisist.ActualVisitDateInString, entityVisist.ActualVisitTime);
            lblMedicalNo.Text = entityVisist.MedicalNo;
            lblPatientName.Text = entityVisist.PatientName;
            lblGender.Text = entityVisist.cfGenderInitial2;
            lblDateOfBirthTime.Text = string.Format("{0} / {1}y" , entityVisist.DateOfBirthInString , entityVisist.AgeInYear);
            lblPhysician.Text = entityVisist.ParamedicName;
            lblServiceUnitName.Text = entityVisist.ServiceUnitName;
            lblBusinessPartnerName.Text = entityVisist.BusinessPartnerName;

            lblCreatedBy.Text = entity.CtreatedByName;
            lblRemarks.Text = string.Format("* Mohon Bukti Pendaftaran ini jangan hilang.");

            department = entityVisist.DepartmentID;

            int ParamedicID = entityVisist.ParamedicID;
        }
    }
}
