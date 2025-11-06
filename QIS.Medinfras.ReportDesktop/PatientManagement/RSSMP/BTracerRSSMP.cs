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
    public partial class BTracerRSSMP : BaseRpt
    {
        private string businessPartner = "";
        private string tipeBPJS = BusinessLayer.GetSettingParameterDt("001", Constant.SettingParameter.FN_TIPE_CUSTOMER_BPJS).ParameterValue;
        private string department = "";

        public BTracerRSSMP()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            List<vConsultVisit> lstEntity = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0} AND GCVisitStatus != '{1}'", param[0], Constant.VisitStatus.CANCELLED));
            
            List<TempClassTracerRegistration> lsTempClassTracerRegistration = new List<TempClassTracerRegistration>();
            foreach (vConsultVisit entity in lstEntity)
            {
                TempClassTracerRegistration oTracerReg = new TempClassTracerRegistration();

                oTracerReg.QueueNo = entity.cfQueueNo;

                List<ServiceUnitAutoBillItem> lstAutoBill = BusinessLayer.GetServiceUnitAutoBillItemList(String.Format("HealthcareServiceUnitID = {0} AND IsDeleted = 0", entity.HealthcareServiceUnitID));
                oTracerReg.RegistrationDateTime = string.Format("{0} {1}", entity.ActualVisitDateInString, entity.VisitTime);
                oTracerReg.MedicalNo = entity.MedicalNo;
                oTracerReg.PatientName = entity.PatientName;
                oTracerReg.NumberGender = string.Format("{0} | {1}", entity.RegistrationNo, entity.Gender);
                oTracerReg.ServiceUnit = entity.ServiceUnitName;
                oTracerReg.Physician = entity.ParamedicName;
                oTracerReg.BusinessPartnerName = entity.BusinessPartner;
                businessPartner = BusinessLayer.GetCustomer(entity.BusinessPartnerID).GCCustomerType;

                lsTempClassTracerRegistration.Add(oTracerReg);
            }

            this.DataSource = lsTempClassTracerRegistration;
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

    public class TempClassTracerRegistration
    {
        private string _QueueNo;
        public string QueueNo
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
    }
}
