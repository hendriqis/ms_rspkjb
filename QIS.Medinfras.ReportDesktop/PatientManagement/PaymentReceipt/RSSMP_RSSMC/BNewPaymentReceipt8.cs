using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BNewPaymentReceipt8 : BaseRpt
    {
        private string city = "";

        public BNewPaymentReceipt8()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID)).FirstOrDefault();
            if (oHealthcare != null)
            {
                xrLogo.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/logo.png");
                cHealthcareName.Text = oHealthcare.HealthcareName;
                cHealthcareAddress.Text = oHealthcare.StreetName;
                cHealthcareCityZipCodes.Text = string.Format("{0} {1}", oHealthcare.City, oHealthcare.ZipCode);
                cHealthcarePhone.Text = oHealthcare.PhoneNo1;
                cHealthcareFax.Text = oHealthcare.FaxNo1;
            }

            city = oHealthcare.City;
            List<GetPaymentReceiptCustom> lstEntity = BusinessLayer.GetPaymentReceiptCustomList(Convert.ToInt32(param[0]));

            vConsultVisit entityVisit = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0}", lstEntity.FirstOrDefault().RegistrationID)).FirstOrDefault();
            
            if (entityVisit.ActualVisitDateInString != "01-Jan-1900")
            {
                cActualVisitDate.Text = entityVisit.ActualVisitDateInString;
            }
            else
            {
                cActualVisitDate.Text = "";
            }

            cAddress.Text = string.Format("{0} {1} {2} {3}", entityVisit.StreetName, entityVisit.County, entityVisit.District, entityVisit.City);

            this.DataSource = lstEntity;
        }

        private void lblTanggal_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string receiptDate = Convert.ToDateTime(GetCurrentColumnValue("ReceiptDate")).ToString(Constant.FormatString.DATE_FORMAT);
            string createdDate = Convert.ToDateTime(GetCurrentColumnValue("CreatedDate")).ToString(Constant.FormatString.DATE_FORMAT);

            if (receiptDate != "01-Jan-1900")
            {
                lblTanggal.Text = string.Format("{0}, {1}", city, receiptDate);
            }
            else
            {
                lblTanggal.Text = string.Format("{0}, {1}", city, createdDate);
            }
        }
    }
}
