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
    public partial class BillPaymentDetailRegistrationAllNHSNew : DevExpress.XtraReports.UI.XtraReport
    {
        List<GetPatientChargesHdDtALLNHS> lstDataBind = null;

        public BillPaymentDetailRegistrationAllNHSNew()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<GetPatientChargesHdDtALLNHS> lst, String registrationID)
        {
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID))[0];
            if (oHealthcare != null)
            {
                xrLogo.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/logo.png");
                cHealthcareName.Text = oHealthcare.HealthcareName;
                cHealthcareAddress.Text = oHealthcare.StreetName;
                cHealthcareCityZipCodes.Text = string.Format("{0} {1}", oHealthcare.City, oHealthcare.ZipCode);
                cHealthcarePhone.Text = oHealthcare.PhoneNo1;
                cHealthcareFax.Text = oHealthcare.FaxNo1;

                lblReportTitle.Text = "OFFICIAL RECEIPT";
            }

            vConsultVisit9 entityCV = BusinessLayer.GetvConsultVisit9List(registrationID).FirstOrDefault();

            string payerAddress1 = "";
            string payerAddress2 = "";
            if (entityCV.BusinessPartnerCode == "PERSONAL")
            {
                if (entityCV.MRN != null && entityCV.MRN != 0)
                {
                    Patient entitypatient = BusinessLayer.GetPatient(entityCV.MRN);

                    if (entitypatient.HomeAddressID != null && entitypatient.HomeAddressID != 0)
                    {
                        Address entityAddressPatient = BusinessLayer.GetAddress(Convert.ToInt32(entitypatient.HomeAddressID));

                        payerAddress1 = entityAddressPatient.StreetName;
                        payerAddress2 = string.Format("{0} {1} {2}", entityAddressPatient.County, entityAddressPatient.District, entityAddressPatient.City);
                    }
                }
                else
                {
                    Guest entitypatient = BusinessLayer.GetGuest(entityCV.GuestID);

                    payerAddress1 = entitypatient.StreetName;
                    payerAddress2 = string.Format("{0} {1} {2}", entitypatient.County, entitypatient.District, entitypatient.City);
                }

            }
            else
            {
                BusinessPartners entityPayer = BusinessLayer.GetBusinessPartners(entityCV.BusinessPartnerID);

                if (entityPayer.AddressID != null && entityPayer.AddressID != 0)
                {
                    Address entityAddressPayer = BusinessLayer.GetAddress(Convert.ToInt32(entityPayer.AddressID));

                    payerAddress1 = entityAddressPayer.StreetName;
                    payerAddress2 = string.Format("{0} {1} {2}", entityAddressPayer.County, entityAddressPayer.District, entityAddressPayer.City);
                }
            }
            
            #region Header : Patient Detail
            cPatientName.Text = entityCV.PatientName;

            if (entityCV.BusinessPartnerCode == "PERSONAL")
            {
                cPayerName.Text = entityCV.PatientName;
            }
            else
            {
                cPayerName.Text = entityCV.BusinessPartnerName;
            }

            cAddressPayer1.Text = payerAddress1;
            cAddressPayer2.Text = payerAddress1;

            cPage.Text = string.Format("1");
            cReceiptNo.Text = entityCV.RegistrationNo;

            if (entityCV.DischargeDate != null && entityCV.DischargeDate.ToString(Constant.FormatString.DATE_FORMAT) != "01-Jan-1900")
            {
                cDateTime.Text = entityCV.DischargeDate.ToString(Constant.FormatString.DATE_FORMAT);
            }
            else
            {
                cDateTime.Text = DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT);
            }
            cCashier.Text = AppSession.UserLogin.UserFullName;
            #endregion

            cMedicalNo.Text = entityCV.MedicalNo;
            cName.Text = entityCV.PatientName;

            lstDataBind = lst;

            this.DataSource = lst;
        }

        private void cGroupInvoice_AfterPrint(object sender, EventArgs e)
        {
            //string groupInvoice = GetCurrentColumnValue("GroupInvoice").ToString();
            //string regNo = "";
            //foreach(GetPatientChargesHdDtALLNHS dtBind in lstDataBind){
            //    if (groupInvoice == dtBind.GroupInvoice)
            //    {
            //        if (regNo != "")
            //        {
            //            regNo += ", ";
            //        }

            //        if (!regNo.Contains(dtBind.RegistrationNo))
            //        {
            //            regNo += dtBind.RegistrationNo;
            //        }
            //    }
            //}

            //cMedicalNo.Text = regNo;
        }

        protected string ResolveUrl(string url)
        {
            return url.Replace("~", AppConfigManager.QISAppVirtualDirectory);
        }
    }
}
