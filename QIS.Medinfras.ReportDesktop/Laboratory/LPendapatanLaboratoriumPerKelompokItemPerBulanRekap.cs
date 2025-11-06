using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPendapatanLaboratoriumPerKelompokItemPerBulanRekap : DevExpress.XtraReports.UI.XtraReport
    {
        public LPendapatanLaboratoriumPerKelompokItemPerBulanRekap()
        {
            InitializeComponent();
        }

        public void InitializeReport(int Year, int Month, int ItemGroupID)
        {
            List<GetTotalLaboratoryTransactionPerMonthPerItemGroupRekap> lstRekap = BusinessLayer.GetTotalLaboratoryTransactionPerMonthPerItemGroupRekap(Year, Month, ItemGroupID);
            this.DataSource = lstRekap;
        }
    }
}
