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
    public partial class BPendaftaran6 : BaseRpt
    {
        private string businessPartner = "";
        private string tipeBPJS = BusinessLayer.GetSettingParameterDt("001", Constant.SettingParameter.FN_TIPE_CUSTOMER_BPJS).ParameterValue;
        private string department = "";

        public BPendaftaran6()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            lblReportProperties.Text = string.Format("MEDINFRAS - {0}, {1}, User ID:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), "admin");

            //SettingParameterDt setvarDT = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.FN_LABEL_HONOR_DOKTER);
            //txtAutoBill.Text = setvarDT.ParameterValue;

            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID))[0];
            vRegistration entityR = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param[0]))[0];
            List<vConsultVisit> lstEntity = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0}", param[0]));
            vConsultVisit entityVisist = lstEntity.FirstOrDefault();
            lblRegistrationNo.Text = string.Format("{0}", entityVisist.RegistrationNo);
            lblQueueNo.Text = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit=1", param[0]))[0].QueueNo.ToString();
            lblPetugas.Text = entityR.Registrar;
            lblStreetName.Text = oHealthcare.StreetName;
            lblCity.Text = oHealthcare.City;
            lblHealthCare.Text = oHealthcare.HealthcareName;

            department = entityVisist.DepartmentID;

            int ParamedicID = entityVisist.ParamedicID;

            List<CBuktiPendaftaranBROS> lstBuktiPendaftaran = new List<CBuktiPendaftaranBROS>();
            foreach (vConsultVisit entity in lstEntity)
            {
                CBuktiPendaftaranBROS entityBPendaftaran = new CBuktiPendaftaranBROS();
                List<ServiceUnitAutoBillItem> lstAutoBill = BusinessLayer.GetServiceUnitAutoBillItemList(String.Format("HealthcareServiceUnitID = {0} AND IsDeleted = 0", entity.HealthcareServiceUnitID));
                entityBPendaftaran.RegistrationDateTime = string.Format("{0} {1}", entity.ActualVisitDateInString, entity.VisitTime);
                entityBPendaftaran.PatientName = string.Format("{0}", entity.PatientName);
                entityBPendaftaran.Age = string.Format("{0} th - {1} bl - {2} hr", entity.AgeInYear, entity.AgeInMonth, entity.AgeInDay);
                entityBPendaftaran.Phone = string.Format("{0} / {1}", entity.MobilePhoneNo1, entity.PhoneNo1);
                entityBPendaftaran.MedicalNo = entity.MedicalNo;
                entityBPendaftaran.ServiceUnit = entity.ServiceUnitName;
                entityBPendaftaran.Physician = entity.ParamedicName;
                entityBPendaftaran.BusinessPartnerName = entity.BusinessPartner;
                entityBPendaftaran.StreetName = entity.StreetName;
                entityBPendaftaran.City = entity.City;
                entityBPendaftaran.Sex = entity.Sex;

                businessPartner = BusinessLayer.GetCustomer(entity.BusinessPartnerID).GCCustomerType;

                string cfPhysicianItem = "";
                List<vPhysicianItem> lstPI = BusinessLayer.GetvPhysicianItemList(string.Format("ParamedicID = {0}", ParamedicID));
                for (int i = 0; i < lstPI.Count(); i++)
                {
                    cfPhysicianItem += " | " + lstPI[i].RegistrationReceiptLabel;
                }
                if (cfPhysicianItem != "")
                {
                    cfPhysicianItem += " | ";
                }

                entityBPendaftaran.PhysicianItem = cfPhysicianItem;

                lstBuktiPendaftaran.Add(entityBPendaftaran);
            }

            this.DataSource = lstBuktiPendaftaran;
        }
    }

    public class CBuktiPendaftaranBROS
    {
        private string _PatientName;

        public string PatientName
        {
            get { return _PatientName; }
            set { _PatientName = value; }
        }
        private string _MedicalNo;

        public string MedicalNo
        {
            get { return _MedicalNo; }
            set { _MedicalNo = value; }
        }
        private string _ServiceUnit;

        public string ServiceUnit
        {
            get { return _ServiceUnit; }
            set { _ServiceUnit = value; }
        }
        private string _RegistrationDateTime;

        public string RegistrationDateTime
        {
            get { return _RegistrationDateTime; }
            set { _RegistrationDateTime = value; }
        }
        private string _Physician;

        public string Physician
        {
            get { return _Physician; }
            set { _Physician = value; }
        }
        private string _BusinessPartnerName;

        public string BusinessPartnerName
        {
            get { return _BusinessPartnerName; }
            set { _BusinessPartnerName = value; }
        }
        private string _City;

        public string City
        {
            get { return _City; }
            set { _City = value; }
        }
        private string _StreetName;

        public string StreetName
        {
            get { return _StreetName; }
            set { _StreetName = value; }
        }
        private string _Catatan;

        public string Catatan
        {
            get { return _Catatan; }
            set { _Catatan = value; }
        }
        private string _PhysicianItem;

        public string PhysicianItem
        {
            get { return _PhysicianItem; }
            set { _PhysicianItem = value; }
        }
        private string _Age;

        public string Age
        {
            get { return _Age; }
            set { _Age = value; }
        }
        private string _Phone;

        public string Phone
        {
            get { return _Phone; }
            set { _Phone = value; }
        }
        private string _Sex;

        public string Sex
        {
            get { return _Sex; }
            set { _Sex = value; }
        }
    }
}
