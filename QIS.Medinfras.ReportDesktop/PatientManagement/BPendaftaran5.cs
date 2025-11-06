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
    public partial class BPendaftaran5 : BaseRpt
    {
        private string businessPartner = "";
        private string tipeBPJS = BusinessLayer.GetSettingParameterDt("001", Constant.SettingParameter.FN_TIPE_CUSTOMER_BPJS).ParameterValue;
        private string department = "";

        public BPendaftaran5()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            SettingParameterDt setvarDT = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.FN_LABEL_HONOR_DOKTER);
            // txtAutoBill.Text = setvarDT.ParameterValue;

            // vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param[0]))[0];
            List<vConsultVisit> lstEntity = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0}", param[0]));
            List<RegistrationBPJS> lstRegBPJS = BusinessLayer.GetRegistrationBPJSList(string.Format("RegistrationID = {0}", param[0]));

            vConsultVisit entityVisist = lstEntity.FirstOrDefault();
            lblRegistrationNo.Text = string.Format("No.Reg : {0}", entityVisist.RegistrationNo);
            lblQueueNo.Text = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", param[0]))[0].cfQueueNo.ToString();
            department = entityVisist.DepartmentID;

            if (lstRegBPJS.Count == 0)
            {
                lblNoSEP.Text = "-";
            }
            else 
            {
                RegistrationBPJS entityRegBPJS = lstRegBPJS.FirstOrDefault();
                lblNoSEP.Text = string.Format("{0}", entityRegBPJS.NoSEP);
            }
            lblTglCetak.Text = string.Format("{0} {1}", DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT), DateTime.Now.ToString("HH:mm"));
            List<StandardCode> lstStdCode = BusinessLayer.GetStandardCodeList(String.Format("ParentID = 'X168'"));
            StandardCode pagi = lstStdCode.Where(t => t.StandardCodeID == Constant.Shift.PAGI).FirstOrDefault();
            StandardCode siang = lstStdCode.Where(t => t.StandardCodeID == Constant.Shift.SIANG).FirstOrDefault();
            StandardCode malam = lstStdCode.Where(t => t.StandardCodeID == Constant.Shift.MALAM).FirstOrDefault();
            string[] ShiftPagi = pagi.TagProperty.Split(';');
            string[] ShiftSiang = siang.TagProperty.Split(';');
            string[] ShiftMalam = malam.TagProperty.Split(';');

            TimeSpan TimeNow = DateTime.Now.TimeOfDay;
            TimeSpan TimePagiFrom = TimeSpan.Parse(string.Format("{0}", ShiftPagi[0]));
            TimeSpan TimePagiTo = TimeSpan.Parse(string.Format("{0}", ShiftPagi[1]));
            TimeSpan TimeSiangFrom = TimeSpan.Parse(string.Format("{0}", ShiftSiang[0]));
            TimeSpan TimeSiangTo = TimeSpan.Parse(string.Format("{0}", ShiftSiang[1]));
            TimeSpan TimeMalamFrom = TimeSpan.Parse(string.Format("{0}", ShiftMalam[0]));
            TimeSpan TimeMalamTo = TimeSpan.Parse(string.Format("{0}", ShiftMalam[1]));

           if ((TimeNow > TimePagiFrom) && (TimeNow < TimePagiTo))
            {
                lblShift.Text = "Pagi";
            }
           else if((TimeNow > TimeSiangFrom) && (TimeNow < TimeSiangTo))
           {
               lblShift.Text = "Siang";
           }
           else
           {
               lblShift.Text = "Malam";
           }
            
            if (entityVisist.IsNewVisit)
            {
                lblStatus.Text = string.Format("Status Kunj. : Baru");
            }
            else 
            {
                lblStatus.Text = string.Format("Status Kunj. : Lama");
            }

            
            List<PatientPaymentHd> lstEntityPayment = BusinessLayer.GetPatientPaymentHdList(string.Format("RegistrationID = {0} AND GCPaymentType = '{1}' AND GCTransactionStatus != '{2}'", param[0], Constant.PaymentType.DOWN_PAYMENT, Constant.TransactionStatus.VOID));
            Decimal payment = 0;
            foreach (PatientPaymentHd p in lstEntityPayment)
            {
                payment += p.TotalPaymentAmount;
            }
            string downPayment = payment.ToString("N2");
            //lblUangMuka.Text = string.Format("Rp. {0}", downPayment);


            int ParamedicID = entityVisist.ParamedicID;

            List<CBuktiPendaftaran> lstBuktiPendaftaran = new List<CBuktiPendaftaran>();
            foreach (vConsultVisit entity in lstEntity)
            {
                CBuktiPendaftaran entityBPendaftaran = new CBuktiPendaftaran();
                List<ServiceUnitAutoBillItem> lstAutoBill = BusinessLayer.GetServiceUnitAutoBillItemList(String.Format("HealthcareServiceUnitID = {0} AND IsDeleted = 0", entity.HealthcareServiceUnitID));
                entityBPendaftaran.RegistrationDateTime = string.Format("{0} {1}", entity.ActualVisitDateInString, entity.VisitTime);
                entityBPendaftaran.PatientName = string.Format("{0} ({1})", entity.PatientName, entity.Sex);
                entityBPendaftaran.MedicalNo = entity.MedicalNo;
                entityBPendaftaran.ServiceUnit = entity.ServiceUnitName;
                entityBPendaftaran.Physician = entity.ParamedicName;
                entityBPendaftaran.BusinessPartnerName = entity.BusinessPartner;
                businessPartner = BusinessLayer.GetCustomer(entity.BusinessPartnerID).GCCustomerType;

                int count = lstAutoBill.Count;
                string listID = "";
                List<vPatientChargesDt> lstPatientChargesDt = null;
                if (count > 0)
                {
                    foreach (ServiceUnitAutoBillItem entityAutoBill in lstAutoBill)
                    {
                        listID += entityAutoBill.ItemID + ",";
                    }
                    listID = listID.Substring(0, listID.Count() - 1);
                    lstPatientChargesDt = BusinessLayer.GetvPatientChargesDtList(string.Format("ItemID IN ({0}) AND VisitID = {1}", listID, entity.VisitID));
                }
                else lstPatientChargesDt = new List<vPatientChargesDt>();

                count = lstPatientChargesDt.Count;
                for (int i = 0; i < (4 - count); i++)
                {
                    vPatientChargesDt pcdt = new vPatientChargesDt();
                    pcdt.ItemName1 = "";
                    pcdt.LineAmount = 0;
                    lstPatientChargesDt.Add(pcdt);
                }

                entityBPendaftaran.ItemName2 = lstPatientChargesDt.ElementAt(0).ItemName1;
                entityBPendaftaran.ItemName3 = lstPatientChargesDt.ElementAt(1).ItemName1;
                entityBPendaftaran.ItemName4 = lstPatientChargesDt.ElementAt(2).ItemName1;
                entityBPendaftaran.ItemName5 = lstPatientChargesDt.ElementAt(3).ItemName1;
                entityBPendaftaran.ItemPrice2 = lstPatientChargesDt.ElementAt(0).LineAmount.ToString() == "0" ? "" : "Rp. " + lstPatientChargesDt.ElementAt(0).LineAmount.ToString("N0");
                entityBPendaftaran.ItemPrice3 = lstPatientChargesDt.ElementAt(1).LineAmount.ToString() == "0" ? "" : "Rp. " + lstPatientChargesDt.ElementAt(1).LineAmount.ToString("N0");
                entityBPendaftaran.ItemPrice4 = lstPatientChargesDt.ElementAt(2).LineAmount.ToString() == "0" ? "" : "Rp. " + lstPatientChargesDt.ElementAt(2).LineAmount.ToString("N0");
                entityBPendaftaran.ItemPrice5 = lstPatientChargesDt.ElementAt(3).LineAmount.ToString() == "0" ? "" : "Rp. " + lstPatientChargesDt.ElementAt(3).LineAmount.ToString("N0");

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

        private void lblno1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (businessPartner == tipeBPJS)
            {
                //lblno1.Visible = false;
            }
        }

        private void lblno2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (businessPartner == tipeBPJS)
            {
                //lblno2.Text = "1";
            }
        }

        private void lblno3_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (businessPartner == tipeBPJS)
            {
               //lblno3.Text = "2";
            }
        }

        private void lblno4_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (businessPartner == tipeBPJS)
            {
                //lblno4.Text = "3";
            }
        }

        private void lblno5_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (businessPartner == tipeBPJS)
            {
                //lblno5.Text = "4";
            }
        }

        private void txtAutoBill_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (businessPartner == tipeBPJS)
            {
               // txtAutoBill.Visible = false;
            }
        }

        private void txtAutoRegistrationReceipt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (businessPartner == tipeBPJS)
            {
                //txtAutoRegistrationReceipt.Visible = false;
            }
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

    public class BBuktiPendaftaran5
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
        private string _ItemName2;

        public string ItemName2
        {
            get { return _ItemName2; }
            set { _ItemName2 = value; }
        }
        private string _ItemName3;

        public string ItemName3
        {
            get { return _ItemName3; }
            set { _ItemName3 = value; }
        }
        private string _ItemName4;

        public string ItemName4
        {
            get { return _ItemName4; }
            set { _ItemName4 = value; }
        }
        private string _ItemName5;

        public string ItemName5
        {
            get { return _ItemName5; }
            set { _ItemName5 = value; }
        }
        private string _ItemPrice2;

        public string ItemPrice2
        {
            get { return _ItemPrice2; }
            set { _ItemPrice2 = value; }
        }
        private string _ItemPrice3;

        public string ItemPrice3
        {
            get { return _ItemPrice3; }
            set { _ItemPrice3 = value; }
        }
        private string _ItemPrice4;

        public string ItemPrice4
        {
            get { return _ItemPrice4; }
            set { _ItemPrice4 = value; }
        }
        private string _ItemPrice5;

        public string ItemPrice5
        {
            get { return _ItemPrice5; }
            set { _ItemPrice5 = value; }
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
    }
}
