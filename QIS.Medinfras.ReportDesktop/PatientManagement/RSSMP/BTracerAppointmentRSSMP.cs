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
    public partial class BTracerAppointmentRSSMP : BaseRpt
    {
        private string businessPartner = "";
        private string tipeBPJS = BusinessLayer.GetSettingParameterDt("001", Constant.SettingParameter.FN_TIPE_CUSTOMER_BPJS).ParameterValue;
        private string department = "";

        public BTracerAppointmentRSSMP()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            List<vAppointment> lstEntity = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID = {0}", param[0]));
            
            List<TempClassTracerAppointment> lstBuktiPendaftaran = new List<TempClassTracerAppointment>();
            foreach (vAppointment entity in lstEntity)
            {
                TempClassTracerAppointment oTracerApp = new TempClassTracerAppointment();

                oTracerApp.QueueNo = entity.QueueNo;

                List<ServiceUnitAutoBillItem> lstAutoBill = BusinessLayer.GetServiceUnitAutoBillItemList(String.Format("HealthcareServiceUnitID = {0} AND IsDeleted = 0", entity.HealthcareServiceUnitID));
                oTracerApp.AppointmentDateTime = entity.StartDateTimeInString;
                oTracerApp.MedicalNo = string.Format("{0} (*)", entity.MedicalNo);
                oTracerApp.PatientName = entity.PatientName;
                oTracerApp.NumberGender = string.Format("{0} | {1}", entity.AppointmentNo, entity.Gender);
                oTracerApp.ServiceUnit = entity.ServiceUnitName;
                oTracerApp.Physician = entity.ParamedicName;
                oTracerApp.BusinessPartnerName = entity.BusinessPartnerName;
                businessPartner = BusinessLayer.GetCustomer(entity.BusinessPartnerID).GCCustomerType;

                lstBuktiPendaftaran.Add(oTracerApp);
            }

            this.DataSource = lstBuktiPendaftaran;
        }

        private void lblQueueNo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (department != "OUTPATIENT")
            {
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }
        }
    }

    public class TempClassTracerAppointment
    {
        private Int16 _QueueNo;
        public Int16 QueueNo
        {
            get { return _QueueNo; }
            set { _QueueNo = value; }
        }

        private string _PatientName;
        public string PatientName
        {
            get { return _PatientName; }
            set { _PatientName = value; }
        }

        private string _NumberGender;
        public string NumberGender
        {
            get { return _NumberGender; }
            set { _NumberGender = value; }
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

        private string _AppointmentDateTime;
        public string AppointmentDateTime
        {
            get { return _AppointmentDateTime; }
            set { _AppointmentDateTime = value; }
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
    }
}
