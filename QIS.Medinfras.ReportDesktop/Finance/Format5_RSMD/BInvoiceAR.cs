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
    public partial class BInvoiceAR : BaseRpt
    {
        public BInvoiceAR()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vARInvoiceHd entityHd = BusinessLayer.GetvARInvoiceHdList(param[0]).FirstOrDefault();

            if (entityHd.BusinessPartnerID != 1)
            {
                vBusinessPartners bp = BusinessLayer.GetvBusinessPartnersList(string.Format("BusinessPartnersID = {0}", entityHd.BusinessPartnerID)).FirstOrDefault();
                lblBusinessPartnerName.Text = bp.BusinessPartnersName;
                lblBusinessPartnerAddress.Text = bp.BusinessPartnersAddress;
                lblBusinessPartnerCP.Text = bp.BusinessPartnersCP;

                //lblCustomerBillToName.Text = bp.BillToName;
                //lblCustomerBillToAddress.Text = bp.BillToAddress;
                //lblCustomerBillToCP.Text = bp.BillToCP;
            }
            else 
            {
                vPatient patient = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", entityHd.MRN)).FirstOrDefault();
                string streetName = patient.StreetName.Trim();
                string city = patient.City.Trim();
                string county = patient.County.Trim();

                if (String.IsNullOrEmpty(streetName))
                {
                    streetName = "";
                }

                if (String.IsNullOrEmpty(city))
                {
                    city = "";
                }

                if (String.IsNullOrEmpty(county))
                {
                    county = "";
                }

                lblBusinessPartnerName.Text = patient.PatientName;
                lblBusinessPartnerAddress.Text = string.Format("{0} {1} {2}", streetName, city, county);
                lblBusinessPartnerCP.Text = string.Format("{0} / {1}", patient.PhoneNo1, patient.MobilePhoneNo1);

                //lblCustomerBillToName.Text = "";
                //lblCustomerBillToAddress.Text = "";
                //lblCustomerBillToCP.Text = "";
            }

            lblARInvoiceDate.Text = entityHd.ARInvoiceDateInString;
            lblARInvoiceNo.Text = entityHd.ARInvoiceNo;
            lblARDueDate.Text = entityHd.DueDateInString;

            lblBankName.Text = string.Format("{0} - Cabang {1}", entityHd.BankName, entityHd.BankBranch);
            lblBankAccountNo.Text = string.Format("A/C {0}", entityHd.BankAccountNo);
            lblBankAccountName.Text = string.Format("a/n. {0}", entityHd.BankAccountName);
            lblBankAddress.Text = string.Format("{0}", entityHd.BankCity);

            string filterExpression = String.Format("{0} AND ISNULL(GCTransactionDetailStatus,'') != '{1}'", param[0], Constant.TransactionStatus.VOID);
            List<vARInvoiceDt> lstEntityDt = BusinessLayer.GetvARInvoiceDtList(filterExpression);

            SettingParameter ttd = BusinessLayer.GetSettingParameter(Constant.SettingParameter.FN_SUPERVISOR_PENAGIHAN_PIUTANG);
            lblSignName.Text = ttd.ParameterValue;
            lblSignCaption.Text = ttd.ParameterName;

            this.DataSource = lstEntityDt;
        }
    }
}
