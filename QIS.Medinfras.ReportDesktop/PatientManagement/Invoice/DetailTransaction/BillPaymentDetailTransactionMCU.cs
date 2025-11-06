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
    public partial class BillPaymentDetailTransactionMCU : DevExpress.XtraReports.UI.XtraReport
    {
        public BillPaymentDetailTransactionMCU()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<GetPatientChargesHdDtAllMCU> lst)
        {
            decimal mainTarifPatient = 0, mainTarifPayer = 0;
            List<GetPatientChargesHdDtAllMCU> lstTemp = (from a in lst orderby a.ItemName1 select a).GroupBy(y => y.ItemName1).Select(x => x.FirstOrDefault()).ToList();
            mainTarifPatient = lstTemp.Sum(x => x.MainTariffPatient);
            mainTarifPayer = lstTemp.Sum(x => x.MainTariffPayer);

            cMainTarifPatient.Text = mainTarifPatient.ToString("N2");
            cMainTarifPayer.Text = mainTarifPayer.ToString("N2");

            this.DataSource = lst;
        }

        private void xrLabel1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String ItemGroupID = Convert.ToString(GetCurrentColumnValue("DetailItemGroupID"));
            if (ItemGroupID == "0")
            {
                xrLabel1.Text = Convert.ToString(GetCurrentColumnValue("DetailItemType"));
            } else 
            {
                xrLabel1.Text = string.Format("{0} | {1}", Convert.ToString(GetCurrentColumnValue("DetailItemType")), Convert.ToString(GetCurrentColumnValue("DetailItemGroupName1")));
            }


            String ItemCreatedDateInString = Convert.ToString(GetCurrentColumnValue("DetailItemCreatedDateInString"));
            if (ItemCreatedDateInString == "01-Jan-1900")
            {
                xrLabel1.Visible = false;
                xrTable2.Visible = false;
            }
            else
            {
                xrLabel1.Visible = true;
                xrTable2.Visible = true;
            }
        }

        private void cGroupChargesQty_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Boolean IsCreatedBySystem = Convert.ToBoolean(GetCurrentColumnValue("DetailIsCreatedBySystem"));
            if (!IsCreatedBySystem)
            {
                cGroupChargesQty.Text = "";
            }
        }

        private void cPersonal_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Boolean IsCreatedBySystem = Convert.ToBoolean(GetCurrentColumnValue("DetailIsCreatedBySystem"));
            if (IsCreatedBySystem)
            {
                cPersonal.Text = "";
            }
        }

        private void cCorporate_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Boolean IsCreatedBySystem = Convert.ToBoolean(GetCurrentColumnValue("DetailIsCreatedBySystem"));
            if (IsCreatedBySystem)
            {
                cCorporate.Text = "";
            }
        }
    }
}
