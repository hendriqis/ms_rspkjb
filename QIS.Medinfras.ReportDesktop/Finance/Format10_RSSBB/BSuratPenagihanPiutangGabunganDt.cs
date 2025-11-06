using System;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BSuratPenagihanPiutangGabunganDt : DevExpress.XtraReports.UI.XtraReport
    {
        public BSuratPenagihanPiutangGabunganDt()
        {
            InitializeComponent();
        }

        public void InitializeReport(int ARReceiptID)
        {
            List<vARReceiptDt> lst = BusinessLayer.GetvARReceiptDtList(string.Format("ARReceiptID = {0} AND IsDeleted = 0", ARReceiptID));

            this.DataSource = lst;
        }
    }
}
