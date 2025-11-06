using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BAmplopPiutangKolektifInstansiRSSBB : BaseRpt
    {
        public BAmplopPiutangKolektifInstansiRSSBB()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID)).FirstOrDefault();
            if (oHealthcare != null)
            {
                cHealthcareName.Text = oHealthcare.HealthcareName;
                cHealthcareAddress.Text = oHealthcare.StreetName;
                cHealthcareCityZipCodes.Text = string.Format("{0} {1}", oHealthcare.City, oHealthcare.ZipCode);
                cHealthcarePhone.Text = oHealthcare.PhoneNo1;
                cHealthcareFax.Text = oHealthcare.FaxNo1;
            }

            vARReceiptHd entity = BusinessLayer.GetvARReceiptHdList(param[0]).FirstOrDefault();
            cCustomerName.Text = entity.PrintAsName;
            cCustomerAddress.Text = entity.BillingStreetName;
            cCustomerCityZipCodes.Text = entity.BillingCity + " " + entity.BillingZipCode;
            lblARNo.Text = string.Format("Nomor : {0}", entity.ARReceiptNo);

        }
    }
}
