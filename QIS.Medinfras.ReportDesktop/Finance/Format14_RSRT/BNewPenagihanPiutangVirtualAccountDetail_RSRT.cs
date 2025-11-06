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
    public partial class BNewPenagihanPiutangVirtualAccountDetail_RSRT : DevExpress.XtraReports.UI.XtraReport
    {

        public BNewPenagihanPiutangVirtualAccountDetail_RSRT()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<GetTransferAccountBankRSRT> lst)
        {
            //vBusinessPartnerVirtualAccount
            this.DataSource = lst;
        }

    }
}
