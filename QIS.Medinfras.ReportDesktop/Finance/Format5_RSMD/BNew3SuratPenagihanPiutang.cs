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
    public partial class BNew3SuratPenagihanPiutang : BaseRpt
    {
        public BNew3SuratPenagihanPiutang()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            List<vARInvoiceHd> lstEntity = BusinessLayer.GetvARInvoiceHdList(param[0]);
            Healthcare healthcare = BusinessLayer.GetHealthcare("001");
            Address address = BusinessLayer.GetAddress(Convert.ToInt32(healthcare.AddressID));
            lblHeaderDate.Text = string.Format("{0}, {1}", address.City, lstEntity.FirstOrDefault().ARInvoiceDateInString);

            SettingParameter ttd = BusinessLayer.GetSettingParameter(Constant.SettingParameter.MANAGER_KEUANGAN);
            lblFooterHealthcareName.Text = healthcare.HealthcareName;
            lblFooterSignName.Text = ttd.ParameterValue;
            lblFooterSignCaption.Text = ttd.ParameterName;

            vARInvoiceHd entity = lstEntity.FirstOrDefault();
            if (entity.BusinessPartnerID == 1)
            {
                vPatient patient = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", entity.MRN)).FirstOrDefault();
                lblHeaderBusinessPartnerName.Text = patient.PatientName;

                string district = patient.District.Trim();
                string city = patient.City.Trim();
                string county = patient.County.Trim();
                string state = patient.State.Trim();

                if (String.IsNullOrEmpty(district))
                {
                    district = "";
                }

                if (String.IsNullOrEmpty(city))
                {
                    city = "";
                }

                if (String.IsNullOrEmpty(county))
                {
                    county = "";
                }

                if (String.IsNullOrEmpty(state))
                {
                    state = "";
                }

                lblHeaderBusinessPartnerAddress.Text = string.Format("{0} {1} {2} {3} {4}", patient.StreetName.Trim(), district, city, county, state);
                lblHeaderBusinessPartnerPhone.Text = string.Format("Telp / No Hp : {0} / {1}", patient.PhoneNo1, patient.MobilePhoneNo1);
            }
            else
            {
                lblHeaderBusinessPartnerName.Text = entity.BusinessPartnerName;
                lblHeaderBusinessPartnerAddress.Text = string.Format("{0} {1} {2} {3} {4}", entity.CustomerBillToStreetName, entity.CustomerBillToDistrict, entity.CustomerBillToCity, entity.CustomerBillToCounty, entity.CustomerBillToState);
                lblHeaderBusinessPartnerPhone.Text = string.Format("Telp / Fax : {0} / {1}", entity.cfPhoneNo, entity.cfFaxNo);
            }


            this.DataSource = lstEntity;
        }

    }
}
