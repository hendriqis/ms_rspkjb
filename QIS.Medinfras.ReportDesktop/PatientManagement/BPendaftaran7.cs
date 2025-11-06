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
    public partial class BPendaftaran7 : BaseRpt
    {
        private string businessPartner = "";
        private string tipeBPJS = BusinessLayer.GetSettingParameterDt("001", Constant.SettingParameter.FN_TIPE_CUSTOMER_BPJS).ParameterValue;
        private string department = "";

        public BPendaftaran7()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            lblReportProperties.Text = string.Format("MEDINFRAS - {0}, {1}, User ID:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), "admin");

            SettingParameterDt setvarDT = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.FN_LABEL_HONOR_DOKTER);

            //vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param[0]))[0];
            List<vConsultVisit> lstEntity = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0}", param[0]));
            vConsultVisit entityVisist = lstEntity.FirstOrDefault();
            lblRegistrationNo.Text = string.Format("No.Registrasi : {0}", entityVisist.RegistrationNo);
            lblQueueNo.Text = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit=1", param[0]))[0].QueueNo.ToString();
            department = entityVisist.DepartmentID;

            int ParamedicID = entityVisist.ParamedicID;


            #region Detail Item
            subDetailItemBuktiPendaftaran.CanGrow = true;
            DetailItemBuktiPendaftaran1.InitializeReport(entityVisist.VisitID);
            #endregion

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

                lstBuktiPendaftaran.Add(entityBPendaftaran);


            }

            this.DataSource = lstBuktiPendaftaran;
        }

    }

}
