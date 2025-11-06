using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BillPaymentSummaryTransactionMCURSSES : DevExpress.XtraReports.UI.XtraReport
    {
        private decimal personalAmount = 0, corporateAmount = 0;
        private int regID = 0;

        public BillPaymentSummaryTransactionMCURSSES()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<GetPatientChargesHdDtAllMCURSSES> lst)
        {
            decimal mainTarifPatient = 0, mainTarifPayer = 0;

            //List<GetPatientChargesHdDtAllMCURSSES> lstTemp = (from a in lst orderby a.ItemName1 select a).GroupBy(y => y.ItemName1).Select(x => x.FirstOrDefault()).ToList();
            //mainTarifPatient = lstTemp.Sum(x => x.MainTariffPatient);
            //mainTarifPayer = lstTemp.Sum(x => x.MainTariffPayer);

            //cMainTarifPatient.Text = mainTarifPatient.ToString("N2");
            //cMainTarifPayer.Text = mainTarifPayer.ToString("N2");

            mainTarifPatient = lst.Sum(x => x.DetailPatientAmount);
            mainTarifPayer = lst.Sum(x => x.DetailPayerAmount);

            cMainTarifPatient.Text = mainTarifPatient.ToString(Constant.FormatString.NUMERIC_2);
            cMainTarifPayer.Text = mainTarifPayer.ToString(Constant.FormatString.NUMERIC_2);

            regID = lst.FirstOrDefault().RegistrationID;

            this.DataSource = lst;
        }

        private void cDetail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String ItemGroupID = Convert.ToString(GetCurrentColumnValue("DetailItemGroupID"));
            if (ItemGroupID == "0")
            {
                cDetail.Text = Convert.ToString(GetCurrentColumnValue("DetailItemType"));
            }
            else
            {
                cDetail.Text = string.Format("{0} | {1}", Convert.ToString(GetCurrentColumnValue("DetailItemType")), Convert.ToString(GetCurrentColumnValue("DetailItemGroupName1")));
            }
            
            String ItemCreatedDateInString = Convert.ToString(GetCurrentColumnValue("DetailItemCreatedDateInString"));
            if (ItemCreatedDateInString == "01-Jan-1900")
            {
                cDetail.Visible = false;
            }
            else
            {
                cDetail.Visible = true;
            }
        }

        private void cGroupChargesQuantity_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Boolean IsCreatedBySystem = Convert.ToBoolean(GetCurrentColumnValue("DetailIsCreatedBySystem"));
            if (!IsCreatedBySystem)
            {
                cGroupChargesQuantity.Text = "";
            }
        }

        private void cPersonalAmount_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            Boolean IsCreatedBySystem = Convert.ToBoolean(GetCurrentColumnValue("DetailIsCreatedBySystem"));
            if (IsCreatedBySystem)
            {
                e.Result = "";
                e.Handled = true;
            }
        }

        private void cCorporateAmount_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            Boolean IsCreatedBySystem = Convert.ToBoolean(GetCurrentColumnValue("DetailIsCreatedBySystem"));
            if (IsCreatedBySystem)
            {
                e.Result = "";
                e.Handled = true;
            }
        }
    }
}
