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
    public partial class BTransaksiFarmasi : BaseRpt
    {
        public BTransaksiFarmasi()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();

            int transactionID = Convert.ToInt32(param[0]);
            List<vPatientChargesDt> entity = BusinessLayer.GetvPatientChargesDtList(String.Format("TransactionID = {0}", transactionID));
            this.DataSource = entity;

            cHealthcareName.Text = entityHealthcare.HealthcareName;
            cCity.Text = entityHealthcare.City;
            cNoTlpn.Text = entityHealthcare.PhoneNo1;

            base.InitializeReport(param);
        }
    }
}
