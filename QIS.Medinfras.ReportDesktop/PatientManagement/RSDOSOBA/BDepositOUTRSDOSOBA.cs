using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BDepositOUTRSDOSOBA : BaseDailyPortraitRpt
    {
        public BDepositOUTRSDOSOBA()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];

            lblHealthcareName2.Text = entityHealthcare.HealthcareName;

            vPatientPaymentHd entityPayment = BusinessLayer.GetvPatientPaymentHdList(string.Format("{0}", param[0]))[0];
            decimal depositOUTAmount = entityPayment.DepositOUTAmount * -1;

            lblLastUpdatedDate.Text = entityHealthcare.City + ", " + entityPayment.PaymentDate.ToString(Constant.FormatString.DATE_FORMAT);
            lblTotalAmountString.Text = "# " + Function.NumberInWords(Convert.ToInt32(depositOUTAmount), true) + " #";
            lblDepositOUTAmount.Text = String.Format("{0:n2}", depositOUTAmount);

            lblUnitPelayanan.Text = entityPayment.ServiceUnitName;

            base.InitializeReport(param);
        }

    }
}
